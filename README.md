# ComparableLibrary
This solution was created to demonstrate how to compare two objects by value. For this, the `IGeneralComparable` interface was created, which provides a way to generate a hash from significant fields, allowing objects to be compared using this hash.

The project was implemented in C# using the `System.Data.HashFunction.MurmurHash` NuGet package for hashing with the MurmurHash3 algorithm.

## Core Idea

- Use the existing **`IGeneralComparable`** marker interface (already in this repo).
- Annotate only those properties that should affect comparison with **`ComparablePropertyAttribute`**.
- Optionally inherit from **`GeneralComparable`** if you want the `HashSum` convenience property; otherwise call the extension directly.
- Compute the hash with the provided extension: `GeneralComparableExtensions.GetHashSum(this IGeneralComparable)`.

## API in this repo

- `IGeneralComparable` — marker interface.
- `ComparablePropertyAttribute` (properties: `Name`, `Order`, `Type`).
- `GeneralComparable` — base class that caches a computed `HashSum`.
- `GeneralComparableExtensions.GetHashSum(IGeneralComparable)` — extension method that:
  - walks public instance properties with `[ComparableProperty]`,
  - normalizes primitives/strings/decimals (culture‑invariant),
  - supports `bool`, floating‑point "R" round‑trip formatting,
  - understands **collections** via `ComparableCollectionType` (`Ordered`/`Unordered`),
  - folds nested `IGeneralComparable` items by their own hash,
  - returns a stable MurmurHash3 hex string.

## Usage Examples (taken from tests in this repo)

### 1) Simple comparable (uses your *SampleComparable*)

```csharp
using ComparableLibrary;
using ComparableLibrary.Utils;

public class SampleComparable : GeneralComparable
{
    [ComparableProperty(2)]
    public string TextProperty { get; set; }

    [ComparableProperty(1)]
    public int NumberProperty { get; set; }

    [ComparableProperty(4)]
    public decimal Price { get; set; }

    [ComparableProperty(3)]
    public DateTime CreatedAt { get; set; }
}

var a = new SampleComparable { TextProperty = "A", NumberProperty = 10, Price = 12.34m, CreatedAt = DateTime.UtcNow };
var b = new SampleComparable { TextProperty = "A", NumberProperty = 10, Price = 12.34m, CreatedAt = a.CreatedAt };

// Either use the base class property:
string hashA = a.HashSum;

// Or the extension method:
string hashB = b.GetHashSum();

bool equalByValue = hashA == hashB; // true when significant properties are equal
```

### 2) Cross‑type mapping by `Name` (uses your *NameMappedAA* and *NameMappedBB*)

```csharp
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

// Instances with the same logical "Key" produce identical hashes
var x = new NameMappedAA { KeyA = "42" };
var y = new NameMappedBB { KeyB = "42" };
bool same = x.GetHashSum() == y.GetHashSum(); // true
```

### 3) Collections (ordered vs unordered)

```csharp
public class Basket : IGeneralComparable
{
    // Order matters (sequence is part of the hash)
    [ComparableProperty(Order = 1, Type = ComparableCollectionType.Ordered)]
    public List<string> Lines { get; set; } = new();

    // Order does not matter (set semantics)
    [ComparableProperty(Order = 2, Type = ComparableCollectionType.Unordered)]
    public HashSet<int> Tags { get; set; } = new();
}
```

## Behavior Notes

- `null` vs empty: encoded distinctly in the hash.
- Strings/numbers/dates: normalized using invariant culture; `float`/`double` use round‑trip (`"R"`) formatting.
- Collections: choose `Type` to reflect semantics; unordered folding is position‑independent.
- Nested models: if a property type implements `IGeneralComparable`, its `GetHashSum()` is folded into the parent hash.

Users can view examples of library usage in ComparableLibraryTest

