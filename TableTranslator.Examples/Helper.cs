using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTranslator.Examples
{
    public static class Helper
    {
        /// <summary>
        /// Simple helper method to view the results of the translation
        /// </summary>
        /// <param name="dataTable">DataTable to print to console</param>
        public static void PrintToConsole(this DataTable dataTable)
        {
            Console.WriteLine("\tTable Name: {0}", dataTable.TableName);
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine("\t-------------- ROW --------------");
                foreach (var col in dataTable.Columns.Cast<DataColumn>())
                {
                    Console.WriteLine("\t{0}: {1}", col.ColumnName, row[col.ColumnName] == DBNull.Value ? "NULL" : row[col.ColumnName]);
                }
            }
            Console.WriteLine();
        }
    }
}
