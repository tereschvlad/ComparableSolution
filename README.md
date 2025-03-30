# ComparableSolution
This soulution was realise for demonstrate how to solving issue how to compare two object by value. For this was created general class GeneralComparable wich give an opportunity to get a hash from significant fields forwarding this hash can be comparable.

Project was realized on C#. Algoritm for hashing MurmurHash3, used System.Data.HashFunction.MurmurHash nuget

# Opportunities

The class GeneralComparable gives an opportunities for comparing several objects by hash of significant fields. 
For getting hash need inheritance your classes, set marks on properties wich need compare adding attribute ComparablePropertyAttribute.
ComparablePropertyAttribute properties:
- Name - need to set for compare several objects different type (default set up the name of property)
- Order - need to set up sequence number of property for put up properties in the same order in several classes (firstly processed properties wich have this next without)
- Type - using only for collection. By ordered and unordered collections. Hash struct differently for ordered and unordered collections.

GeneralComparable have property for saving hash HashSum.

How to work with GeneralComparable class presented in testing project ComparableLibraryTest in GeneralComparableTest.



