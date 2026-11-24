using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;
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

    [Fact]
    public void Partition_NullInterval_ThrowsArgumentNullException()
    {
        // Arrange
        IBasicInterval<int> interval = null!;

        // Act
        Action act = () => interval.Partition(5, IntDefaultLengthOperator.Instance);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Partition_NullLengthOperator_ThrowsArgumentNullException()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 20, true, true);

        // Act
        Action act = () => interval.Partition(5, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Partition_ZeroOrNegativeStep_ThrowsArgumentOutOfRangeException(int step)
    {
        // Arrange
        var interval = new TestBasicInterval(10, 20, true, true);

        // Act
        Action act = () => interval.Partition(step, IntDefaultLengthOperator.Instance);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("step");
    }

    [Fact]
    public void Partition_IntermediateEndsOpen_SplitsIntoExpectedDisjointIntervals()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 28, true, true);

        // Act
        var result = interval.Partition(5, IntDefaultLengthOperator.Instance, intermediateEndsClosed: false);

        // Assert
        result.Should().HaveCount(4);

        result[0].Start.Should().Be(10);
        result[0].End.Should().Be(15);
        result[0].StartIncluded.Should().BeTrue();
        result[0].EndIncluded.Should().BeFalse();

        result[1].Start.Should().Be(15);
        result[1].End.Should().Be(20);
        result[1].StartIncluded.Should().BeTrue();
        result[1].EndIncluded.Should().BeFalse();

        result[2].Start.Should().Be(20);
        result[2].End.Should().Be(25);
        result[2].StartIncluded.Should().BeTrue();
        result[2].EndIncluded.Should().BeFalse();

        result[3].Start.Should().Be(25);
        result[3].End.Should().Be(28);
        result[3].StartIncluded.Should().BeTrue();
        result[3].EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void Partition_IntermediateEndsClosed_SplitsIntoExpectedDisjointIntervals()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 28, true, true);

        // Act
        var result = interval.Partition(5, IntDefaultLengthOperator.Instance, intermediateEndsClosed: true);

        // Assert
        result.Should().HaveCount(4);

        result[0].Start.Should().Be(10);
        result[0].End.Should().Be(15);
        result[0].StartIncluded.Should().BeTrue();
        result[0].EndIncluded.Should().BeTrue();

        result[1].Start.Should().Be(15);
        result[1].End.Should().Be(20);
        result[1].StartIncluded.Should().BeFalse();
        result[1].EndIncluded.Should().BeTrue();

        result[2].Start.Should().Be(20);
        result[2].End.Should().Be(25);
        result[2].StartIncluded.Should().BeFalse();
        result[2].EndIncluded.Should().BeTrue();

        result[3].Start.Should().Be(25);
        result[3].End.Should().Be(28);
        result[3].StartIncluded.Should().BeFalse();
        result[3].EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void Partition_OpenInterval_PreservesInitialStartAndFinalEndInclusions()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 28, false, false);

        // Act - intermediateEndsClosed = false
        var resultOpen = interval.Partition(5, IntDefaultLengthOperator.Instance, intermediateEndsClosed: false);

        // Assert
        resultOpen.Should().HaveCount(4);
        resultOpen[0].StartIncluded.Should().BeFalse();
        resultOpen[0].EndIncluded.Should().BeFalse();
        resultOpen[1].StartIncluded.Should().BeTrue();
        resultOpen[1].EndIncluded.Should().BeFalse();
        resultOpen[2].StartIncluded.Should().BeTrue();
        resultOpen[2].EndIncluded.Should().BeFalse();
        resultOpen[3].StartIncluded.Should().BeTrue();
        resultOpen[3].EndIncluded.Should().BeFalse();

        // Act - intermediateEndsClosed = true
        var resultClosed = interval.Partition(5, IntDefaultLengthOperator.Instance, intermediateEndsClosed: true);

        // Assert
        resultClosed.Should().HaveCount(4);
        resultClosed[0].StartIncluded.Should().BeFalse();
        resultClosed[0].EndIncluded.Should().BeTrue();
        resultClosed[1].StartIncluded.Should().BeFalse();
        resultClosed[1].EndIncluded.Should().BeTrue();
        resultClosed[2].StartIncluded.Should().BeFalse();
        resultClosed[2].EndIncluded.Should().BeTrue();
        resultClosed[3].StartIncluded.Should().BeFalse();
        resultClosed[3].EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void Partition_ExactMultipleStep_ProducesExpectedSubIntervals()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 25, true, true);

        // Act
        var result = interval.Partition(5, IntDefaultLengthOperator.Instance);

        // Assert
        result.Should().HaveCount(3);

        result[0].Start.Should().Be(10);
        result[0].End.Should().Be(15);
        result[0].StartIncluded.Should().BeTrue();
        result[0].EndIncluded.Should().BeFalse();

        result[1].Start.Should().Be(15);
        result[1].End.Should().Be(20);
        result[1].StartIncluded.Should().BeTrue();
        result[1].EndIncluded.Should().BeFalse();

        result[2].Start.Should().Be(20);
        result[2].End.Should().Be(25);
        result[2].StartIncluded.Should().BeTrue();
        result[2].EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void Partition_StepGreaterThanIntervalLength_ReturnsSingleIntervalMatchingSource()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 14, true, false);

        // Act
        var result = interval.Partition(10, IntDefaultLengthOperator.Instance);

        // Assert
        result.Should().HaveCount(1);
        result[0].Start.Should().Be(10);
        result[0].End.Should().Be(14);
        result[0].StartIncluded.Should().BeTrue();
        result[0].EndIncluded.Should().BeFalse();
    }

    [Fact]
    public void Partition_PointInterval_ReturnsSinglePointInterval()
    {
        // Arrange
        var interval = new TestBasicInterval(5, 5, true, true);

        // Act
        var result = interval.Partition(2, IntDefaultLengthOperator.Instance);

        // Assert
        result.Should().HaveCount(1);
        result[0].Start.Should().Be(5);
        result[0].End.Should().Be(5);
        result[0].StartIncluded.Should().BeTrue();
        result[0].EndIncluded.Should().BeTrue();
    }

    [Fact]
    public void Partition_ResultConsolidation_RecoversOriginalInterval()
    {
        // Arrange
        var interval = new TestBasicInterval(10, 28, true, true);

        // Act
        var result = interval.Partition(5, IntDefaultLengthOperator.Instance);
        var consolidated = result.Consolidate();

        // Assert
        consolidated.Should().HaveCount(1);
        consolidated[0].IsEquivalentIntervalTo(interval).Should().BeTrue();
    }
}
