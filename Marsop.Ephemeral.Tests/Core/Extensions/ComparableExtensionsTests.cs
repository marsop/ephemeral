using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class ComparableExtensionsTests
{
    [Fact]
    public void IsBetweenBothIncluded_MaxLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        int current = 5;
        int min = 10;
        int max = 0;

        // Act
        Action act = () => current.IsBetweenBothIncluded(min, max);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("max");
    }
}
