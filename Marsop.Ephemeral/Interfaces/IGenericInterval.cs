﻿// <copyright file="IInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Interfaces;

public interface IGenericInterval<TBoundary>
    where TBoundary : notnull, IComparable<TBoundary>, IEquatable<TBoundary>
{
    /// <summary>
    /// Gets the final point of the interval
    /// </summary>
    TBoundary End { get; }

    /// <summary>
    /// Gets a value indicating whether the end timestamp is included in the interval
    /// </summary>
    bool EndIncluded { get; }

    /// <summary>
    /// Gets the starting point of the interval
    /// </summary>
    TBoundary Start { get; }

    /// <summary>
    /// Gets a value indicating whether the start timestamp is included in the interval
    /// </summary>
    bool StartIncluded { get; }
}