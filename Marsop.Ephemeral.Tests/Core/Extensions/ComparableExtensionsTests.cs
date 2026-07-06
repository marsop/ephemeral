using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class ComparableExtensionsTests
{
    [Fact]
    public void IsBetweenMaxIncluded_MaxLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var current = 5;
        var min = 10;
        var max = 0;

        // Act
        Action act = () => current.IsBetweenMaxIncluded(min, max);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("max");
    }
}
