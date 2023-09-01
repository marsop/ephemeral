// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Interfaces;

public interface IHasDuration<T>
{
    /// <summary>
    /// Gets the difference between start and end as <see cref="T"/>
    /// </summary>
    T Duration { get; }
}