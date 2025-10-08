# ComparableLibrary
This solution was created to demonstrate how to compare two objects by value. For this, the `IGeneralComparable` interface was created, which provides a way to generate a hash from significant fields, allowing objects to be compared using this hash.

The project was implemented in C# using the `System.Data.HashFunction.MurmurHash` NuGet package for hashing with the MurmurHash3 algorithm.

## Installation
```
dotnet add package ComparableLibrary --version 1.1.1
```

# Features
The IGeneralComparable interface allows you to compare multiple objects by generating a hash from significant fields.
To generate a hash, your classes need to implement IGeneralComparable and mark the properties that need to be compared using the ComparablePropertyAttribute.
ComparablePropertyAttribute properties:
- Name - needs to be set to compare objects of different types (default: the property’s name).
- Order - defines the sequence number for ordering properties across multiple classes to ensure they are processed in the same order. Properties with this attribute are processed first, followed by those without it.
- Type - used only for collections. It distinguishes between ordered and unordered collections, and the hash structure differs accordingly.

A base class, GeneralComparable, also exists and has a property for storing the hash: HashSum.

To use it, implement the IGeneralComparable interface in your class and apply the attribute to properties that are important for calculating the hash.

See more on [GitHub](https://github.com/tereschvlad/ComparableSolution)

