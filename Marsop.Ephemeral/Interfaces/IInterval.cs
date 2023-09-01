// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

/// <summary>
/// Interface for classes implementing an interval
/// </summary>
public interface IInterval : IGenericInterval<DateTimeOffset>
{
    /// <summary>
    /// Gets the difference between start and end as <see cref="TimeSpan"/>
    /// </summary>
    TimeSpan Duration => End - Start;
}