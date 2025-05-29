// <copyright file="DisjointIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Marsop.Ephemeral.Core.Exceptions;
using Marsop.Ephemeral.Core.Interfaces;
using Marsop.Ephemeral.Core.Extensions;

namespace Marsop.Ephemeral.Core.Implementation;

/// <summary>
/// Disjoint interval set class
/// </summary>
public class DisjointIntervalSet<TBoundary, TLength> :
    IDisjointIntervalSet<TBoundary, TLength>
    where TBoundary : notnull, IComparable<TBoundary>
{
    /// <summary>
    /// Internal sorted list of intervals
    /// </summary>
    private SortedList<IInterval<TBoundary, TLength>, IInterval<TBoundary, TLength>> _intervals = new(new IntervalStartComparer<TBoundary>());

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    public DisjointIntervalSet(ILengthOperator<TBoundary, TLength> lengthOperator)
    {
        if (lengthOperator is null)
        {
            throw new ArgumentNullException(nameof(lengthOperator));
        }

        LengthOperator = lengthOperator;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
    /// </summary>
    /// <param name="intervals">an <see cref="Array"/> of <see cref="IInterval<TBoundary, TLength>"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(
        ILengthOperator<TBoundary, TLength> lengthOperator,
        params IInterval<TBoundary, TLength>[] intervals)
        : this(lengthOperator)
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
    /// <param name="intervals">an <see cref="IEnumerable{T}"/> of <see cref="IInterval<TBoundary, TLength>"/> to initialize the set</param>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public DisjointIntervalSet(
        ILengthOperator<TBoundary, TLength> lengthOperator, 
        IEnumerable<IInterval<TBoundary, TLength>> intervals)
        : this(lengthOperator)
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

    /// <inheritdoc cref="IInterval<TBoundary, TLength>.Count"/>
    public int Count => _intervals.Count;

    /// <inheritdoc cref="IDisjointIntervalSet.End"/>
    public TBoundary End => this.Max(x => x.End);

    /// <inheritdoc cref="IDisjointIntervalSet.EndIncluded"/>
    public bool EndIncluded { get; }

    /// <inheritdoc cref="IDisjointIntervalSet.IsContiguous"/>
    public bool IsContiguous => this.Consolidate().Count < 2;

    /// <inheritdoc cref="IInterval<TBoundary, TLength>.IsReadOnly"/>
    public bool IsReadOnly => false;

    /// <inheritdoc cref="IDisjointIntervalSet.Start"/>
    public TBoundary Start => this.Min(x => x.Start);

    /// <inheritdoc cref="IDisjointIntervalSet.StartIncluded"/>
    public bool StartIncluded { get; }
    public ILengthOperator<TBoundary, TLength> LengthOperator { get; }

    /// <inheritdoc cref="IList{T}.this[int]"/>
    public IInterval<TBoundary, TLength> this[int index]
    {
        get => _intervals.Values[index];
        set => _intervals.Values[index] = value;
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given interval is <code>null</code></exception>
    /// <exception cref="OverlapException">an exception is thrown if given interval overlaps another interval</exception>
    public void Add(IInterval<TBoundary, TLength> item)
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
    public bool Contains(IInterval<TBoundary, TLength> item) => item != null && _intervals.ContainsKey(item);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo(IInterval<TBoundary, TLength>[] array, int arrayIndex) => _intervals.Values.CopyTo(array, arrayIndex);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<IInterval<TBoundary, TLength>> GetEnumerator() => _intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => _intervals.Values.GetEnumerator();

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    /// <exception cref="ArgumentNullException">an exception is thrown if given parameter is <code>null</code></exception>
    public int IndexOf(IInterval<TBoundary, TLength> item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        return _intervals.Values.IndexOf(item);
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert(int index, IInterval<TBoundary, TLength> item)
    {
        throw new NotSupportedException("The Set is always ordered, please use Add()");
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove(IInterval<TBoundary, TLength> item) => item != null && _intervals.Remove(item);

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
