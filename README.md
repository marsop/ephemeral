# Ephemeral

![Nuget](https://img.shields.io/nuget/dt/ephemeral?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/v/ephemeral?style=for-the-badge)

![](https://raw.githubusercontent.com/marsop/ephemeral/master/docs/img/EphemeralLogoCropped.png)

C# Library to handle intervals (composite start and end)

- Targets `.NET Standard 2.0` for wide compatibility.
- Support for generic types in intervals (e.g. `DateTimeOffset`, `int`, `double`).
- Support for open and closed intervals.
- Support for common operations like Covers(), Intersect(), Join(), etc. via extension methods.
- Support for disjoint collections of intervals via `DisjointIntervalSet`.
- Makes use of Optional to avoid returning `null` in many of the built-in operations.

The API-documentation is hosted at [albertogregorio.com/ephemeral](https://albertogregorio.com/ephemeral).

## Generics in Version 1

Starting from version 1, Ephemeral introduces generic intervals allowing boundaries of any type that implements `IComparable<TBoundary>`. It defines two primary categories of intervals:

- **Basic intervals** (`IBasicInterval<TBoundary>`, `BasicInterval<TBoundary>`): Work with any comparable type without requiring length metrics or arithmetic operators. They support boundary-based operations such as containment (`Covers`), overlaps (`Intersects`, `Intersect`), contiguous checks (`IsContiguouslyFollowedBy`, `IsContiguouslyPrecededBy`), and ordering (`StartsBefore`).
- **Full intervals** (`FullInterval<TBoundary, TLength>`, `IHasLength<TLength>`): Extend basic intervals by incorporating length measurement (`TLength`) via an `ILengthOperator<TBoundary, TLength>`. They support length-dependent operations such as `DefaultMeasure()`, shifting (`Shift`, `ShiftStart`, `ShiftEnd`), intersection length (`LengthOfIntersect`), and subtraction (`Subtract`).

To facilitate migration, Ephemeral provides several utility classes corresponding to standard types that extend `FullInterval`:
- `DateTimeOffsetInterval` (for `DateTimeOffset`, measured with `TimeSpan`)
- `IntInterval` (for `int`, measured with `int`)
- `DoubleInterval` (for `double`, measured with `double`)
- `DateOnlyInterval` (for `DateOnly`, measured in days as `int`, available in .NET 6+ targets)
- `TimeOnlyInterval` (for `TimeOnly`, measured with `TimeSpan`, available in .NET 6+ targets)

## Interval Length

In Ephemeral, intervals are parameterized by both their boundary type (`TBoundary`) and their measurement type (`TLength`). The length represents the difference or distance between the start and end of an interval.

### Standard Length Types

Utility classes extending `FullInterval<TBoundary, TLength>` implement `IHasLength<TLength>` and provide standard length calculations out of the box:

- `DoubleInterval`: boundary `double`, length `double` (difference between end and start).
- `IntInterval`: boundary `int`, length `int` (difference between end and start).
- `DateTimeOffsetInterval`: boundary `DateTimeOffset`, length `TimeSpan` (time duration).
- `DateOnlyInterval`: boundary `DateOnly`, length `int` (number of days).
- `TimeOnlyInterval`: boundary `TimeOnly`, length `TimeSpan` (elapsed time).

### Measuring an Interval

You can obtain the length of a full interval using `DefaultMeasure()`:

```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.DefaultMeasure() -> 4
```

```csharp
var interval = DoubleInterval.CreateClosed(2.0, 6.0);
double length = interval.DefaultMeasure(); // returns 4.0

var degenerate = DoubleInterval.CreatePoint(42.0);
double pointLength = degenerate.DefaultMeasure(); // returns 0.0
```

For basic intervals (`IBasicInterval<TBoundary>`), length can be measured explicitly by providing an `ICanMeasure<TBoundary, TLength>` or `ILengthOperator<TBoundary, TLength>` via `Measure()` or `WithMeasure()`:

```csharp
var basic = new BasicInterval<double>(2.0, 6.0, true, true);
double length = basic.Measure(DoubleDefaultLengthOperator.Instance); // returns 4.0
```

### How Length is Used

Length (`TLength`) is utilized across various Ephemeral operations:

- **Shifting boundaries (`Shift`, `ShiftStart`, `ShiftEnd`)**: Offsets of type `TLength` are applied to interval boundaries using the interval's `ILengthOperator<TBoundary, TLength>`.
- **Intersection length (`LengthOfIntersect`)**: Calculates the `TLength` of the overlapping region between two intervals, returning zero if they do not intersect.
- **Interval sets (`DisjointIntervalSet`)**: Collections of disjoint intervals use an `ILengthOperator<TBoundary, TLength>` to compute bounding intervals (`GetBoundingInterval()`), apply offsets, and manage unions and subtractions.

## Interval Operations

Here are graphical representations of common interval operations over the real line (using `DoubleInterval`). Operations that only depend on boundary comparisons require a **basic interval** (`IBasicInterval<TBoundary>`), while operations that translate boundaries or compute metrics require a **full interval** (`FullInterval<TBoundary, TLength>`) or an explicit `ILengthOperator`.

### `Covers()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if an interval completely contains another interval (or point).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Covers(B)     ->  true
```

### `Intersects()` / `Intersect()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if two intervals overlap. `Intersect()` returns the overlapping interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Intersects(B) ->  true
A.Intersect(B)  ->          [=======]          ([4, 6])
```

### `Join()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Merges two overlapping or contiguous intervals into a single interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.Join(B)       ->  [=======================]  ([2, 8])
```

### `Subtract()`
*Requires: Full interval (`FullInterval<TBoundary, TLength>`), or a basic interval with an explicit `ILengthOperator`*

Subtracts one interval from another, returning a disjoint set of intervals.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======================]  (A = [2, 8])
Interval B:                 [=======]          (B = [4, 6])

A.Subtract(B)   ->  [=======)       (=======]  ([2, 4) and (6, 8])
```

### `Union()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`) with an explicit `ILengthOperator`*

Combines two intervals into a collection (`DisjointIntervalSet`).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                         [=======]  (B = [6, 8])

A.Union(B)      ->  [=======]       [=======]  ([2, 4] and [6, 8])
```

### `IsContiguouslyFollowedBy()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if one interval starts exactly where another ends (without overlap, typically open/closed).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======)                  (A = [2, 4))
Interval B:                 [=======]          (B = [4, 6])

A.IsContiguouslyFollowedBy(B) -> true
```

### `IsContiguouslyPrecededBy()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if one interval ends exactly where another starts (without overlap, typically open/closed).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:                 [=======]          (A = [4, 6])
Interval B:         [=======)                  (B = [2, 4))

A.IsContiguouslyPrecededBy(B) -> true
```

### `StartsBefore()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if an interval starts before another interval.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:                 [=======]          (B = [4, 6])

A.StartsBefore(B) -> true
```

### `IsEquivalentIntervalTo()`
*Requires: Basic interval (`IBasicInterval<TBoundary>`)*

Checks if two intervals are equivalent (have the same boundaries and inclusion rules).
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])
Interval B:         [=======]                  (B = [2, 4])
Interval C:         [=======)                  (C = [2, 4))

A.IsEquivalentIntervalTo(B) -> true
A.IsEquivalentIntervalTo(C) -> false
```

### `Shift()`
*Requires: Full interval (`FullInterval<TBoundary, TLength>`), or a basic interval with an explicit `ILengthOperator`*

Shifts an interval by a given duration/offset.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [=======]                  (A = [2, 4])

A.Shift(4)      ->                  [=======]  ([6, 8])
```


### `ShiftStart()`
*Requires: Full interval (`FullInterval<TBoundary, TLength>`), or a basic interval with an explicit `ILengthOperator`*

Shifts the start of an interval by a given duration/offset.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftStart(2) ->          [=======]          ([4, 6])
```

### `ShiftEnd()`
*Requires: Full interval (`FullInterval<TBoundary, TLength>`), or a basic interval with an explicit `ILengthOperator`*

Shifts the end of an interval by a given duration/offset.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])

A.ShiftEnd(2)   ->  [=======================]  ([2, 8])
```

### `LengthOfIntersect()`
*Requires: Full interval (`FullInterval<TBoundary, TLength>`), or a basic interval with an explicit measurer (`ICanMeasure` / `ILengthOperator`)*

Calculates the length of the intersection between two intervals.
```text
Real Line:  0---1---2---3---4---5---6---7---8---9---10

Interval A:         [===============]          (A = [2, 6])
Interval B:                 [===============]  (B = [4, 8])

A.LengthOfIntersect(B) -> 2
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

### Double Interval Example

```csharp
var range1 = DoubleInterval.CreateClosed(1.5, 4.5);
var range2 = DoubleInterval.CreateOpen(3.0, 6.0);

range1.Intersects(range2); // returns true
range1.Covers(4.5); // returns true
range2.Covers(3.0); // returns false (because it's open)
```

### Generic Basic Interval Example

Basic intervals work with any type implementing `IComparable<T>`, without requiring length operators or metrics:

```csharp
var stringInterval = new BasicInterval<string>("apple", "orange", true, true);
var otherStringInterval = new BasicInterval<string>("banana", "pear", true, true);

stringInterval.Covers("cherry"); // returns true
stringInterval.Intersects(otherStringInterval); // returns true
```

### Interval Collection Example

```csharp
var lengthOperator = DateTimeOffsetTimeSpanLengthOperator.Instance;
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

### DateOnly Interval Example

```csharp
var start = new DateOnly(2023, 1, 1);
var end = new DateOnly(2023, 12, 31);
var year = DateOnlyInterval.CreateClosed(start, end);

var other = DateOnlyInterval.CreateClosed(new DateOnly(2023, 6, 1), new DateOnly(2023, 8, 31));
year.Covers(other); // returns true
```

### TimeOnly Interval Example

```csharp
var morningStart = new TimeOnly(8, 0);
var morningEnd = new TimeOnly(12, 0);
var morning = TimeOnlyInterval.CreateClosed(morningStart, morningEnd);

var workDay = TimeOnlyInterval.CreateClosed(new TimeOnly(8, 0), new TimeOnly(17, 0));
workDay.Covers(morning); // returns true
```

## Agent Skills

Starting with version 0.3.0, the `Ephemeral` package includes AI agent skills following the [Agent Skills](https://agentskills.io) standard, packaged directly under `skills/` within the NuGet package. These skills provide instructions and detection tools to help AI coding assistants and developers discover and migrate interval logic to Ephemeral abstractions.

### `adapt-to-ephemeral`

Located at `skills/adapt-to-ephemeral/`:
- **Purpose**: Guides refactoring ad-hoc boundary and range implementations into Ephemeral intervals.
- **When to Use**:
  - Codebases containing paired boundary properties or variables (e.g., `Start`/`End`, `From`/`To`, `Min`/`Max`).
  - Manual range comparison logic (containment checks, overlap tests, interval intersection).
  - Custom interval consolidation or merging routines.
- **Contents**:
  - `SKILL.md`: Detailed refactoring patterns mapping common manual interval operations to Ephemeral methods (`Covers`, `Intersects`, `Intersect`, `Join`, `Shift`, `DisjointIntervalSet`).
  - `scripts/find-interval-candidates.ps1`: A PowerShell script to scan C# source files for potential range variables and candidate patterns for refactoring.

