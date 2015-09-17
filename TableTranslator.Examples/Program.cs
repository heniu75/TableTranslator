using System;
using System.Collections.Generic;
using TableTranslator.Examples.Model;
using TableTranslator.Examples.Simple;

namespace TableTranslator.Examples
{
    class Program
    {
        #region set up data
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
            Translator.AddProfile<ExampleProfile>();
            Translator.Initialize();

            // perform our translations
            Console.WriteLine("Basic{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, Person>(_people, "Basic").PrintToConsole();

            Console.WriteLine("BasicAllMembers{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, Person>(_people, "BasicAllMembers").PrintToConsole();

            Console.WriteLine("AllMembersWithPredicate{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithPredicate").PrintToConsole();

            Console.WriteLine("AllMembersWithOrderer{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithOrderer").PrintToConsole();

            Console.WriteLine("AllMembersWithBindingFlags{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, Person>(_people, "AllMembersWithBindingFlags").PrintToConsole();

            Console.WriteLine("Struct{0}", Environment.NewLine);
            Translator.Translate<ExampleProfile, int>(new List<int> { 1,2,3 }, "Struct").PrintToConsole();


            Console.ReadLine();
        }
    }
}
