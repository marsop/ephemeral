// <copyright file="BasicInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Exceptions;
using Marsop.Ephemeral.Core.Interfaces;
using Marsop.Ephemeral.Core.Extensions;

namespace Marsop.Ephemeral.Core.Implementation;

public record BasicInterval<TBoundary> : 
    IBasicInterval<TBoundary>
    where TBoundary : IComparable<TBoundary>
{
    /// <inheritdoc cref="IBasicInterval.Start"/>
    public TBoundary Start { get; }
    
    /// <inheritdoc cref="IBasicInterval.End"/>
    public TBoundary End { get; }
    
    /// <inheritdoc cref="IBasicInterval.StartIncluded"/>
    public bool StartIncluded { get; }

    /// <inheritdoc cref="IBasicInterval.EndIncluded"/>
    public bool EndIncluded { get; }

/// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public BasicInterval(TBoundary start, TBoundary end, bool startIncluded, bool endIncluded)
    {
        Start = start;
        End = end;
        StartIncluded = startIncluded;
        EndIncluded = endIncluded;

        if (!IsValid)
        {
            throw new InvalidLengthException(GetTextualRepresentation());
        }
    }

    /// <summary>
    /// Creates an interval with both start and end included
    /// </summary>
    /// <param name="start">the starting <see cref="TBoundary"/></param>
    /// <param name="end">the ending <see cref="TBoundary"/></param>
    /// <returns>an <see cref="BasicInterval<>"/> with both start and end included</returns>
    public static BasicInterval<TBoundary> CreateClosed(TBoundary start, TBoundary end) =>
        new(start, end, true, true);

    /// <summary>
    /// Creates an interval with neither start or end included
    /// </summary>
    /// <param name="start">the starting <see cref="TBoundary"/></param>
    /// <param name="end">the ending <see cref="TBoundary"/></param>
    /// <returns>an <see cref="BasicInterval<>"/> with neither start or end included</returns>
    public static BasicInterval<TBoundary> CreateOpen(TBoundary start, TBoundary end) =>
        new(start, end, false, false);

    /// <summary>
    /// Creates an interval with duration 0
    /// </summary>
    /// <param name="boundary">the <see cref="TBoundary"/></param>
    /// <returns>an <see cref="BasicInterval<>"/> with start and end point set with the given <see cref="TBoundary"/></returns>
    public static BasicInterval<TBoundary> CreatePoint(TBoundary boundary) =>
        CreateClosed(boundary, boundary);

    /// <summary>
    /// Get a representation of the interval.
    /// Open intervals are represented with parenthesis (a,b)
    /// Close intervals are represented with brakets [a,b]
    /// </summary>
    /// <returns>a <see cref="string"/> that represent the interval</returns>
    protected string GetTextualRepresentation()
    {
        var startDelimiter = StartIncluded ? "[" : "(";
        var endDelimiter = EndIncluded ? "]" : ")";
        return $"{startDelimiter}{Start} , {End}{endDelimiter}";
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => GetTextualRepresentation();

    public static BasicInterval<TBoundary> From(IBasicInterval<TBoundary> interval)
    {
        return new BasicInterval<TBoundary>(
            interval.Start,
            interval.End,
            interval.StartIncluded,
            interval.EndIncluded);
    }

    /// <summary>
    /// Checks if the current <see cref="BasicInterval<>"/> has coherent starting and ending points
    /// </summary>
    /// <returns><code>true</code> if starting and ending points are valid, <code>false</code> otherwise</returns>
    public bool IsValid => Start.IsLessThan(End) || Start.IsEqualTo(End) && StartIncluded && EndIncluded;
}