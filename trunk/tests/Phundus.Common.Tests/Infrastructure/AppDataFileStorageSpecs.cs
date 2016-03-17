namespace Phundus.Common.Tests.Infrastructure
{
    using System;
    using System.IO;
    using Common.Infrastructure;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class app_data_file_storage_concern : Observes<AppDataFileStorage>
    {
        protected static string baseDirectory;

        private Cleanup cleanup = () =>
            Directory.Delete(baseDirectory, true);

        private Establish ctx = () =>
        {
            baseDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("D"));
            sut_factory.create_using(() =>
                new AppDataFileStorage(baseDirectory));
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

    [Subject(typeof (AppDataFileStorage))]
    public class when_storing_a_file_version_less_than_zero : app_data_file_storage_concern
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.Store(Storage.Orders, "fileName.pdf", createStream(), -1));

        private It should_throw_argument_out_of_range_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentOutOfRangeException>();
    }

    [Subject(typeof (AppDataFileStorage))]
    public class when_storing_a_file_with_version : app_data_file_storage_concern
    {
        private Because of = () =>
            sut.Store(Storage.Orders, "fileName.pdf", createStream(), 99);

        private It should_write_file_to_storage_directory = () =>
            fileInfo("Orders", "fileName-99.pdf").Exists.ShouldBeTrue();
    }

    [Subject(typeof (AppDataFileStorage))]
    public class when_storing_an_existing_file_with_same_version : app_data_file_storage_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
                sut.Store(Storage.Orders, "fileName.pdf", createStream(20), 99));

        private Because of = () =>
            sut.Store(Storage.Orders, "fileName.pdf", createStream(10), 99);

        private It should_overwrite_file = () =>
            fileInfo("Orders", "fileName-99.pdf").Length.ShouldEqual(10);
    }

    [Subject(typeof (AppDataFileStorage))]
    public class when_getting_an_non_existent_file_with_version : app_data_file_storage_concern
    {
        private static object result;

        private Because of = () =>
            result = sut.Get(Storage.Orders, "notExisting.pdf", 1);

        private It should_return_null = () =>
            result.ShouldBeNull();
    }

    [Subject(typeof (AppDataFileStorage))]
    public class when_getting_an_existing_file_with_version : app_data_file_storage_concern
    {
        private static Stream stream;
        private static Stream result;

        private Cleanup cleanup = () =>
            result.Close();

        private Establish ctx = () =>
        {
            stream = createStream(30);
            sut_setup.run(sut =>
                sut.Store(Storage.Orders, "fileName.pdf", stream, 1));
        };

        private Because of = () =>
            result = sut.Get(Storage.Orders, "fileName.pdf", 1);

        private It should_return_stream = () =>
            result.ShouldEqual(stream);
    }

    [Subject(typeof (AppDataFileStorage))]
    public class when_getting_an_existing_file_without_version : app_data_file_storage_concern
    {
        private static Stream stream;
        private static Stream result;

        private Cleanup cleanup = () =>
            result.Close();

        private Establish ctx = () => sut_setup.run(sut =>
        {
            for (var i = 8; i <= 12; i++)
            {
                stream = createStream(i);
                sut.Store(Storage.Orders, "fileName.pdf", stream, i);
            }
        });

        private Because of = () =>
            result = sut.Get(Storage.Orders, "fileName.pdf", -1);

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