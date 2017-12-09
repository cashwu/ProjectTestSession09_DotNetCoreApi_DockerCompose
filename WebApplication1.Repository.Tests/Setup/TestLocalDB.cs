using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebApplication1.Repository.Tests.Setup
{
    public class TestLocalDB
    {
        internal const string LocalDbMasterConnectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        internal static string LocalDbSampleDbConnectionString =>
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=SampleDB;Integrated Security=True";

        private string LocalDbFileName { get; set; }

        internal string DatabaseName { get; set; }

        public TestLocalDB()
        {
        }

        public TestLocalDB(string dataBaseName)
        {
            this.DatabaseName = dataBaseName;

            this.CreateDatabase();
        }

        private string DatabaseFilePath()
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string localDbFileName = this.LocalDbFileName;
            if (string.IsNullOrWhiteSpace(localDbFileName))
            {
                localDbFileName = this.DatabaseName;
            }

            string filePath = Path.Combine(directoryName, localDbFileName);
            return filePath;
        }

        internal void CreateDatabase()
        {
            this.DestroyDatabase();

            var fileName = string.Concat(this.DatabaseFilePath(), ".mdf");

            string sqlCommand = $@"
              IF(db_id(N'{this.DatabaseName}') IS NULL)
              BEGIN
                CREATE DATABASE [{this.DatabaseName}]
                ON (NAME = N'{this.DatabaseName}',
                FILENAME = '{fileName}')
              END";

            using (var connection = new SqlConnection(LocalDbMasterConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = sqlCommand;
                cmd.ExecuteNonQuery();
            }
        }

        internal void DestroyDatabase()
        {
            var queryCommand = $@"
                SELECT [physical_name] FROM [sys].[master_files] 
                WHERE [database_id] = DB_ID('{this.DatabaseName}')";

            var fileNames = ExecuteSqlQuery
            (
                LocalDbMasterConnectionString,
                string.Format(queryCommand, this.DatabaseName),
                row => (string)row["physical_name"]
            );

            var executeCommand = $@"
                ALTER DATABASE {this.DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                EXEC sp_detach_db '{this.DatabaseName}', 'true'";

            if (fileNames.Any())
            {
                ExecuteSqlCommand
                (
                    LocalDbMasterConnectionString,
                    string.Format(executeCommand, this.DatabaseName)
                );

                fileNames.ForEach(File.Delete);
            }

            var fileName = this.DatabaseFilePath();
            try
            {
                var mdfPath = string.Concat(fileName, ".mdf");
                var ldfPath = string.Concat(fileName, "_log.ldf");

                var mdfExists = File.Exists(mdfPath);
                var ldfExists = File.Exists(ldfPath);

                if (mdfExists) File.Delete(mdfPath);
                if (ldfExists) File.Delete(ldfPath);
            }
            catch
            {
                Console.WriteLine("Could not delete the files (open in Visual Studio?)");
            }
        }

        private static void ExecuteSqlCommand(string connectionString, string commandText)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static List<T> ExecuteSqlQuery<T>(string connectionString,
                                                  string queryText,
                                                  Func<SqlDataReader, T> read)
        {
            var result = new List<T>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = queryText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(read(reader));
                        }
                    }
                }
            }

            return result;
        }
    }
}