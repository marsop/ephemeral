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

    public override ILengthOperator<int, int> Operator => IntDefaultLengthOperator.Instance;

    public override string ToString() => base.ToString();

    public new static IntInterval CreateClosed(int start, int end) => new(start, end, true, true);

    public new static IntInterval CreateOpen(int start, int end) => new(start, end, false, false);

    public new static IntInterval CreatePoint(int boundary) => CreateClosed(boundary, boundary);

}
