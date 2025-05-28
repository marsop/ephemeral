// <copyright file="LengthOperatorExtensions.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;

namespace Marsop.Ephemeral.Interfaces;

public static class LengthOperatorExtensions
{
    public static TLength MeasureInterval<TBoundary, TLength>(
        this ILengthOperator<TBoundary, TLength> lengthOperator,
        IBasicInterval<TBoundary> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        if (interval is null)
        {
            throw new ArgumentNullException(nameof(interval), "Interval cannot be null.");
        }

        return lengthOperator.Measure(interval.Start, interval.End);
    }
}