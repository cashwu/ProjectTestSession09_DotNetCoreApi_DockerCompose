using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Dapper;

namespace WebApplication1.Repository.Tests.Setup
{
    public class TestLocalDbProcess
    {
        private const string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=SampleDB;Integrated Security=True";

        private static TestLocalDB TestLocalDB
        {
            get
            {
                TestLocalDB testLocalDb = new TestLocalDB
                {
                    DatabaseName = "SampleDB"
                };
                return testLocalDb;
            }
        }

        public static void CreateDatabase()
        {
            TestLocalDB.CreateDatabase();
        }

        public static void CreateTable()
        {
            // create table
            var createTableScript = @"create_table_schema.sql";
            ExecuteDbScript("TestData", createTableScript);
        }

        public static void PrepareData()
        {
            // insert test data
            var insertDataScript = @"insert_data.sql";
            ExecuteDbScript("TestData", insertDataScript);
        }

        public static void DestroyDatabase()
        {
            TestLocalDB.DestroyDatabase();
        }

        private static void ExecuteDbScript(string folderName, string fileName)
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
    }
}