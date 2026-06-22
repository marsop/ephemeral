# Ephemeral

![Nuget](https://img.shields.io/nuget/dt/ephemeral?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/v/ephemeral?style=for-the-badge)

![](docs/img/EphemeralLogoCropped.png)

C# Library to handle intervals (composite start and end)

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

## FullInterval Operations

These are additional graphical representation for operations specific to `FullInterval` and basic measured intervals.

### `Shift()`
Shifts both the start and end of the interval by a given offset.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])

A.Shift(2)      ->                  [=======]  ([4, 6])
A.Shift(-1)     ->      [=======]              ([1, 3])
```

### `ShiftStart()`
Shifts the start of the interval by a given offset, while keeping the end fixed.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:             [===========]          (A = [3, 6])

A.ShiftStart(1) ->          [=======]          ([4, 6])
A.ShiftStart(-1)->  [===============]          ([2, 6])
```

### `ShiftEnd()`
Shifts the end of the interval by a given offset, while keeping the start fixed.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])

A.ShiftEnd(2)   ->  [===============]          ([2, 6])
A.ShiftEnd(-1)  ->  [===]                      ([2, 3])
```

### `LengthOfIntersect()`
Computes the length (or duration) of the intersection of two intervals.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.LengthOfIntersect(B) ->   2                  ([4, 6] length is 2)
```

## Disjoint Interval Set Operations

These are graphical representations for operations on interval collections (`DisjointIntervalSet`). A Disjoint Interval Set maintains a collection of non-overlapping intervals, automatically merging contiguous or overlapping ones.

### `Consolidate()`
Joins adjacent or overlapping intervals in the set into the minimum amount of intervals.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set A:              [===]   [===]              ([2, 3], [4, 5])
A.Consolidate() ->  [===]   [===]             (Assuming open/closed bounds prevent joining)

Set B:              [===](==============]      ([2, 3], (3, 7])
B.Consolidate() ->  [===================]      ([2, 7])
```

### `Join()`
Adds a new interval or another set into the collection, merging as necessary to maintain disjointness.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set A:              [===]           [===]      ([2, 3], [6, 7])
Interval B:                 [===]              (B = [4, 5])

A.Join(B)       ->  [===]   [===]   [===]      (Adds without merging)

Interval C:                 [===========]      (C = [4, 7])
A.Join(C)       ->  [===]   [===========]      (Overlaps [6, 7], merges into [4, 7])
```

### `Intersect()`
Finds the common intervals between the set and an interval (or another set).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set A:              [===]           [===]      ([2, 3], [6, 7])
Interval B:         [===================]      (B = [2, 7])

A.Intersect(B)  ->  [===]           [===]      ([2, 3], [6, 7])

Interval C:                 [=======]          (C = [4, 6])
A.Intersect(C)  ->                  []         (Intersection at [6, 6] or empty if open)
```

### `Covers()`
Checks if the entire interval or point is covered by any interval in the set.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set A:              [=======]       [===]      ([2, 4], [6, 7])
Interval B:             [===]                  (B = [3, 4])
A.Covers(B)     ->  true

Interval C:                 [===========]      (C = [4, 7])
A.Covers(C)     ->  false                      (Gap between 4 and 6)
```

### `GetBoundingInterval()`
Returns a single interval spanning from the earliest start to the latest end in the set.
```text
Real Line:         0---1---2---3---4---5---6---7---8---9---10

Set A:                     [===]           [===]      ([2, 3], [6, 7])
A.GetBoundingInterval() -> [===================]      ([2, 7])
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
