using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.ColumnConfiguration_Test.SeededIdentityColumnConfiguration_Test
{
    [TestFixture]
    public class Seeded_correctly : InitializedTranslatorTestBase
    {
        [Test]
        public void Basic_seeded()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededBasic");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.IsTrue(seeds.IsSequential());
        }

        [Test]
        public void Seeded_with_custom_seed()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededStartingAt10");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.AreEqual(10, seeds.First());
            Assert.IsTrue(seeds.IsSequential());
        }

        [Test]
        public void Seeded_with_custom_increment()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededIncrementOf5");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.IsTrue(seeds.IsSequential(5));
        }

        [Test]
        public void Seeded_with_custom_seed_and_custom_increment()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededStartingAt10AndIncrementOf5");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.AreEqual(10, seeds.First());
            Assert.IsTrue(seeds.IsSequential(5));
        }

        [Test]
        public void Seeded_with_custom_seed_of_zero()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededStartingAt0");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.AreEqual(0, seeds.First());
            Assert.IsTrue(seeds.IsSequential());
        }

        [Test]
        public void Seeded_with_negative_custom_seed_increment()
        {
            Translator.AddProfile<IdentityProfile>();
            Translator.ApplyUpdates();
            var dataTable = Translator.Translate<IdentityProfile, int>(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, "SeededStartingAtNeg10AndIncrementOfNeg5");

            var seeds = dataTable.Rows.Cast<DataRow>().Select(row => (long)row[0]).ToList();
            Assert.AreEqual(-10, seeds.First());
            Assert.IsTrue(seeds.IsSequential(-5));
        }
    }
}