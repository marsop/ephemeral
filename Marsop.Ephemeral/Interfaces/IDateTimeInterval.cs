// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

using System;

public interface IDateTimeInterval : IGenericInterval<DateTime>
{
    /// <summary>
    /// Gets the difference between start and end as <see cref="TimeSpan"/>
    /// </summary>
    TimeSpan Duration => End - Start;
}