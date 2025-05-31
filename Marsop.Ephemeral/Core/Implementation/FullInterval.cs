// <copyright file="FullInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core;

public abstract record FullInterval<TBoundary, TLength> :
    AbstractMeasuredInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public FullInterval(TBoundary start, TBoundary end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public abstract ILengthOperator<TBoundary, TLength> Operator { get; }

    public override TLength DefaultMeasure() => Operator.Measure(this);

    public override string ToString() => base.ToString();
}
