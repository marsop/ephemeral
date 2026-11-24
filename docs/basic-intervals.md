# Basic Intervals

A **Basic Interval** represents a continuous range between two boundary points (`Start` and `End`) of any comparable type. In Ephemeral, basic intervals are modeled by the `IBasicInterval<TBoundary>` interface and its canonical implementation, `BasicInterval<TBoundary>`.

Unlike full intervals, basic intervals do not require arithmetic operations, distance metrics, or length calculation. They require only that the boundary type implements `IComparable<TBoundary>`, making them suitable for numbers, timestamps, strings, versions, or custom domain entities.

---

## The `IBasicInterval<TBoundary>` Interface

The `IBasicInterval<TBoundary>` interface defines the foundational contract for all intervals in Ephemeral:

```csharp
namespace Marsop.Ephemeral.Core;

public interface IBasicInterval<TBoundary>
    where TBoundary : notnull, IComparable<TBoundary>
{
    /// <summary>
    /// Gets the starting point of the interval.
    /// </summary>
    TBoundary Start { get; }

    /// <summary>
    /// Gets a value indicating whether the start is included in the interval.
    /// </summary>
    bool StartIncluded { get; }

    /// <summary>
    /// Gets the final point of the interval.
    /// </summary>
    TBoundary End { get; }

    /// <summary>
    /// Gets a value indicating whether the end is included in the interval.
    /// </summary>
    bool EndIncluded { get; }
}
```

### Boundary Inclusivity and Mathematical Notation

Ephemeral supports open, closed, and half-open intervals:

- **Closed `[a, b]`**: Both `Start` and `End` are included (`StartIncluded = true`, `EndIncluded = true`). Contains all $x$ such that $a \le x \le b$.
- **Open `(a, b)`**: Neither endpoint is included (`StartIncluded = false`, `EndIncluded = false`). Contains all $x$ such that $a < x < b$.
- **Right-Open / Half-Open `[a, b)`**: Start is included, end is excluded (`StartIncluded = true`, `EndIncluded = false`). Contains all $x$ such that $a \le x < b$.
- **Left-Open / Half-Open `(a, b]`**: Start is excluded, end is included (`StartIncluded = false`, `EndIncluded = true`). Contains all $x$ such that $a < x \le b$.
- **Degenerate Point `[a, a]`**: A single-point interval where `Start` equals `End` and both are included.

### Validity Rules

An interval is valid if:

$$\text{Start} < \text{End} \quad \text{or} \quad (\text{Start} = \text{End} \land \text{StartIncluded} \land \text{EndIncluded})$$

If `Start > End`, or if `Start == End` while either boundary is open (e.g. `(a, a)`), attempting to instantiate the interval throws an `InvalidLengthException`.

---

## Basic Intervals Provided: `BasicInterval<TBoundary>`

Ephemeral provides `BasicInterval<TBoundary>` as the primary immutable implementation of `IBasicInterval<TBoundary>`.

### Constructors and Factory Methods

```csharp
using Marsop.Ephemeral.Core;

// Direct constructor
var interval = new BasicInterval<int>(1, 10, startIncluded: true, endIncluded: false); // [1, 10)

// Static factory methods
var closed = BasicInterval<int>.CreateClosed(1, 10); // [1, 10]
var open = BasicInterval<int>.CreateOpen(1, 10);     // (1, 10)
var point = BasicInterval<int>.CreatePoint(5);       // [5, 5]

// Copying from any IBasicInterval implementation
var copy = BasicInterval<int>.From(closed);
```

### Generic Boundary Usage

Because `BasicInterval<TBoundary>` works with any `IComparable<TBoundary>`, you can create intervals over types where arithmetic distance has no standard meaning, such as strings:

```csharp
using Marsop.Ephemeral.Core;

var directoryRange = BasicInterval<string>.CreateClosed("apple", "orange");

directoryRange.Covers("banana"); // returns true
directoryRange.Covers("zebra");  // returns false
directoryRange.ToString();       // "[apple, orange]"
```

---

## Methods for Basic Intervals

Methods available on `IBasicInterval<TBoundary>` are divided into two categories:

1. **Pure Boundary Methods**: Require only boundary comparisons (`IComparable<TBoundary>`).
2. **Metric-Assisted Extension Methods**: Accept an explicit `ILengthOperator<TBoundary, TLength>` or `ICanMeasure<TBoundary, TLength>` parameter to perform translations and metric calculations.

---

### Pure Boundary Methods

These methods require no length operator and work directly on any `IBasicInterval<TBoundary>`.

#### `Covers(TBoundary point)`
Checks whether a specific point is contained within the interval, taking endpoint inclusion into account.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.Covers(2.0) -> true
A.Covers(4.0) -> true
A.Covers(7.0) -> false
```

```csharp
var interval = BasicInterval<double>.CreateClosed(2.0, 6.0);
interval.Covers(2.0); // true
interval.Covers(7.0); // false
```

#### `Covers(IBasicInterval<TBoundary> other)`
Checks whether this interval completely encloses another interval.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Covers(B)     ->  true
```

```csharp
var a = BasicInterval<int>.CreateClosed(2, 8);
var b = BasicInterval<int>.CreateClosed(4, 6);
a.Covers(b); // true
b.Covers(a); // false
```

#### `Intersects(IBasicInterval<TBoundary> other)`
Returns `true` if this interval and `other` have at least one common point; otherwise `false`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Intersects(B) ->  true
```

```csharp
var a = BasicInterval<int>.CreateClosed(2, 6);
var b = BasicInterval<int>.CreateClosed(4, 8);
a.Intersects(b); // true
```

#### `Intersect(IBasicInterval<TBoundary> other)`
Calculates the overlapping region between two intervals, returning an `Option<BasicInterval<TBoundary>>`. If the intervals do not overlap, returns `Option.None`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Intersect(B)  ->          [=======]          ([4, 6])
```

```csharp
var a = BasicInterval<int>.CreateClosed(2, 6);
var b = BasicInterval<int>.CreateClosed(4, 8);

Option<BasicInterval<int>> overlap = a.Intersect(b);
if (overlap.HasValue)
{
    Console.WriteLine(overlap.ValueOrFailure()); // [4, 6]
}
```

#### `Join(IBasicInterval<TBoundary> other)`
Combines two overlapping or contiguous intervals into a single encompassing interval. Throws `ArgumentException` if the intervals are disjoint with a gap.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Join(B)       ->  [=======================]  ([2, 8])
```

```csharp
var a = BasicInterval<int>.CreateClosed(2, 6);
var b = BasicInterval<int>.CreateClosed(4, 8);
BasicInterval<int> merged = a.Join(b); // [2, 8]
```

#### `IsContiguouslyFollowedBy(IBasicInterval<TBoundary> other)`
Checks if `other` starts exactly at the boundary where this interval ends without overlapping (e.g. `[2, 4)` and `[4, 6]`).

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======)                  (A = [2, 4))
Interval B:                 [=======]          (B = [4, 6])

A.IsContiguouslyFollowedBy(B) -> true
```

```csharp
var a = new BasicInterval<int>(2, 4, true, false); // [2, 4)
var b = new BasicInterval<int>(4, 6, true, true);  // [4, 6]
a.IsContiguouslyFollowedBy(b); // true
```

#### `IsContiguouslyPrecededBy(IBasicInterval<TBoundary> other)`
Checks if `other` ends exactly where this interval starts without overlapping.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:                 [=======]          (A = [4, 6])
Interval B:         [=======)                  (B = [2, 4))

A.IsContiguouslyPrecededBy(B) -> true
```

```csharp
var a = new BasicInterval<int>(4, 6, true, true);  // [4, 6]
var b = new BasicInterval<int>(2, 4, true, false); // [2, 4)
a.IsContiguouslyPrecededBy(b); // true
```

#### `StartsBefore(IBasicInterval<TBoundary> other)`
Checks if this interval starts before `other`. If start points are equal, an included start boundary is considered to start before an excluded start boundary.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                 [=======]          (B = [4, 6])

A.StartsBefore(B) -> true
```

#### `IsEquivalentIntervalTo(IBasicInterval<TBoundary> other)`
Checks whether two intervals are mathematically identical (equal `Start`, `End`, `StartIncluded`, and `EndIncluded`).

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:         [=======]                  (B = [2, 4])
Interval C:         [=======)                  (C = [2, 4))

A.IsEquivalentIntervalTo(B) -> true
A.IsEquivalentIntervalTo(C) -> false
```

---

### Metric-Assisted Extension Methods

When you need metric calculations on basic intervals, you can pass an explicit `ILengthOperator<TBoundary, TLength>` or `ICanMeasure<TBoundary, TLength>`.

#### `Measure(measurer)`
Computes the length of the basic interval using the provided measurer.

```csharp
using Marsop.Ephemeral.Numerics;

var interval = BasicInterval<double>.CreateClosed(2.0, 6.0);
double length = interval.Measure(DoubleDefaultLengthOperator.Instance); // 4.0
```

#### `WithMeasure(measurer)`
Wraps the basic interval into a `BasicMeasuredInterval<TBoundary, TLength>` that caches the measured length.

```csharp
BasicMeasuredInterval<double, double> measured = 
    interval.WithMeasure(DoubleDefaultLengthOperator.Instance);
double len = measured.DefaultMeasure(); // 4.0
```

#### `Shift(offset, measurer)`
Shifts both `Start` and `End` boundaries by `offset`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])

A.Shift(4)      ->                  [=======]  ([6, 8])
```

```csharp
var shifted = interval.Shift(4.0, DoubleDefaultLengthOperator.Instance);
```

#### `ShiftStart(offset, measurer)`
Shifts only the `Start` boundary by `offset`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftStart(2) ->          [=======]          ([4, 6])
```

#### `ShiftEnd(offset, measurer)`
Shifts only the `End` boundary by `offset`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftEnd(2)   ->  [=======================]  ([2, 8])
```

#### `LengthOfIntersect(other, measurer)`
Calculates the metric length of the intersection between two intervals. Returns `Zero()` if disjoint.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.LengthOfIntersect(B) -> 2
```

#### `Subtract(other, measurer)`
Subtracts `other` from this interval, returning a `DisjointIntervalSet<TBoundary, TLength>` of the remaining pieces.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Subtract(B)   ->  [=======)       (=======]  ([2, 4) and (6, 8])
```

#### `Union(other, measurer)`
Combines two intervals into a `DisjointIntervalSet<TBoundary, TLength>`. If they do not overlap, the set contains both separate intervals; if they overlap, they are merged.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                         [=======]  (B = [6, 8])

A.Union(B)      ->  [=======]       [=======]  ([2, 4] and [6, 8])
```

#### `Partition(step, measurer, intermediateEndsClosed)`
Splits the interval into contiguous sub-intervals of length `step`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]          (A = [2, 8])

A.Partition(2) ->   [=======)                          ([2, 4))
                            [=======)                  ([4, 6))
                                    [=======]          ([6, 8])
```

---

## Next Steps

- Explore [Full Intervals](full-intervals.md) to use intervals with built-in length operators.
- Explore [Interval Sets](interval-sets/index.md) to manage collections of disjoint intervals.
- Explore [Custom Intervals](custom-intervals.md) to define intervals for your own domain types.
