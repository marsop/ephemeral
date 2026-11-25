using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Net6.Temporal;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Net6.Tests.Temporal;

public class TimeOnlyTimeSpanLengthOperatorTests
{
    [Fact]
    public void Measure_NullInterval_ThrowsArgumentNullException()
    {
        Action act = () => TimeOnlyTimeSpanLengthOperator.Instance.Measure(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Measure_InvalidInterval_ThrowsInvalidLengthException()
    {
        var intervalMock = new Mock<IBasicInterval<TimeOnly>>();
        intervalMock.Setup(i => i.Start).Returns(new TimeOnly(14, 0));
        intervalMock.Setup(i => i.End).Returns(new TimeOnly(10, 0));

        Action act = () => TimeOnlyTimeSpanLengthOperator.Instance.Measure(intervalMock.Object);
        act.Should().Throw<InvalidLengthException>();
    }
}
