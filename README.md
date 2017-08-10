# ephemeral
C# Library to handle time intervals

Supports for Open and Closed (time) intervals.
Supports common operations like Covers(), Intersect(), Join(), etc..


Interval Example:

```
var now = DateTimeOffset.UtcNow;
Interval yesterday = Interval.CreateOpen(now.AddDays(-1), now);
Interval today = yesterday.Shift(TimeSpan.FromDays(1));

Assert.IsFalse(yesterday.Overlaps(today));
```

Interval Collection Example:

```
DisjointIntervalCollection collection = new DisjointIntervalCollection();
collection.Add(yesterday);
collection.Add(today);

Assert.AreEqual(collection.Start, yesterday.Start);
Assert.AreEqual(collection.End, today.End);

```


Copyright (c) 2016 Alberto Gregorio

