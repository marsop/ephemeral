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
        return new(interval.Operator, [.. BasicIntervalExtensions.Subtract(interval, other, interval.Operator)]);
    }

    public static DisjointIntervalSet<TBoundary, TLength> ToIntervalSet<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return new(interval.Operator, [interval]);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> Shift<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .Shift(interval, offset, interval.Operator)
            .WithMetric(interval.Operator);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> ShiftStart<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .ShiftStart(interval, offset, interval.Operator)
            .WithMetric(interval.Operator);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> ShiftEnd<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions
            .ShiftEnd(interval, offset, interval.Operator)
            .WithMetric(interval.Operator);
    }

    public static TLength LengthOfIntersect<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        return BasicIntervalExtensions.LengthOfIntersect(interval, other, interval.Operator);
    }
}