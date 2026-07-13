using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class BasicIntervalExtensionsTests
{
    private record TestBasicInterval : BasicInterval<int>
    {
        public TestBasicInterval(int start, int end, bool startIncluded, bool endIncluded)
            : base(start, end, startIncluded, endIncluded)
        {
        }
    }

    [Fact]
    public void Covers_Boundary_NullInterval_ThrowsArgumentNullException()
    {
        // Arrange
        IBasicInterval<int> interval = null!;

        // Act
        Action action = () => interval.Covers(5);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(-5)]
    public void Covers_Boundary_LessThanStart_ReturnsFalse(int boundary)
    {
        // Arrange
        var interval = new TestBasicInterval(5, 10, true, true);

        // Act
        var result = interval.Covers(boundary);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(11)]
    [InlineData(20)]
    public void Covers_Boundary_GreaterThanEnd_ReturnsFalse(int boundary)
    {
        // Arrange
        var interval = new TestBasicInterval(5, 10, true, true);

        // Act
        var result = interval.Covers(boundary);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Covers_Boundary_EqualToStart_NotIncluded_ReturnsFalse()
    {
        // Arrange
        var interval = new TestBasicInterval(5, 10, false, true);

        // Act
        var result = interval.Covers(5);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Covers_Boundary_EqualToEnd_NotIncluded_ReturnsFalse()
    {
        // Arrange
        var interval = new TestBasicInterval(5, 10, true, false);

        // Act
        var result = interval.Covers(10);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(5, true, true)]
    [InlineData(10, true, true)]
    [InlineData(7, true, true)]
    [InlineData(7, false, false)]
    public void Covers_Boundary_InsideInterval_ReturnsTrue(int boundary, bool startIncluded, bool endIncluded)
    {
        // Arrange
        var interval = new TestBasicInterval(5, 10, startIncluded, endIncluded);

        // Act
        var result = interval.Covers(boundary);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Intersect_DisjointIntervals_ReturnsNone()
    {
        // Arrange
        var first = new TestBasicInterval(1, 2, true, true);
        var second = new TestBasicInterval(3, 4, true, true);

        // Act
        var result = first.Intersect(second);

        // Assert
        result.HasValue.Should().BeFalse();
    }
}
