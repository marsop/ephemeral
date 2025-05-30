// <copyright file="AbstractMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Core.Implementation;

public abstract record AbstractMetricInterval<TBoundary, TLength> :
    BasicInterval<TBoundary>,
    IHasLength<TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public AbstractMetricInterval(
        TBoundary start,
        TBoundary end,
        bool startIncluded,
        bool endIncluded) :
        base(start, end, startIncluded, endIncluded)
    {
    }

    public abstract TLength Length();
}
