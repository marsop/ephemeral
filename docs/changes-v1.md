# Changelog

## Version 0.2.0

- Added XML documentation for all types and public members in `Marsop.Ephemeral` and `Marsop.Ephemeral.Net6`.
- Enabled generation of XML documentation files.

## Version 1.x

- New "base" classes `BasicInterval` and `FullInterval` introducing generics.
- Utility classes have been included to help migrating: `DateTimeOffsetInterval`, `IntInterval`, `DoubleInterval`.
- `Duration()` is now called `Length()` and it is an extension method.
- `DurationUntilNow()` is no longer supported.
- `Interval` has been renamed or replaced by specialized utilities.
- `AggregatedDuration` is no longer supported in DisjointedSets.

## Version 0.x

- Inclusion of `DisjointIntervalSet` to collect intervals without overlaps.