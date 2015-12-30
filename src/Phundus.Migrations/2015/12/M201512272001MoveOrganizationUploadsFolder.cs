namespace Phundus.Migrations
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using FluentMigrator;

    [Migration(201512272002)]
    public class M201512272002MoveOrganizationUploadsFolder : MigrationBase
    {
        private readonly string _basePath = HostingEnvironment.MapPath(@"~\Content\Uploads");

        public override void Up()
        {
            Execute.WithConnection(SelectOrganizations);
        }

        private void SelectOrganizations(IDbConnection connection, IDbTransaction tx)
        {
            var cmd = connection.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT [Id], [Guid] FROM [Organization]";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.NextResult())
                {
                    var id = (int) reader["Id"];
                    var guid = Guid.Parse((string) reader["Guid"]);
                    
                    CopyDirectory(id, guid);
                }
            }
        }

        private void CopyDirectory(int id, Guid guid)
        {
            var sourceDirName = Path.Combine(_basePath, "Organizations", id.ToString(CultureInfo.InvariantCulture));
            if (!Directory.Exists(sourceDirName))
                return;

            var destDirName = Path.Combine(_basePath, "Organizations", guid.ToString("N"));
            if (Directory.Exists(destDirName))
                return;

            DirectoryCopy(sourceDirName, destDirName, true);
            File.WriteAllText(Path.Combine(sourceDirName, "Moved.txt"),
                "This folder was moved to " + destDirName);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (var file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}