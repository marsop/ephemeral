using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class ComparableExtensionsTests
{
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
}
