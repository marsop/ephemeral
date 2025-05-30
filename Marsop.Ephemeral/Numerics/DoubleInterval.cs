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

    public override ILengthOperator<double, double> Operator => DoubleDefaultLengthOperator.Instance;

    public override string ToString() => base.ToString();

    public new static DoubleInterval CreateClosed(double start, double end) => new(start, end, true, true);

    public new static DoubleInterval CreateOpen(double start, double end) => new(start, end, false, false);

    public new static DoubleInterval CreatePoint(double boundary) => CreateClosed(boundary, boundary);
}