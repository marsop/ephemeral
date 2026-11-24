# Intersect()

The `Intersect()` extension method computes the intersection of an `IDisjointIntervalSet<TBoundary, TLength>` with a target interval, returning a new `DisjointIntervalSet` containing only the overlapping segments.

## Method Signature

```csharp
public static DisjointIntervalSet<TBoundary, TLength> Intersect<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set,
    IBasicInterval<TBoundary> interval)
    where TBoundary : notnull, IComparable<TBoundary>;
```

## How It Works

`Intersect()` evaluates the target interval against each interval in the set. For every interval in the set that overlaps with the target, the overlapping segment is calculated (using `IBasicInterval.Intersect()`). The non-empty intersection segments are collected and returned as a new `DisjointIntervalSet<TBoundary, TLength>`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:              [=======]       [=======]  ([2, 4] and [6, 8])
Interval A:                 [===============]  (A = [3, 7])

S.Intersect(A)  ->          [===]   [===]      ([3, 4] and [6, 7])
```

## Example

```csharp
using System;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;

// Set with two disjoint intervals: [2, 4] and [6, 8]
var set = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(2, 4),
    IntInterval.CreateClosed(6, 8)
);

// Target interval spanning across both intervals: [3, 7]
var target = IntInterval.CreateClosed(3, 7);

var intersectionSet = set.Intersect(target);

Console.WriteLine(intersectionSet.Count); // 2
Console.WriteLine(intersectionSet[0]);   // [3, 4]
Console.WriteLine(intersectionSet[1]);   // [6, 7]
```

## Behavior Details

- If the target interval does not overlap any interval in the set, the result is an empty `DisjointIntervalSet`.
- The boundaries (open or closed) of each resulting segment reflect the intersection rules of the respective intervals.
- The returned set preserves the `LengthOperator` of the original set.

## See Also

- [Interval Sets Overview](index.md)
- [Methods Overview](../methods.md)
