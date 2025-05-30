using System;
using Marsop.Ephemeral.Core.Implementation;
using Marsop.Ephemeral.Core.Interfaces;

namespace Marsop.Ephemeral.Core.Extensions;

public static class FullIntervalExtensions
{
    public static DisjointIntervalSet<TBoundary, TLength> Subtract<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return new(interval.LengthOperator, [.. BasicIntervalExtensions.Subtract(interval, other, interval.LengthOperator)]);
    }

    public static DisjointIntervalSet<TBoundary, TLength> ToIntervalSet<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return new(interval.LengthOperator, [interval]);
    }

    public static BasicMetricInterval<TBoundary, TLength> Shift<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .Shift(interval, offset, interval.LengthOperator)
            .WithMetric(interval.LengthOperator);
    }

    public static BasicMetricInterval<TBoundary, TLength> ShiftStart<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .ShiftStart(interval, offset, interval.LengthOperator)
            .WithMetric(interval.LengthOperator);
    }

    public static BasicMetricInterval<TBoundary, TLength> ShiftEnd<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .ShiftEnd(interval, offset, interval.LengthOperator)
            .WithMetric(interval.LengthOperator);
    }

    public static TLength LengthOfIntersect<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions.LengthOfIntersect(interval, other, interval.LengthOperator);
    }
}