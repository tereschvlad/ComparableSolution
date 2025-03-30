# ComparableSolution
This solution was created to demonstrate how to compare two objects by value. For this was created general class GeneralComparable wich give an opportunity to get a hash from significant fields forwarding this hash can be comparable.

Project was realized on C#. Algoritm for hashing MurmurHash3, used System.Data.HashFunction.MurmurHash nuget

# Opportunities

The class GeneralComparable gives an opportunities for comparing several objects by hash of significant fields. 
For getting hash need inheritance your classes, set marks on properties wich need compare adding attribute ComparablePropertyAttribute.
ComparablePropertyAttribute properties:
- Name - need to set for compare several objects different type (default set up the name of property)
- Order - need to set up sequence number of property for put up properties in the same order in several classes (firstly processed properties wich have this next without)
- Type - using only for collection. By ordered and unordered collections. Hash struct differently for ordered and unordered collections.

GeneralComparable have property for saving hash HashSum.

## Example how working it working. Class SampleComparable is inheritanced GeneralComparable. More examples can see in project for testing ComparableLibraryTest
```C#
public void HashSum_Should_Be_Same_For_Identical_Objects()
{
    var obj1 = new SampleComparable
    {
        TextProperty = "Test",
        NumberProperty = 15,
        OrderedNames = new List<string> { "A", "B", "C" },
        UnorderedNames = new List<string> { "X", "Y", "Z" },
        NestedObject = new NestedComparable { NestedText = "Inside", NestedNumber = 123 },
        AlternativeOrderList = new List<AlternativeComparable>()
        {
            new AlternativeComparable { CustomText = "Test", CustomNumber = 100 },
            new AlternativeComparable { CustomText = "Test2", CustomNumber = 101 }
        },
        AlternativeUnorderList = new List<AlternativeComparable>()
        {
            new AlternativeComparable { CustomText = "Test3", CustomNumber = 102 },
            new AlternativeComparable { CustomText = "Test4", CustomNumber = 103 }
        }
    };

    var obj2 = new SampleComparable
    {
        TextProperty = "Test",
        NumberProperty = 15,
        OrderedNames = new List<string> { "A", "B", "C" },
        UnorderedNames = new List<string> { "Z", "X", "Y" },
        NestedObject = new NestedComparable { NestedText = "Inside", NestedNumber = 123 },
        AlternativeOrderList = new List<AlternativeComparable>()
        {
            new AlternativeComparable { CustomText = "Test", CustomNumber = 100 },
            new AlternativeComparable { CustomText = "Test2", CustomNumber = 101 }
        },
        AlternativeUnorderList = new List<AlternativeComparable>()
        {
            new AlternativeComparable { CustomText = "Test4", CustomNumber = 103 },
            new AlternativeComparable { CustomText = "Test3", CustomNumber = 102 }
        }
    };

    Assert.Equal(obj1.HashSum, obj2.HashSum);
}

Assert.Equal(obj1.HashSum, obj2.HashSum);

```

