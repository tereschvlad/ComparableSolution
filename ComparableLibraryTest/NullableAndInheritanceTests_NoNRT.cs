using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComparableLibrary;
using ComparableLibrary.Utils;

namespace ComparableLibraryTest
{
    public class NullableAndInheritanceTests_NoNRT
    {
        [Fact]
        public void NullableScalars_SameValues_Equals()
        {
            var a = new NrtScalarsModel { I = null, S = null, Dt = null };
            var b = new NrtScalarsModel { I = null, S = null, Dt = null };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NullableScalars_NullVsValue_NotEqual()
        {
            var a = new NrtScalarsModel { I = null, S = "x", Dt = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc) };
            var b = new NrtScalarsModel { I = 0, S = "x", Dt = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc) };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void NullableScalars_NullStringVsEmptyString_NotEqual()
        {
            var a = new NrtScalarsModel { I = 1, S = null, Dt = null };
            var b = new NrtScalarsModel { I = 1, S = "", Dt = null };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void ParentWithChild_NullChildVsSameChild_NotEqual()
        {
            var withNull = new NrtParentChildModel { Child1 = null };
            var withChild = new NrtParentChildModel { Child1 = new NrtParentChildModel.Child { Name = "Vlad", Age = 30 } };
            Assert.NotEqual(withNull.GetHashSum(), withChild.GetHashSum());
        }

        [Fact]
        public void ParentWithChild_SameChildValues_Equals()
        {
            var a = new NrtParentChildModel { Child1 = new NrtParentChildModel.Child { Name = "A", Age = null } };
            var b = new NrtParentChildModel { Child1 = new NrtParentChildModel.Child { Name = "A", Age = null } };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void ParentWithChild_DifferentChildValues_NotEqual()
        {
            var a = new NrtParentChildModel { Child1 = new NrtParentChildModel.Child { Name = "A", Age = 10 } };
            var b = new NrtParentChildModel { Child1 = new NrtParentChildModel.Child { Name = "A", Age = 11 } };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void UnorderedNullableList_SameMultisetWithNulls_Equals()
        {
            var a = new NrtUnorderedStringList { Items = new() { "x", null, "y", "x" } };
            var b = new NrtUnorderedStringList { Items = new() { "y", "x", null, "x" } };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void UnorderedNullableList_NullElementMatters_NotEqual()
        {
            var withNull = new NrtUnorderedStringList { Items = new() { "x", null } };
            var without = new NrtUnorderedStringList { Items = new() { "x" } };
            Assert.NotEqual(withNull.GetHashSum(), without.GetHashSum());
        }

        [Fact]
        public void OrderedNullableComplexList_OrderMatters_NotEqual()
        {
            var a = new NrtOrderedComplexList
            {
                Items = new()
                {
                    new NrtOrderedComplexList.Item { Id = 1, Name = "A" },
                    null,
                    new NrtOrderedComplexList.Item { Id = 2, Name = null },
                }
            };

            var b = new NrtOrderedComplexList
            {
                Items = new()
                {
                    null,
                    new NrtOrderedComplexList.Item { Id = 1, Name = "A" },
                    new NrtOrderedComplexList.Item { Id = 2, Name = null },
                }
            };

            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void OrderedNullableComplexList_SameSequence_Equals()
        {
            var a = new NrtOrderedComplexList
            {
                Items = new()
                {
                    new NrtOrderedComplexList.Item { Id = 1, Name = "A" },
                    null,
                    new NrtOrderedComplexList.Item { Id = 2, Name = null },
                }
            };

            var b = new NrtOrderedComplexList
            {
                Items = new()
                {
                    new NrtOrderedComplexList.Item { Id = 1, Name = "A" },
                    null,
                    new NrtOrderedComplexList.Item { Id = 2, Name = null },
                }
            };

            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void Inheritance_Public_DerivedAddsField_NotEqualWhenDifferent()
        {
            NrtBaseModel a = new NrtDerivedModel { Id = 5, Extra = "x" };
            NrtBaseModel b = new NrtDerivedModel { Id = 5, Extra = "y" };
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void Inheritance_Public_BasePartOnly_EqualsWhenSame()
        {
            NrtBaseModel a = new NrtDerivedModel { Id = 42, Extra = null };
            NrtBaseModel b = new NrtDerivedModel { Id = 42, Extra = null };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void Inheritance_Public_DerivedVsBaseWithSameBaseValues_NotEqual()
        {
            var baseOnly = new NrtBaseModel { Id = 7 };
            var withDerived = new NrtDerivedModel { Id = 7, Extra = "present" };
            Assert.NotEqual(baseOnly.GetHashSum(), withDerived.GetHashSum());
        }

        // keep the alias-mapping sample unchanged
        [Fact]
        public void NameMapping_WithNullables_CrossTypeEquality()
        {
            var a = new NameAliasA { KeyA = null };
            var b = new NameAliasB { KeyB = null };
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }
    }

    public class NrtScalarsModel : IGeneralComparable
    {
        [ComparableProperty(Order = 0)] public int? I { get; set; }
        [ComparableProperty(Order = 1)] public string S { get; set; }
        [ComparableProperty(Order = 2)] public DateTime? Dt { get; set; }
    }

    public class NrtParentChildModel : IGeneralComparable
    {
        [ComparableProperty(Order = 0)] 
        public Child Child1 { get; set; }

        public class Child : IGeneralComparable
        {
            [ComparableProperty(Order = 0)] public string Name { get; set; }
            [ComparableProperty(Order = 1)] public int? Age { get; set; }
        }
    }

    public class NrtUnorderedStringList : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Unordered)]
        public List<string> Items { get; set; } = new();
    }

    public class NrtOrderedComplexList : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Type = ComparableCollectionType.Ordered)]
        public List<Item> Items { get; set; } = new();

        public class Item : IGeneralComparable
        {
            [ComparableProperty(Order = 0)] public int? Id { get; set; }
            [ComparableProperty(Order = 1)] public string Name { get; set; }
        }
    }

    public class NrtBaseModel : IGeneralComparable
    {
        [ComparableProperty(Order = 0)] public int? Id { get; set; }
    }

    public class NrtDerivedModel : NrtBaseModel
    {
        [ComparableProperty(Order = 1)] public string Extra { get; set; }
    }

    // unchanged helper types for name-mapping
    public class NameAliasA : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")] public string KeyA { get; set; }
    }
    public class NameAliasB : IGeneralComparable
    {
        [ComparableProperty(Order = 0, Name = "Key")] public string KeyB { get; set; }
    }

}
