using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class Translate_to_DT : InitializedTranslatorTestBase
    {
        private readonly List<TestPerson> people = new List<TestPerson>
        {
            new TestPerson {PublicProperty = "Chris", PublicField = 100},
            new TestPerson {PublicProperty = "Aubrey", PublicField = 101},
            new TestPerson {PublicProperty = "John", PublicField = 102}
        };

        [Test]
        public void Translate_to_DT_number_of_rows()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1");
            Assert.AreEqual(3, dt.Rows.Count);
        }

        [Test]
        public void Translate_to_DT_values()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var rows = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1").Rows;

            Assert.AreEqual("Chris", rows[0][0]);
            Assert.AreEqual(27, (int)rows[0][1]);
            Assert.AreEqual(108, (int)rows[0][2]);
            Assert.AreEqual(999, (int)rows[0][3]);

            Assert.AreEqual("Aubrey", rows[1][0]);
            Assert.AreEqual(27, (int)rows[1][1]);
            Assert.AreEqual(109, (int)rows[1][2]);
            Assert.AreEqual(999, (int)rows[0][3]);

            Assert.AreEqual("John", rows[2][0]);
            Assert.AreEqual(27, (int)rows[2][1]);
            Assert.AreEqual(110, (int)rows[2][2]);
            Assert.AreEqual(999, (int)rows[0][3]);
        }

        [Test]
        public void Translate_to_DT_column_types()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var cols = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1").Columns;
            Assert.AreEqual(typeof(string), cols[0].DataType);
            Assert.AreEqual(typeof(int), cols[1].DataType);
            Assert.AreEqual(typeof(int), cols[2].DataType);
            Assert.AreEqual(typeof(int), cols[3].DataType);
        }

        [Test]
        public void Translate_to_DT_column_order()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var columnNames = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1").GetColumnNames();
            Assert.AreEqual("ColOne", columnNames.First());
            Assert.AreEqual("ColTwo", columnNames.Skip(1).First());
            Assert.AreEqual("ColThree", columnNames.Skip(2).First());
            Assert.AreEqual("ColFour", columnNames.Skip(3).First());
        }

        [Test]
        public void Translate_to_DT_table_name_is_name_of_translation_when_explicit()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1");
            Assert.AreEqual("Translation1", dt.TableName);
        }

        [Test]
        public void Translate_to_DT_table_name_is_name_of_type_when_no_explicit_translation_name_given()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<BasicProfile, bool>(new List<bool> {true, false, true, false});
            Assert.AreEqual("Boolean", dt.TableName);
        }

        [Test]
        public void Translate_to_DT_table_name_is_name_of_type_with_generic_formatting_when_no_explicit_translation_name_given()
        {
            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<GenericsProfile, List<bool>>(new List<List<bool>>());
            Assert.AreEqual("List<Boolean>", dt.TableName);
        }

        [Test]
        public void Translate_to_DT_null_item_in_source_gets_ignored()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<BasicProfile, TestPerson>(new List<TestPerson>(people) { null }, "Translation1");
            Assert.AreEqual(3, dt.Rows.Count);
        }

        [Test]
        public void Translate_to_DT_allow_DbNull_only_for_nullable_types()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var cols = Translator.Translate<BasicProfile, TestPerson>(people, "Translation1").Columns;
            Assert.AreEqual(true, cols[0].AllowDBNull);
            Assert.AreEqual(false, cols[1].AllowDBNull);
        }

        [Test]
        public void Translate_to_DT_returns_empty_table_structure_when_source_is_null()
        {
            Translator.AddProfile<BasicProfile>();
            Translator.ApplyUpdates();
            var dt = Translator.Translate<BasicProfile, TestPerson>(null, "Translation1");
            Assert.AreEqual(0, dt.Rows.Count);
            Assert.AreEqual(4, dt.Columns.Count);
        }

        [Test]
        public void Profile_level_column_prefix_and_suffix_applied_to_all_columns_and_tranlations_not_and_does_not_override_translation_specifics()
        {
            Translator.AddProfile<PrefixSuffixProfile>();
            Translator.ApplyUpdates();

            var table = Translator.Translate<PrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "ProfileInherited1").GetColumnNames();
            var table2 = Translator.Translate<PrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "ProfileInherited2").GetColumnNames();
            var table3 = Translator.Translate<PrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "TranslationSpecific").GetColumnNames();

            Assert.IsTrue(table.All(x => x.StartsWith("PROFPRE_") && x.EndsWith("_PROFSUF")));
            Assert.IsTrue(table2.All(x => x.StartsWith("PROFPRE_") && x.EndsWith("_PROFSUF")));
            Assert.IsTrue(!table3.Any(x => x.StartsWith("PROFPRE_") && x.EndsWith("_PROFSUF")));
        }

        [Test]
        public void Translation_level_column_prefix_and_suffix_applied_to_all_columns_and_tranlations_specific_overrides_profile_level()
        {
            Translator.AddProfile<PrefixSuffixProfile>();
            Translator.ApplyUpdates();

            var table3 = Translator.Translate<PrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "TranslationSpecific").GetColumnNames();

            Assert.IsTrue(table3.All(x => x.StartsWith("TRANPRE_") && x.EndsWith("_TRANSUF")));
        }

        [Test]
        public void Translation_level_null_column_prefix_and_suffix_uses_non_null_profile_level_prefix_and_suffix()
        {
            Translator.AddProfile<PrefixSuffixProfile>();
            Translator.ApplyUpdates();

            var columnNames = Translator.Translate<PrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "NullTranslationSpecific").GetColumnNames();
            Assert.IsTrue(columnNames.All(x => x.StartsWith("PROFPRE_") && x.EndsWith("_PROFSUF")));
        }

        [Test]
        public void Profile_level_null_column_prefix_and_suffix_does_not_change_column_name()
        {
            Translator.AddProfile<NullPrefixSuffixProfile>();
            Translator.ApplyUpdates();

            var table = Translator.Translate<NullPrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "NullPrefixSuffix").GetColumnNames();
            Assert.IsTrue(!table.Any(x => x.StartsWith("PROFPRE_") && x.EndsWith("_PROFSUF")));
        }

        [Test]
        public void Translation_level_null_column_prefix_and_suffix_does_not_change_column_name()
        {
            Translator.AddProfile<NullPrefixSuffixProfile>();
            Translator.ApplyUpdates();

            var table = Translator.Translate<NullPrefixSuffixProfile, TestPerson>(new List<TestPerson>(), "TranslationLevelNullPrefixSuffix").GetColumnNames();
            Assert.IsTrue(!table.Any(x => x.StartsWith("TRANPRE_") && x.EndsWith("_TRANSUF")));
        }

        [Test]
        public void Translation_for_model_with_one_generic()
        {
            var oneGenerics = new List<Generics.OneGeneric<int>>
            {
                new Generics.OneGeneric<int> {TData = 1},
                new Generics.OneGeneric<int> {TData = 2},
                new Generics.OneGeneric<int> {TData = 3}
            };

            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            var dt = new DataTable();
            Assert.DoesNotThrow(() => dt = Translator.Translate<GenericsProfile, Generics.OneGeneric<int>>(oneGenerics, "IntGeneric"));
            Assert.AreEqual(3, dt.Rows.Count);
        }

        [Test]
        public void Translation_for_model_with_multiple_generic()
        {
            var threeGenerics = new List<Generics.ThreeGenerics<int, DateTime, string>>
            {
                new Generics.ThreeGenerics<int, DateTime, string> {TData = 1, KData = DateTime.Now, JData = "J1"},
                new Generics.ThreeGenerics<int, DateTime, string> {TData = 2, KData = DateTime.Now, JData = "J2"},
                new Generics.ThreeGenerics<int, DateTime, string> {TData = 3, KData = DateTime.Now, JData = "J3"}
            };

            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            var dt = new DataTable();
            Assert.DoesNotThrow(() => dt = Translator.Translate<GenericsProfile, Generics.ThreeGenerics<int, DateTime, string>>(threeGenerics));
            Assert.AreEqual(3, dt.Rows.Count);
        }

        [Test]
        public void Translation_for_model_with_nested_generics()
        {
            var nestedGenerics = new List<Generics.OneGeneric<Generics.OneGeneric<bool>>>
            {
                new Generics.OneGeneric<Generics.OneGeneric<bool>> { TData = new Generics.OneGeneric<bool> { TData = true}},
                new Generics.OneGeneric<Generics.OneGeneric<bool>> { TData = new Generics.OneGeneric<bool> { TData = false}},
                new Generics.OneGeneric<Generics.OneGeneric<bool>> { TData = new Generics.OneGeneric<bool> { TData = true}}
            };

            Translator.AddProfile<GenericsProfile>();
            Translator.ApplyUpdates();
            var dt = new DataTable();
            Assert.DoesNotThrow(() => dt = Translator.Translate<GenericsProfile, Generics.OneGeneric<Generics.OneGeneric<bool>>>(nestedGenerics, "NestedGeneric"));
            Assert.AreEqual(3, dt.Rows.Count);
        }
    }
}