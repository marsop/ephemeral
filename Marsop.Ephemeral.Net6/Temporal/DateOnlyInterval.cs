using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal;

public record DateOnlyInterval : FullInterval<DateOnly, int>
{
    public DateOnlyInterval(DateOnly start, DateOnly end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<DateOnly, int> Operator =>
        DateOnlyDaysLengthOperator.Instance;

    public override string ToString() => base.ToString();

    public new static DateOnlyInterval CreateClosed(DateOnly start, DateOnly end) => new(start, end, true, true);

    public new static DateOnlyInterval CreateOpen(DateOnly start, DateOnly end) => new(start, end, false, false);

    public new static DateOnlyInterval CreatePoint(DateOnly boundary) => CreateClosed(boundary, boundary);


}