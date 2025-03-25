using Xunit.Sdk;

namespace ComparableLibraryTest
{
    public class GeneralComparableTest
    {
        [Fact]
        public void CheckConstructCorrectHashForCompare()
        {
            var test1 = new TestClass
            {
                Prop1 = 1,
                Prop2 = "First Instance",
                Prop3 = new string[] { "Item1", "Item2" },
                Prop4 = new List<string> { "String1", "String2" },
                Prop5 = "Additional Info",
                TestClassSecond1 = new List<TestClassSecond>
            {
                new TestClassSecond
                {
                    Prop1 = 10,
                    Prop2 = "Second Instance Prop2",
                    Prop3 = "Nested Instance 1"
                }
            },
                TestClass2 = new TestClassSecond
                {
                    Prop1 = 20,
                    Prop2 = "Another Prop2",
                    Prop3 = "Nested Instance 2"
                },
                Prop6 = new List<string> { "String1", "String2" }
            };
            var test2 = new TestClass
            {
                Prop1 = 1,
                Prop2 = "First Instance",
                Prop3 = new string[] { "Item1", "Item2" },
                Prop4 = new List<string> { "String1", "String2" },
                Prop5 = "Additional Info",
                TestClassSecond1 = new List<TestClassSecond>
            {
                new TestClassSecond
                {
                    Prop1 = 10,
                    Prop2 = "Second Instance Prop2",
                    Prop3 = "Nested Instance 1"
                }
            },
                TestClass2 = new TestClassSecond
                {
                    Prop1 = 20,
                    Prop2 = "Another Prop2",
                    Prop3 = "Nested Instance 2"
                },
                Prop6 = new List<string> { "String2", "String1" }
            };

            var hash1 = test1.GetHashSum();
            var hash2 = test2.GetHashSum();

            Assert.Equal(hash1, hash2);
        }
    }
}
