// <copyright file="AbstractMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Core;

public abstract record AbstractMeasuredInterval<TBoundary, TLength> :
    BasicInterval<TBoundary>,
    IHasLength<TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public AbstractMeasuredInterval(
        TBoundary start,
        TBoundary end,
        bool startIncluded,
        bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public abstract TLength DefaultMeasure();
}
