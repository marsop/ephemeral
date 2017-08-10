# ephemeral
C# Library to handle time intervals

Example:

```
var now = DateTimeOffset.UtcNow;
Interval yesterday = new Interval(now.AddDays(-1), now);
Interval today = yesterday.Shift(TimeSpan.FromDays(1));
```


Copyright (c) 2016 Alberto Gregorio

