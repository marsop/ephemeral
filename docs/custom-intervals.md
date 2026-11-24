# Custom Intervals

Ephemeral is designed for extensibility. While the library provides built-in interval types for standard .NET primitives (`int`, `double`, `DateTimeOffset`, `DateOnly`, `TimeOnly`), you can easily create custom intervals for your own domain types—such as `decimal` financial numbers, custom timestamps, units of measure, coordinates, or version numbers.

This guide explains how to design and implement custom intervals, what architectural decisions to consider, and provides complete, end-to-end working examples.

---

## Architectural Decisions: What to Consider

Before writing code, consider the following decisions:

### 1. Basic Interval vs. Full Interval

| Factor | Basic Interval (`IBasicInterval<TBoundary>`) | Full Interval (`FullInterval<TBoundary, TLength>`) |
| :--- | :--- | :--- |
| **Requirements** | Boundary type must implement `IComparable<TBoundary>`. | Boundary must be `IComparable<TBoundary>`, plus an `ILengthOperator<TBoundary, TLength>`. |
| **Metric / Distance** | No length metric required. | Length metric (`TLength`) is required. |
| **Supported Operations** | Containment (`Covers`), overlaps (`Intersects`, `Intersect`), contiguous checks (`IsContiguouslyFollowedBy`), ordering (`StartsBefore`). | All basic operations, plus `DefaultMeasure()`, shifting (`Shift`, `ShiftStart`, `ShiftEnd`), `Subtract()`, `Partition()`, and native `DisjointIntervalSet` support. |
| **Implementation Effort** | None if using `BasicInterval<TBoundary>`, or implement 4 properties on your existing entity. | Create a length operator and derive from `FullInterval<TBoundary, TLength>`. |

- **Choose a Basic Interval** if your boundary type only supports relative ordering, but addition or arithmetic distance is meaningless (e.g., lexical ranges like `"alpha"` to `"omega"`, or semantic version numbers like `1.0.0` to `2.3.1`).
- **Choose a Full Interval** if your boundaries have a well-defined distance metric and translation arithmetic (e.g., currency amounts, kilometers, calendar dates with custom calendars).

---

### 2. Choosing `TBoundary` and `TLength`

`TBoundary` and `TLength` can be the same type or different types:

1. **Same Type (`TBoundary == TLength`)**: Common for continuous numeric spaces.
   - Example: `decimal` boundaries with `decimal` lengths.
   - Example: `float` boundaries with `float` lengths.
2. **Different Types (`TBoundary != TLength`)**: Common for temporal and discrete scales.
   - Example: `DateTime` boundaries with `TimeSpan` length.
   - Example: `DateOnly` boundaries with `int` days length.
   - Example: Custom `Instant` boundaries with `Duration` length.

---

### 3. Implementing `ILengthOperator<TBoundary, TLength>`

The `ILengthOperator` interface defines the arithmetic rules for the interval:

```csharp
namespace Marsop.Ephemeral.Core;

public interface ILengthOperator<TBoundary, TLength> : ICanMeasure<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    /// <summary>
    /// Translates a boundary forward by a given length.
    /// </summary>
    TBoundary Apply(TBoundary boundary, TLength length);

    /// <summary>
    /// Calculates the length of an interval from Start to End.
    /// </summary>
    TLength Measure(IBasicInterval<TBoundary> interval);

    /// <summary>
    /// Returns the zero/identity value for TLength.
    /// </summary>
    TLength Zero();
}
```

#### Conventions and Best Practices for Length Operators

- **Monotonicity**: When `length > Zero()`, `Apply(boundary, length)` must return a boundary strictly greater than `boundary`.
- **Zero Invariance**: `Apply(boundary, Zero())` must equal `boundary`.
- **Non-Negative Measure**: `Measure(interval)` must return a non-negative length ($\ge \text{Zero()}$).
- **Singleton Pattern**: In Ephemeral, default length operators are typically stateless singletons:
  - Mark the class `sealed`.
  - Provide a private constructor with a descriptive comment: `// Private constructor to prevent external instantiation and enforce the singleton pattern.`
  - Expose a public static `Instance` property.

---

### 4. Interval Invariants and Validation

When deriving from `FullInterval<TBoundary, TLength>` or `BasicInterval<TBoundary>`:

- **Start and End Invariant**: Ephemeral automatically validates that `Start < End` or `(Start == End && StartIncluded && EndIncluded)`. If an invalid interval is constructed, `InvalidLengthException` is thrown.
- **Immutability**: Make your custom interval an immutable `record` or use readonly properties.

---

## Step-by-Step Implementation Guide

Let's build a complete custom `DecimalInterval` for high-precision financial or numeric calculations.

### Step 1: Implement the Length Operator

```csharp
using System;
using Marsop.Ephemeral.Core;

namespace MyCompany.Numerics;

/// <summary>
/// Length operator for intervals with decimal boundaries and lengths.
/// </summary>
public sealed class DecimalDefaultLengthOperator : ILengthOperator<decimal, decimal>
{
    private static readonly DecimalDefaultLengthOperator _instance = new();

    /// <summary>
    /// Gets the singleton instance of the operator.
    /// </summary>
    public static ILengthOperator<decimal, decimal> Instance => _instance;

    private DecimalDefaultLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    /// <inheritdoc/>
    public decimal Apply(decimal boundary, decimal length) => boundary + length;

    /// <inheritdoc/>
    public decimal Measure(IBasicInterval<decimal> interval)
    {
        if (interval is null) throw new ArgumentNullException(nameof(interval));
        return interval.End - interval.Start;
    }

    /// <inheritdoc/>
    public decimal Zero() => 0m;
}
```

### Step 2: Implement the Custom Interval Class

Inherit from `FullInterval<decimal, decimal>` and override the `Operator` property:

```csharp
using System;
using Marsop.Ephemeral.Core;

namespace MyCompany.Numerics;

/// <summary>
/// Represents an interval with decimal boundaries.
/// </summary>
public record DecimalInterval : FullInterval<decimal, decimal>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DecimalInterval"/> class.
    /// </summary>
    public DecimalInterval(decimal start, decimal end, bool startIncluded = true, bool endIncluded = true)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    /// <inheritdoc/>
    public override ILengthOperator<decimal, decimal> Operator => DecimalDefaultLengthOperator.Instance;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => base.ToString();

    /// <summary>
    /// Creates a closed decimal interval [start, end].
    /// </summary>
    public new static DecimalInterval CreateClosed(decimal start, decimal end) =>
        new(start, end, true, true);

    /// <summary>
    /// Creates an open decimal interval (start, end).
    /// </summary>
    public new static DecimalInterval CreateOpen(decimal start, decimal end) =>
        new(start, end, false, false);

    /// <summary>
    /// Creates a point interval [boundary, boundary].
    /// </summary>
    public new static DecimalInterval CreatePoint(decimal boundary) =>
        CreateClosed(boundary, boundary);
}
```

### Step 3: Use Your Custom Interval

Your custom interval now has access to all Ephemeral methods natively:

```csharp
using System;
using MyCompany.Numerics;
using Marsop.Ephemeral.Core;

// Create intervals
var priceTierA = DecimalInterval.CreateClosed(10.00m, 50.00m);
var priceTierB = DecimalInterval.CreateClosed(30.00m, 80.00m);

// 1. Measure length directly
decimal range = priceTierA.DefaultMeasure(); // 40.00m

// 2. Containment and overlap checks
bool covers = priceTierA.Covers(25.50m);     // true
bool overlaps = priceTierA.Intersects(priceTierB); // true

// 3. Shift boundaries
var shifted = priceTierA.Shift(10.00m);      // [20.00, 60.00]

// 4. Subtract intervals
DisjointIntervalSet<decimal, decimal> remaining = priceTierA.Subtract(priceTierB);
// remaining contains [10.00, 30.00)

// 5. Partition interval into equal steps
DisjointIntervalSet<decimal, decimal> bins = priceTierA.Partition(10.00m);
// bins contains [10.00, 20.00), [20.00, 30.00), [30.00, 40.00), [40.00, 50.00]

// 6. Use in DisjointIntervalSet
var set = new DisjointIntervalSet<decimal, decimal>(
    DecimalDefaultLengthOperator.Instance,
    DecimalInterval.CreateClosed(0m, 10m),
    DecimalInterval.CreateClosed(20m, 30m)
);
bool setCovers = set.Covers(5m); // true
```

---

## Implementing `IBasicInterval<TBoundary>` on Existing Entities

If you already have a domain class that represents a scheduled booking, shift, or numeric range, you do not need to derive from `FullInterval` or `BasicInterval`. You can implement `IBasicInterval<TBoundary>` directly on your domain class:

```csharp
using System;
using Marsop.Ephemeral.Core;

public class WorkShift : IBasicInterval<DateTimeOffset>
{
    public string EmployeeId { get; set; } = string.Empty;

    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }

    // IBasicInterval implementation
    public DateTimeOffset Start => StartTime;
    public DateTimeOffset End => EndTime;
    public bool StartIncluded => true;
    public bool EndIncluded => false; // Shift is [Start, End)
}
```

Now, all extension methods from `BasicIntervalExtensions` (such as `Covers`, `Intersects`, `Intersect`, `IsContiguouslyFollowedBy`, `StartsBefore`) are immediately available on `WorkShift`:

```csharp
var shift1 = new WorkShift { StartTime = todayAt9, EndTime = todayAt13 };
var shift2 = new WorkShift { StartTime = todayAt13, EndTime = todayAt17 };

bool contiguous = shift1.IsContiguouslyFollowedBy(shift2); // true!
```

---

## Custom Length Operator with Different Boundary and Length Types

Consider an interval over `DateTime` measured in whole `int` business hours, or calendar dates skipping weekends:

```csharp
using System;
using Marsop.Ephemeral.Core;

public sealed class DateTimeHoursLengthOperator : ILengthOperator<DateTime, int>
{
    public static readonly DateTimeHoursLengthOperator Instance = new();

    private DateTimeHoursLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    public DateTime Apply(DateTime boundary, int hours) => boundary.AddHours(hours);

    public int Measure(IBasicInterval<DateTime> interval) =>
        (int)(interval.End - interval.Start).TotalHours;

    public int Zero() => 0;
}
```

You can then create basic intervals of `DateTime` and measure or shift them using this operator:

```csharp
var schedule = BasicInterval<DateTime>.CreateClosed(startDate, endDate);
int hours = schedule.Measure(DateTimeHoursLengthOperator.Instance);
var nextWeekSchedule = schedule.Shift(168, DateTimeHoursLengthOperator.Instance);
```

---

## Checklist for Custom Interval Authors

Before publishing or finalizing custom intervals, ensure:

- [x] **Comparable Boundary**: `TBoundary` implements `IComparable<TBoundary>`.
- [x] **Monotonic `Apply()`**: `Apply(b, positiveLength)` advances `b` strictly forward.
- [x] **Identity `Zero()`**: `Apply(b, Zero())` leaves `b` unchanged.
- [x] **Singleton Operator**: The length operator is sealed and uses a private constructor.
- [x] **Factory Methods**: The interval class provides `CreateClosed`, `CreateOpen`, and `CreatePoint`.
- [x] **Immutability**: Intervals cannot be mutated after creation.
- [x] **Error Handling**: Invalid boundaries trigger `InvalidLengthException`.

---

## Next Steps

- Explore [Basic Intervals](basic-intervals.md) for pure boundary operations.
- Explore [Full Intervals](full-intervals.md) for built-in full interval features.
- Explore [Interval Sets](interval-sets/index.md) to manage collections of intervals.
- Return to [Home](index.md).
