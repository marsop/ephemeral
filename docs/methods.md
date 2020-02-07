The following methods are available when you implement `IDisjointIntervalSet`:

- `Covers(t: DateTime) : bool`
- `Join(s: IDisjointIntevalSet) : IDisjointIntevalSet`
- `Join(i: IInterval) : IDisjointIntevalSet`
- `Intersect(i : IInterval) : IDisjointIntevalSet`
- `Consolidate() : IDisjointIntevalSet`

The following methods are available when you implement `IInterval`:

- `Covers(t: DateTime) : bool`
- `Covers(i: IInterval) : bool`
- `DurationUntilNow(): TimeSpan`
- `ToInterval(): IInterval`
- `Union(i: IInterval) : IDisjointIntevalSet`
- `Intersect(i : IInterval) : Option<IInterval>`
- `DurationOfIntersect(IInterval) : TimeSpan`
- `Intersects(i: IInterval): bool`
- `IsContiguouslyFollowedBy(i: IInterval) : bool`
- `IsContiguouslyPrecededBy(i: IInterval) : bool`
- `StartsBefore(i: IInterval) : bool`
