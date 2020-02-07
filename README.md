# ephemeral

![](EphemeralLogoCropped.png)

C# Library to handle time intervals (composite start and end)

- Support for open and closed (time) intervals.
- Support for common operations like Covers(), Intersect(), Join(), etc..
- Support for (disjoint) collections of intervals.

Please check all the available options in the [Wiki](https://github.com/marsop/ephemeral/wiki).

## Interval Example

```
var now = DateTimeOffset.UtcNow;
Interval yesterday = Interval.CreateOpen(now.AddDays(-1), now);
Interval today = yesterday.Shift(TimeSpan.FromDays(1));

yesterday.Overlaps(today); // returns true
```

## Interval Collection Example

```
IDisjointIntervalSet collection = new DisjointIntervalSet();
collection.Add(yesterday);
collection.Add(today);
collection.Start == yesterday.Start; // true
collection.End == today.End; // true

var collection2 = yesterday.Union(today);
collection1.equals(collection2); // true

var consolidatedCollection = collection2.Consolidate();
collection2.Count(); // 2
consolidatedCollection.Count(); // 1

```
