using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Implementation;

public class FullIntervalTests
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
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        int start = 5;
        int end = 10;
        bool startIncluded = true;
        bool endIncluded = false;

        // Act
        var interval = new TestFullInterval(start, end, startIncluded, endIncluded);

        // Assert
        interval.Start.Should().Be(start);
        interval.End.Should().Be(end);
        interval.StartIncluded.Should().Be(startIncluded);
        interval.EndIncluded.Should().Be(endIncluded);
    }

    [Fact]
    public void DefaultMeasure_UsesOperatorToCalculateMeasure()
    {
        // Arrange
        var interval = new TestFullInterval(5, 15, true, true);

        // Act
        var measure = interval.DefaultMeasure();

        // Assert
        measure.Should().Be(10);
    }

    [Fact]
    public void ToString_ReturnsBaseToStringImplementation()
    {
        // Arrange
        var interval = new TestFullInterval(2, 8, false, true);

        // Act
        var result = interval.ToString();

        // Assert
        result.Should().Be("(2, 8]");
    }
}
