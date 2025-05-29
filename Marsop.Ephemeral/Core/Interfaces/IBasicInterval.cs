// <copyright file="IBasicInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core.Interfaces;

using System;

public interface IBasicInterval<TBoundary>
   where TBoundary : notnull, IComparable<TBoundary>
{
    /// <summary>
    /// Gets the final point of the interval
    /// </summary>
    TBoundary End { get; }

    /// <summary>
    /// Gets a value indicating whether the end is included in the interval
    /// </summary>
    bool EndIncluded { get; }

    /// <summary>
    /// Gets the starting point of the interval
    /// </summary>
    TBoundary Start { get; }

    /// <summary>
    /// Gets a value indicating whether the start is included in the interval
    /// </summary>
    bool StartIncluded { get; }
}
