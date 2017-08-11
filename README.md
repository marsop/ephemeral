# ephemeral
C# Library to handle time intervals

Supports for Open and Closed (time) intervals.
Supports common operations like Covers(), Intersect(), Join(), etc..


Interval Example:

```
var now = DateTimeOffset.UtcNow;
Interval yesterday = Interval.CreateOpen(now.AddDays(-1), now);
Interval today = yesterday.Shift(TimeSpan.FromDays(1));

yesterday.Overlaps(today); // true
```

Interval Collection Example:

```
IDisjointIntervalSet collection = new DisjointIntervalSet();
collection.Add(yesterday);
collection.Add(today);
collection.Start == yesterday.Start; // true
collection.End == today.End; // true

var collection2 = yesterday.Union(today);
collection1.equals(collection2); // true

var collection3 = collection2.Consolidate();
collection2.Count(); // 2
collection3.Count(); // 1

```


Copyright (c) 2016 Alberto Gregorio

