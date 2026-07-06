using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class ComparableExtensionsTests
{
    [Theory]
    [InlineData(5, 3, true)]
    [InlineData(3, 5, false)]
    [InlineData(5, 5, false)]
    public void IsGreaterThan_Int_ReturnsExpectedResult(int value, int other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5.5, 3.3, true)]
    [InlineData(3.3, 5.5, false)]
    [InlineData(5.5, 5.5, false)]
    public void IsGreaterThan_Double_ReturnsExpectedResult(double value, double other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("b", "a", true)]
    [InlineData("a", "b", false)]
    [InlineData("a", "a", false)]
    public void IsGreaterThan_String_ReturnsExpectedResult(string value, string other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5, 0, 10, true)]   // current is exactly between min and max
    [InlineData(0, 0, 10, false)]  // current == min
    [InlineData(10, 0, 10, true)]  // current == max
    [InlineData(-1, 0, 10, false)] // current < min
    [InlineData(11, 0, 10, false)] // current > max
    [InlineData(5, 5, 5, false)]   // min == max, current == min == max
    public void IsBetweenMaxIncluded_WithVariousBoundaries_ReturnsExpectedResult(int current, int min, int max, bool expectedResult)
    {
        // Act
        bool result = current.IsBetweenMaxIncluded(min, max);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void IsBetweenMaxIncluded_MaxLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        int current = 5;
        int min = 10;
        int max = 0;

        // Act
        Action act = () => current.IsBetweenMaxIncluded(min, max);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*max*");
    }

    [Fact]
    public void IsBetweenBothIncluded_WhenMaxIsLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        int min = 10;
        int max = 5;
        int current = 7;

        Action act = () => current.IsBetweenBothIncluded(min, max);

        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("max");
    }

    [Theory]
    [InlineData(5, 1, 10)] // Current is between min and max
    [InlineData(1, 1, 10)] // Current is equal to min
    [InlineData(10, 1, 10)] // Current is equal to max
    [InlineData(5, 5, 5)] // Min equals max and current equals them
    public void IsBetweenBothIncluded_WhenCurrentIsWithinBoundaries_ReturnsTrue(int current, int min, int max)
    {
        bool result = current.IsBetweenBothIncluded(min, max);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 1, 10)] // Current is less than min
    [InlineData(11, 1, 10)] // Current is greater than max
    [InlineData(4, 5, 5)] // Min equals max and current is less
    [InlineData(6, 5, 5)] // Min equals max and current is greater
    public void IsBetweenBothIncluded_WhenCurrentIsOutsideBoundaries_ReturnsFalse(int current, int min, int max)
    {
        bool result = current.IsBetweenBothIncluded(min, max);

        result.Should().BeFalse();
    }
}
