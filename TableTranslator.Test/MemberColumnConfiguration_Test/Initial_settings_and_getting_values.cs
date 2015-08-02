using System;
using System.Reflection;
using NUnit.Framework;
using TableTranslator.Model.ColumnConfigurations;
using TableTranslator.Test.TestModels;

namespace TableTranslator.Test.MemberColumnConfiguration_Test
{
    public class Initial_settings_and_getting_values
    {
        private string PrivateProperty { get; set; }
        private int PrivateField;
        private BindingFlags _bindingFlags;
        private Type _personModelType;
        private TestPerson _personModel;
        private TestParent _parentModel;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            this._bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

            this._personModel = Mother.GetTestPersonModel();
            this._personModelType = this._personModel.GetType();
            this._parentModel = new TestParent { TestPerson = this._personModel };

            this.PrivateProperty = "Hello5";
            this.PrivateField = 105;
        }

        [Test]
        public void Output_type_is_set_based_on_member_type()
        {
            var config = new MemberColumnConfiguration(this._personModelType.GetProperty("NullableDate", this._bindingFlags), 0, "ColName", DBNull.Value, "NullableDate");
            Assert.AreEqual(typeof(DateTime?), config.OutputType);
        }

        [Test]
        public void Full_property_name_of_child_member_is_allowed()
        {
            Assert.DoesNotThrow(() => new MemberColumnConfiguration(this._personModelType.GetProperty("PublicProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.PublicProperty"));
        }

        [Test]
        public void Get_correct_value_for_top_level_members_of_all_access_levels()
        {
            var publicPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("PublicProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "PublicProperty");
            var publicFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("PublicField", this._bindingFlags), 0, "ColName", DBNull.Value, "PublicField");
            var internalPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("InternalProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "InternalProperty");
            var internalFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("InternalField", this._bindingFlags), 0, "ColName", DBNull.Value, "InternalField");
            var protectedInternalPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("ProtectedInternalProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "ProtectedInternalProperty");
            var protectedInternalFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("ProtectedInternalField", this._bindingFlags), 0, "ColName", DBNull.Value, "ProtectedInternalField");
            var privatePropertyConfig = new MemberColumnConfiguration(this.GetType().GetProperty("PrivateProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "PrivateProperty");
            var privateFieldConfig = new MemberColumnConfiguration(this.GetType().GetField("PrivateField", this._bindingFlags), 0, "ColName", DBNull.Value, "PrivateField");
            var staticPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("StaticProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "StaticProperty");
            var staticFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("StaticField", this._bindingFlags), 0, "ColName", DBNull.Value, "StaticField");
            var constFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("ConstantField", this._bindingFlags), 0, "ColName", DBNull.Value, "ConstantField");


            Assert.AreEqual(this._personModel.PublicProperty, publicPropertyConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this._personModel.PublicField, publicFieldConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this._personModel.InternalProperty, internalPropertyConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this._personModel.InternalField, internalFieldConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this._personModel.ProtectedInternalProperty, protectedInternalPropertyConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this._personModel.ProtectedInternalField, protectedInternalFieldConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(this.PrivateProperty, privatePropertyConfig.GetValueFromObject(this));
            Assert.AreEqual(this.PrivateField, privateFieldConfig.GetValueFromObject(this));
            Assert.AreEqual(TestPerson.StaticProperty, staticPropertyConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(TestPerson.StaticField, staticFieldConfig.GetValueFromObject(this._personModel));
            Assert.AreEqual(TestPerson.ConstantField, constFieldConfig.GetValueFromObject(this._personModel));
        }

        [Test]
        public void Get_correct_value_for_child_members_of_all_access_levels()
        {
            var publicPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("PublicProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.PublicProperty");
            var publicFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("PublicField", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.PublicField");
            var internalPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("InternalProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.InternalProperty");
            var internalFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("InternalField", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.InternalField");
            var protectedInternalPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("ProtectedInternalProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.ProtectedInternalProperty");
            var protectedInternalFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("ProtectedInternalField", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.ProtectedInternalField");
            var staticPropertyConfig = new MemberColumnConfiguration(this._personModelType.GetProperty("StaticProperty", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.StaticProperty");
            var staticFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("StaticField", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.StaticField");
            var constantFieldConfig = new MemberColumnConfiguration(this._personModelType.GetField("ConstantField", this._bindingFlags), 0, "ColName", DBNull.Value, "TestPerson.ConstantField");


            Assert.AreEqual(this._parentModel.TestPerson.PublicProperty, publicPropertyConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(this._parentModel.TestPerson.PublicField, publicFieldConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(this._parentModel.TestPerson.InternalProperty, internalPropertyConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(this._parentModel.TestPerson.InternalField, internalFieldConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(this._parentModel.TestPerson.ProtectedInternalProperty, protectedInternalPropertyConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(this._parentModel.TestPerson.ProtectedInternalField, protectedInternalFieldConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(TestPerson.StaticProperty, staticPropertyConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(TestPerson.StaticField, staticFieldConfig.GetValueFromObject(this._parentModel));
            Assert.AreEqual(TestPerson.ConstantField, constantFieldConfig.GetValueFromObject(this._parentModel));
        }
    }
}