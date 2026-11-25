# Interval Sets (`DisjointIntervalSet`)

An **Interval Set** (`IDisjointIntervalSet<TBoundary, TLength>`) represents an ordered collection of mutually disjoint (non-overlapping) intervals over a real line or timeline. In Ephemeral, the primary implementation is `DisjointIntervalSet<TBoundary, TLength>`.

Interval sets are particularly useful for modeling scenarios where time or numeric ranges are fragmented, such as:
- Available time slots on a calendar or booking system.
- Discontinuous working shifts or operating hours.
- Sparse numeric coverage and multi-segment ranges.
- Computing remaining intervals after subtracting busy periods.

## Core Characteristics and Invariants

1. **Disjointness Invariant**: At any time, no two intervals in the set may overlap (`Intersects() == true`). Attempting to add an interval that overlaps any existing interval throws an `OverlapException`.
2. **Automatic Ordering**: Intervals in the set are always maintained in sorted order by their `Start` boundary (using `IntervalStartComparer<TBoundary>`). Direct indexed insertion (`Insert`) is not supported and throws `NotSupportedException`.
3. **Length Measurement Integration**: Each set holds an `ILengthOperator<TBoundary, TLength>` which allows operations such as consolidation, joins, and computing the bounding interval.

## Creating an Interval Set

### Constructors

You can instantiate a `DisjointIntervalSet` directly using an `ILengthOperator`:

```csharp
var lengthOperator = IntDefaultLengthOperator.Instance;

// Empty set
var set = new DisjointIntervalSet<int, int>(lengthOperator);

// Initialized with array or params
var setWithParams = new DisjointIntervalSet<int, int>(
    lengthOperator,
    IntInterval.CreateClosed(1, 3),
    IntInterval.CreateClosed(5, 8)
);

// Initialized from an IEnumerable
var intervals = new List<IBasicInterval<int>>
{
    IntInterval.CreateClosed(1, 3),
    IntInterval.CreateClosed(5, 8)
};
var setFromEnumerable = new DisjointIntervalSet<int, int>(lengthOperator, intervals);
```

### From Interval Operations

Interval sets are also returned naturally from interval operations:

- **From a single interval**: `interval.ToIntervalSet()`
- **Interval subtraction**: `intervalA.Subtract(intervalB)` returns a `DisjointIntervalSet` representing the remaining segments.
- **Interval union**: `intervalA.Union(intervalB, lengthOperator)` returns a `DisjointIntervalSet` containing both intervals.

## Properties

`IDisjointIntervalSet<TBoundary, TLength>` exposes several properties to inspect the overall bounds and structure of the set:

| Property | Type | Description |
| :--- | :--- | :--- |
| `Start` | `TBoundary` | The earliest start boundary among all contained intervals. |
| `StartIncluded` | `bool` | Whether the start boundary of the earliest interval is included. |
| `End` | `TBoundary` | The latest end boundary among all contained intervals. |
| `EndIncluded` | `bool` | Whether the end boundary of the latest interval is included. |
| `IsContiguous` | `bool` | `true` if all contained intervals are contiguous (i.e. mergeable into a single interval with no gaps) or if the set has fewer than 2 intervals. |
| `Count` | `int` | The total number of disjoint intervals currently in the set. |
| `LengthOperator` | `ILengthOperator<TBoundary, TLength>` | The length operator used for interval calculations. |

## Collection Operations

`DisjointIntervalSet<TBoundary, TLength>` implements `IList<IBasicInterval<TBoundary>>` and `ICollection<IBasicInterval<TBoundary>>`:

- `Add(IBasicInterval<TBoundary> item)`: Adds an interval to the set. Throws `OverlapException` if the interval overlaps an existing interval.
- `Remove(IBasicInterval<TBoundary> item)`: Removes the specified interval from the set.
- `Contains(IBasicInterval<TBoundary> item)`: Checks whether the specified interval exists in the set.
- `Clear()`: Removes all intervals from the set.
- `this[int index]`: Accesses an interval by its sorted index.

## Methods

Ephemeral provides dedicated extension methods for `IDisjointIntervalSet<TBoundary, TLength>`. Each method is documented on its own page:

- **[`Consolidate()`](consolidate.md)**: Merges adjacent, contiguous intervals into a minimal number of continuous intervals.
- **[`Covers()`](covers.md)**: Checks if a point or interval is completely covered by the intervals in the set.
- **[`Intersect()`](intersect.md)**: Computes the intersection between an interval and each interval in the set.
- **[`Join()`](join.md)**: Merges an interval or another interval set into the set, combining overlapping intervals.
- **[`GetBoundingInterval()`](get-bounding-interval.md)**: Returns an `Option` containing the minimal interval spanning from the earliest start to the latest end of the set, or `Option.None` when empty.

## See Also

- [Basic Intervals](../basic-intervals.md)
- [Full Intervals](../full-intervals.md)
- [Custom Intervals](../custom-intervals.md)
- [Methods Overview](../methods.md)
- [Home](../index.md)
