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

        [ComparableProperty(type: ComparableCollectionType.Ordered)]
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

    public sealed class Person : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public int Id { get; set; }

        [ComparableProperty(Order = 1)]
        public string Name { get; set; }
    }

    public sealed class TagSet : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Unordered)]
        public List<string> Tags { get; set; } = new();
    }

    public sealed class Sequence : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Ordered)]
        public List<string> Items { get; set; } = new();
    }

    public sealed class NameMappedA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")]
        public string KeyAA { get; set; }
    }

    public sealed class NameMappedB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")]
        public string KeyBB { get; set; }
    }

    public sealed class Numbers : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public int I { get; set; }

        [ComparableProperty(Order = 1)]
        public double D { get; set; }
    }

    public sealed class OrderableItem : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public int Id { get; set; }

        [ComparableProperty(Order = 1)]
        public string Name { get; set; }
    }

    public sealed class OrderedComplexList : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Ordered)]
        public List<OrderableItem> Items { get; set; } = new();
    }

    public class NullableScalars : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public int? I { get; set; }

        [ComparableProperty(Order = 1)]
        public string S { get; set; }

        [ComparableProperty(Order = 2)]
        public DateTime? Dt { get; set; }
    }

    public class ParentWithChild : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public Child ChildA { get; set; }

        public class Child : IGeneralComparable
        {
            [ComparableProperty(Order = 0)]
            public string Name { get; set; }

            [ComparableProperty(Order = 1)]
            public int? Age { get; set; }
        }
    }

    public class UnorderedNullableList : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Unordered)]
        public List<string> Items { get; set; } = new();
    }

    public class OrderedNullableComplexList : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Ordered)]
        public List<Item> Items { get; set; } = new();

        public class Item : IGeneralComparable
        {
            [ComparableProperty(Order = 0)]
            public int? Id { get; set; }

            [ComparableProperty(Order = 1)]
            public string Name { get; set; }
        }
    }

    // Inheritance hierarchy with public types
    public class BaseModel : IGeneralComparable
    {
        [ComparableProperty(Order = 0)]
        public int? Id { get; set; }
    }

    public class DerivedModel : BaseModel
    {
        [ComparableProperty(Order = 1)]
        public string Extra { get; set; }
    }

    // Cross-type mapping using ComparableProperty.Name with nullables
    public class NameMappedAA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")]
        public string KeyA { get; set; }
    }

    public class NameMappedBB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")]
        public string KeyB { get; set; }
    }
}
