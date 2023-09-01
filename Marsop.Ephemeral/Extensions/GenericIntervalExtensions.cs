// <copyright file="IntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Interfaces;
using System;

namespace Marsop.Ephemeral.Extensions;

/// <summary>
/// Extension methods for <see cref="IGenericInterval{T}"/> /> objects
/// </summary>
public static class GenericIntervalExtensions
{
    /// <summary>
    /// Verify if the interval covers the given boundary/>
    /// </summary>
    /// <param name="interval">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="boundary">the boundary</param>
    /// <returns><code>true</code> if the offset is covered by the <see cref="IGenericInterval{T}"/>, <code>false</code> otherwise</returns>
    public static bool Covers<T>(this IGenericInterval<T> interval, T boundary)
        where T : IComparable<T>, IEquatable<T>
    {
        if (boundary.IsLessThan(interval.Start))
        {
            return false;
        }

        if (boundary.IsGreaterThan(interval.End))
        {
            return false;
        }

        if (boundary.Equals(interval.Start) && !interval.StartIncluded)
        {
            return false;
        }

        if (boundary.Equals(interval.End) && !interval.EndIncluded)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the given <see cref="IGenericInterval{T}"/> follows seamlessly and without overlap the current <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="i">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="o">the <see cref="IGenericInterval{T}"/> instance to check</param>
    /// <returns><code>true</code> if the given <see cref="IGenericInterval{T}"/> is followed with the current one</returns>
    public static bool IsContiguouslyFollowedBy<T>(this IGenericInterval<T> i, IGenericInterval<T> o)
        where T : IComparable<T>, IEquatable<T> =>
        i.End.Equals(o.Start) && (i.EndIncluded != o.StartIncluded);

    /// <summary>
    /// Checks if the current <see cref="IGenericInterval{T}"/> follows seamlessly and without overlap the given <see cref="IGenericInterval{T}"/>
    /// </summary>
    /// <param name="i">the current <see cref="IGenericInterval{T}"/> instance</param>
    /// <param name="o">the <see cref="IGenericInterval{T}"/> instance to check</param>
    /// <returns><code>true</code> if the <see cref="IGenericInterval{T}"/> is preceded the the given <see cref="IGenericInterval{T}"/>, <code>false</code> otherwise</returns>
    public static bool IsContiguouslyPrecededBy<T>(this IGenericInterval<T> i, IGenericInterval<T> o)
        where T : IComparable<T>, IEquatable<T> =>
        o.IsContiguouslyFollowedBy(i);
}