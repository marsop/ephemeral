using System;
using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Temporal;

public sealed class DateTimeOffsetTimeSpanLengthOperator :
    ILengthOperator<DateTimeOffset, TimeSpan>
{
    private static readonly DateTimeOffsetTimeSpanLengthOperator _instance = new();

    public static ILengthOperator<DateTimeOffset, TimeSpan> Instance => _instance;

    private DateTimeOffsetTimeSpanLengthOperator() { }

    public DateTimeOffset Apply(DateTimeOffset boundary, TimeSpan length) => boundary.Add(length);

    public TimeSpan Measure(IBasicInterval<DateTimeOffset> interval) => interval.End - interval.Start;

    public TimeSpan Zero() => TimeSpan.Zero;
}
