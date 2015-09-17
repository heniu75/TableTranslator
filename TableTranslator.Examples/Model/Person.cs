using System;

namespace TableTranslator.Examples.Model
{
    class Person
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsMarried { get; set; }
        public Address Address { get; set; }

        public static bool IsWarmBlooded { get; private set; }

        public Person()
        {
            IsWarmBlooded = true;
        }
    }

    class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
