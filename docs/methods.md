# Methods Reference

This page provides an index of methods available in Ephemeral, categorized by interface and interval type. For in-depth guides, diagrams, and code examples, visit the dedicated documentation sections:

- **[Basic Intervals](basic-intervals.md)**: Pure boundary operations and metric-assisted extensions on `IBasicInterval<TBoundary>`.
- **[Full Intervals](full-intervals.md)**: Intrinsic length, translation, and subtraction on `FullInterval<TBoundary, TLength>`.
- **[Interval Sets](interval-sets/index.md)**: Ordered, disjoint collections of intervals via `IDisjointIntervalSet<TBoundary, TLength>`.
- **[Custom Intervals](custom-intervals.md)**: How to create domain-specific interval types and custom `ILengthOperator` implementations.

---

## Methods available from `IBasicInterval<TBoundary>` (Basic Intervals)

See the [Basic Intervals Guide](basic-intervals.md) for full descriptions and visual diagrams.

### Pure Boundary Methods
- `Covers(t: TBoundary) : bool`
- `Covers(i: IBasicInterval<TBoundary>) : bool`
- `Intersects(i: IBasicInterval<TBoundary>) : bool`
- `Intersect(i: IBasicInterval<TBoundary>) : Option<BasicInterval<TBoundary>>`
- `Join(i: IBasicInterval<TBoundary>) : BasicInterval<TBoundary>`
- `IsContiguouslyFollowedBy(i: IBasicInterval<TBoundary>) : bool`
- `IsContiguouslyPrecededBy(i: IBasicInterval<TBoundary>) : bool`
- `StartsBefore(i: IBasicInterval<TBoundary>) : bool`
- `IsEquivalentIntervalTo(i: IBasicInterval<TBoundary>) : bool`

### Metric-Assisted Extension Methods (Requires Measurer)
- `Measure(measurer: ILengthOperator<TBoundary, TLength>) : TLength`
- `WithMeasure(measurer: ILengthOperator<TBoundary, TLength>) : BasicMeasuredInterval<TBoundary, TLength>`
- `Shift(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>) : BasicInterval<TBoundary>`
- `ShiftStart(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>) : BasicInterval<TBoundary>`
- `ShiftEnd(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>) : BasicInterval<TBoundary>`
- `LengthOfIntersect(i: IBasicInterval<TBoundary>, measurer: ICanMeasure<TBoundary, TLength>) : TLength`
- `Subtract(i: IBasicInterval<TBoundary>, measurer: ILengthOperator<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`
- `Union(i: IBasicInterval<TBoundary>, measurer: ILengthOperator<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`
- `Partition(step: TLength, measurer: ILengthOperator<TBoundary, TLength>, intermediateEndsClosed: bool = false) : DisjointIntervalSet<TBoundary, TLength>`

---

## Methods available from `FullInterval<TBoundary, TLength>` (Full Intervals)

See the [Full Intervals Guide](full-intervals.md) for full descriptions and visual diagrams.

- `DefaultMeasure() : TLength`
- `Shift(offset: TLength) : BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftStart(offset: TLength) : BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftEnd(offset: TLength) : BasicMeasuredInterval<TBoundary, TLength>`
- `LengthOfIntersect(i: IBasicInterval<TBoundary>) : TLength`
- `Subtract(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`
- `ToIntervalSet() : DisjointIntervalSet<TBoundary, TLength>`
- `Partition(step: TLength, intermediateEndsClosed: bool = false) : DisjointIntervalSet<TBoundary, TLength>`
- *Inherited from `BasicInterval<TBoundary>`*: `Covers`, `Intersects`, `Intersect`, `Join`, `IsContiguouslyFollowedBy`, `IsContiguouslyPrecededBy`, `StartsBefore`, `IsEquivalentIntervalTo`.

---

## Methods available from `IDisjointIntervalSet<TBoundary, TLength>` (Interval Sets)

See the [Interval Sets Guide](interval-sets/index.md) for concepts, invariants, and details.

- [`Consolidate() : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/consolidate.md)
- [`Covers(t: TBoundary) : bool`](interval-sets/covers.md)
- [`Covers(i: IBasicInterval<TBoundary>) : bool`](interval-sets/covers.md)
- [`Intersect(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/intersect.md)
- [`Join(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/join.md)
- [`Join(s: IDisjointIntervalSet<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/join.md)
- [`GetBoundingInterval() : BasicMeasuredInterval<TBoundary, TLength>`](interval-sets/get-bounding-interval.md)

---

# Factory Methods

## Basic Intervals (`BasicInterval<TBoundary>`)
- `BasicInterval<TBoundary>.CreateClosed(start, end)`
- `BasicInterval<TBoundary>.CreateOpen(start, end)`
- `BasicInterval<TBoundary>.CreatePoint(boundary)`
- `BasicInterval<TBoundary>.From(interval)`

## Full Interval Utility Classes (`DateTimeOffsetInterval`, `IntInterval`, `DoubleInterval`, etc.)
- `CreateClosed(start, end)`
- `CreateOpen(start, end)`
- `CreatePoint(boundary)`

## Interval Sets (`DisjointIntervalSet<TBoundary, TLength>`)
- `DisjointIntervalSet(lengthOperator)`
- `DisjointIntervalSet(lengthOperator, params intervals[])`
- `DisjointIntervalSet(lengthOperator, IEnumerable<intervals>)`
