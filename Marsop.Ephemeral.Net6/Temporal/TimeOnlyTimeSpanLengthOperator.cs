using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal;

/// <summary>
/// Provides length calculation between two <see cref="TimeOnly"/> values as a <see cref="TimeSpan"/>.
/// </summary>
public class TimeOnlyTimeSpanLengthOperator : ILengthOperator<TimeOnly, TimeSpan>
{
    private TimeOnlyTimeSpanLengthOperator() { }

    public static TimeOnlyTimeSpanLengthOperator Instance { get; } = new TimeOnlyTimeSpanLengthOperator();

    public TimeOnly Apply(TimeOnly boundary, TimeSpan length) => boundary.Add(length);

    public TimeSpan Measure(TimeOnly boundary1, TimeOnly boundary2) => boundary2 - boundary1;

    public TimeSpan Zero() => TimeSpan.Zero;
}