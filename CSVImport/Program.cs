using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;

namespace CSVReaderApp
{
    class CSVReader
    {
        static void Main(string[] args)
        {
            String CSVPath = "C:\\Users\\Ejozz\\Desktop\\customer_orders.csv";
            String SQLConnString = "Data Source = DESKTOP-DQ5UGS2\\SQLEXPRESS; Initial Catalog = AdventureWorksLT2019; Integrated Security = SSPI;";
            Console.WriteLine("Reading CSV from " + CSVPath);
            InsertDataTableIntoSQL(GetDataTableFromCSV(CSVPath), SQLConnString);
            Console.WriteLine("\nCSV Read");
            
        }

        static DataTable GetDataTableFromCSV(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            //try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(",");
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    //int count = 2; //debugging
                    foreach (string column in colFields) //Parsing header to create columns
                    {
                        //Console.WriteLine(column); //debugging
                        DataColumn datacolumn = new DataColumn(column);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                    }
                    while (!csvReader.EndOfData) //Parsing rows and adding to respective column
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            // Console.WriteLine(fieldData[i]); //debugging
                             if (fieldData[i] == "")
                             {
                                 fieldData[i] = null;
                             }
                        }
                        //Console.WriteLine("Line: " + count+ " - " + fieldData.Length + "  -  " + fieldData[fieldData.Length-1]); //debugging

                        csvData.Rows.Add(fieldData);
                        //count++; //debugging
                    }
                    //Console.WriteLine("Columns: "+colFields.Length); //debugging
                }
            }
          /*  catch (Exception ex)
            {
                return null;
            }*/
            return csvData;
        }

        static void InsertDataTableIntoSQL(DataTable csvFileData, String SQLConnString)
        {
            using (SqlConnection dbConnection = new SqlConnection(SQLConnString))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "ImportStaging";
                    foreach (var column in csvFileData.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    s.WriteToServer(csvFileData);
                }
            }
        }
    }
}