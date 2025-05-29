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
    private SortedList<IInterval<DateTimeOffset, TimeSpan>, IInterval<DateTimeOffset, TimeSpan>> _intervals = new(new IntervalStartComparer<DateTimeOffset>());

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    public DisjointIntervalSet()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    /// <param name="intervals">an <see cref="Array"/> of <see cref="IInterval<DateTimeOffset, TimeSpan>"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(params IInterval<DateTimeOffset, TimeSpan>[] intervals)
    {
        if (intervals is null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        if (intervals?.Count() > 0)
        {
            foreach (var interval in intervals)
            {
                Add(interval);
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    /// <param name="intervals">an <see cref="IEnumerable{T}"/> of <see cref="IInterval<DateTimeOffset, TimeSpan>"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(IEnumerable<IInterval<DateTimeOffset, TimeSpan>> intervals)
    {
        if (intervals is null)
        {
            throw new ArgumentNullException(nameof(intervals));
        }

        foreach (var interval in intervals)
        {
            Add(interval);
        }
    }

    /// <inheritdoc cref="IDisjointIntervalSet.AggregatedDuration"/>
    public TimeSpan AggregatedDuration => TimeSpan.FromTicks(this.Sum(x => x.Length().Ticks));

    /// <inheritdoc cref="IInterval<DateTimeOffset, TimeSpan>.Count"/>
    public int Count => _intervals.Count;

    /// <inheritdoc cref="IDisjointIntervalSet.End"/>
    public DateTimeOffset End => this.Max(x => x.End);

    /// <inheritdoc cref="IDisjointIntervalSet.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <inheritdoc cref="IDisjointIntervalSet.IsContiguous"/>
    public bool IsContiguous => this.Consolidate().Count < 2;

    /// <inheritdoc cref="IInterval<DateTimeOffset, TimeSpan>.IsReadOnly"/>
    public bool IsReadOnly => false;

    /// <inheritdoc cref="IDisjointIntervalSet.Start"/>
    public DateTimeOffset Start => this.Min(x => x.Start);

    /// <inheritdoc cref="IDisjointIntervalSet.StartIncluded"/>
    public bool StartIncluded { get; }

    /// <inheritdoc cref="IList{T}.this[int]"/>
    public IInterval<DateTimeOffset, TimeSpan> this[int index]
    {
        get => _intervals.Values[index];
        set => _intervals.Values[index] = value;
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given interval is <code>null</code></exception>
    /// <exception cref="OverlapException">an exception is thrown if given interval overlaps another interval</exception>
    public void Add(IInterval<DateTimeOffset, TimeSpan> item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (this.Any(x => x.Intersects(item)))
        {
            throw new OverlapException(nameof(item));
        }

        _intervals.Add(item, item);
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear() => _intervals.Clear();

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains(IInterval<DateTimeOffset, TimeSpan> item) => item != null && _intervals.ContainsKey(item);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(IInterval<DateTimeOffset, TimeSpan>[] array, int arrayIndex) => _intervals.Values.CopyTo(array, arrayIndex);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<IInterval<DateTimeOffset, TimeSpan>> GetEnumerator() => _intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => _intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public int IndexOf(IInterval<DateTimeOffset, TimeSpan> item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return _intervals.Values.IndexOf(item);
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert(int index, IInterval<DateTimeOffset, TimeSpan> item)
    {
        throw new NotSupportedException("The Set is always ordered, please use Add()");
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove(IInterval<DateTimeOffset, TimeSpan> item) => item != null && _intervals.Remove(item);

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    /// <exception cref="ArgumentOutOfRangeException">an exception is thrown if index is less than zero or index is equal to or greater than intervals count</exception>
    public void RemoveAt(int index)
    {
        if (index >= 0 && _intervals.Count > index)
        {
            _intervals.RemoveAt(index);
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}
