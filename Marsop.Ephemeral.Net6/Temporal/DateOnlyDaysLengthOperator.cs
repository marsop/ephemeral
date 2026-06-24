using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Provides default length operations for DateOnly values.
/// </summary>
public sealed class DateOnlyDaysLengthOperator : ILengthOperator<DateOnly, int>
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="DateOnlyDaysLengthOperator"/>.
    /// </summary>
    public static DateOnlyDaysLengthOperator Instance { get; } = new DateOnlyDaysLengthOperator();

    private DateOnlyDaysLengthOperator()
    {
        // Private constructor to prevent external instantiation and enforce the singleton pattern.
    }

    /// <inheritdoc/>
    public DateOnly Apply(DateOnly boundary, int length) => boundary.AddDays(length);

    /// <inheritdoc/>
    public int Measure(IBasicInterval<DateOnly> interval)
    {
        return (interval.End.ToDateTime(TimeOnly.MinValue) - interval.Start.ToDateTime(TimeOnly.MinValue)).Days;
    }

    /// <inheritdoc/>
    public int Zero() => 0;
}