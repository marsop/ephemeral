// <copyright file="IDisjointIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral.Core.Interfaces;

using System;
using System.Collections.Generic;

/// <summary>
/// Collection of disjoint IIntervals
/// </summary>
public interface IDisjointIntervalSet<TBoundary, TLength> : 
    IList<IInterval<TBoundary, TLength>>
    where TBoundary : IComparable<TBoundary>
{

    ILengthOperator<TBoundary, TLength> LengthOperator { get; }

    /// <summary>
    /// Gets the end of the latest contained Interval
    /// </summary>
    TBoundary End { get; }

    /// <summary>
    /// Gets a value indicating whether the End is included
    /// </summary>
    bool EndIncluded { get; }

    /// <summary>
    /// Gets a value indicating whether all the contained intervals are contiguous or the set is empty
    /// </summary>
    bool IsContiguous { get; }

    /// <summary>
    /// Gets the Start of the earliest contained Interval
    /// </summary>
    TBoundary Start { get; }

    /// <summary>
    /// Gets a value indicating whether the Start is included
    /// </summary>
    bool StartIncluded { get; }
}
