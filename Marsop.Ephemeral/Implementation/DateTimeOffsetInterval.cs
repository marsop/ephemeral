// <copyright file="DateTimeOffsetInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using System;

namespace Marsop.Ephemeral.Implementation;

/// <summary>
/// Immutable Interval Base class
/// </summary>
public class DateTimeOffsetInterval :
IDateTimeOffsetInterval,
IIntervalFactory<DateTimeOffsetInterval, DateTimeOffset>
{
    /// <inheritdoc cref="IDateTimeOffsetInterval.End"/>
    public DateTimeOffset End { get; }

    /// <inheritdoc cref="IDateTimeOffsetInterval.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <summary>
    /// Checks if the current <see cref="DateTimeOffsetInterval"/> has coherent starting and ending points
    /// </summary>
    /// <returns><code>true</code> if starting and ending points are valid, <code>false</code> otherwise</returns>
    public bool IsValid => Start.IsLessThan(End) || (Start.IsEqualTo(End) && StartIncluded && EndIncluded);

    /// <inheritdoc cref="IDateTimeOffsetInterval.Start"/>
    public DateTimeOffset Start { get; }

    /// <inheritdoc cref="IDateTimeOffsetInterval.StartIncluded"/>
    public bool StartIncluded { get; }

    public TimeSpan Length() => Measure(Start, End);

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class
    /// </summary>
    /// <param name="start">starting <see cref="DateTimeOffset"/></param>
    /// <param name="duration">the interval duration</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public DateTimeOffsetInterval(DateTimeOffset start, TimeSpan duration, bool startIncluded, bool endIncluded) : this(start, start.Add(duration), startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class
    /// </summary>
    /// <param name="start">a <see cref="ITimestamped"/> instance representing the starting point</param>
    /// <param name="end">a <see cref="ITimestamped"/> instance representing the ending point</param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public DateTimeOffsetInterval(ITimestamped start, ITimestamped end, bool startIncluded, bool endIncluded) : this(start.Timestamp, end.Timestamp, startIncluded, endIncluded)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetInterval" /> class
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <param name="startIncluded">a flag indicating whether the starting point is included</param>
    /// <param name="endIncluded">a flag indicating whether the ending point is included</param>
    public DateTimeOffsetInterval(DateTimeOffset start, DateTimeOffset end, bool startIncluded, bool endIncluded)
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
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="DateTimeOffsetInterval"/> with both start and end included</returns>
    public static DateTimeOffsetInterval CreateClosed(DateTimeOffset start, DateTimeOffset end) => new DateTimeOffsetInterval(start, end, true, true);

    /// <summary>
    /// Creates an interval with neither start or end included
    /// </summary>
    /// <param name="start">the starting <see cref="DateTimeOffset"/></param>
    /// <param name="end">the ending <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="DateTimeOffsetInterval"/> with neither start or end included</returns>
    public static DateTimeOffsetInterval CreateOpen(DateTimeOffset start, DateTimeOffset end) => new DateTimeOffsetInterval(start, end, false, false);

    /// <summary>
    /// Creates an interval with duration 0
    /// </summary>
    /// <param name="timestamp">the <see cref="DateTimeOffset"/></param>
    /// <returns>an <see cref="DateTimeOffsetInterval"/> with start and end point set with the given <see cref="DateTimeOffset"/></returns>
    public static DateTimeOffsetInterval CreatePoint(DateTimeOffset timestamp) => CreateClosed(timestamp, timestamp);


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

    public IInterval<DateTimeOffset, TimeSpan> CreateNew(DateTimeOffset start, bool startIncluded, DateTimeOffset end, bool endIncluded)
    {
        return new DateTimeOffsetInterval(start, end, startIncluded, endIncluded);
    }

    public DateTimeOffset Apply(DateTimeOffset boundary, TimeSpan length)
    {
        return boundary.Add(length);
    }

    public TimeSpan Measure(DateTimeOffset boundary1, DateTimeOffset boundary2)
    {
        return boundary2 - boundary1;
    }

    public TimeSpan Zero() => TimeSpan.Zero;

    public DateTimeOffsetInterval CreateFrom(IBasicInterval<DateTimeOffset> i) => From(i);

    public static DateTimeOffsetInterval From(IBasicInterval<DateTimeOffset> i)
    {
        return new DateTimeOffsetInterval(i.Start, i.End, i.StartIncluded, i.EndIncluded);
    }
}