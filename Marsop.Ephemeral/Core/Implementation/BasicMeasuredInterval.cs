// <copyright file="BasicMetricInterval.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Core.Implementation;

public record BasicMeasuredInterval<TBoundary, TLength> :
    AbstractMeasuredInterval<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    public BasicMeasuredInterval(
        TBoundary start,
        TBoundary end,
        bool startIncluded,
        bool endIncluded,
        ICanMeasure<TBoundary, TLength> measureCalculator)
        : base(start, end, startIncluded, endIncluded)
    {
        if (measureCalculator is null)
        {
            throw new ArgumentNullException(nameof(measureCalculator));
        }

        _measure = measureCalculator.Measure(this);
    }

    private readonly TLength _measure;

    public override TLength DefaultMeasure()  => _measure;
}
