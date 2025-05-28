The following methods are available when any class implements `IDisjointIntervalSet`:

- `Covers(t: DateTime) : bool`
- `Join(s: IDisjointIntevalSet) : IDisjointIntevalSet`
- `Join(i: IInterval) : IDisjointIntevalSet`
- `Intersect(i : IInterval) : IDisjointIntevalSet`
- `Consolidate() : IDisjointIntevalSet`

The following methods are available when any class implements `IInterval` (as extension methods):

- `Covers(t: DateTimeOffset) : bool` *
- `Shift(t: TimeSpan): Interval` *
- `Covers(i: IInterval) : bool` *
- `DurationOfIntersect(i: IInterval): TimeSpan` *
- `DurationUntilNow(tp: TimeProvider): TimeSpan`
- `Intersect(i : IInterval) : Option<IInterval>` *
- `Intersects(i: IInterval): bool`
- `IsContiguouslyFollowedBy(i: IInterval) : bool`
- `IsContiguouslyPrecededBy(i: IInterval) : bool`
- `StartsBefore(i: IInterval) : bool`
- `ToInterval(): Interval`
- `Union(i: IInterval) : IDisjointIntervalSet`

Static methods to create `Interval`

- `CreateClosed`
- `CreateOpen`
- `CreatePoint`

- `Intersect` 
- `Join`
- `Subtract(i: IInterval) : IDisjointIntervalSet`




