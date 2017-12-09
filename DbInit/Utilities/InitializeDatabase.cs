using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DbInit.Utilities
{
    public class InitializeDatabase
    {
        public InitializeDatabase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string ConnectionString
        {
            get
            {
                var connectionString = Configuration.GetConnectionString("Database");
                return connectionString;
            }
        }

        public void Execute()
        {
            this.CreateDatabase();
            this.CreateTable();
            this.InsertData();
        }

        private void CreateDatabase()
        {
            var createDbScript = @"create_database_complex.sql";
            this.ExecuteDbScript("TestData", createDbScript);
        }

        private void CreateTable()
        {
            var createTableScript = @"create_table_schema.sql";
            this.ExecuteDbScriptWithTransaction("TestData", createTableScript);
        }

        private void InsertData()
        {
            var insertDataScript = @"insert_data.sql";
            this.ExecuteDbScriptWithTransaction("TestData", insertDataScript);
        }

        private void ExecuteDbScriptWithTransaction(string folderName, string fileName)
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directoryName, folderName, fileName);

            if (!File.Exists(filePath))
            {
                return;
            }

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var script = File.ReadAllText(filePath);
                    conn.Execute(sql: script, transaction: trans);
                    trans.Commit();
                }
            }
        }

        private void ExecuteDbScript(string folderName, string fileName)
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directoryName, folderName, fileName);

            if (!File.Exists(filePath))
            {
                return;
            }

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                var script = File.ReadAllText(filePath);
                conn.Execute(sql: script);
            }
        }
    }
}