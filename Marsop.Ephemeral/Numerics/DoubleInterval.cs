using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Numerics;

public record DoubleInterval : 
    FullInterval<double, double>
{
    public DoubleInterval(double start, double end, bool startIncluded = true, bool endIncluded = true)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    public override ILengthOperator<double, double> LengthOperator => DoubleStandardLengthOperator.Instance;
}