# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.0] - 2025-06-02

### Added
- Generalized intervals beyond `DateTimeOffset` using `IBasicInterval<TBoundary>` and `BasicInterval<TBoundary>`.
- Core generic interval interfaces and implementation.
- .NET 6 targeted extensions and types (`DateOnlyInterval`, `TimeOnlyInterval`).
- Polyglot notebooks for documentation (`notebooks/` directory).
- Extensive new test suites for generic interval behaviors.

### Changed
- Promoted 0.2.0 from pre-release (beta) to stable release.
- Refactored `DateTimeOffsetInterval` to inherit from new generic classes.
- Target frameworks updated across projects to include `net6.0` and `net10.0` for tests.
- Replaced GroupBy and ToDictionary with a single pass loop in IntervalSetExtensions.Join to reduce heap allocations and execution time.

## [0.1.10] - 2020-07-01

### Added
- Initial interval operations including overlapping, unions, and intersections on `DateTimeOffset` types.
- DisjointIntervalSet implementation.
