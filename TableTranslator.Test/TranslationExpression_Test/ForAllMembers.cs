using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.TranslationExpression_Test
{
    [TestFixture]
    public class ForAllMembers : InitializedTranslatorTestBase
    {
        [Test]
        public void All_Members()
        {
            Translator.AddProfile<AllMembersProfile>();
            Translator.ApplyUpdates();
            var colName = Translator.Translate<AllMembersProfile, AllMembersModel>(new List<AllMembersModel>(), "All").GetColumnNames();
            CollectionAssert.Contains(colName, "Age");
            CollectionAssert.Contains(colName, "Name");
            CollectionAssert.Contains(colName, "IsMale");
            CollectionAssert.DoesNotContain(colName, "MethodShouldNotBeIncluded");
        }

        [Test]
        public void All_Members_With_Predicate()
        {
            Translator.AddProfile<AllMembersProfile>();
            Translator.ApplyUpdates();
            var colName = Translator.Translate<AllMembersProfile, AllMembersModel>(new List<AllMembersModel>(), "AllWithMPredicate").GetColumnNames();
            CollectionAssert.Contains(colName, "Name");
            CollectionAssert.Contains(colName, "IsMale");
            CollectionAssert.DoesNotContain(colName, "Age");
        }

        [Test]
        public void All_Members_With_Orderer()
        {
            Translator.AddProfile<AllMembersProfile>();
            Translator.ApplyUpdates();
            var colNames = Translator.Translate<AllMembersProfile, AllMembersModel>(new List<AllMembersModel>(), "AllByNameDesc").GetColumnNames();
            Assert.AreEqual("Name", colNames[0]);
            Assert.AreEqual("IsMale", colNames[1]);
            Assert.AreEqual("Age", colNames[2]);
        }

        [Test]
        public void All_Members_With_Binding_Flags()
        {
            Translator.AddProfile<AllMembersProfile>();
            Translator.ApplyUpdates();
            var colNames = Translator.Translate<AllMembersProfile, AllMembersModel>(new List<AllMembersModel>(), "AllNoStaticBindingFlags").GetColumnNames();
            CollectionAssert.Contains(colNames, "Name");
            CollectionAssert.Contains(colNames, "Age");
            CollectionAssert.DoesNotContain(colNames, "IsMale");
        }

        public class AllMembersProfile : TranslationProfile
        {
            protected override void Configure()
            {
                AddTranslation<AllMembersModel>(new TranslationSettings("All"))
                    .AddColumnConfigurationForAllMembers();

                AddTranslation<AllMembersModel>(new TranslationSettings("AllByNameDesc"))
                    .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                    {
                        Orderer = new NameDescComparer()
                    });

                AddTranslation<AllMembersModel>(new TranslationSettings("AllWithMPredicate"))
                    .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                    {
                        Predicate = x => x.Name.ToUpper().Contains("M")
                    });

                AddTranslation<AllMembersModel>(new TranslationSettings("AllNoStaticBindingFlags"))
                    .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                    {
                        BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    });
            }

            public class NameDescComparer : IComparer<MemberInfo>
            {
                public int Compare(MemberInfo x, MemberInfo y)
                {
                    return string.Compare(x.Name, y.Name, StringComparison.CurrentCulture) * -1;
                }
            }
        }

        public class AllMembersModel
        {
            public int Age { get; set; }
            public string Name;
            public static bool IsMale { get; set; }

            public string MethodShouldNotBeIncluded()
            {
                return string.Empty;
            }
        }
    }
}
