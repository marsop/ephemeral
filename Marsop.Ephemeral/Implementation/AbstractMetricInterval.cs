// <copyright file="AbstractMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public abstract record AbstractMetricInterval<TBoundary, TLength> :
    BasicInterval<TBoundary>,
    IMetricInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public AbstractMetricInterval(
        TBoundary start,
         TBoundary end,
          bool startIncluded,
           bool endIncluded)
        : base(start, end, startIncluded, endIncluded)
    {
    }

    public abstract ILengthOperator<TBoundary, TLength> LengthOperator { get; }

    public TLength Length() => LengthOperator.MeasureInterval(this);
}
