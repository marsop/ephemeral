using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace seasonal
{
    public class DisjointIntervalSet : IList<IInterval>, IIntervalSet
    {

        private SortedList<IInterval> _intervals;

        public DisjointIntervalSet()
        {
            _intervals = new List<IInterval>();
        }

        public DisjointIntervalSet(IEnumerable<IInterval> intervals)
        {
            _intervals = intervals.OrderBy(x => x.Start).ToList();
        }

        public DateTimeOffset Start => this.Min(x => x.Start);
        public DateTimeOffset End => this.Max(x => x.End);
        public TimeSpan AggregatedDuration => TimeSpan.FromTicks(this.Sum(x => x.Duration.Ticks));
        public bool HasOverlap => this.Any(x => this.Any(y => x.Overlaps(x)));

        public int Count => _intervals.Count;

        public bool IsReadOnly => false;

        public IInterval this[int index]
        {
            get { return _intervals[index]; }
            set { _intervals[index] = value; }
        }

        public int IndexOf(IInterval item) => _intervals.IndexOf(item);

        public void Insert(int index, IInterval item) {
            if (this.Any(x => x.Overlaps(item)))
                throw new OverlapException(nameof(item));
            
            _intervals.Insert(index, item);
        } 

        public void RemoveAt(int index) => _intervals.RemoveAt(index);

        public void Add(IInterval item)
        {
            if (this.Any(x => x.Overlaps(item)))
                throw new OverlapException(nameof(item));

            _intervals.Add(item);
        }

        public void Clear() => _intervals.Clear();

        public bool Contains(IInterval item) => _intervals.Contains(item);

        public void CopyTo(IInterval[] array, int arrayIndex) => _intervals.CopyTo(array, arrayIndex);

        public bool Remove(IInterval item) => _intervals.Remove(item);

        public IEnumerator<IInterval> GetEnumerator() => _intervals.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _intervals.GetEnumerator();
    }
}