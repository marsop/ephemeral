using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Marsop.Ephemeral
{
    public class DisjointIntervalSet : IDisjointIntervalSet
    {

        private SortedList<IInterval, IInterval> _intervals = new SortedList<IInterval, IInterval>(new IntervalStartComparer());

        public DisjointIntervalSet() { }

        public DisjointIntervalSet(params IInterval[] intervals)
        {
            if (intervals != null && intervals.Count() > 0) {

                foreach (var interval in intervals)
                {
                    Add(interval);
                }
            }
        }

        public DisjointIntervalSet(IEnumerable<IInterval> intervals)
        {
            foreach (var interval in intervals)
            {
                Add(interval);
            }
        }

        public DateTimeOffset Start => this.Min(x => x.Start);

        public DateTimeOffset End => this.Max(x => x.End);

        public TimeSpan AggregatedDuration => TimeSpan.FromTicks(this.Sum(x => x.Duration.Ticks));

        // true if either there are no intervals or all can be smashed together into one
        public bool IsContiguous => this.Consolidate().Count < 2;

        public int Count => _intervals.Count;

        public bool IsReadOnly => false;


        public bool StartIncluded { get; }

        public bool EndIncluded { get; }

        public IInterval this[int index]
        {
            get { return _intervals.Values[index]; }
            set { _intervals.Values[index] = value; }
        }

        public int IndexOf(IInterval item) => _intervals.Values.IndexOf(item);

        public void Insert(int index, IInterval item)
        {
            throw new NotSupportedException("The Set is always ordered, please use Add()");
        }

        public void RemoveAt(int index) => _intervals.RemoveAt(index);

        public void Add(IInterval item)
        {
            if (this.Any(x => x.Intersects(item)))
                throw new OverlapException(nameof(item));

            _intervals.Add(item, item);
        }

        public void Clear() => _intervals.Clear();

        public bool Contains(IInterval item) => _intervals.ContainsKey(item);

        public void CopyTo(IInterval[] array, int arrayIndex) => _intervals.Values.CopyTo(array, arrayIndex);

        public bool Remove(IInterval item) => _intervals.Remove(item);

        public IEnumerator<IInterval> GetEnumerator() => _intervals.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _intervals.Values.GetEnumerator();
    }
}