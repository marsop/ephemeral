// <copyright file="InvalidDurationException.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Exceptions;

/// <summary>
/// Invalid interval duration exception
/// </summary>
public class InvalidDurationException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidDurationException" /> class
    /// </summary>
    public InvalidDurationException()
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidDurationException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidDurationException(string message, string paramName) : base(message, paramName)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidDurationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidDurationException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
    {
    }
}
