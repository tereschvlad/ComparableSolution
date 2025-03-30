# ComparableSolution
This solution was created to demonstrate how to compare two objects by value. For this was created general class GeneralComparable which provides a way to generate a hash from significant fields allowing objects to be compared using this hash.

The project was implemented in C# using the System.Data.HashFunction.MurmurHash NuGet package for hashing with the MurmurHash3 algorithm.

# Features

The class GeneralComparable provides an opportunity to compare multiple objects by generating a hash from significant fields.
To generate a hash, your classes need to inherit from GeneralComparable and mark properties that need to be compared using the ComparablePropertyAttribute.
ComparablePropertyAttribute properties:
- Name - Needs to be set to compare objects of different types (default: uses the name of the property).
- Order - Defines the sequence number for ordering properties in multiple classes to ensure they are processed in the same order. Properties with this attribute are processed first, followed by those without it.
- Type - Used only for collections. It distinguishes between ordered and unordered collections, and the hash structure differs accordingly.

GeneralComparable has property for saving hash HashSum.

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

