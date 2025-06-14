# Extension Methods

The following methods are implemented as extension methods to be able to use them from your own implementations.

## Methods available from `IDisjointIntervalSet`

- `Covers(t: DateTime) : bool`
- `Covers(i: IInterval) : bool`
- `Join(s: IDisjointIntevalSet) : DisjointIntevalSet`
- `Join(i: IInterval) : DisjointIntevalSet`
- `Intersect(i : IInterval) : DisjointIntevalSet`
- `Consolidate() : DisjointIntevalSet`
  - Creates a new Set with the minimum number of intervals inside (merges together intervals that are continguous)
- `GetBoundingInterval() : Interval`

## Methods available from `IInterval`

- `Covers(t: DateTimeOffset) : bool` *
- `Shift(t: TimeSpan): Interval` *
- `Covers(i: IInterval) : bool` *
- `DurationOfIntersect(i: IInterval): TimeSpan` *
- `Intersect(i : IInterval) : Option<Interval>` *
- `Intersects(i: IInterval): bool`
- `Join(i: IInterval): Interval`
- `IsContiguouslyFollowedBy(i: IInterval) : bool`
- `IsContiguouslyPrecededBy(i: IInterval) : bool`
- `StartsBefore(i: IInterval) : bool`
- `ToInterval(): Interval`
- `Union(i: IInterval) : DisjointIntervalSet`
- `Subtract(i: IInterval) : DisjointIntervalSet`

# Factory Methods

## `Interval`

- `CreateClosed(start, end)`
- `CreateOpen(start, end)`
- `CreatePoint(start)`

## `DisjointIntervalSet` 

TBD




