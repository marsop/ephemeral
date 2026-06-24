using System;
using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Temporal;

/// <summary>
/// Provides a length operator for intervals with <see cref="DateTimeOffset"/> boundaries measured by <see cref="TimeSpan"/>.
/// </summary>
public sealed class DateTimeOffsetTimeSpanLengthOperator :
    ILengthOperator<DateTimeOffset, TimeSpan>
{
    private static readonly DateTimeOffsetTimeSpanLengthOperator _instance = new();

    /// <summary>
    /// Gets the singleton instance of the <see cref="DateTimeOffsetTimeSpanLengthOperator"/>.
    /// </summary>
    public static ILengthOperator<DateTimeOffset, TimeSpan> Instance => _instance;

    private DateTimeOffsetTimeSpanLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    /// <inheritdoc/>
    public DateTimeOffset Apply(DateTimeOffset boundary, TimeSpan length) => boundary.Add(length);

    /// <inheritdoc/>
    public TimeSpan Measure(IBasicInterval<DateTimeOffset> interval) => interval.End - interval.Start;

    /// <inheritdoc/>
    public TimeSpan Zero() => TimeSpan.Zero;
}
