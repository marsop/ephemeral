# Consolidate()

The `Consolidate()` extension method joins adjacent, contiguous intervals in an `IDisjointIntervalSet<TBoundary, TLength>` to produce a new set with the minimal number of intervals.

## Method Signature

```csharp
public static DisjointIntervalSet<TBoundary, TLength> Consolidate<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set)
    where TBoundary : notnull, IComparable<TBoundary>;
```

## How It Works

Although a `DisjointIntervalSet` guarantees that contained intervals do not overlap, it may contain intervals that are contiguously adjacent without overlapping (e.g. `[2, 4)` and `[4, 8]`). 

`Consolidate()` traverses the sorted intervals and merges any pair where the first interval is contiguously followed by the second (`cachedItem.IsContiguouslyFollowedBy(item)`), returning a consolidated `DisjointIntervalSet<TBoundary, TLength>`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:
  Interval 1:       [=======)                  ([2, 4))
  Interval 2:               [===============]  ([4, 8])

S.Consolidate() ->
                    [=======================]  ([2, 8])
```

## Example

```csharp
using System;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;

// Create two adjacent intervals: [2, 4) and [4, 8]
var intervalA = new BasicInterval<int>(2, 4, true, false); // [2, 4)
var intervalB = new BasicInterval<int>(4, 8, true, true);  // [4, 8]

var set = new DisjointIntervalSet<int, int>(lengthOperator, intervalA, intervalB);
Console.WriteLine(set.Count); // 2

var consolidated = set.Consolidate();
Console.WriteLine(consolidated.Count); // 1
Console.WriteLine(consolidated[0]);    // [2, 8]
```

## Behavior Details

- If the set is empty, `Consolidate()` returns a new empty set.
- If no intervals in the set are contiguous, `Consolidate()` returns a set containing all original intervals.
- The returned set uses the same `LengthOperator` as the original set.

## See Also

- [Interval Sets Overview](index.md)
- [Methods Overview](../methods.md)
