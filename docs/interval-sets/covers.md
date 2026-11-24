# Covers()

The `Covers()` extension method checks whether an `IDisjointIntervalSet<TBoundary, TLength>` contains a specific point or entirely covers another interval.

## Method Signatures

```csharp
// Overload 1: Check point containment
public static bool Covers<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set,
    TBoundary timestamp)
    where TBoundary : notnull, IComparable<TBoundary>;

// Overload 2: Check interval containment
public static bool Covers<TBoundary, TLength>(
    this IDisjointIntervalSet<TBoundary, TLength> set,
    IBasicInterval<TBoundary> interval)
    where TBoundary : notnull, IComparable<TBoundary>;
```

## Point Containment (`Covers(TBoundary)`)

Checks if the given point or timestamp is covered by at least one interval in the set (`set.Any(x => x.Covers(timestamp))`).

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:              [=======]       [=======]  ([2, 4] and [6, 8])

S.Covers(3)     ->  true
S.Covers(5)     ->  false
```

### Example

```csharp
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;
var set = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(2, 4),
    IntInterval.CreateClosed(6, 8)
);

bool has3 = set.Covers(3); // true
bool has5 = set.Covers(5); // false (falls in the gap)
```

## Interval Containment (`Covers(IBasicInterval)`)

Checks if the specified interval is entirely contained within the set. 

> [!NOTE]
> `Covers(IBasicInterval)` automatically calls `Consolidate()` internally. Therefore, if contiguous intervals in the set span the target interval seamlessly across their boundary, the method correctly evaluates to `true`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Set S:
  Interval 1:       [=======)                  ([2, 4))
  Interval 2:               [===============]  ([4, 8])

Interval A:                 [=======]          (A = [3, 5])
Interval B:         [===============]          (B = [1, 5])

S.Covers(A)     ->  true (covered across consolidated [2, 8])
S.Covers(B)     ->  false (1 is outside the set)
```

### Example

```csharp
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;

var lengthOperator = IntDefaultLengthOperator.Instance;
var set = new DisjointIntervalSet<int, int>(
    lengthOperator,
    new BasicInterval<int>(2, 4, true, false), // [2, 4)
    new BasicInterval<int>(4, 8, true, true)   // [4, 8]
);

var targetA = IntInterval.CreateClosed(3, 5); // [3, 5]
var targetB = IntInterval.CreateClosed(1, 5); // [1, 5]

bool coversA = set.Covers(targetA); // true
bool coversB = set.Covers(targetB); // false
```

## See Also

- [Consolidate()](consolidate.md)
- [Interval Sets Overview](index.md)
- [Methods Overview](../methods.md)
