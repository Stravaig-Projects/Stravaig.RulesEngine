using System;
using NUnit.Framework;
using Shouldly;
using Stravaig.RulesEngine.Compiler;

namespace Stravaig.RulesEngine.Tests
{
    [TestFixture]
    public class ExpressionBuilderPropertyTypeTests
    {
        public class TestContext
        {
            public int AnInteger { get; set; }
            public string AString { get; set; }
            public bool ABoolean { get; set; }
            public DateTime ADateTime { get; set; }
            public decimal ADecimal { get; set; }
            
        }

        [Test]
        public void IntegerIsConvertedSuccessfully()
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("AnInteger", "==", "123", Array.Empty<Enum>());

            func(new TestContext { AnInteger = 123 }).ShouldBeTrue();
            func(new TestContext { AnInteger = 456 }).ShouldBeFalse();
        }
        
        [Test]
        public void DecimalIsConvertedSuccessfully()
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("ADecimal", "==", "123.456", Array.Empty<Enum>());

            func(new TestContext { ADecimal = 123.456M }).ShouldBeTrue();
            func(new TestContext { ADecimal = 111.222M }).ShouldBeFalse();
        }
        
        [Test]
        public void StringIsConvertedSuccessfully()
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("AString", "==", "abc", Array.Empty<Enum>());

            func(new TestContext { AString = "abc" }).ShouldBeTrue();
            func(new TestContext { AString = "def" }).ShouldBeFalse();
        }
        
        [Test]
        [TestCase("true")]
        [TestCase("True")]
        [TestCase("TRUE")]
        public void BooleanTrueIsConvertedSuccessfully(string booleanAsString)
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("ABoolean", "==", booleanAsString, Array.Empty<Enum>());

            func(new TestContext { ABoolean = true }).ShouldBeTrue();
            func(new TestContext { ABoolean = false }).ShouldBeFalse();
        }

        [Test]
        [TestCase("false")]
        [TestCase("False")]
        [TestCase("FALSE")]
        public void BooleanFalseIsConvertedSuccessfully(string booleanAsString)
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("ABoolean", "==", booleanAsString, Array.Empty<Enum>());

            func(new TestContext { ABoolean = false }).ShouldBeTrue();
            func(new TestContext { ABoolean = true }).ShouldBeFalse();
        }
        
        [Test]
        [TestCase("2021-08-31 17:29:45", 2021, 08, 31, 17, 29, 45, 0)]
        [TestCase("2021-08-31 17:29:45.123", 2021, 08, 31, 17, 29, 45, 123)]
        [TestCase("2021-08-31", 2021, 08, 31, 0, 0, 0, 0)]
        [TestCase("2021-08-31T17:29:45", 2021, 08, 31, 17, 29, 45, 0)]
        [TestCase("2021-08-31T17:29:45.123", 2021, 08, 31, 17, 29, 45, 123)]
        public void DateTimeIsConvertedSuccessfully(string dateTimeAsString, int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            var func = new ExpressionBuilder()
                .Build<TestContext>("ADateTime", "==", dateTimeAsString, Array.Empty<Enum>());

            func(new TestContext { ADateTime = new DateTime(year, month, day, hour, minute, second, millisecond) }).ShouldBeTrue();
            func(new TestContext { ADateTime = DateTime.UnixEpoch }).ShouldBeFalse();
        }

    }
}