using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Implementation;

public class BasicIntervalTests
{
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
    public void From_NullInterval_ThrowsArgumentNullException()
    {
        Action act = () => BasicInterval<int>.From(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void From_InvalidInterval_ThrowsInvalidLengthException()
    {
        var invalid = new UncheckedInterval(10, 5, true, true);
        Action act = () => BasicInterval<int>.From(invalid);
        act.Should().Throw<InvalidLengthException>();
    }
}
