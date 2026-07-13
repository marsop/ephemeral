using System;
using FluentAssertions;
using Marsop.Ephemeral.Net6.Temporal;
using Xunit;

namespace Marsop.Ephemeral.Net6.Tests.Temporal;

public class TimeOnlyIntervalTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var start = new TimeOnly(1, 0);
        var end = new TimeOnly(2, 0);
        var interval = new TimeOnlyInterval(start, end, true, false);

        interval.Start.Should().Be(start);
        interval.End.Should().Be(end);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreateClosed_SetsPropertiesCorrectly()
    {
        var start = new TimeOnly(1, 0);
        var end = new TimeOnly(2, 0);
        var interval = TimeOnlyInterval.CreateClosed(start, end);

        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void CreateOpen_SetsPropertiesCorrectly()
    {
        var start = new TimeOnly(1, 0);
        var end = new TimeOnly(2, 0);
        var interval = TimeOnlyInterval.CreateOpen(start, end);

        interval.StartIncluded.Should().BeFalse();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreatePoint_SetsPropertiesCorrectly()
    {
        var boundary = new TimeOnly(1, 0);
        var interval = TimeOnlyInterval.CreatePoint(boundary);

        interval.Start.Should().Be(boundary);
        interval.End.Should().Be(boundary);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void DefaultMeasure_ShouldBeCorrect_ForStandardTimes()
    {
        var start = new TimeOnly(1, 0);
        var end = new TimeOnly(2, 0);
        var interval = TimeOnlyInterval.CreateClosed(start, end);

        var duration = interval.DefaultMeasure();

        duration.Should().Be(TimeSpan.FromHours(1));
    }

    [Fact]
    public void DefaultMeasure_ShouldBeCorrect_ForMinValueToMaxValue()
    {
        var interval = TimeOnlyInterval.CreateClosed(TimeOnly.MinValue, TimeOnly.MaxValue);

        var duration = interval.DefaultMeasure();

        var expectedDuration = TimeOnly.MaxValue.ToTimeSpan() - TimeOnly.MinValue.ToTimeSpan();
        duration.Should().Be(expectedDuration);
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var start = new TimeOnly(1, 0);
        var end = new TimeOnly(2, 0);

        var closedInterval = TimeOnlyInterval.CreateClosed(start, end);
        closedInterval.ToString().Should().Be($"[{start}, {end}]");

        var openInterval = TimeOnlyInterval.CreateOpen(start, end);
        openInterval.ToString().Should().Be($"({start}, {end})");
    }

    [Fact]
    public void OperatorApply_ShouldAddTimespanCorrectly()
    {
        var interval = TimeOnlyInterval.CreatePoint(new TimeOnly(1, 0));
        var result = interval.Operator.Apply(interval.Start, TimeSpan.FromHours(1));

        result.Should().Be(new TimeOnly(2, 0));
    }

    [Fact]
    public void OperatorZero_ShouldReturnZero()
    {
        var interval = TimeOnlyInterval.CreatePoint(new TimeOnly(1, 0));
        interval.Operator.Zero().Should().Be(TimeSpan.Zero);
    }
}
