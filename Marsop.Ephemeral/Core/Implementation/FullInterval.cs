// <copyright file="FullInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Core.Implementation;

public abstract record FullInterval<TBoundary, TLength> :
    AbstractMetricInterval<TBoundary, TLength>,
    IHasLengthOperator<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public FullInterval(TBoundary start, TBoundary end, bool startIncluded, bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public abstract ILengthOperator<TBoundary, TLength> LengthOperator { get; }

    public override TLength Length() => LengthOperator.Measure(Start, End);
}
