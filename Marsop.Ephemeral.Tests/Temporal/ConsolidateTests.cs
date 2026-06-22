using System;
using System.Linq;
using Xunit;
using Marsop.Ephemeral.Temporal;
using Marsop.Ephemeral.Core;

namespace Marsop.Ephemeral.Tests.Temporal
{
    public class ConsolidateTests
    {
        private static DateTimeOffsetInterval IntervalClosedOpen(DateTimeOffset start, DateTimeOffset end, bool startIncluded = true, bool endIncluded = false)
            => new DateTimeOffsetInterval(start, end, startIncluded, endIncluded);

        [Fact]
        public void Consolidate_EmptySet_ReturnsEmpty()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var set = i1.ToIntervalSet();
            set.Remove(i1);
            var consolidated = set.Consolidate();
            Assert.Empty(consolidated);
        }

        [Fact]
        public void Consolidate_SingleItem_ReturnsSame()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var set = i1.ToIntervalSet();
            var consolidated = set.Consolidate();
            Assert.Single(consolidated);
            Assert.Equal(i1.Start, consolidated.First().Start);
            Assert.Equal(i1.End, consolidated.First().End);
        }

        [Fact]
        public void Consolidate_NonAdjacentItems_ReturnsAll()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddMinutes(2), now.AddMinutes(3));
            var set = i1.ToIntervalSet();
            set.Add(i2);
            var consolidated = set.Consolidate();
            Assert.Equal(2, consolidated.Count);
            Assert.Equal(i1.Start, consolidated.First().Start);
            Assert.Equal(i2.End, consolidated.Last().End);
        }

        [Fact]
        public void Consolidate_MixedAdjacentAndNonAdjacent_ReturnsCorrectSubset()
        {
            var now = DateTimeOffset.UtcNow;
            var i1 = IntervalClosedOpen(now, now.AddMinutes(1));
            var i2 = IntervalClosedOpen(now.AddMinutes(1), now.AddMinutes(2));
            var i3 = IntervalClosedOpen(now.AddMinutes(3), now.AddMinutes(4));
            var set = i1.ToIntervalSet();
            set.Add(i2);
            set.Add(i3);
            var consolidated = set.Consolidate();
            Assert.Equal(2, consolidated.Count);
            var firstConsolidated = consolidated.First();
            var lastConsolidated = consolidated.Last();
            Assert.Equal(now, firstConsolidated.Start);
            Assert.Equal(now.AddMinutes(2), firstConsolidated.End);
            Assert.Equal(now.AddMinutes(3), lastConsolidated.Start);
            Assert.Equal(now.AddMinutes(4), lastConsolidated.End);
        }
    }
}
