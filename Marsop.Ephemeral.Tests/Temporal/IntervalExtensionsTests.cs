using FluentAssertions;
using Optional.Unsafe;
using Marsop.Ephemeral.Core.Extensions;
using System;
using Xunit;
using Marsop.Ephemeral.Temporal;

namespace Marsop.Ephemeral.Tests.Temporal;

public class IntervalExtensionsTests
{
    private readonly RandomHelper _randomHelper = new();

    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, false, false, false)]
    public void Test_Covers(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
    {
        //Given
        var date = _randomHelper.GetRandomDateTimeOffset();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
        var other = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

        //When
        var result = source.Covers(other);

        //Then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Test_Shift(bool startIncludedInterval, bool endIncludedInterval)
    {
        //Given
        var date = _randomHelper.GetRandomDateTimeOffset();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedInterval, endIncludedInterval);

        var shiftAmount = TimeSpan.FromHours(1);

        //When
        var result = source.Shift(shiftAmount);

        //Then
        result.End.Should().BeExactly(source.End.AddHours(1));
        result.Start.Should().BeExactly(source.Start.AddHours(1));
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Test_Shift_Empty(bool startIncludedInterval, bool endIncludedInterval)
    {
        //Given
        var date = _randomHelper.GetRandomDateTimeOffset();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedInterval, endIncludedInterval);

        //When
        var result = source.Shift(TimeSpan.Zero);

        //Then
        result.IsEquivalentIntervalTo(source).Should().BeTrue();
    }

    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, false, false, false)]
    public void Test_Intersect(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
    {
        //Given
        var date = _randomHelper.GetRandomDateTimeOffset();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
        var other = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

        //When
        var result = source
            .Intersect(other)
            .ValueOrFailure();

        //Then
        result.Should().BeEquivalentTo(other);
    }

    [Fact]
    public void LengthOfIntersect_ReturnsCorrectDuration_WhenIntervalsOverlap()
    {
        // Given
        var date = _randomHelper.GetRandomDateTimeOffset();
        var intervalA = new DateTimeOffsetInterval(date.AddHours(8), date.AddHours(12), true, true);
        var intervalB = new DateTimeOffsetInterval(date.AddHours(10), date.AddHours(14), true, true);

        // When
        var duration = intervalA.LengthOfIntersect(intervalB);

        // Then
        duration.Should().Be(intervalA.End - intervalB.Start);
    }

    [Fact]
    public void LengthOfIntersect_ReturnsZero_WhenIntervalsDoNotOverlap()
    {
        // Given
        var date = _randomHelper.GetRandomDateTimeOffset();
        var intervalA = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(10), true, true);
        var intervalB = _randomHelper.GetInterval(date.AddHours(11), date.AddHours(12), true, true);

        // When
        var duration = intervalA.LengthOfIntersect(intervalB);

        // Then
        duration.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void LengthOfIntersect_ReturnsLength_WhenIntervalsAreIdentical()
    {
        // Given
        var date = _randomHelper.GetRandomDateTimeOffset();
        var intervalA = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), true, true);
        var intervalB = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), true, true);

        // When
        var duration = intervalA.LengthOfIntersect(intervalB);

        // Then
        duration.Should().Be(intervalA.Length());
    }
}