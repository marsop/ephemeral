// <copyright file="IntervalStartComparer.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using System.Collections.Generic;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

/// <summary>
/// Interval starting point comparer class
/// </summary>
public class IntervalStartComparer : IComparer<IInterval<DateTimeOffset, TimeSpan>>
{
    /// <inheritdoc cref="IComparer{T}.Compare"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if at least one of the given parameters is <code>null</code></exception>
    public int Compare(IInterval<DateTimeOffset, TimeSpan> x, IInterval<DateTimeOffset, TimeSpan> y)
    {
        if (x == null)
        {
            throw new ArgumentNullException(nameof(x));
        }

        if (y == null)
        {
            throw new ArgumentNullException(nameof(y));
        }

        var startComparison = x.Start.CompareTo(y.Start);
        if (startComparison != 0)
        {
            return startComparison;
        }

        if (x.StartIncluded && !y.StartIncluded)
        {
            return -1;
        }

        if (!x.StartIncluded && y.StartIncluded)
        {
            return 1;
        }

        return 0;
    }
}
