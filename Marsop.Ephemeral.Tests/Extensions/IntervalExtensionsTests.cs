using FluentAssertions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using System;
using Xunit;

namespace Marsop.Ephemeral.Tests.Extensions;

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
        var date = _randomHelper.GetDateTime();

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
        var date = _randomHelper.GetDateTime();

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
        var date = _randomHelper.GetDateTime();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedInterval, endIncludedInterval);

        //When
        var result = source.Shift(TimeSpan.Zero);

        //Then
        result.Should().BeEquivalentTo(source);
    }

    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(false, true, false, true)]
    [InlineData(false, false, false, false)]
    public void Test_Intersect(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
    {
        //Given
        var date = _randomHelper.GetDateTime();

        var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
        var other = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

        //When
        var result = source.Intersect(other).Match(x => x.ToInterval(), () => default(IInterval));

        //Then
        result.Should().BeEquivalentTo(other);
    }
}