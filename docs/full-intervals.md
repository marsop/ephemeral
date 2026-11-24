# Full Intervals

A **Full Interval** (`FullInterval<TBoundary, TLength>`) extends a basic interval by incorporating an intrinsic length metric and translation arithmetic via an `ILengthOperator<TBoundary, TLength>`.

While basic intervals require callers to supply an explicit length operator for metric-dependent operations, full intervals carry their own operator natively. This enables streamlined, natural syntax for measuring, shifting, subtracting, and partitioning intervals.

---

## Architecture and Core Hierarchy

The full interval architecture links boundary comparisons with metric distance calculations:

```text
IBasicInterval<TBoundary>          IHasLength<TLength>
        ▲                                   ▲
        │                                   │
 BasicInterval<TBoundary>   AbstractMeasuredInterval<TBoundary, TLength>
                                            ▲
                                            │
                               FullInterval<TBoundary, TLength>
                                            ▲
                  ┌─────────────────────────┼─────────────────────────┐
                  │                         │                         │
        DateTimeOffsetInterval         IntInterval              DoubleInterval
        (DateOnlyInterval / TimeOnlyInterval in .NET 6+)
```

### The `FullInterval<TBoundary, TLength>` Class

```csharp
namespace Marsop.Ephemeral.Core;

public abstract record FullInterval<TBoundary, TLength> :
    AbstractMeasuredInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    protected FullInterval(TBoundary start, TBoundary end, bool startIncluded, bool endIncluded)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Gets the length operator associated with this interval.
    /// </summary>
    public abstract ILengthOperator<TBoundary, TLength> Operator { get; }

    /// <summary>
    /// Measures the interval using its intrinsic operator.
    /// </summary>
    public override TLength DefaultMeasure() => Operator.Measure(this);
}
```

### The Role of `ILengthOperator<TBoundary, TLength>`

The `ILengthOperator` provides three key operations:

1. **`Apply(TBoundary boundary, TLength length) : TBoundary`**: Translates a boundary forward by a length offset.
2. **`Measure(IBasicInterval<TBoundary> interval) : TLength`**: Calculates the distance between `End` and `Start`.
3. **`Zero() : TLength`**: Returns the zero/identity value for `TLength`.

Depending on the interval type, `TLength` can either match `TBoundary` or represent a distinct difference type:

| Interval Class | Boundary Type (`TBoundary`) | Length Type (`TLength`) | Description |
| :--- | :--- | :--- | :--- |
| `DateTimeOffsetInterval` | `DateTimeOffset` | `TimeSpan` | Calendar time intervals measured in durations. |
| `IntInterval` | `int` | `int` | Discrete integer intervals. |
| `DoubleInterval` | `double` | `double` | Continuous numeric intervals on the real line. |
| `DateOnlyInterval` | `DateOnly` | `int` | Calendar date intervals measured in whole days (.NET 6+). |
| `TimeOnlyInterval` | `TimeOnly` | `TimeSpan` | Clock time intervals measured in time durations (.NET 6+). |

---

## Built-in Full Interval Classes

Ephemeral provides ready-to-use full interval classes for the most common .NET data types.

### `DateTimeOffsetInterval`
Targets `.NET Standard 2.0+`. Represents a timeline range between two points in time.

```csharp
using Marsop.Ephemeral.Temporal;

var start = DateTimeOffset.UtcNow;
var end = start.AddHours(2);

var meeting = DateTimeOffsetInterval.CreateClosed(start, end);
TimeSpan duration = meeting.DefaultMeasure(); // 02:00:00
```

### `IntInterval`
Targets `.NET Standard 2.0+`. Represents a range over integers.

```csharp
using Marsop.Ephemeral.Numerics;

var pages = IntInterval.CreateClosed(1, 10);
int pageCount = pages.DefaultMeasure(); // 9 (10 - 1)
```

### `DoubleInterval`
Targets `.NET Standard 2.0+`. Represents continuous floating-point intervals.

```csharp
using Marsop.Ephemeral.Numerics;

var tolerance = DoubleInterval.CreateClosed(-0.5, 0.5);
double width = tolerance.DefaultMeasure(); // 1.0
```

### `DateOnlyInterval`
Targets `.NET 6.0+` (`Marsop.Ephemeral.Net6`). Represents a calendar date range measured in days.

```csharp
using Marsop.Ephemeral.Net6.Temporal;

var q1 = DateOnlyInterval.CreateClosed(new DateOnly(2025, 1, 1), new DateOnly(2025, 3, 31));
int days = q1.DefaultMeasure(); // 89 days
```

### `TimeOnlyInterval`
Targets `.NET 6.0+` (`Marsop.Ephemeral.Net6`). Represents a time-of-day interval measured with `TimeSpan`.

```csharp
using Marsop.Ephemeral.Net6.Temporal;

var shift = TimeOnlyInterval.CreateClosed(new TimeOnly(9, 0), new TimeOnly(17, 0));
TimeSpan shiftLength = shift.DefaultMeasure(); // 08:00:00
```

---

## Factory Methods

All built-in full interval classes provide standard static factory methods:

```csharp
// Closed: [start, end]
var closed = DoubleInterval.CreateClosed(2.0, 6.0);

// Open: (start, end)
var open = DoubleInterval.CreateOpen(2.0, 6.0);

// Point / Degenerate: [point, point]
var point = DoubleInterval.CreatePoint(42.0);
```

---

## Full Interval Methods Explained

Because `FullInterval<TBoundary, TLength>` carries its own `Operator`, all methods can be invoked directly without passing length operators.

---

### `DefaultMeasure()`

Measures the interval's length from `Start` to `End` using the interval's intrinsic operator.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.DefaultMeasure() -> 4
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 6.0);
double length = interval.DefaultMeasure(); // 4.0
```

---

### `Shift(TLength offset)`

Translates both the `Start` and `End` boundaries forward (or backward for negative offsets) by the specified `offset`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])

A.Shift(4)      ->                  [=======]  ([6, 8])
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 4.0);
var shifted = interval.Shift(4.0); // [6.0, 8.0]
```

---

### `ShiftStart(TLength offset)`

Translates only the `Start` boundary by `offset`. The new start must remain less than or equal to `End` (or equal if both are closed); otherwise, an `InvalidLengthException` is thrown.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftStart(2) ->          [=======]          ([4, 6])
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 6.0);
var contracted = interval.ShiftStart(2.0); // [4.0, 6.0]
```

---

### `ShiftEnd(TLength offset)`

Translates only the `End` boundary by `offset`. The new end must remain greater than or equal to `Start`; otherwise, an `InvalidLengthException` is thrown.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftEnd(2)   ->  [=======================]  ([2, 8])
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 6.0);
var expanded = interval.ShiftEnd(2.0); // [2.0, 8.0]
```

---

### `LengthOfIntersect(IBasicInterval<TBoundary> other)`

Computes the metric length of the overlapping region between this interval and `other`. If the intervals do not intersect, returns `Zero()`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.LengthOfIntersect(B) -> 2
```

```csharp
var a = DoubleInterval.CreateClosed(2.0, 6.0);
var b = DoubleInterval.CreateClosed(4.0, 8.0);

double overlapLen = a.LengthOfIntersect(b); // 2.0
```

---

### `Subtract(IBasicInterval<TBoundary> other)`

Subtracts `other` from this interval. The result is returned as a `DisjointIntervalSet<TBoundary, TLength>` containing zero, one, or two remaining sub-intervals.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Subtract(B)   ->  [=======)       (=======]  ([2, 4) and (6, 8])
```

```csharp
var a = DoubleInterval.CreateClosed(2.0, 8.0);
var b = DoubleInterval.CreateClosed(4.0, 6.0);

DisjointIntervalSet<double, double> remaining = a.Subtract(b);
// remaining[0] is [2.0, 4.0)
// remaining[1] is (6.0, 8.0]
```

---

### `ToIntervalSet()`

Converts this single interval into a `DisjointIntervalSet<TBoundary, TLength>` containing only this interval.

```csharp
var interval = DoubleInterval.CreateClosed(1.0, 5.0);
DisjointIntervalSet<double, double> set = interval.ToIntervalSet();

Console.WriteLine(set.Count); // 1
Console.WriteLine(set[0]);    // [1, 5]
```

---

### `Partition(TLength step, bool intermediateEndsClosed = false)`

Splits the interval into contiguous sub-intervals of length `step`. 

The `intermediateEndsClosed` parameter controls the inclusivity of inner boundaries:
- `false` (default): inner boundaries are half-open `[start, end)`.
- `true`: inner boundaries are closed `[start, end]`.

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]          (A = [2, 8])

A.Partition(2) ->   [=======)                          ([2, 4))
                            [=======)                  ([4, 6))
                                    [=======]          ([6, 8])
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 8.0);
DisjointIntervalSet<double, double> chunks = interval.Partition(2.0);

// chunks contains: [2, 4), [4, 6), and [6, 8]
```

---

### Inherited Basic Interval Methods

Because `FullInterval<TBoundary, TLength>` inherits `BasicInterval<TBoundary>`, all boundary-only operations described in [Basic Intervals](basic-intervals.md) work directly on full intervals:

- **`Covers(point)`**: checks if a point is within the interval.
- **`Covers(other)`**: checks if another interval is completely contained.
- **`Intersects(other)`**: checks if two intervals overlap.
- **`Intersect(other)`**: returns `Option<BasicInterval<TBoundary>>` of the overlap.
- **`Join(other)`**: merges overlapping/contiguous intervals into one.
- **`IsContiguouslyFollowedBy(other)`**: checks if `other` immediately touches the end.
- **`IsContiguouslyPrecededBy(other)`**: checks if `other` immediately touches the start.
- **`StartsBefore(other)`**: checks start boundary ordering.
- **`IsEquivalentIntervalTo(other)`**: checks mathematical equivalence.

```csharp
var a = DateTimeOffsetInterval.CreateClosed(now, now.AddHours(4));
var b = DateTimeOffsetInterval.CreateClosed(now.AddHours(1), now.AddHours(2));

bool covers = a.Covers(b); // true
bool overlaps = a.Intersects(b); // true
```

---

## Practical Examples

### Scheduling and Meeting Shifts

```csharp
using Marsop.Ephemeral.Temporal;

var now = DateTimeOffset.UtcNow;
var morningMeeting = DateTimeOffsetInterval.CreateClosed(now, now.AddHours(1));

// Shift meeting forward by 24 hours
var tomorrowMeeting = morningMeeting.Shift(TimeSpan.FromHours(24));

// Verify non-intersection
bool conflict = morningMeeting.Intersects(tomorrowMeeting); // false
```

### DateOnly Working Periods (.NET 6+)

```csharp
using Marsop.Ephemeral.Net6.Temporal;

var projectPhase1 = DateOnlyInterval.CreateClosed(new DateOnly(2025, 1, 1), new DateOnly(2025, 1, 31));
var vacation = DateOnlyInterval.CreateClosed(new DateOnly(2025, 1, 15), new DateOnly(2025, 1, 20));

// Subtract vacation days from the project phase
var workingSegments = projectPhase1.Subtract(vacation);
// workingSegments contains: [2025-01-01, 2025-01-15) and (2025-01-20, 2025-01-31]
```

---

## Next Steps

- Explore [Basic Intervals](basic-intervals.md) for lightweight intervals without metric requirements.
- Explore [Interval Sets](interval-sets/index.md) to manage collections of non-overlapping intervals.
- Explore [Custom Intervals](custom-intervals.md) to build your own intervals and length operators.
