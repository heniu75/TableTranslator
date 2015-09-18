using System;
using System.Linq;
using System.Reflection;
using TableTranslator.Examples.Model;
using TableTranslator.Model;
using TableTranslator.Model.ColumnConfigurations.Identity;
using TableTranslator.Model.Settings;

namespace TableTranslator.Examples
{
    class ExampleProfile : TranslationProfile
    {
        // Giving my profile an explicit name (this is optional, by default the profile will be name of the class)
        public override string ProfileName => "MyProfile";


        /// <summary>
        /// Use Configure to add your translations. There can be as many translations as you want for as many types as you want.
        /// </summary>
        protected override void Configure()
        {
            AddTranslation<Person>(new TranslationSettings("Miscellaneous"))
                // some members of Person
                .AddColumnConfiguration(x => x.Name) 
                .AddColumnConfiguration(x => x.Birthday)
                .AddColumnConfiguration(x => x.IsMarried)
                // static member of Person
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


            AddTranslation<Person>(new TranslationSettings("AllMembers"))
                // adds column configurations for all members using these BindingFlags (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                .AddColumnConfigurationForAllMembers(); 


            AddTranslation<Person>(new TranslationSettings("AllMembersWithPredicate"))
                // adds column configurations for all members whose names contains 'r'
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                {
                    Predicate = x => x.Name.Contains("r")
                });


            AddTranslation<Person>(new TranslationSettings("AllMembersWithOrderer"))
                // adds column configurations for all members and orders them according to the provided IComparer<MemberInfo> (MemberNameLengthDescendingOrderer in this example)
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                {
                    Orderer = new MemberNameLengthDescendingOrderer()
                });


            AddTranslation<Person>(new TranslationSettings("AllMembersWithBindingFlags"))
                // adds column configurations for all members using custom BindingFlags (this is excluding static members)
                .AddColumnConfigurationForAllMembers(new GetAllMemberSettings
                {
                    BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                });


            AddTranslation<Person>(
                new TranslationSettings(
                    new GuidIdentityColumnConfiguration("MyGuidId"), // add an identity column configuration to the translation
                    "VariousOptionalSettings", // translation name
                    "PREFIX_", // prefix for all column names in the translation
                    "_SUFFIX")) // suffix for all column names in the translation
                .AddColumnConfiguration(x => x.Name,
                    new ColumnConfigurationSettings<string>
                    {
                        ColumnName = "DifferentName", // column name
                        NullReplacement = "I am NULL!" // if value is null, replace with this value
                    });


            AddTranslation<int>(new TranslationSettings("ForAStruct"))
                .AddColumnConfiguration(x => x, new ColumnConfigurationSettings<int> {ColumnName = "X"})
                .AddColumnConfiguration(x => x * 10, new ColumnConfigurationSettings<int> {ColumnName = "X Times 10"})
                .AddColumnConfiguration(x => DateTime.Now.AddDays(x), new ColumnConfigurationSettings<DateTime> { ColumnName = "X Days From Now" });
        }



        static int Multiply(int x, int y)
        {
            return x * y;
        }
    }
}
