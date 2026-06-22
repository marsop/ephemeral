using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Implementation;

public class BasicMeasuredIntervalTests
{
    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Constructor_WithValidParameters_SetsPropertiesCorrectly(bool startIncluded, bool endIncluded)
    {
        // Arrange
        int start = 5;
        int end = 10;
        int expectedMeasure = 5;

        var mockMeasureCalculator = new Mock<ICanMeasure<int, int>>();
        mockMeasureCalculator
            .Setup(m => m.Measure(It.IsAny<IBasicInterval<int>>()))
            .Returns(expectedMeasure);

        // Act
        var interval = new BasicMeasuredInterval<int, int>(
            start,
            end,
            startIncluded,
            endIncluded,
            mockMeasureCalculator.Object);

        // Assert
        interval.Start.Should().Be(start);
        interval.End.Should().Be(end);
        interval.StartIncluded.Should().Be(startIncluded);
        interval.EndIncluded.Should().Be(endIncluded);
        interval.DefaultMeasure().Should().Be(expectedMeasure);
    }

    [Fact]
    public void Constructor_NullMeasureCalculator_ThrowsArgumentNullException()
    {
        // Arrange
        int start = 5;
        int end = 10;

        // Act
        Action act = () => new BasicMeasuredInterval<int, int>(
            start,
            end,
            true,
            true,
            null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("measureCalculator");
    }

    [Fact]
    public void DefaultMeasure_ReturnsCalculatedMeasure()
    {
        // Arrange
        int expectedMeasure = 42;
        var mockMeasureCalculator = new Mock<ICanMeasure<int, int>>();
        mockMeasureCalculator
            .Setup(m => m.Measure(It.IsAny<IBasicInterval<int>>()))
            .Returns(expectedMeasure);

        var interval = new BasicMeasuredInterval<int, int>(
            0,
            10,
            true,
            true,
            mockMeasureCalculator.Object);

        // Act
        var measure = interval.DefaultMeasure();

        // Assert
        measure.Should().Be(expectedMeasure);
        mockMeasureCalculator.Verify(m => m.Measure(interval), Times.Once);
    }
}
