using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TableTranslator.Test.TestModel;
using TableTranslator.Test.TestModel.Profiles;

namespace TableTranslator.Test.Translator_Test
{
    [TestFixture]
    public class ReverseTranslate : InitializedTranslatorTestBase
    {
        private readonly List<TestPerson> people = new List<TestPerson>
        {
            new TestPerson {PublicProperty = "Chris", PublicField = 100},
            new TestPerson {PublicProperty = "Aubrey", PublicField = 101},
            new TestPerson {PublicProperty = "John", PublicField = 102}
        };

        [Test]
        [Ignore]
        public void Translate_to_ObjectResult_Test()
        {
            //Translator.AddProfile<BasicProfile3>();
            //Translator.ApplyUpdates();
            //var dt = Translator.Translate<BasicProfile3, TestPerson>(people);
            //var result = Translator.ReverseTranslate<BasicProfile3, TestPerson>(dt);

            //Assert.AreEqual(3, result.Count());

            //var first = result.First();
            //Assert.AreEqual("Chris", first.Object.PublicProperty);
            //Assert.AreEqual(100, first.Object.PublicField);
            //Assert.AreEqual("Bag Item", first.DataBag["BagItem1"]);
            //Assert.AreEqual(12345, first.DataBag["BagItem2"]);
        }
    }
}