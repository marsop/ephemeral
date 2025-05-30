using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Provides default length operations for DateOnly values.
/// </summary>
public class DateOnlyDaysLengthOperator : ILengthOperator<DateOnly, int>
{
    public static DateOnlyDaysLengthOperator Instance { get; } = new DateOnlyDaysLengthOperator();

    private DateOnlyDaysLengthOperator() { }

    public DateOnly Apply(DateOnly boundary, int length) => boundary.AddDays(length);

    public int Measure(IBasicInterval<DateOnly> interval)
    {
        return (interval.End.ToDateTime(TimeOnly.MinValue) - interval.Start.ToDateTime(TimeOnly.MinValue)).Days;
    }

    public int Zero() => 0;
}