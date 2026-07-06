using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Extensions;

public class ComparableExtensionsTests
{
    [Theory]
    [InlineData(5, 3, true)]
    [InlineData(3, 5, false)]
    [InlineData(5, 5, false)]
    public void IsGreaterThan_Int_ReturnsExpectedResult(int value, int other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(5.5, 3.3, true)]
    [InlineData(3.3, 5.5, false)]
    [InlineData(5.5, 5.5, false)]
    public void IsGreaterThan_Double_ReturnsExpectedResult(double value, double other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("b", "a", true)]
    [InlineData("a", "b", false)]
    [InlineData("a", "a", false)]
    public void IsGreaterThan_String_ReturnsExpectedResult(string value, string other, bool expected)
    {
        var result = value.IsGreaterThan(other);
        result.Should().Be(expected);
    }
}