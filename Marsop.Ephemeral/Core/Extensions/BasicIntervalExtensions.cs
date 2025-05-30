// <copyright file="BasicIntervalExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using Marsop.Ephemeral.Core.Interfaces;
using Marsop.Ephemeral.Core.Implementation;

namespace Marsop.Ephemeral.Core.Extensions;

public static class BasicIntervalExtensions
{
    public static BasicMetricInterval<TBoundary, TLength> WithMetric<TBoundary, TLength>(
        this IBasicInterval<TBoundary> interval,
        ILengthOperator<TBoundary, TLength> lengthOperator)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        if (interval is null) throw new ArgumentNullException(nameof(interval));
        if (lengthOperator is null) throw new ArgumentNullException(nameof(lengthOperator));

        return new BasicMetricInterval<TBoundary, TLength>(
            interval.Start,
            interval.End,
            interval.StartIncluded,
            interval.EndIncluded,
            lengthOperator);
    }
}
