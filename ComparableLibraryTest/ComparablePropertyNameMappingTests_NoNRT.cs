using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComparableLibrary;
using ComparableLibrary.Utils;
using static ComparableLibraryTest.ParentWithChild;

namespace ComparableLibraryTest
{
    public class ComparablePropertyNameMappingTests_NoNRT
    {
        [Fact]
        public void NameMapping_Simple_AliasesMatch()
        {
            var a = new AliasA { KeyA = "X" };
            var b = new AliasB { KeyB = "X" };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_Simple_DifferentValues_NotEqual()
        {
            var a = new AliasA { KeyA = "X" };
            var b = new AliasB { KeyB = "Y" };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_CaseSensitive_DifferentCase_NotEqual()
        {
            var a = new CaseAliasUpper { Value = "v" }; // Name = "Key"
            var b = new CaseAliasLower { Value = "v" }; // Name = "key"
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_NestedObject_AliasesMatch()
        {
            var a = new ParentA { Child = new ChildA { Code = "ABC" } };
            var b = new ParentB { Kid = new ChildB { CodeX = "ABC" } };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_NestedObject_NullVsValue_NotEqual()
        {
            var a = new ParentA { Child = null };
            var b = new ParentB { Kid = new ChildB { CodeX = "ABC" } };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_UnorderedCollections_AliasesMatchRegardlessOfOrder()
        {
            var a = new TagWordsA { Words = new() { "a", "b", null, "a" } };
            var b = new TagWordsB { Tags = new() { "b", "a", "a", null } };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_OrderedCollections_AliasesRespectOrder()
        {
            var a = new SequenceA { Seq = new() { "x", "y" } };
            var b = new SequenceB { Items = new() { "y", "x" } };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NameMapping_DuplicateNameInSameType_NotEqualToSingleProperty()
        {
            var two = new TwoPropsSameAlias { K1 = "A", K2 = "B" }; // both Name="K"
            var one = new OnePropAlias { K = "A" };
            Assert.NotEqual(two.GetHashSum(), one.GetHashSum());
        }

        [Fact]
        public void NameMapping_MultiProperty_CrossTypeEquality()
        {
            var m1 = new AB_A { A1 = "foo", B1 = "bar" };
            var m2 = new AB_B { X = "foo", Y = "bar" };
            Assert.Equal(m1.GetHashSum(), m2.GetHashSum());
        }

        [Fact]
        public void NameMapping_NullVsEmpty_NotEqual()
        {
            var a = new AliasA { KeyA = null };
            var b = new AliasB { KeyB = "" };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }
    }

    public class AliasA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")] public string KeyA { get; set; }
    }
    public class AliasB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")] public string KeyB { get; set; }
    }

    public class CaseAliasUpper : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")] public string Value { get; set; }
    }
    public class CaseAliasLower : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "key")] public string Value { get; set; }
    }

    public class ParentA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Child")] public ChildA Child { get; set; }
    }
    public class ChildA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "ChildKey")] public string Code { get; set; }
    }

    public class ParentB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Child")] public ChildB Kid { get; set; }
    }
    public class ChildB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "ChildKey")] public string CodeX { get; set; }
    }

    public class TagWordsA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Tags", Type = ComparableCollectionType.Unordered)]
        public List<string> Words { get; set; } = new();
    }
    public class TagWordsB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Tags", Type = ComparableCollectionType.Unordered)]
        public List<string> Tags { get; set; } = new();
    }

    public class SequenceA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Seq", Type = ComparableCollectionType.Ordered)]
        public List<string> Seq { get; set; } = new();
    }
    public class SequenceB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Seq", Type = ComparableCollectionType.Ordered)]
        public List<string> Items { get; set; } = new();
    }

    public class TwoPropsSameAlias : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "K")] public string K1 { get; set; }
        [ComparableProperty(Order = 1, Name = "K")] public string K2 { get; set; }
    }
    public class OnePropAlias : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "K")] public string K { get; set; }
    }

    public class AB_A : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "A")] public string A1 { get; set; }
        [ComparableProperty(Order = 1, Name = "B")] public string B1 { get; set; }
    }
    public class AB_B : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "A")] public string X { get; set; }
        [ComparableProperty(Order = 1, Name = "B")] public string Y { get; set; }
    }
}
