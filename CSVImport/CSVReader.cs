using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;

namespace CSVReaderApp
{
    public static class CSVReader
    {
        public static DataTable GetDataTableFromCSV(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(",");
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields) //Parsing header to create columns
                    {
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
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
              catch (Exception ex)
              {
                  return null;
              }
            return csvData;
        }

        public static void InsertDataTableIntoSQL(DataTable csvFileData, String SQLConnString)
        {
            using (SqlConnection dbConnection = new SqlConnection(SQLConnString))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "SalesLT.ImportStaging";
                    foreach (var column in csvFileData.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    s.WriteToServer(csvFileData);
                }
                dbConnection.Close();
            }
        }
    }
}

