using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.Data.SqlClient;

namespace CSVReaderApp
{
    public static class SQLFuncs
    {
        public static void InitDB(string sqlConnectionString)
        {
            string script = File.ReadAllText(@"C:\Users\Ejozz\source\repos\CSVImport\CSVImport\InitDB.sql");

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            server.ConnectionContext.ExecuteNonQuery(script);
        }

        public static void ProcessImport(string sqlConnectionString)
        {
           string script = File.ReadAllText(@"C:\Users\Ejozz\source\repos\CSVImport\CSVImport\ProcessImport.sql");

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
