using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TableTranslator.Test.TestModels;

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
    }

    public static class Mother
    {
        public static IntInAndOutDelegate IntInAndOutDelegate()
        {
            return x => x;
        }

        public static IntInAndStringOutDelegate IntInAndStringOutDelegate()
        {
            return x => string.Format("The number is {0}", x);
        }

        public static TestPerson GetTestPersonModel()
        {
            var model = new TestPerson
            {
                PublicProperty = "Hello1",
                PublicField = 101,
                InternalProperty = "Hello2",
                InternalField = 102,
                ProtectedInternalProperty = "Hello3",
                ProtectedInternalField = 103,
                NullableDate = new DateTime()
            };

            TestPerson.StaticProperty = "Hello4";
            TestPerson.StaticField = 104;

            return model;
        }
    }
}
