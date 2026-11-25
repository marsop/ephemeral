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

    private sealed class UncheckedInterval : IBasicInterval<int>
    {
        public UncheckedInterval(int start, int end, bool startIncluded, bool endIncluded)
        {
            Start = start;
            End = end;
            StartIncluded = startIncluded;
            EndIncluded = endIncluded;
        }

        public int Start { get; }
        public bool StartIncluded { get; }
        public int End { get; }
        public bool EndIncluded { get; }
    }

    [Fact]
    public void IsValidInterval_NullInterval_ThrowsArgumentNullException()
    {
        IBasicInterval<int> interval = null!;
        Action act = () => interval.IsValidInterval();
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Theory]
    [InlineData(1, 5, true, true, true)]
    [InlineData(1, 5, false, false, true)]
    [InlineData(1, 5, true, false, true)]
    [InlineData(1, 5, false, true, true)]
    [InlineData(5, 5, true, true, true)]
    [InlineData(10, 5, true, true, false)]
    [InlineData(5, 5, false, false, false)]
    [InlineData(5, 5, true, false, false)]
    [InlineData(5, 5, false, true, false)]
    public void IsValidInterval_VariousBoundaries_ReturnsExpectedResult(
        int start,
        int end,
        bool startIncluded,
        bool endIncluded,
        bool expected)
    {
        var interval = new UncheckedInterval(start, end, startIncluded, endIncluded);
        interval.IsValidInterval().Should().Be(expected);
    }

    public static TheoryData<IBasicInterval<int>> InvalidIntervals => new()
    {
        new UncheckedInterval(10, 5, true, true),
        new UncheckedInterval(5, 5, false, false),
        new UncheckedInterval(5, 5, true, false),
        new UncheckedInterval(5, 5, false, true),
    };

    [Theory]
    [MemberData(nameof(InvalidIntervals))]
    public void Operations_InvalidPrimaryInterval_ThrowInvalidLengthException(IBasicInterval<int> invalid)
    {
        var valid = new TestBasicInterval(1, 10, true, true);
        var op = IntDefaultLengthOperator.Instance;

        ((Action)(() => invalid.Measure(op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.WithMeasure(op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.IsEquivalentIntervalTo(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Covers(5))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Covers(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Shift(1, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.ShiftStart(1, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.ShiftEnd(1, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.LengthOfIntersect(valid, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Intersect(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Intersects(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.IsContiguouslyFollowedBy(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.IsContiguouslyPrecededBy(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.StartsBefore(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Join(valid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Subtract(valid, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Union(valid, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => invalid.Partition(1, op))).Should().Throw<InvalidLengthException>();
    }

    [Theory]
    [MemberData(nameof(InvalidIntervals))]
    public void Operations_InvalidSecondaryInterval_ThrowInvalidLengthException(IBasicInterval<int> invalid)
    {
        var valid = new TestBasicInterval(1, 10, true, true);
        var op = IntDefaultLengthOperator.Instance;

        ((Action)(() => valid.IsEquivalentIntervalTo(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Covers(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.LengthOfIntersect(invalid, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Intersect(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Intersects(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.IsContiguouslyFollowedBy(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.IsContiguouslyPrecededBy(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.StartsBefore(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Join(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Subtract(invalid, op))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.Union(invalid, op))).Should().Throw<InvalidLengthException>();
    }

    [Fact]
    public void Operations_NullArguments_ThrowArgumentNullException()
    {
        IBasicInterval<int> nullInterval = null!;
        ILengthOperator<int, int> nullOp = null!;
        var valid = new TestBasicInterval(1, 10, true, true);
        var op = IntDefaultLengthOperator.Instance;

        ((Action)(() => nullInterval.Shift(1, op))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.Shift(1, null!))).Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
        ((Action)(() => nullInterval.ShiftStart(1, op))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.ShiftStart(1, null!))).Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
        ((Action)(() => nullInterval.ShiftEnd(1, op))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.ShiftEnd(1, null!))).Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
        ((Action)(() => valid.LengthOfIntersect(new TestBasicInterval(20, 30, true, true), nullOp))).Should().Throw<ArgumentNullException>().WithParameterName("measurer");
        ((Action)(() => nullInterval.IsContiguouslyFollowedBy(valid))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.IsContiguouslyFollowedBy(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => nullInterval.IsContiguouslyPrecededBy(valid))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.IsContiguouslyPrecededBy(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => nullInterval.StartsBefore(valid))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.StartsBefore(nullInterval))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.Subtract(valid, nullOp))).Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
        ((Action)(() => nullInterval.Union(valid, op))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.Union(nullInterval, op))).Should().Throw<ArgumentNullException>();
        ((Action)(() => valid.Union(valid, nullOp))).Should().Throw<ArgumentNullException>().WithParameterName("lengthOperator");
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
