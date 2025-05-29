using System;
using System.Linq;
using Marsop.Ephemeral.Core.Extensions;
using Xunit;
using Marsop.Ephemeral.Temporal;

namespace Marsop.Ephemeral.Tests.Extensions
{
    public class IntervalSetExtensionsTests
    {
        private static StandardInterval IntervalClosedOpen(DateTimeOffset start, DateTimeOffset end, bool startIncluded = true, bool endIncluded = false)
            => new StandardInterval(start, end, startIncluded, endIncluded);


        [Fact]
        public void Consolidate_JoinsAdjacentIntervals()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(DateTimeOffset.MinValue, now);
            var i2 = IntervalClosedOpen(now, DateTimeOffset.MaxValue);
            var set = new DisjointStandardIntervalSet(i1, i2);
            var consolidated = set.Consolidate();
            Assert.Single(consolidated);
            Assert.Equal(i1.Start, consolidated.First().Start);
            Assert.Equal(i2.End, consolidated.First().End);
        }

        [Fact]
        public void Covers_ReturnsTrueIfTimestampIsCovered()
        {
            var now = DateTimeOffset.UtcNow;
            var interval = IntervalClosedOpen(now.AddMinutes(-1), now.AddMinutes(1));
            var set = new DisjointStandardIntervalSet(interval);
            Assert.True(set.Covers(now));
        }

        [Fact]
        public void Covers_ReturnsFalseIfTimestampIsNotCovered()
        {
            var now = DateTimeOffset.UtcNow;
            var interval = IntervalClosedOpen(now.AddMinutes(-2), now.AddMinutes(-1));
            var set = new DisjointStandardIntervalSet(interval);
            Assert.False(set.Covers(now));
        }

        [Fact]
        public void Intersect_ReturnsIntersectionWithInterval()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now.AddMinutes(-2), now.AddMinutes(2));
            var i2 = IntervalClosedOpen(now.AddMinutes(-1), now.AddMinutes(1));
            var set = new DisjointStandardIntervalSet(i1);
            var result = set.Intersect(i2);
            Assert.Single(result);
            Assert.Equal(i2.Start, result.Single().Start);
            Assert.Equal(i2.End, result.Single().End);
        }

        [Fact]
        public void Join_WithSet_JoinsTwoSets()
        {
            var now = new DateTimeOffset(1999, 01, 01, 10, 0, 0, TimeSpan.Zero);
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddMinutes(1), now.AddMinutes(2));
            var set1 = new DisjointStandardIntervalSet(i1);
            var set2 = new DisjointStandardIntervalSet(i2);
            var joined = set1.Join(set2);
            Assert.Equal(i1.Start, joined.First().Start);
            Assert.Equal(i2.End, joined.Last().End);
        }

        [Fact]
        public void GetBoundingInterval_ReturnsCorrectBounds()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddMinutes(2), now.AddMinutes(3));
            var set = new DisjointStandardIntervalSet(i1, i2);
            var bounding = set.GetBoundingInterval();
            Assert.Equal(i1.Start, bounding.Start);
            Assert.Equal(i2.End, bounding.End);
        }

        [Fact]
        public void Join_WithInterval_JoinsOverlappingIntervals()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddSeconds(30), now.AddMinutes(2));
            var set = new DisjointStandardIntervalSet(i1);
            var joined = set.Join(i2);
            Assert.Single(joined);
            Assert.Equal(i1.Start, joined.First().Start);
            Assert.Equal(i2.End, joined.First().End);
        }

        [Fact]
        public void Join_WithInterval_JoinsNonOverlappingIntervals()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddMinutes(2), now.AddMinutes(3));
            var set = new DisjointStandardIntervalSet(i1);
            var joined = set.Join(i2);
            Assert.Equal(2, joined.Count);
            Assert.Contains(joined, x => x.Start == i1.Start && x.End == i1.End);
            Assert.Contains(joined, x => x.Start == i2.Start && x.End == i2.End);
        }
    }
}
