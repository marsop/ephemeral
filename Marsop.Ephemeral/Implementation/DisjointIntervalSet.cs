// <copyright file="DisjointIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Marsop.Ephemeral.Exceptions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;

namespace Marsop.Ephemeral.Implementation;

/// <summary>
/// Disjoint interval set class
/// </summary>
public class DisjointIntervalSet : IDisjointIntervalSet<DateTimeOffset, TimeSpan>
{
    /// <summary>
    /// Internal sorted list of intervals
    /// </summary>
    private SortedList<IDateTimeOffsetInterval, IDateTimeOffsetInterval> _intervals = new SortedList<IDateTimeOffsetInterval, IDateTimeOffsetInterval>(new IntervalStartComparer());

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    public DisjointIntervalSet()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    /// <param name="intervals">an <see cref="Array"/> of <see cref="IDateTimeOffsetInterval"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(params IDateTimeOffsetInterval[] intervals)
    {
        if (intervals == null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        if (intervals?.Count() > 0)
        {
            foreach (var interval in intervals)
            {
                this.Add(interval);
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    /// <param name="intervals">an <see cref="IEnumerable{T}"/> of <see cref="IDateTimeOffsetInterval"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(IEnumerable<IDateTimeOffsetInterval> intervals)
    {
        if (intervals == null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        foreach (var interval in intervals)
        {
            this.Add(interval);
        }
    }

    /// <inheritdoc cref="IDisjointIntervalSet.AggregatedDuration"/>
    public TimeSpan AggregatedDuration => TimeSpan.FromTicks(this.Sum(x => x.Length().Ticks));

    /// <inheritdoc cref="IDateTimeOffsetInterval.Count"/>
    public int Count => this._intervals.Count;

    /// <inheritdoc cref="IDisjointIntervalSet.End"/>
    public DateTimeOffset End => this.Max(x => x.End);

    /// <inheritdoc cref="IDisjointIntervalSet.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <inheritdoc cref="IDisjointIntervalSet.IsContiguous"/>
    public bool IsContiguous => this.Consolidate().Count < 2;

    /// <inheritdoc cref="IDateTimeOffsetInterval.IsReadOnly"/>
    public bool IsReadOnly => false;

    /// <inheritdoc cref="IDisjointIntervalSet.Start"/>
    public DateTimeOffset Start => this.Min(x => x.Start);

    /// <inheritdoc cref="IDisjointIntervalSet.StartIncluded"/>
    public bool StartIncluded { get; }

    /// <inheritdoc cref="IList{T}.this[int]"/>
    public IDateTimeOffsetInterval this[int index]
    {
        get => this._intervals.Values[index];
        set => this._intervals.Values[index] = value;
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given interval is <code>null</code></exception>
    /// <exception cref="OverlapException">an exception is thrown if given interval overlaps another interval</exception>
    public void Add(IDateTimeOffsetInterval item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (this.Any(x => x.Intersects(item)))
        {
            throw new OverlapException(nameof(item));
        }

        this._intervals.Add(item, item);
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear() => this._intervals.Clear();

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains(IDateTimeOffsetInterval item) => item != null && this._intervals.ContainsKey(item);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(IDateTimeOffsetInterval[] array, int arrayIndex) => this._intervals.Values.CopyTo(array, arrayIndex);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<IDateTimeOffsetInterval> GetEnumerator() => this._intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => this._intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public int IndexOf(IDateTimeOffsetInterval item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return this._intervals.Values.IndexOf(item);
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert(int index, IDateTimeOffsetInterval item)
    {
        throw new NotSupportedException("The Set is always ordered, please use Add()");
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove(IDateTimeOffsetInterval item) => item != null && this._intervals.Remove(item);

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    /// <exception cref="ArgumentOutOfRangeException">an exception is thrown if index is less than zero or index is equal to or greater than intervals count</exception>
    public void RemoveAt(int index)
    {
        if (index >= 0 && this._intervals.Count > index)
        {
            this._intervals.RemoveAt(index);
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}
