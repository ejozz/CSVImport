namespace CSVReaderApp
{
    class AppMain
    {
        static void Main(string[] args)
        {
            String CSVPath = @"C:\Users\Ejozz\source\repos\CSVImport\CSVImport\customer_orders.csv";
            String SQLConnString = @"Data Source = DESKTOP-DQ5UGS2\SQLEXPRESS; Initial Catalog = AdventureWorksLT2019; Integrated Security = SSPI;";
            SQLFuncs.InitDB(SQLConnString);
            Console.WriteLine("Reading CSV from " + CSVPath);
            CSVReader.InsertDataTableIntoSQL(CSVReader.GetDataTableFromCSV(CSVPath), SQLConnString);
            Console.WriteLine("\nCSV Read");
            SQLFuncs.ProcessImport(SQLConnString);
            Console.WriteLine("\nImport Processed");
        }

    }   
}