using System;

namespace Marsop.Ephemeral.Core;

public static class FullIntervalExtensions
{
    public static DisjointIntervalSet<TBoundary, TLength> Subtract<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();
        other.ValidateInterval();

        return new(interval.Operator, [.. BasicIntervalExtensions.Subtract(interval, other, interval.Operator)]);
    }

    public static DisjointIntervalSet<TBoundary, TLength> ToIntervalSet<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();

        return new(interval.Operator, [interval]);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> Shift<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();

        return BasicIntervalExtensions
            .Shift(interval, offset, interval.Operator)
            .WithMeasure(interval.Operator);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> ShiftStart<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();

        return BasicIntervalExtensions
            .ShiftStart(interval, offset, interval.Operator)
            .WithMeasure(interval.Operator);
    }

    public static BasicMeasuredInterval<TBoundary, TLength> ShiftEnd<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength offset)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();

        return BasicIntervalExtensions
            .ShiftEnd(interval, offset, interval.Operator)
            .WithMeasure(interval.Operator);
    }

    public static TLength LengthOfIntersect<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        IBasicInterval<TBoundary> other)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();
        other.ValidateInterval();

        return BasicIntervalExtensions.LengthOfIntersect(interval, other, interval.Operator);
    }

    /// <summary>
    /// Partitions an interval into sub-intervals of a specified length using the interval's default length operator.
    /// </summary>
    /// <typeparam name="TBoundary">The type of the interval boundaries.</typeparam>
    /// <typeparam name="TLength">The type representing the length of intervals.</typeparam>
    /// <param name="interval">The source <see cref="FullInterval{TBoundary, TLength}"/> instance.</param>
    /// <param name="step">The length of each partition chunk.</param>
    /// <param name="intermediateEndsClosed">A flag indicating whether intermediate interval ends are closed (<c>true</c>) or open (<c>false</c>). Defaults to <c>false</c>.</param>
    /// <returns>A <see cref="DisjointIntervalSet{TBoundary, TLength}"/> containing the partitioned sub-intervals.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="interval"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="step"/> does not advance the boundary forward.</exception>
    public static DisjointIntervalSet<TBoundary, TLength> Partition<TBoundary, TLength>(
        this FullInterval<TBoundary, TLength> interval,
        TLength step,
        bool intermediateEndsClosed = false)
        where TBoundary : notnull, IComparable<TBoundary>
    {
        interval.ValidateInterval();

        return BasicIntervalExtensions.Partition(interval, step, interval.Operator, intermediateEndsClosed);
    }
}
