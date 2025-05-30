using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Net6.Temporal
{
    public record TimeOnlyInterval : FullInterval<TimeOnly, TimeSpan>
    {
        public TimeOnlyInterval(TimeOnly start, TimeOnly end, bool startIncluded, bool endIncluded) :
            base(start, end, startIncluded, endIncluded)
        {
        }

        public override ILengthOperator<TimeOnly, TimeSpan> Operator =>
            TimeOnlyTimeSpanLengthOperator.Instance;
    }
}