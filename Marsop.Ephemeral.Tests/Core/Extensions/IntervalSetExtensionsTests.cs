using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class IntervalSetExtensionsTests
{
    private static readonly ILengthOperator<int, int> Operator = IntDefaultLengthOperator.Instance;

    private sealed class UncheckedInterval : IBasicInterval<int>
    {
        public UncheckedInterval(int start, int end, bool startIncluded, bool endIncluded)
        {
            Start = start;
            End = end;
            StartIncluded = startIncluded;
            EndIncluded = endIncluded;
        }

        public int Start { get; }
        public bool StartIncluded { get; }
        public int End { get; }
        public bool EndIncluded { get; }
    }

    [Fact]
    public void GetBoundingInterval_EmptySet_ReturnsNone()
    {
        var set = new DisjointIntervalSet<int, int>(Operator);

        var result = set.GetBoundingInterval();

        result.HasValue.Should().BeFalse();
    }

    [Fact]
    public void GetBoundingInterval_NonEmptySet_ReturnsSomeBoundingInterval()
    {
        var set = new DisjointIntervalSet<int, int>(
            Operator,
            IntInterval.CreateOpen(2, 4),
            IntInterval.CreateClosed(6, 8));

        var result = set.GetBoundingInterval();

        result.HasValue.Should().BeTrue();
        result.MatchSome(bounds =>
        {
            bounds.Start.Should().Be(2);
            bounds.End.Should().Be(8);
            bounds.StartIncluded.Should().BeFalse();
            bounds.EndIncluded.Should().BeTrue();
            bounds.DefaultMeasure().Should().Be(6);
        });
    }

    [Fact]
    public void Operations_NullSet_ThrowArgumentNullException()
    {
        IDisjointIntervalSet<int, int> nullSet = null!;
        var validSet = new DisjointIntervalSet<int, int>(Operator);
        var validInterval = IntInterval.CreateClosed(1, 5);

        ((Action)(() => nullSet.Consolidate())).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => nullSet.Covers(3))).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => nullSet.Covers(validInterval))).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => nullSet.Intersect(validInterval))).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => nullSet.Join(validSet))).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => validSet.Join((IDisjointIntervalSet<int, int>)null!))).Should().Throw<ArgumentNullException>().WithParameterName("other");
        ((Action)(() => nullSet.Join(validInterval))).Should().Throw<ArgumentNullException>().WithParameterName("set");
        ((Action)(() => nullSet.GetBoundingInterval())).Should().Throw<ArgumentNullException>().WithParameterName("s");
    }

    [Fact]
    public void Operations_InvalidOrNullIntervalArgument_ThrowExpectedExceptions()
    {
        var emptySet = new DisjointIntervalSet<int, int>(Operator);
        var invalid = new UncheckedInterval(10, 5, true, true);
        IBasicInterval<int> nullInterval = null!;

        ((Action)(() => emptySet.Covers(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => emptySet.Covers(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => emptySet.Intersect(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => emptySet.Intersect(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => emptySet.Join(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => emptySet.Join(invalid))).Should().Throw<InvalidLengthException>();
    }
}
