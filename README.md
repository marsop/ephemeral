# Ephemeral

![Nuget](https://img.shields.io/nuget/dt/ephemeral?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/v/ephemeral?style=for-the-badge)

![](docs/img/EphemeralLogoCropped.png)

C# Library to handle intervals (composite start and end)

- Targets `.NET Standard 2.0` for wide compatibility.
- Support for generic types in intervals (e.g. `DateTimeOffset`, `int`, `double`).
- Support for open and closed intervals.
- Support for common operations like Covers(), Intersect(), Join(), etc. via extension methods.
- Support for disjoint collections of intervals via `DisjointIntervalSet`.
- Makes use of Optional to avoid returning `null` in many of the built-in operations.

The API-documentation is hosted at [albertogregorio.com/ephemeral](https://albertogregorio.com/ephemeral).

## Generics in Version 1

Starting from version 1, Ephemeral introduces generic intervals allowing boundaries of any type that implements `IComparable<TBoundary>`. It defines the interfaces `IBasicInterval<TBoundary>` and `IInterval<TBoundary, TLength>`, as well as base implementations `BasicInterval<TBoundary>` and `FullInterval<TBoundary, TLength>`.

To facilitate migration, Ephemeral provides several utility classes corresponding to standard types:
- `DateTimeOffsetInterval` (for `DateTimeOffset`)
- `IntInterval` (for `int`)
- `DoubleInterval` (for `double`)
- `DateOnlyInterval` and `TimeOnlyInterval` (available in .NET 6+ targets)

These utility classes extend `FullInterval` to offer standard duration logic right out of the box, similar to how `Interval` worked in older versions.

## Interval Operations

Here are graphical representations of common interval operations over the real line (using `DoubleInterval`).

### `Covers()`
Checks if an interval completely contains another interval (or point).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Covers(B)     ->  true
```

### `Intersects()` / `Intersect()`
Checks if two intervals overlap. `Intersect()` returns the overlapping interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Intersects(B) ->  true
A.Intersect(B)  ->          [=======]          ([4, 6])
```

### `Join()`
Merges two overlapping or contiguous intervals into a single interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Join(B)       ->  [=======================]  ([2, 8])
```

### `Subtract()`
Subtracts one interval from another, returning a disjoint set of intervals.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Subtract(B)   ->  [=======)       (=======]  ([2, 4) and (6, 8])
```

### `Union()`
Combines two intervals into a collection (`DisjointIntervalSet`).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                         [=======]  (B = [6, 8])

A.Union(B)      ->  [=======]       [=======]  ([2, 4] and [6, 8])
```

### `IsContiguouslyFollowedBy()`
Checks if one interval starts exactly where another ends (without overlap, typically open/closed).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======)                  (A = [2, 4))
Interval B:                 [=======]          (B = [4, 6])

A.IsContiguouslyFollowedBy(B) -> true
```

### `StartsBefore()`
Checks if an interval starts before another interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                 [=======]          (B = [4, 6])

A.StartsBefore(B) -> true
```

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
