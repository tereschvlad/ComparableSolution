# ComparableLibrary

ComparableLibrary is a .NET library for comparing objects by selected property values. Classes implement the `IGeneralComparable` interface and mark significant properties with `ComparablePropertyAttribute`. The library generates a hash from these properties, allowing objects to be compared efficiently.

The library is implemented in C# and uses `XxHash128` from the `System.IO.Hashing` NuGet package.

## Installation

```bash
dotnet add package ComparableLibrary --version 2.0.0
```

## Features

The `IGeneralComparable` interface allows objects to be compared using hashes generated from their significant properties.

To include a property in the comparison, mark it with `ComparablePropertyAttribute`.

`ComparablePropertyAttribute` supports the following properties:

- `Name` — specifies the logical property name. Use the same name to compare corresponding properties from different object types. By default, the original property name is used.
- `Order` — defines the order in which properties are processed. Corresponding properties in different object types should use the same order.
- `Type` — defines how collections are processed:
  - `Ordered` — the order of collection items affects the hash.
  - `Unordered` — the order of collection items does not affect the hash.

Properties without `ComparablePropertyAttribute` are not included in the hash.

The optional `GeneralComparable` base class implements `IGeneralComparable` and provides the cached `HashSum` property.

You can also implement `IGeneralComparable` directly and generate the hash using the `GetHashSum()` extension method.

## Important

`XxHash128` is a fast, non-cryptographic hashing algorithm. It is suitable for object comparison and change detection, but it should not be used for passwords, digital signatures, or security checks.

Earlier versions of ComparableLibrary used MurmurHash3. Hashes generated with `XxHash128` are different and are not compatible with hashes from previous versions. Regenerate any hashes stored in a database, cache, or file after upgrading.

See more in the [ComparableLibrary GitHub repository](https://github.com/tereschvlad/ComparableSolution).

## License

ComparableLibrary is available under the [MIT License](https://github.com/tereschvlad/ComparableSolution/blob/master/LICENSE).
