using System;
using FluentAssertions;
using Marsop.Ephemeral.Numerics;
using Xunit;

namespace Marsop.Ephemeral.Tests.Numerics;

public class DoubleIntervalTests
{
    [Fact]
    public void CreateClosed_ShouldCreateIntervalWithBothEndsIncluded()
    {
        var interval = DoubleInterval.CreateClosed(5.5, 10.5);

        interval.Start.Should().Be(5.5);
        interval.End.Should().Be(10.5);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void CreateOpen_ShouldCreateIntervalWithBothEndsExcluded()
    {
        var interval = DoubleInterval.CreateOpen(5.5, 10.5);

        interval.Start.Should().Be(5.5);
        interval.End.Should().Be(10.5);
        interval.StartIncluded.Should().BeFalse();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreatePoint_ShouldCreateIntervalWithSameStartAndEndIncluded()
    {
        var interval = DoubleInterval.CreatePoint(7.7);

        interval.Start.Should().Be(7.7);
        interval.End.Should().Be(7.7);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void DefaultMeasure_ShouldReturnDifference()
    {
        var interval = DoubleInterval.CreateClosed(5.5, 10.5);
        interval.DefaultMeasure().Should().Be(5.0);
    }

    [Fact]
    public void ToString_ShouldReturnExpectedFormat()
    {
        var interval = DoubleInterval.CreateClosed(5.5, 10.5);
        interval.ToString().Should().Be("[5.5, 10.5]");

        var openInterval = DoubleInterval.CreateOpen(5.5, 10.5);
        openInterval.ToString().Should().Be("(5.5, 10.5)");
    }
}
