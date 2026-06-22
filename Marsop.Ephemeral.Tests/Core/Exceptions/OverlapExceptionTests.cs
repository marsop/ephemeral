using System;
using FluentAssertions;
using Marsop.Ephemeral.Core;
using Xunit;

namespace Marsop.Ephemeral.Tests.Core.Exceptions;

public class OverlapExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateInstance()
    {
        var exception = new OverlapException();
        exception.Should().NotBeNull();
        exception.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        var message = "Test message";
        var exception = new OverlapException(message);
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void Constructor_WithMessageAndParamName_ShouldSetMessageAndParamName()
    {
        var message = "Test message";
        var paramName = "TestParam";
        var exception = new OverlapException(message, paramName);
        exception.Message.Should().Contain(message);
        exception.ParamName.Should().Be(paramName);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetMessageAndInnerException()
    {
        var message = "Test message";
        var innerException = new Exception("Inner exception");
        var exception = new OverlapException(message, innerException);
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void Constructor_WithMessageParamNameAndInnerException_ShouldSetAllProperties()
    {
        var message = "Test message";
        var paramName = "TestParam";
        var innerException = new Exception("Inner exception");
        var exception = new OverlapException(message, paramName, innerException);
        exception.Message.Should().Contain(message);
        exception.ParamName.Should().Be(paramName);
        exception.InnerException.Should().Be(innerException);
    }
}
