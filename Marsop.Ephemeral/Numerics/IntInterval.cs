using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Numerics;

public record IntInterval :
    FullInterval<int, int>
{
    public IntInterval(int start, int end, bool startIncluded = true, bool endIncluded = true)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<int, int> LengthOperator => IntDefaultLengthOperator.Instance;

}
