using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Marsop.Ephemeral.Net6.Temporal;
using Moq;
using Xunit;

namespace Marsop.Ephemeral.Net6.Tests.Temporal;

public class DateOnlyDaysLengthOperatorTests
{
    [Fact]
    public void Measure_NullInterval_ThrowsArgumentNullException()
    {
        Action act = () => DateOnlyDaysLengthOperator.Instance.Measure(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("interval");
    }

    [Fact]
    public void Measure_InvalidInterval_ThrowsInvalidLengthException()
    {
        var intervalMock = new Mock<IBasicInterval<DateOnly>>();
        intervalMock.Setup(i => i.Start).Returns(new DateOnly(2024, 5, 10));
        intervalMock.Setup(i => i.End).Returns(new DateOnly(2024, 5, 1));

        Action act = () => DateOnlyDaysLengthOperator.Instance.Measure(intervalMock.Object);
        act.Should().Throw<InvalidLengthException>();
    }
}
