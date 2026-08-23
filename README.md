# ComparableLibrary

ComparableLibrary is a .NET library that generates deterministic hashes from selected object properties. It is useful for comparing complex objects, nested models, and collections without writing custom comparison logic for every type.

Only properties marked with `ComparablePropertyAttribute` are included in the hash. The current implementation uses `XxHash128` from the `System.IO.Hashing` package and returns a 128-bit hash as a 32-character hexadecimal string.

> `XxHash128` is a fast, non-cryptographic hashing algorithm. It is suitable for data comparison and change detection, but not for passwords, digital signatures, or tamper protection. Hash collisions are possible, so verify the original values when exact equality must be guaranteed.

## Core API

- `IGeneralComparable` — marker interface for comparable models.
- `ComparablePropertyAttribute` — marks a property for hashing and supports `Name`, `Order`, and `Type` settings.
- `GeneralComparable` — optional base class that calculates and caches the `HashSum` value.
- `GeneralComparableExtensions.GetHashSum(IGeneralComparable)` — generates a hash directly from any model that implements `IGeneralComparable`.

The hashing logic:

- reads public instance properties marked with `[ComparableProperty]`;
- processes properties according to their configured `Order`;
- formats supported values consistently;
- supports ordered and unordered collections;
- includes nested `IGeneralComparable` models;
- generates the final hash with `XxHash128`.

## Usage

### Simple object comparison

```csharp
using ComparableLibrary;
using ComparableLibrary.Utils;

public class SampleComparable : GeneralComparable
{
    [ComparableProperty(1)]
    public int Number { get; set; }

    [ComparableProperty(2)]
    public string Text { get; set; }

    [ComparableProperty(3)]
    public decimal Price { get; set; }

    [ComparableProperty(4)]
    public DateTime CreatedAt { get; set; }
}

var createdAt = DateTime.UtcNow;

var first = new SampleComparable
{
    Number = 10,
    Text = "A",
    Price = 12.34m,
    CreatedAt = createdAt
};

var second = new SampleComparable
{
    Number = 10,
    Text = "A",
    Price = 12.34m,
    CreatedAt = createdAt
};

string firstHash = first.HashSum;
string secondHash = second.GetHashSum();

bool haveSameComparableValues = firstHash == secondHash; // true
```

### Map properties from different types

Use `Name` when different model properties represent the same logical value.

```csharp
public class FirstModel : IGeneralComparable
{
    [ComparableProperty(Order = 1, Name = "Key")]
    public string FirstKey { get; set; }
}

public class SecondModel : IGeneralComparable
{
    [ComparableProperty(Order = 1, Name = "Key")]
    public string SecondKey { get; set; }
}

var first = new FirstModel { FirstKey = "42" };
var second = new SecondModel { SecondKey = "42" };

bool haveSameComparableValues =
    first.GetHashSum() == second.GetHashSum(); // true
```

### Compare collections

```csharp
public class Basket : IGeneralComparable
{
    // Item order affects the hash.
    [ComparableProperty(
        Order = 1,
        Type = ComparableCollectionType.Ordered)]
    public List<string> Lines { get; set; } = new();

    // Item order does not affect the hash.
    [ComparableProperty(
        Order = 2,
        Type = ComparableCollectionType.Unordered)]
    public HashSet<int> Tags { get; set; } = new();
}
```

For unordered collections, values are sorted before hashing. Duplicate values are still included.

## Behavior

- `null` and an empty value produce different hash input.
- Numbers use invariant-culture formatting; `float` and `double` use round-trip formatting.
- `DateTime` and `TimeSpan` values are represented by ticks.
- Arrays and generic `IEnumerable<T>` collections are supported.
- Nested models that implement `IGeneralComparable` contribute their own hash.
- `GetHashSum()` returns `null` when no properties are marked with `[ComparableProperty]`.
- `GeneralComparable.HashSum` is cached after its first calculation. Initialize the object before reading it, because later property changes do not invalidate the cached value.

## Migrating from earlier versions

Earlier versions used MurmurHash3. Hashes generated with `XxHash128` are not compatible with the old values. If hashes are stored in a database, cache, or file, regenerate them after upgrading.

More usage examples are available in the `ComparableLibraryTest` project.

## License

This project is licensed under the [MIT License](LICENSE).
