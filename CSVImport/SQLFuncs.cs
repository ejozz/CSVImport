using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.Data.SqlClient;

namespace CSVReaderApp
{
    public static class SQLFuncs
    {
        public static void InitDB(string sqlConnectionString)
        {
            String CurrentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            String script = File.ReadAllText(Path.Combine(CurrentDir, @"InitDB.sql"));

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            server.ConnectionContext.ExecuteNonQuery(script);
        }

        public static void ProcessImport(string sqlConnectionString)
        {
            String CurrentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            String script = File.ReadAllText(Path.Combine(CurrentDir, @"ProcessImport.sql"));

            SqlConnection conn = new SqlConnection(sqlConnectionString);

            Server server = new Server(new ServerConnection(conn));

            server.ConnectionContext.ExecuteNonQuery(script);
        }

        public static bool ValidateRows(int RowCount, string sqlConnectionString)
        {
            String Command = "SELECT COUNT(*) FROM SalesLT.ImportStaging;";
            using (SqlConnection myConnection = new SqlConnection(sqlConnectionString))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(Command, myConnection))
                {
                    int Result = (int)myCommand.ExecuteScalar();
                    return RowCount == Result;
                }
            } 
        }
    }
}
