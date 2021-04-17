using System.Data.SqlClient;

namespace OrmPerformanceTests
{
    class SqlConnectionFactory
    {
        private const string serverName = @"localhost\SQLEXPRESS";
        private const string databaseName = "DatabasePerformanceTests";

        public SqlConnection Create() => new SqlConnection(getConnectionString(serverName, databaseName));


        private static string getConnectionString(string server, string databaseName)
            => $"Server={server};Database={databaseName};Integrated Security=True";
    }
}
