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

    public int Measure(DateOnly boundary1, DateOnly boundary2)
    {
        return (boundary2.ToDateTime(TimeOnly.MinValue) - boundary1.ToDateTime(TimeOnly.MinValue)).Days;
    }

    public int Zero() => 0;
}