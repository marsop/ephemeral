// <copyright file="DisjointIntervalSet.cs" company="Marsop">
//     https://github.com/marsop/ephemeral
// </copyright>

namespace Marsop.Ephemeral
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Marsop.Ephemeral.Extensions;

    /// <summary>
    /// Disjoint interval set class
    /// </summary>
    public class DisjointIntervalSet : IDisjointIntervalSet
    {
        /// <summary>
        /// Internal sorted list of intervals
        /// </summary>
        private SortedList<IInterval, IInterval> _intervals = new SortedList<IInterval, IInterval>(new IntervalStartComparer());

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
        /// </summary>
        public DisjointIntervalSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisjointIntervalSet" /> class
        /// </summary>
        /// <param name="intervals">an <see cref="Array"/> of <see cref="IInterval"/> to initialize the set</param>
        public DisjointIntervalSet(params IInterval[] intervals)
        {
            if (intervals != null && intervals.Count() > 0)
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
        /// <param name="intervals">an <see cref="IEnumerable{T}"/> of <see cref="IInterval"/> to initialize the set</param>
        public DisjointIntervalSet(IEnumerable<IInterval> intervals)
        {
            foreach (var interval in intervals)
            {
                this.Add(interval);
            }
        }

        /// <inheritdoc cref="IDisjointIntervalSet.Start"/>
        public DateTimeOffset Start => this.Min(x => x.Start);

        /// <inheritdoc cref="IDisjointIntervalSet.End"/>
        public DateTimeOffset End => this.Max(x => x.End);

        /// <inheritdoc cref="IDisjointIntervalSet.AggregatedDuration"/>
        public TimeSpan AggregatedDuration => TimeSpan.FromTicks(this.Sum(x => x.Duration.Ticks));

        /// <inheritdoc cref="IDisjointIntervalSet.IsContiguous"/>
        public bool IsContiguous => this.Consolidate().Count < 2;

        /// <inheritdoc cref="ICollection{Marsop.Ephemeral.IInterval}.Count"/>
        public int Count => this._intervals.Count;

        /// <inheritdoc cref="ICollection{Marsop.Ephemeral.IInterval}.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IDisjointIntervalSet.StartIncluded"/>
        public bool StartIncluded { get; }

        /// <inheritdoc cref="IDisjointIntervalSet.EndIncluded"/>
        public bool EndIncluded { get; }
        
        /// <inheritdoc cref="IList{T}.this[int]"/>
        public IInterval this[int index]
        {
            get { return this._intervals.Values[index]; }
            set { this._intervals.Values[index] = value; }
        }

        /// <inheritdoc cref="IList{T}.IndexOf"/>
        public int IndexOf(IInterval item) => this._intervals.Values.IndexOf(item);

        /// <inheritdoc cref="IList{T}.Insert"/>
        public void Insert(int index, IInterval item)
        {
            throw new NotSupportedException("The Set is always ordered, please use Add()");
        }

        /// <inheritdoc cref="IList{T}.RemoveAt"/>
        public void RemoveAt(int index) => this._intervals.RemoveAt(index);

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void Add(IInterval item)
        {
            if (this.Any(x => x.Intersects(item)))
            {
                throw new OverlapException(nameof(item));
            }

            this._intervals.Add(item, item);
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void Clear() => this._intervals.Clear();

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains(IInterval item) => this._intervals.ContainsKey(item);

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo(IInterval[] array, int arrayIndex) => this._intervals.Values.CopyTo(array, arrayIndex);

        /// <inheritdoc cref="ICollection{T}.Remove"/>
        public bool Remove(IInterval item) => this._intervals.Remove(item);

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<IInterval> GetEnumerator() => this._intervals.Values.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => this._intervals.Values.GetEnumerator();
    }
}