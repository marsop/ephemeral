// <copyright file="BasicMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

public record BasicMetricInterval<TBoundary, TLength> :
    BasicInterval<TBoundary>,
    IMetricInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public BasicMetricInterval(
        TBoundary start,
         TBoundary end,
          bool startIncluded,
           bool endIncluded,
           ILengthOperator<TBoundary, TLength> lengthOperator)
        : base(start, end, startIncluded, endIncluded)
    {
        LengthOperator = lengthOperator ?? throw new ArgumentNullException(nameof(lengthOperator));
    }

    public ILengthOperator<TBoundary, TLength> LengthOperator { get; }

    public TLength Length() => LengthOperator.MeasureInterval(this);
}
