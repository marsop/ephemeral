using System;
using FluentAssertions;
using Marsop.Ephemeral.Numerics;
using Marsop.Ephemeral.Core;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Tests.Numerics;

public class DoubleDefaultLengthOperatorTests
{
    [Fact]
    public void Apply_ShouldAddLengthToBoundary()
    {
        var result = DoubleDefaultLengthOperator.Instance.Apply(5.5, 2.0);
        result.Should().Be(7.5);
    }

    [Fact]
    public void Measure_ShouldReturnDifferenceBetweenEndAndStart()
    {
        var intervalMock = new Mock<IBasicInterval<double>>();
        intervalMock.Setup(i => i.Start).Returns(2.0);
        intervalMock.Setup(i => i.End).Returns(7.5);

        var result = DoubleDefaultLengthOperator.Instance.Measure(intervalMock.Object);
        result.Should().Be(5.5);
    }

    [Fact]
    public void Measure_WithDoubleInterval_ShouldReturnDifference()
    {
        var interval = DoubleInterval.CreateClosed(2.0, 7.5);
        var result = DoubleDefaultLengthOperator.Instance.Measure(interval);
        result.Should().Be(5.5);
    }

    [Fact]
    public void Zero_ShouldReturnZero()
    {
        var result = DoubleDefaultLengthOperator.Instance.Zero();
        result.Should().Be(0.0);
    }
}
