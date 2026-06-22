using System;
using FluentAssertions;
using Marsop.Ephemeral.Net6.Temporal;
using Xunit;

namespace Marsop.Ephemeral.Net6.Tests.Temporal;

public class DateOnlyIntervalTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2023, 1, 10);
        var interval = new DateOnlyInterval(start, end, true, false);

        interval.Start.Should().Be(start);
        interval.End.Should().Be(end);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreateClosed_SetsPropertiesCorrectly()
    {
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2023, 1, 10);
        var interval = DateOnlyInterval.CreateClosed(start, end);

        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void CreateOpen_SetsPropertiesCorrectly()
    {
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2023, 1, 10);
        var interval = DateOnlyInterval.CreateOpen(start, end);

        interval.StartIncluded.Should().BeFalse();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreatePoint_SetsPropertiesCorrectly()
    {
        var boundary = new DateOnly(2023, 1, 1);
        var interval = DateOnlyInterval.CreatePoint(boundary);

        interval.Start.Should().Be(boundary);
        interval.End.Should().Be(boundary);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void DefaultMeasure_ShouldBeCorrect_ForStandardDates()
    {
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2023, 1, 10);
        var interval = DateOnlyInterval.CreateClosed(start, end);

        var duration = interval.DefaultMeasure();

        duration.Should().Be(9);
    }

    [Fact]
    public void DefaultMeasure_ShouldBeCorrect_ForLeapYear()
    {
        var start = new DateOnly(2024, 2, 28);
        var end = new DateOnly(2024, 3, 1);
        var interval = DateOnlyInterval.CreateClosed(start, end);

        var duration = interval.DefaultMeasure();

        duration.Should().Be(2);
    }

    [Fact]
    public void DefaultMeasure_ShouldBeCorrect_ForMinValueToMaxValue()
    {
        var interval = DateOnlyInterval.CreateClosed(DateOnly.MinValue, DateOnly.MaxValue);

        var duration = interval.DefaultMeasure();

        var expectedDuration = (DateOnly.MaxValue.DayNumber - DateOnly.MinValue.DayNumber);
        duration.Should().Be(expectedDuration);
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2023, 1, 10);

        var closedInterval = DateOnlyInterval.CreateClosed(start, end);
        closedInterval.ToString().Should().Be($"[{start}, {end}]");

        var openInterval = DateOnlyInterval.CreateOpen(start, end);
        openInterval.ToString().Should().Be($"({start}, {end})");
    }

    [Fact]
    public void OperatorApply_ShouldAddDaysCorrectly()
    {
        var interval = DateOnlyInterval.CreatePoint(new DateOnly(2023, 1, 1));
        var result = interval.Operator.Apply(interval.Start, 10);

        result.Should().Be(new DateOnly(2023, 1, 11));
    }

    [Fact]
    public void OperatorZero_ShouldReturnZero()
    {
        var interval = DateOnlyInterval.CreatePoint(new DateOnly(2023, 1, 1));
        interval.Operator.Zero().Should().Be(0);
    }
}
