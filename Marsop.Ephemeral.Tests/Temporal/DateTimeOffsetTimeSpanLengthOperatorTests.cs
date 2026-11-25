using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Temporal;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Tests.Temporal;

public class DateTimeOffsetTimeSpanLengthOperatorTests
{
    [Fact]
    public void Measure_NullInterval_ThrowsArgumentNullException()
    {
        Action act = () => DateTimeOffsetTimeSpanLengthOperator.Instance.Measure(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Measure_InvalidInterval_ThrowsInvalidLengthException()
    {
        var now = DateTimeOffset.UtcNow;
        var intervalMock = new Mock<IBasicInterval<DateTimeOffset>>();
        intervalMock.Setup(i => i.Start).Returns(now.AddHours(1));
        intervalMock.Setup(i => i.End).Returns(now);

        Action act = () => DateTimeOffsetTimeSpanLengthOperator.Instance.Measure(intervalMock.Object);
        act.Should().Throw<InvalidLengthException>();
    }
}
