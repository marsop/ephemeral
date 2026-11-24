# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Comprehensive documentation and guides explaining Basic Intervals (`IBasicInterval<TBoundary>`) versus Full Intervals (`FullInterval<TBoundary, TLength>`), interval length measurement (`DefaultMeasure()`, `Measure()`), and generic basic interval usage.
- NuGet package version badge to documentation homepage (`docs/index.md`).

### Changed
- Migrated NuGet package publishing from static API key credentials to OpenID Connect (OIDC) Trusted Publishing via `NuGet/login`.
- Standardized GitHub Actions workflow filenames and display titles to use kebab-case `verb-target` convention (`build-code.yml`, `publish-docs.yml`, `publish-nuget.yml`).
- Switched repository logo to raw GitHub URL to ensure it renders properly on NuGet.org.

## [0.3.0] - 2026-11-23

### Added
- Packaged AI agent skill `adapt-to-ephemeral` inside the NuGet package under `skills/adapt-to-ephemeral/` to assist coding agents and developers in refactoring codebases to Ephemeral intervals.
- Helper discovery script `find-interval-candidates.ps1` to detect candidate interval patterns in existing C# codebases.
- Visual examples and documentation for `ShiftStart()`, `ShiftEnd()`, `Shift()`, `IsEquivalentIntervalTo()`, `IsContiguouslyPrecededBy()`, `LengthOfIntersect()`, `DateOnlyInterval`, `TimeOnlyInterval`, and `DoubleInterval`.

## [0.2.0] - 2025-06-02

### Added
- Generalized intervals beyond `DateTimeOffset` using `IBasicInterval<TBoundary>` and `BasicInterval<TBoundary>`.
- Core generic interval interfaces and implementation.
- .NET 6 targeted extensions and types (`DateOnlyInterval`, `TimeOnlyInterval`).
- Utility classes for standard types: `DateTimeOffsetInterval`, `IntInterval`, `DoubleInterval`.
- XML documentation comments for all public types and members in `Marsop.Ephemeral` and `Marsop.Ephemeral.Net6`.
- Polyglot notebooks for documentation (`notebooks/` directory).
- Extensive new test suites for generic interval behaviors.

### Changed
- Refactored `DateTimeOffsetInterval` to inherit from new generic classes.
- Renamed `Duration()` to `Length()` implemented as an extension method.
- Replaced legacy `Interval` class with `BasicInterval`, `FullInterval`, and specialized utility classes.
- Target frameworks updated across projects to include `net6.0` and `net10.0` for tests.
- Replaced GroupBy and ToDictionary with a single pass loop in IntervalSetExtensions.Join to reduce heap allocations and execution time.

### Removed
- `DurationUntilNow()` method.
- `AggregatedDuration` property on `DisjointIntervalSet`.

## [0.1.10] - 2020-07-01

### Added
- Initial interval operations including overlapping, unions, and intersections on `DateTimeOffset` types.
- DisjointIntervalSet implementation.

[Unreleased]: https://github.com/marsop/ephemeral/compare/0.3.0...HEAD
[0.3.0]: https://github.com/marsop/ephemeral/compare/0.2.0...0.3.0
[0.2.0]: https://github.com/marsop/ephemeral/compare/v0.1.10...0.2.0
[0.1.10]: https://github.com/marsop/ephemeral/releases/tag/v0.1.10

