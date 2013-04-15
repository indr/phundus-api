using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace phiNdus.fundus.Web.App_Start
{
    using System.Configuration;
    using System.Data.SqlServerCe;
    using System.IO;

    /// <summary>
    /// <connectionStrings>
    ///   <add name="phundus" connectionString="Data Source=testdb.sdf" providerName="System.Data.SqlServerCe.4.0" />
    /// </connectionStrings>
    /// Es müsste dann auch noch die NHibernate-Konfig angepasst werden...
    /// </summary>
    public class StartMitSqlServerCe
    {
        public void TuEs()
        {
            if (ConfigurationManager.ConnectionStrings["phundus"].ProviderName == "System.Data.SqlServerCe.4.0")
            {
                var connectionString = ConfigurationManager.ConnectionStrings["phundus"].ConnectionString;
                var connection = new SqlCeConnection(connectionString);
                if (!File.Exists(ReplaceDataDirectory(connection.Database)))
                {
                    var engine = new SqlCeEngine(connectionString);
                    engine.CreateDatabase();
                }
            }
        }


        /// <summary>
        /// Replaces the data directory with a local path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A local path with the resolved 'DataDirectory' mapping.</returns>
        private string ReplaceDataDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && path.Contains("|DataDirectory|"))
            {
                var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                if (!string.IsNullOrEmpty(dataDirectory))
                {
                    path = path.Contains(@"|\")
                        ? path.Replace("|DataDirectory|", dataDirectory)
                        : path.Replace("|DataDirectory|", dataDirectory + System.IO.Path.DirectorySeparatorChar);
                }
            }

            return path;
        }
    }
}