<link rel="shortcut icon" type="image/x-icon" href="{{ site.baseurl }}/img/favicon.ico">

# ephemeral

![](img/EphemeralLogoCropped.png)

C# Library to handle intervals (composite start and end)

- Targets `.NET Standard 2.0` for wide compatibility.
- Support for generic types in intervals (e.g. `DateTimeOffset`, `int`, `double`).
- Support for open and closed intervals.
- Support for common operations like Covers(), Intersect(), Join(), etc. via extension methods.
- Support for disjoint collections of intervals via `DisjointIntervalSet`.

A new version (1.0) of this library including generic intervals has been released. See more info at [Breaking Changes v1](changes-v1.md)

Please check all the available options in the [Methods Section](methods.md).

This documentation is hosted at [albertogregorio.com/ephemeral](albertogregorio.com/ephemeral).

## Generics in Version 1

Starting from version 1, Ephemeral introduces generic intervals allowing boundaries of any type that implements `IComparable<TBoundary>`. It defines the interfaces `IBasicInterval<TBoundary>` and `IInterval<TBoundary, TLength>`, as well as base implementations `BasicInterval<TBoundary>` and `FullInterval<TBoundary, TLength>`.

To facilitate migration, Ephemeral provides several utility classes corresponding to standard types:
- `DateTimeOffsetInterval` (for `DateTimeOffset`)
- `IntInterval` (for `int`)
- `DoubleInterval` (for `double`)
- `DateOnlyInterval` and `TimeOnlyInterval` (available in .NET 6+ targets)

These utility classes extend `FullInterval` to offer standard duration logic right out of the box, similar to how `Interval` worked in older versions.

## Examples

### Time Interval Example

```csharp
var now = DateTimeOffset.UtcNow;
var yesterday = DateTimeOffsetInterval.CreateOpen(now.AddDays(-1), now);
var today = yesterday.Shift(TimeSpan.FromDays(1));

yesterday.Intersects(today); // returns false (because they are open/closed contiguously)
```

### Integer Interval Example

```csharp
var firstTen = IntInterval.CreateClosed(1, 10);
var nextTen = IntInterval.CreateClosed(11, 20);

firstTen.IsContiguouslyFollowedBy(nextTen); // returns false if both are closed.
```

### Interval Collection Example

```csharp
var lengthOperator = DateTimeOffsetStandardLengthOperator.Instance;
var collection = new DisjointIntervalSet<DateTimeOffset, TimeSpan>(lengthOperator);
collection.Add(yesterday);
collection.Add(today);

collection.Start == yesterday.Start; // true
collection.End == today.End; // true

var collection2 = yesterday.Union(today, lengthOperator);

var consolidatedCollection = collection2.Consolidate();
collection2.Count(); // 2
consolidatedCollection.Count(); // 1
```
