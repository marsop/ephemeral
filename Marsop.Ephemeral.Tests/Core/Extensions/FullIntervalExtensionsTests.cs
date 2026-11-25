using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Numerics;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class FullIntervalExtensionsTests
{
    private class TestLengthOperator : ILengthOperator<int, int>
    {
        public static readonly TestLengthOperator Instance = new();

        public int Apply(int boundary, int length) => boundary + length;

        public int Measure(IBasicInterval<int> interval) => interval.End - interval.Start;

        public int Zero() => 0;
    }

    private record TestFullInterval : FullInterval<int, int>
    {
        public TestFullInterval(int start, int end, bool startIncluded, bool endIncluded)
            : base(start, end, startIncluded, endIncluded)
        {
        }

        public override ILengthOperator<int, int> Operator => TestLengthOperator.Instance;
        public sealed override string ToString() => base.ToString();
    }


    [Fact]
    public void ToIntervalSet_ValidInterval_ReturnsDisjointIntervalSetContainingInterval()
    {
        // Arrange
        var start = 5;
        var end = 10;
        var interval = new TestFullInterval(start, end, true, false);

        // Act
        var result = interval.ToIntervalSet();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Should().BeSameAs(interval);
        result.LengthOperator.Should().BeSameAs(interval.Operator);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Shift_PositiveOffset_ShouldShiftBoundsAndMaintainMeasure(bool startIncluded, bool endIncluded)
    {
        // Arrange
        var interval = new TestFullInterval(5, 10, startIncluded, endIncluded);
        int offset = 3;

        // Act
        var result = interval.Shift(offset);

        // Assert
        result.Start.Should().Be(8);
        result.End.Should().Be(13);
        result.StartIncluded.Should().Be(startIncluded);
        result.EndIncluded.Should().Be(endIncluded);
        result.DefaultMeasure().Should().Be(5);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Shift_NegativeOffset_ShouldShiftBoundsAndMaintainMeasure(bool startIncluded, bool endIncluded)
    {
        // Arrange
        var interval = new TestFullInterval(5, 10, startIncluded, endIncluded);
        int offset = -2;

        // Act
        var result = interval.Shift(offset);

        // Assert
        result.Start.Should().Be(3);
        result.End.Should().Be(8);
        result.StartIncluded.Should().Be(startIncluded);
        result.EndIncluded.Should().Be(endIncluded);
        result.DefaultMeasure().Should().Be(5);
    }

    [Fact]
    public void Partition_NullInterval_ThrowsArgumentNullException()
    {
        // Arrange
        FullInterval<int, int> interval = null!;

        // Act
        Action act = () => interval.Partition(5);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Partition_DefaultIntermediateEndsClosed_PartitionsWithOpenIntermediateEnds()
    {
        // Arrange
        var interval = new TestFullInterval(10, 28, true, true);

        // Act
        var result = interval.Partition(5);

        // Assert
        result.Should().HaveCount(4);
        result.LengthOperator.Should().BeSameAs(interval.Operator);

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
    public void Partition_IntermediateEndsClosedTrue_PartitionsWithClosedIntermediateEnds()
    {
        // Arrange
        var interval = new TestFullInterval(10, 28, true, true);

        // Act
        var result = interval.Partition(5, intermediateEndsClosed: true);

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
    public void Partition_DoubleIntervalExampleFromIssue_ProducesExpectedChunks()
    {
        // Arrange
        var i = DoubleInterval.CreateClosed(10.0, 28.0);

        // Act
        var result = i.Partition(5.0, intermediateEndsClosed: false);

        // Assert
        result.Should().HaveCount(4);
        result[0].ToString().Should().Be("[10, 15)");
        result[1].ToString().Should().Be("[15, 20)");
        result[2].ToString().Should().Be("[20, 25)");
        result[3].ToString().Should().Be("[25, 28]");
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
    public void Operations_NullFullInterval_ThrowArgumentNullException()
    {
        FullInterval<int, int> nullInterval = null!;
        var valid = new TestFullInterval(1, 5, true, true);

        ((Action)(() => nullInterval.Subtract(valid))).Should().Throw<ArgumentNullException>().WithParameterName("interval");
        ((Action)(() => nullInterval.ToIntervalSet())).Should().Throw<ArgumentNullException>().WithParameterName("interval");
        ((Action)(() => nullInterval.Shift(1))).Should().Throw<ArgumentNullException>().WithParameterName("interval");
        ((Action)(() => nullInterval.ShiftStart(1))).Should().Throw<ArgumentNullException>().WithParameterName("interval");
        ((Action)(() => nullInterval.ShiftEnd(1))).Should().Throw<ArgumentNullException>().WithParameterName("interval");
        ((Action)(() => nullInterval.LengthOfIntersect(valid))).Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Operations_InvalidOtherInterval_ThrowInvalidLengthException()
    {
        var valid = new TestFullInterval(1, 5, true, true);
        var invalid = new UncheckedInterval(10, 2, true, true);

        ((Action)(() => valid.Subtract(invalid))).Should().Throw<InvalidLengthException>();
        ((Action)(() => valid.LengthOfIntersect(invalid))).Should().Throw<InvalidLengthException>();
    }
}
