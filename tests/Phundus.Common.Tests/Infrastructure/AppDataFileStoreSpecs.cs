namespace Phundus.Common.Tests.Infrastructure
{
    using System;
    using System.IO;
    using Common.Infrastructure;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class app_data_file_store_concern : Observes<IFileStore, AppDataFileStore>
    {
        protected static string baseDirectory;

        private Cleanup cleanup = () =>
            Directory.Delete(baseDirectory, true);

        private Establish ctx = () =>
        {
            baseDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("D"));
            sut_factory.create_using(() =>
                new AppDataFileStore(Path.Combine(baseDirectory, "teststorage")));
        };

        protected static Stream createStream(int size = 1)
        {
            var random = new Random();
            var result = new MemoryStream();
            for (var i = 0; i < size; i++)
                result.WriteByte(Convert.ToByte(random.Next(0, 255)));
            return result;
        }

        protected static FileInfo fileInfo(string storage, string fileName)
        {
            return new FileInfo(Path.Combine(baseDirectory, storage, fileName));
        }
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_get_files : app_data_file_store_concern
    {
        private static StoredFileInfo[] result;

        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.Add("file-b.pdf", createStream(40), 0);
                sut.Add("file-a.pdf", createStream(20), 1);
                sut.Add("file-a.pdf", createStream(30), 2);
            });

        private Because of = () =>
            result = sut.GetFiles();

        private It should_return_distinct_unversioned_files = () =>
        {
            result.Length.ShouldEqual(2);
            result[0].ShouldMatch(c => c.Name == "file-a.pdf" && c.Version == 2);
            result[1].ShouldMatch(c => c.Name == "file-b.pdf" && c.Version == 0);
        };
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_removing_an_non_existing_file : app_data_file_store_concern
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.Remove("nonExisting.pdf"));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_removing_an_existing_file : app_data_file_store_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.Add("version.pdf", createStream(20), 2);
                sut.Add("version.pdf", createStream(20), 3);
            });

        private Because of = () =>
            sut.Remove("version.pdf");

        private It should_delete_all_versions = () =>
        {
            fileInfo("Orders", "version-2.pdf").Exists.ShouldBeFalse();
            fileInfo("Orders", "version-3.pdf").Exists.ShouldBeFalse();
        };
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_storing_a_file_version_less_than_zero : app_data_file_store_concern
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.Add("fileName.pdf", createStream(), -1, true));

        private It should_throw_argument_out_of_range_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_storing_a_file_with_version : app_data_file_store_concern
    {
        private static StoredFileInfo result;

        private Because of = () =>
            result = sut.Add("fileName.pdf", createStream(), 99, true);

        private It should_return_stored_file_info = () =>
        {
            result.ShouldNotBeNull();
            result.Name.ShouldEqual("fileName.pdf");
            result.Extension.ShouldEqual("pdf");
            result.FullName.ShouldEqual(fileInfo("teststorage", "fileName-99.pdf").FullName);
            result.Version.ShouldEqual(99);
        };

        private It should_write_file_to_storage_directory = () =>
            fileInfo("teststorage", "fileName-99.pdf").Exists.ShouldBeTrue();
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_storing_an_existing_file_with_same_version_and_overwrite_option : app_data_file_store_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Add("fileName.pdf", createStream(20), 99));

        private Because of = () =>
            sut.Add("fileName.pdf", createStream(10), 99, true);

        private It should_overwrite_file = () =>
            fileInfo("teststorage", "fileName-99.pdf").Length.ShouldEqual(10);
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_storing_an_existing_file_with_same_version_and_without_overwrite_option :
        app_data_file_store_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Add("fileName.pdf", createStream(20), 99));

        private Because of = () =>
            spec.catch_exception(() =>
                sut.Add("fileName.pdf", createStream(10), 99, false));

        private It should_throw_io_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<IOException>();
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_getting_an_non_existent_file_with_version : app_data_file_store_concern
    {
        private static object result;

        private Because of = () =>
            result = sut.Get("notExisting.pdf", 1);

        private It should_return_null = () =>
            result.ShouldBeNull();
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_getting_an_existing_file_with_version : app_data_file_store_concern
    {
        private static Stream stream;
        private static StoredFileInfo result;

        private Establish ctx = () =>
        {
            stream = createStream(30);
            sut_setup.run(sut =>
                sut.Add("fileName.pdf", stream, 1));
        };

        private Because of = () =>
            result = sut.Get("fileName.pdf", 1);

        private It should_return_stream = () =>
        {
            using (var resultStream = sut.GetStream(result))
            {
                resultStream.ShouldEqual(stream);
            }
        };
    }

    [Subject(typeof (AppDataFileStore))]
    public class when_getting_an_existing_file_without_version : app_data_file_store_concern
    {
        private static Stream stream;
        private static StoredFileInfo result;

        private Establish ctx = () => sut_setup.run(sut =>
        {
            for (var i = 8; i <= 12; i++)
            {
                stream = createStream(i);
                sut.Add("fileName.pdf", stream, i);
            }
        });

        private Because of = () =>
            result = sut.Get("fileName.pdf", -1);

        private It should_return_highest_version = () =>
            result.Length.ShouldEqual(12);
    }


    public static class ShouldExtensions
    {
        public static void ShouldEqual(this Stream actual, Stream expected)
        {
            actual.Length.ShouldEqual(expected.Length);
            actual.Seek(0, SeekOrigin.Begin);
            expected.Seek(0, SeekOrigin.Begin);
            for (var i = 0; i < actual.Length; i++)
            {
                var b = actual.ReadByte();
                b.ShouldEqual(expected.ReadByte());
            }
        }
    }
}