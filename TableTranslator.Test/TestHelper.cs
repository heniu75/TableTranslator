using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TableTranslator.Test
{
    public static class TestHelper
    {
        public static List<string> GetColumnNames(this DataTable table)
        {
            if (table == null)
            {
                return new List<string>();
            }

            return table.Columns.Cast<DataColumn>()
                .OrderBy(x => x.Ordinal)
                .Select(x => x.ColumnName).ToList();
        }

        public static void ResetTranslator()
        {
            if (!Translator.IsInitialized)
            {
                Translator.Initialize();
            }
            Translator.RemoveAllProfiles();
            Translator.ApplyUpdates();
        }

        public static bool IsSequential(this List<long> array, long increment = 1)
        {
            return array.Zip(array.Skip(1), (a, b) => (a + increment) == b).All(x => x);
        }
    }
}