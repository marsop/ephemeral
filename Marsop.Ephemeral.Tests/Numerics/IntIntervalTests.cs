using System;
using FluentAssertions;
using Marsop.Ephemeral.Numerics;
using Xunit;

namespace Marsop.Ephemeral.Tests.Numerics;

public class IntIntervalTests
{
    [Fact]
    public void CreateClosed_ShouldCreateIntervalWithBothEndsIncluded()
    {
        var interval = IntInterval.CreateClosed(5, 10);

        interval.Start.Should().Be(5);
        interval.End.Should().Be(10);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void CreateOpen_ShouldCreateIntervalWithBothEndsExcluded()
    {
        var interval = IntInterval.CreateOpen(5, 10);

        interval.Start.Should().Be(5);
        interval.End.Should().Be(10);
        interval.StartIncluded.Should().BeFalse();
        interval.EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void CreatePoint_ShouldCreateIntervalWithSameStartAndEndIncluded()
    {
        var interval = IntInterval.CreatePoint(7);

        interval.Start.Should().Be(7);
        interval.End.Should().Be(7);
        interval.StartIncluded.Should().BeTrue();
        interval.EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void DefaultMeasure_ShouldReturnDifference()
    {
        var interval = IntInterval.CreateClosed(5, 10);
        interval.DefaultMeasure().Should().Be(5);
    }

    [Fact]
    public void ToString_ShouldReturnExpectedFormat()
    {
        var interval = IntInterval.CreateClosed(5, 10);
        interval.ToString().Should().Be("[5, 10]");

        var openInterval = IntInterval.CreateOpen(5, 10);
        openInterval.ToString().Should().Be("(5, 10)");
    }
}
