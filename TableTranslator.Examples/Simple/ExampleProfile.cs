using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableTranslator.Examples.Model;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Examples.Simple
{
    class ExampleProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<Person>(new TranslationSettings("Basic"))
                // some members of Person
                .AddColumnConfiguration(x => x.Name) 
                .AddColumnConfiguration(x => x.Birthday)
                .AddColumnConfiguration(x => x.IsMarried)
                .AddColumnConfiguration(x => Person.IsWarmBlooded)
                // some members from Address that belongs to Person
                .AddColumnConfiguration(x => x.Address.Street) 
                .AddColumnConfiguration(x => x.Address.City)
                .AddColumnConfiguration(x => x.Address.State)
                .AddColumnConfiguration(x => x.Address.PostalCode)
                // use an expression using an instance of Person
                .AddColumnConfiguration(x => string.Format("{0}|{1}|{2}|{3}", x.Address.Street, x.Address.City, x.Address.State, x.Address.PostalCode),
                    new ColumnConfigurationSettings<string> { ColumnName = "AddressOneLiner"})
                // call a method you have defined without referencing an instance of Person
                .AddColumnConfiguration(Multiply(2, 5), new ColumnConfigurationSettings<int> {ColumnName = "SomeSimpleMath"})
                // use a built in method without referencing an instance of Person
                .AddColumnConfiguration(string.Join("", "Hello".Reverse()), new ColumnConfigurationSettings<string> { ColumnName = "ReverseMe!"})
                // simple constant value
                .AddColumnConfiguration(8, new ColumnConfigurationSettings<int> { ColumnName = "OxygenAtomicNumber"});


            AddTranslation<Person>(new TranslationSettings("BasicAllMembers"))
                .AddColumnConfigurationForAllMembers();


            AddTranslation<Person>(new TranslationSettings("AllMembersWithPredicate"))
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings { Predicate = x => x.Name.Contains("r")});


            AddTranslation<Person>(new TranslationSettings("AllMembersWithOrderer"))
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings { Orderer = new MemberNameLengthDescendingOrderer() });


            AddTranslation<Person>(new TranslationSettings("AllMembersWithBindingFlags"))
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                {
                    BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                });


            AddTranslation<int>(new TranslationSettings("Struct"))
                .AddColumnConfiguration(x => x, new ColumnConfigurationSettings<int> {ColumnName = "X"})
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> {ColumnName = "X Times 10"})
                .AddColumnConfiguration(x => DateTime.Now.AddDays(x), new ColumnConfigurationSettings<DateTime> { ColumnName = "X Days From Now" });
        }

        static int Multiply(int x, int y)
        {
            return x * y;
        }
    }

    class MemberNameLengthDescendingOrderer : IComparer<MemberInfo>
    {
        public int Compare(MemberInfo x, MemberInfo y)
        {
            if (x.Name.Length > y.Name.Length)
            {
                return -1;
            }
            if (x.Name.Length < y.Name.Length)
            {
                return 1;
            }
            return 0;
        }
    }
}
