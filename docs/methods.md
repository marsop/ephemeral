# Extension Methods

The following methods are implemented as extension methods to be able to use them from your own implementations.

## Methods available from `IDisjointIntervalSet<TBoundary, TLength>`

- `Covers(t: TBoundary) : bool`
- `Covers(i: IBasicInterval<TBoundary>) : bool`
- `Join(s: IDisjointIntervalSet<TBoundary, TLength>) : DisjointIntervalSet<TBoundary, TLength>`
- `Join(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`
- `Intersect(i : IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`
- `Consolidate() : DisjointIntervalSet<TBoundary, TLength>`
  - Creates a new Set with the minimum number of intervals inside (merges together intervals that are continguous)
- `GetBoundingInterval() : BasicMeasuredInterval<TBoundary, TLength>`

## Methods available from `IBasicInterval<TBoundary>`

- `Measure(measurer: ILengthOperator<TBoundary, TLength>): TLength`
- `WithMeaure(measurer: ILengthOperator<TBoundary, TLength>): BasicMeasuredInterval<TBoundary, TLength>`
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

## Methods available from `IInterval<TBoundary, TLength>`

- `Shift(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftStart(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `ShiftEnd(offset: TLength): BasicMeasuredInterval<TBoundary, TLength>`
- `LengthOfIntersect(i: IBasicInterval<TBoundary>): TLength`
- `Subtract(i: IBasicInterval<TBoundary>) : DisjointIntervalSet<TBoundary, TLength>`
- `ToIntervalSet() : DisjointIntervalSet<TBoundary, TLength>`

# Factory Methods

## Utility classes (`DateTimeOffsetInterval`, `IntInterval`, `DoubleInterval`, etc.)

- `CreateClosed(start, end)`
- `CreateOpen(start, end)`
- `CreatePoint(start)`

## `DisjointIntervalSet<TBoundary, TLength>`

TBD
