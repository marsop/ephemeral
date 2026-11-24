# Extension Methods

The following methods are implemented as extension methods to be able to use them from your own implementations.

## Methods available from `IDisjointIntervalSet<TBoundary, TLength>`

For detailed concepts and invariants, see the [Interval Sets Overview](interval-sets/index.md).

- [`Covers(t: TBoundary) : bool`](interval-sets/covers.md)
- [`Covers(i: IBasicInterval<TBoundary>) : bool`](interval-sets/covers.md)
- [`Join(s: IDisjointIntervalSet<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/join.md)
- [`Join(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/join.md)
- [`Intersect(i : IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/intersect.md)
- [`Consolidate() : DisjointIntervalSet<TBoundary, TLength>`](interval-sets/consolidate.md)
  - Creates a new Set with the minimum number of intervals inside (merges together intervals that are contiguous)
- [`GetBoundingInterval() : BasicMeasuredInterval<TBoundary, TLength>`](interval-sets/get-bounding-interval.md)

## Methods available from `IBasicInterval<TBoundary>` (Basic Intervals)

- `Measure(measurer: ILengthOperator<TBoundary, TLength>): TLength`
- `WithMeasure(measurer: ILengthOperator<TBoundary, TLength>): BasicMeasuredInterval<TBoundary, TLength>`
- `IsEquivalentIntervalTo(i: IBasicInterval<TBoundary>) : bool`
- `Covers(t: TBoundary) : bool`
- `Covers(i: IBasicInterval<TBoundary>) : bool`
- `Shift(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>): BasicInterval<TBoundary>`
- `ShiftStart(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>): BasicInterval<TBoundary>`
- `ShiftEnd(offset: TLength, measurer: ILengthOperator<TBoundary, TLength>): BasicInterval<TBoundary>`
- `LengthOfIntersect(i: IBasicInterval<TBoundary>, measurer: ILengthOperator<TBoundary, TLength>): TLength`
- `Intersect(i : IBasicInterval<TBoundary>) : Option<BasicInterval<TBoundary>>`
- `Intersects(i: IBasicInterval<TBoundary>): bool`
- `IsContiguouslyFollowedBy(i: IBasicInterval<TBoundary>) : bool`
- `IsContiguouslyPrecededBy(i: IBasicInterval<TBoundary>) : bool`
- `StartsBefore(i: IBasicInterval<TBoundary>) : bool`
- `Join(i: IBasicInterval<TBoundary>): BasicInterval<TBoundary>`
- `Subtract(i: IBasicInterval<TBoundary>, measurer: ILengthOperator<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`
- `Union(i: IBasicInterval<TBoundary>, measurer: ILengthOperator<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`
- `Partition(step: TLength, measurer: ILengthOperator<TBoundary, TLength>, intermediateEndsClosed: bool = false): DisjointIntervalSet<TBoundary, TLength>`

## Methods available from `FullInterval<TBoundary, TLength>` (Full Intervals)

- `DefaultMeasure() : TLength`
- `Shift(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftStart(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftEnd(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `LengthOfIntersect(i: IBasicInterval<TBoundary>): TLength`
- `Subtract(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`
- `ToIntervalSet() : DisjointIntervalSet<TBoundary, TLength>`
- `Partition(step: TLength, intermediateEndsClosed: bool = false): DisjointIntervalSet<TBoundary, TLength>`

# Factory Methods

## Utility classes (`DateTimeOffsetInterval`, `IntInterval`, `DoubleInterval`, etc.)

- `CreateClosed(start, end)`
- `CreateOpen(start, end)`
- `CreatePoint(start)`

## `DisjointIntervalSet<TBoundary, TLength>`

See [Interval Sets Guide](interval-sets/index.md) for full constructor and collection usage.

- `DisjointIntervalSet(lengthOperator)`
- `DisjointIntervalSet(lengthOperator, params intervals[])`
- `DisjointIntervalSet(lengthOperator, IEnumerable<intervals>)`
