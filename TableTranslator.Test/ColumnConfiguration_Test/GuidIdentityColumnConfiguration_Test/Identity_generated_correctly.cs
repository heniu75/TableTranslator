﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.ColumnConfiguration_Test.GuidIdentityColumnConfiguration_Test
{
    [TestFixture]
    public class Identity_generated_correctly : InitializedTranslatorTestBase
    {
        [Test]
        public void Identity_column_has_unique_guid_for_every_row()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "GuidNoColumnNameProvided");

            var guids = dataTable.Rows.Cast<DataRow>().Select(row => (Guid) row[0]).ToList();
            Assert.AreEqual(10, guids.Distinct().Count());
        }
    }
}
