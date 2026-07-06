using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
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
}
