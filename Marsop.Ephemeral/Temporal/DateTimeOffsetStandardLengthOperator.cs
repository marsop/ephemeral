using System;
using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Temporal;

public sealed class DateTimeOffsetTimeSpanLengthOperator :
    ILengthOperator<DateTimeOffset, TimeSpan>
{
    private static readonly DateTimeOffsetTimeSpanLengthOperator _instance = new();

    public static ILengthOperator<DateTimeOffset, TimeSpan> Instance => _instance;

    // Required to enforce the singleton pattern by preventing external instantiation.
    private DateTimeOffsetTimeSpanLengthOperator() { }

    public DateTimeOffset Apply(DateTimeOffset boundary, TimeSpan length) => boundary.Add(length);

    public TimeSpan Measure(IBasicInterval<DateTimeOffset> interval) => interval.End - interval.Start;

    public TimeSpan Zero() => TimeSpan.Zero;
}
