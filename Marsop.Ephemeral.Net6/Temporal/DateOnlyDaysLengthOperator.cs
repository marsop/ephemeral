using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Provides default length operations for DateOnly values.
/// </summary>
public sealed class DateOnlyDaysLengthOperator : ILengthOperator<DateOnly, int>
{
    public static DateOnlyDaysLengthOperator Instance { get; } = new DateOnlyDaysLengthOperator();

    private DateOnlyDaysLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    public DateOnly Apply(DateOnly boundary, int length) => boundary.AddDays(length);

    public int Measure(IBasicInterval<DateOnly> interval)
    {
        return (interval.End.ToDateTime(TimeOnly.MinValue) - interval.Start.ToDateTime(TimeOnly.MinValue)).Days;
    }

    public int Zero() => 0;
}