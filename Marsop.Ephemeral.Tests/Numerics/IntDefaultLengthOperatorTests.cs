using System;
using FluentAssertions;
using Marsop.Ephemeral.Numerics;
using Marsop.Ephemeral.Core;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Tests.Numerics;

public class IntDefaultLengthOperatorTests
{
    [Fact]
    public void Apply_ShouldAddLengthToBoundary()
    {
        var result = IntDefaultLengthOperator.Instance.Apply(5, 2);
        result.Should().Be(7);
    }

    [Fact]
    public void Measure_ShouldReturnDifferenceBetweenEndAndStart()
    {
        var intervalMock = new Mock<IBasicInterval<int>>();
        intervalMock.Setup(i => i.Start).Returns(2);
        intervalMock.Setup(i => i.End).Returns(7);

        var result = IntDefaultLengthOperator.Instance.Measure(intervalMock.Object);
        result.Should().Be(5);
    }

    [Fact]
    public void Measure_WithIntInterval_ShouldReturnDifference()
    {
        var interval = IntInterval.CreateClosed(2, 7);
        var result = IntDefaultLengthOperator.Instance.Measure(interval);
        result.Should().Be(5);
    }

    [Fact]
    public void Zero_ShouldReturnZero()
    {
        var result = IntDefaultLengthOperator.Instance.Zero();
        result.Should().Be(0);
    }
}
