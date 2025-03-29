using ComparableLibrary;

namespace ComparableLibraryTest
{
    public class TestClass : GeneralComparable
    {
        [ComparableProperty(Name = "Test1", Order = 2)]
        public int? Prop1 { get; set; }

        [ComparableProperty(Order = 3)]
        public string Prop2 { get; set; }

        [ComparableProperty(Name = "Test2")]
        public string[] Prop3 { get; set; }

        [ComparableProperty(Name = "Test3", Order = 2)]
        public List<string> Prop4 { get; set; }

        [ComparableProperty()]
        public string Prop5 { get; set; }

        [ComparableProperty(Order = 7)]
        public IEnumerable<TestClassSecond> TestClassSecond1 { get; set; }

        [ComparableProperty(Order = 1)]
        public TestClassSecond TestClass2 { get; set; }

        [ComparableProperty(Order = 8, Type = ComparableCollectionType.Unordered)]
        public List<string> Prop6 { get; set; }

        [ComparableProperty(Type = ComparableCollectionType.Unordered)]
        public int[] Prop7 { get; set; }

    }

    public class TestClassSecond : GeneralComparable
    {
        [ComparableProperty(Order = 2)]
        public int Prop1 { get; set; }

        public string Prop2 { get; set; }

        [ComparableProperty(Order = 1)]
        public string Prop3 { get; set; }
    }

    public class TestClassThird : TestClassSecond
    {
        [ComparableProperty()]
        public double Prop10 { get; set; }
    }
}
