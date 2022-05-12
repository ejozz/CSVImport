using System.Data;

namespace CSVReaderApp
{
    class AppMain
    {
        static void Main(string[] args)
        {
            //SET CONNECTION STRING HERE
            String SQLConnString = @"Data Source = DESKTOP-DQ5UGS2\SQLEXPRESS; Initial Catalog = AdventureWorksLT2019; Integrated Security = SSPI;";
            
            String CurrentDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            
            String CSVPath = Path.Combine(CurrentDir, @"customer_orders.csv");
            
            SQLFuncs.InitDB(SQLConnString);
            
            Console.WriteLine("Reading CSV from " + CSVPath);
            
            DataTable csvData = CSVReader.GetDataTableFromCSV(CSVPath);
            
            CSVReader.InsertDataTableIntoSQL(csvData, SQLConnString);
            
            int numRows = csvData.Rows.Count;
            
            Console.WriteLine("\nNum Rows Validated: " + SQLFuncs.ValidateRows(numRows,SQLConnString));
            
            Console.WriteLine("\nCSV Read. Lines: " + numRows);
            
            SQLFuncs.ProcessImport(SQLConnString);
            
            Console.WriteLine("\nImport Processed");
            
        }

    }   
}