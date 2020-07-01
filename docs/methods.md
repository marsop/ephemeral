The following methods are available when any class implements `IDisjointIntervalSet`:

- `Covers(t: DateTime) : bool`
- `Join(s: IDisjointIntevalSet) : IDisjointIntevalSet`
- `Join(i: IInterval) : IDisjointIntevalSet`
- `Intersect(i : IInterval) : IDisjointIntevalSet`
- `Consolidate() : IDisjointIntevalSet`

The following methods are available when any class implements `IInterval`:

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
