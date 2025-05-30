using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal;

public record TimeOnlyInterval : FullInterval<TimeOnly, TimeSpan>
{
    public TimeOnlyInterval(TimeOnly start, TimeOnly end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<TimeOnly, TimeSpan> Operator =>
        TimeOnlyTimeSpanLengthOperator.Instance;

    public override string ToString() => base.ToString();

    public new static TimeOnlyInterval CreateClosed(TimeOnly start, TimeOnly end) => new(start, end, true, true);

    public new static TimeOnlyInterval CreateOpen(TimeOnly start, TimeOnly end) => new(start, end, false, false);

    public new static TimeOnlyInterval CreatePoint(TimeOnly boundary) => CreateClosed(boundary, boundary);
}