// <copyright file="Interval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using Optional;
using System;

namespace Marsop.Ephemeral.Implementation;

/// <summary>
/// Immutable Interval Base class
/// </summary>
public class Interval : IInterval, IEquatable<IInterval>
{
    /// <inheritdoc cref="IInterval.End"/>
    public DateTimeOffset End { get; }

    /// <inheritdoc cref="IInterval.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <summary>
    /// Checks if the current <see cref="Interval"/> has coherent starting and ending points
    /// </summary>
    /// <returns><code>true</code> if starting and ending points are valid, <code>false</code> otherwise</returns>
    public bool IsValid => Start.IsLessThan(End) || (Start.IsEqualTo(End) && StartIncluded && EndIncluded);

    /// <inheritdoc cref="IInterval.Start"/>
    public DateTimeOffset Start { get; }

    /// <inheritdoc cref="IInterval.StartIncluded"/>
    public bool StartIncluded { get; }

    public TimeSpan Length() => this.Duration();

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">starting <see cref="DateTimeOffset"/></param>
    /// <param name="duration">the interval duration</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded) : this(start, start.Add(duration), startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
    /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) : this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interval" /> class
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public Interval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded)
    {
        Start = start;
        End = end;

        StartIncluded = startIncluded;
        EndIncluded = endIncluded;

        if (!IsValid)
        {
            throw new InvalidDurationException(GetTextualRepresentation());
        }
    }

    /// <summary>
    /// Creates an interval with both start and end included
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with both start and end included</returns>
    public static Interval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new Interval(start, end, true, true);

    /// <summary>
    /// Creates an interval with neither start or end included
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with neither start or end included</returns>
    public static Interval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new Interval(start, end, false, false);

    /// <summary>
    /// Creates an interval with duration 0
    /// </summary>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="Interval"/> with start and end point set with the given <see cref="DateTimeOffset"/></returns>
    public static Interval CreatePoint(DateTimeOffset timestamp) => CreateClosed(timestamp, timestamp);

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(IInterval other) => other != null && (Start == other.Start && End == other.End && StartIncluded == other.StartIncluded && EndIncluded == other.EndIncluded);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return GetTextualRepresentation();
    }

    /// <summary>
    /// Get a representation of the interval.
    /// Open intervals are represented with parenthesis (a,b)
    /// Close intervals are represented with brakets [a,b]
    /// </summary>
    /// <returns>a <see cref="String"/> that represent the interval</returns>
    private string GetTextualRepresentation()
    {
        var startDelimiter = StartIncluded ? "[" : "(";
        var endDelimiter = EndIncluded ? "]" : ")";
        return $"{startDelimiter}{Start} => {End}{endDelimiter}";
    }
}