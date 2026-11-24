# GetBoundingInterval()

The `GetBoundingInterval()` extension method returns the minimum interval that completely encloses all intervals contained within an `IDisjointIntervalSet<TBoundary, TLength>`.

## Method Signature

```csharp
public static BasicMeasuredInterval<TBoundary, TLength> GetBoundingInterval<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> s)
    where TBoundary : notnull, IComparable<TBoundary>;
```

## How It Works

`GetBoundingInterval()` computes the smallest contiguous interval spanning from the earliest `Start` boundary to the latest `End` boundary across all intervals in the set:
- The start boundary is `s.Start`, with inclusion determined by `s.Covers(s.Start)`.
- The end boundary is `s.End`, with inclusion determined by `s.Covers(s.End)`.
- The returned interval is a `BasicMeasuredInterval<TBoundary, TLength>` measured by the set's `LengthOperator`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:              [=======]       [=======]  ([2, 4] and [6, 8])

S.GetBoundingInterval() ->
                    [=======================]  ([2, 8])
```

## Example

```csharp
using System;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;

// Set with two disjoint intervals separated by a gap
var set = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(2, 4),
    IntInterval.CreateClosed(6, 8)
);

BasicMeasuredInterval<int, int> bounds = set.GetBoundingInterval();

Console.WriteLine(bounds.Start); // 2
Console.WriteLine(bounds.End);   // 8
Console.WriteLine(bounds);       // [2, 8]

// Because it is a measured interval, you can measure its total span
int totalSpan = bounds.Measure(lengthOperator); // 6
```

## See Also

- [Interval Sets Overview](index.md)
- [Methods Overview](../methods.md)
