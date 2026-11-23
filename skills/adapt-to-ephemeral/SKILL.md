---
name: adapt-to-ephemeral
description: Guide for discovering range and interval patterns in existing C# codebases and refactoring them to use Ephemeral interval abstractions and operations.
---

# Adapting Existing Code to Ephemeral

This skill guides you through migrating ad-hoc interval and range implementations to the `Ephemeral` library.

## When to Use

Use this skill when you encounter:
- Paired boundary variables or properties (e.g., `start` and `end`, `from` and `to`, `min` and `max`).
- Manual range comparisons (point containment checks, overlap checks, interval intersections).
- Custom logic for combining or merging contiguous/overlapping time slots or numeric ranges.

## Package Reference

Ensure the appropriate Ephemeral package is referenced in the project:
```xml
<PackageReference Include="Ephemeral" />
```
For projects targeting .NET 6+ using `DateOnly` or `TimeOnly`:
```xml
<PackageReference Include="Ephemeral.Net6" />
```

## Available Interval Types

Ephemeral provides strongly typed intervals implementing `IBasicInterval<TBoundary>` and extending `FullInterval<TBoundary, TLength>`:
- `DateTimeOffsetInterval`: for `DateTimeOffset` boundaries measured with `TimeSpan`.
- `IntInterval`: for `int` boundaries measured with `int`.
- `DoubleInterval`: for `double` boundaries measured with `double`.
- `DateOnlyInterval` (in `Ephemeral.Net6`): for `DateOnly` boundaries measured in days (`int`).
- `TimeOnlyInterval` (in `Ephemeral.Net6`): for `TimeOnly` boundaries measured with `TimeSpan`.
- `BasicInterval<TBoundary>`: generic interval for any `TBoundary : IComparable<TBoundary>`.

## Common Refactoring Patterns

### 1. Replacing Paired Boundary Properties
Before:
```csharp
public class Booking
{
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
}
```
After:
```csharp
using Marsop.Ephemeral.Temporal;

public class Booking
{
    public DateTimeOffsetInterval Period { get; set; }
}
```

### 2. Point Containment (`Covers`)
Before:
```csharp
bool isWithin = timestamp >= start && timestamp <= end;
```
After:
```csharp
using Marsop.Ephemeral.Core;

bool isWithin = interval.Covers(timestamp);
```

### 3. Overlap Checking (`Intersects`)
Before:
```csharp
bool overlaps = start1 <= end2 && end1 >= start2;
```
After:
```csharp
using Marsop.Ephemeral.Core;

bool overlaps = intervalA.Intersects(intervalB);
```

### 4. Overlap Calculation (`Intersect`)
Before:
```csharp
var overlapStart = start1 > start2 ? start1 : start2;
var overlapEnd = end1 < end2 ? end1 : end2;
if (overlapStart <= overlapEnd)
{
    // handle overlap
}
```
After:
```csharp
using Marsop.Ephemeral.Core;

intervalA.Intersect(intervalB).Match(
    overlap => { /* use overlap.Start and overlap.End */ },
    () => { /* no intersection */ }
);
```

### 5. Joining Contiguous or Overlapping Intervals (`Join`)
Before:
```csharp
if (start1 <= end2 && end1 >= start2)
{
    var mergedStart = start1 < start2 ? start1 : start2;
    var mergedEnd = end1 > end2 ? end1 : end2;
}
```
After:
```csharp
using Marsop.Ephemeral.Core;

intervalA.Join(intervalB).Match(
    merged => { /* use merged */ },
    () => { /* intervals are not contiguous or overlapping */ }
);
```

### 6. Shifting Boundaries (`Shift`, `ShiftStart`, `ShiftEnd`)
Before:
```csharp
start = start.Add(duration);
end = end.Add(duration);
```
After:
```csharp
using Marsop.Ephemeral.Core;

var shifted = interval.Shift(duration);
```

### 7. Managing Collections of Intervals (`DisjointIntervalSet` and `Consolidate`)
Before:
```csharp
// Manual sorting and merging of scheduled slots
var sorted = slots.OrderBy(s => s.Start).ToList();
// manual merging loop...
```
After:
```csharp
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Temporal;

var set = new DisjointIntervalSet<DateTimeOffset, TimeSpan>(DateTimeOffsetTimeSpanLengthOperator.Instance);
foreach (var slot in slots)
{
    set.Add(slot);
}
var consolidated = set.Consolidate();
```
