// <copyright file="InvalidLengthException.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core;

/// <summary>
/// Invalid interval duration exception
/// </summary>
public class InvalidLengthException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidLengthException" /> class
    /// </summary>
    public InvalidLengthException()
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidLengthException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidLengthException(string message, string paramName) : base(message, paramName)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidLengthException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public InvalidLengthException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
    {
    }
}
