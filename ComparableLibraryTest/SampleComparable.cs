using ComparableLibrary;

namespace ComparableLibraryTest
{
    public class SampleComparable : GeneralComparable
    {
        [ComparableProperty(2)]
        public string TextProperty { get; set; }

        [ComparableProperty(1)]
        public int NumberProperty { get; set; }

        [ComparableProperty(4)]
        public decimal Price { get; set; }

        [ComparableProperty(3)]
        public bool IsEnabled { get; set; }

        [ComparableProperty(6)]
        public DateTime Timestamp { get; set; }

        [ComparableProperty(5)]
        public TimeSpan Duration { get; set; }

        [ComparableProperty(7)]
        public int? OptionalNumber { get; set; }

        [ComparableProperty(9, type: ComparableCollectionType.Unordered)]
        public List<string> UnorderedNames { get; set; }

        [ComparableProperty(8)]
        public List<string> OrderedNames { get; set; }

        [ComparableProperty(10)]
        public NestedComparable NestedObject { get; set; }

        public double DoubleProperty { get; set; }
        public string SimpleText { get; set; }

        [ComparableProperty(type: ComparableCollectionType.Unordered)]
        public List<AlternativeComparable> AlternativeOrderList { get; set; }

        [ComparableProperty(type: ComparableCollectionType.Unordered)]
        public List<AlternativeComparable> AlternativeUnorderList { get; set; }
    }

    public class NestedComparable : GeneralComparable
    {
        [ComparableProperty(2)]
        public int NestedNumber { get; set; }

        [ComparableProperty(1)]
        public string NestedText { get; set; }
    }

    public class AlternativeComparable : GeneralComparable
    {
        [ComparableProperty(2, "NestedNumber")]
        public int CustomNumber { get; set; }

        [ComparableProperty(1, "NestedText")]
        public string CustomText { get; set; }
    }
}
