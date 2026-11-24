# Join()

The `Join()` extension method merges an interval or another interval set into an existing `IDisjointIntervalSet<TBoundary, TLength>`. Any overlapping or contiguous intervals are combined together, preserving the disjointness invariant.

## Method Signatures

```csharp
// Overload 1: Join a single interval into the set
public static DisjointIntervalSet<TBoundary, TLength> Join<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set,
    IBasicInterval<TBoundary> interval)
    where TBoundary : notnull, IComparable<TBoundary>;

// Overload 2: Join another interval set into the set
public static DisjointIntervalSet<TBoundary, TLength> Join<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set,
    IDisjointIntervalSet<TBoundary, TLength> other)
    where TBoundary : notnull, IComparable<TBoundary>;
```

## How It Works

### Joining an Interval (`Join(IBasicInterval)`)

When joining an interval:
- Any existing intervals that **do not overlap** with the new interval are preserved as-is.
- Any intervals that **overlap** with the new interval are merged together with it into a single enlarged interval.
- If the new interval is completely disjoint from all existing intervals, it is simply added to the set.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:              [=======]                  (S = {[2, 4]})
Interval A:                 [===============]  (A = [4, 8])

S.Join(A)       ->  [=======================]  ({[2, 8]})
```

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:              [=======]                  (S = {[2, 4]})
Interval B:                         [=======]  (B = [6, 8])

S.Join(B)       ->  [=======]       [=======]  ({[2, 4], [6, 8]})
```

### Joining an Interval Set (`Join(IDisjointIntervalSet)`)

When joining another interval set:
1. The current set is first consolidated.
2. Each interval from the other set is joined sequentially into the accumulator.
3. The resulting set combines all intervals from both sets, merging any overlapping or contiguous segments.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S1:             [=======]                  (S1 = {[2, 4]})
Set S2:                     [=======]          (S2 = {[4, 6]})

S1.Join(S2)     ->  [===============]          ({[2, 6]})
```

## Example

```csharp
using System;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;

// Initial set: [2, 4]
var set = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(2, 4)
);

// 1. Join an overlapping interval [3, 7] -> results in [2, 7]
var singleJoined = set.Join(IntInterval.CreateClosed(3, 7));
Console.WriteLine(singleJoined.Count); // 1
Console.WriteLine(singleJoined[0]);   // [2, 7]

// 2. Join a disjoint interval [9, 10] -> results in {[2, 7], [9, 10]}
var withDisjoint = singleJoined.Join(IntInterval.CreateClosed(9, 10));
Console.WriteLine(withDisjoint.Count); // 2

// 3. Join another set
var otherSet = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(6, 9) // bridges [2, 7] and [9, 10]
);
var setJoined = withDisjoint.Join(otherSet);
Console.WriteLine(setJoined.Count); // 1
Console.WriteLine(setJoined[0]);   // [2, 10]
```

## See Also

- [Consolidate()](consolidate.md)
- [Interval Sets Overview](index.md)
- [Methods Overview](../methods.md)
