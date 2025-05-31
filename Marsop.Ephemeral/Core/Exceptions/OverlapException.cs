// <copyright file="OverlapException.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core;

/// <summary>
/// Overlap between intervals exception
/// </summary>
public class OverlapException : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OverlapException" /> class
    /// </summary>
    public OverlapException()
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public OverlapException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public OverlapException(string message, string paramName) : base(message, paramName)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public OverlapException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <inheritdoc cref="ArgumentException"/>
    public OverlapException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
    {
    }
}
