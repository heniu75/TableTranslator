using System;
using System.Collections.Generic;
using TableTranslator.Examples.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Examples
{
    class Program
    {
        #region set up sample data
        static readonly List<Person> _people = new List<Person>
            {
                new Person
                {
                    Name = "Tom",
                    Birthday = new DateTime(1980, 1, 10),
                    IsMarried = true,
                    Address = new Address {Street = "123 Main St", City = "Boston", State = "MA", PostalCode = "02115"}
                },
                new Person
                {
                    Name = "Sally",
                    Birthday = new DateTime(1965, 8, 1),
                    IsMarried = true,
                    Address = new Address {Street = "987 South Ave", City = "Los Angeles", State = "CA", PostalCode = "91106"}
                },
                new Person
                {
                    Name = "Jim",
                    Birthday = new DateTime(1987, 12, 14),
                    IsMarried = false,
                    Address = new Address {Street = "456 2nd St", City = "Cleveland", State = "OH", PostalCode = "44023"}
                }
            };
        #endregion

        static void Main(string[] args)
        {
            // add our example profile
            Translator.AddProfile<ExampleProfile>();

            // initialize the translator
            Translator.Initialize();

            // translate data using the various translations in our profile
            PrintTranslationHeader("Miscellaneous");
            Translator.Translate<ExampleProfile, Person>(_people, "Miscellaneous").PrintToConsole();

            PrintTranslationHeader("AllMembers");
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembers").PrintToConsole();

            PrintTranslationHeader("AllMembersWithPredicate");
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithPredicate").PrintToConsole();

            PrintTranslationHeader("AllMembersWithOrderer");
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithOrderer").PrintToConsole();

            PrintTranslationHeader("AllMembersWithBindingFlags");
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithBindingFlags").PrintToConsole();

            PrintTranslationHeader("VariousOptionalSettings");
            Translator.Translate<ExampleProfile, Person>(_people, "VariousOptionalSettings").PrintToConsole();

            PrintTranslationHeader("ForAStruct");
            Translator.Translate<ExampleProfile, int>(new List<int> { 1, 2, 3 }, "ForAStruct").PrintToConsole();

            // NOTE: The same translation can translate a list to a DataTable and/or DbParameter
            PrintTranslationHeader("Miscellaneous (as a DbParameter)");
            Translator.TranslateToDbParameter<ExampleProfile, Person>(_people,
                new DbParameterSettings("myParameterName", "myDatabaseObjName"), "Miscellaneous").PrintToConsole();

            Console.ReadLine();
        }

        static void PrintTranslationHeader(string translationName)
        {
            Console.WriteLine("[{0}]{1}", translationName, Environment.NewLine);
        }
    }
}
