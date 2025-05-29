using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal;

public record DateOnlyInterval : FullInterval<DateOnly, int>
{
    public DateOnlyInterval(DateOnly start, DateOnly end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<DateOnly, int> LengthOperator =>
        DateOnlyDaysLengthOperator.Instance;
}