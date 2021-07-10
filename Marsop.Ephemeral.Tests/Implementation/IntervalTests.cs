using System;
using System.Linq;
using FluentAssertions;
using Marsop.Ephemeral.Implementation;
using Xunit;

namespace Marsop.Ephemeral.Tests.Implementation
{
    public class IntervalTests
    {
        private readonly RandomHelper _randomHelper = new RandomHelper();

        [Fact]
        public void Test_Subtract_Null()
        {
            //Given
            var now = _randomHelper.GetDateTime();

            var interval = _randomHelper.GetInterval(now, null);

            Assert.Throws<ArgumentNullException>(() => Interval.Subtract(null, null));
            Assert.Throws<ArgumentNullException>(() => Interval.Subtract(interval, null));
            Assert.Throws<ArgumentNullException>(() => Interval.Subtract(null, interval));
        }

        [Fact]
        public void Test_Subtract_NoIntersection()
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12));
            var subtraction = _randomHelper.GetInterval(date.AddHours(14), date.AddHours(18));

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            result.Should().ContainSingle("No intersection, should return the first interval");
            result.First().Should().BeEquivalentTo(source);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Test_Subtract_Full(bool startIncludedInterval, bool endIncludedInterval)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedInterval, endIncludedInterval);
            var subtraction = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedInterval, endIncludedInterval);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            result.Should().BeEmpty("Full intersection, should return empty");
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Subtract_Inner(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var subtraction = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            var expected1 = _randomHelper.GetInterval(source.Start, subtraction.Start, source.StartIncluded, !subtraction.StartIncluded);
            var expected2 = _randomHelper.GetInterval(subtraction.End, source.End, !subtraction.EndIncluded, source.EndIncluded);
            result.Count.Should().Be(2);
            result.First().Should().BeEquivalentTo(expected1);
            result.Last().Should().BeEquivalentTo(expected2);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void Test_Subtract_SourceStartsBefore(bool startIncludedIntervalA, bool startIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA);
            var subtraction = _randomHelper.GetInterval(date.AddHours(11), date.AddHours(13), startIncludedIntervalB);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            var expected = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(11), startIncludedIntervalA, !startIncludedIntervalB);
            result.Should().ContainSingle();
            result.First().Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Subtract_SourceStartsEqual(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var subtraction = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            var expected = _randomHelper.GetInterval(date.AddHours(11), date.AddHours(12), !endIncludedIntervalB, endIncludedIntervalA);
            result.Should().ContainSingle();
            result.First().Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Subtract_SourceEndsEqual(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var subtraction = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(12), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            var expected = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(9), startIncludedIntervalA, !startIncludedIntervalB);
            result.Should().ContainSingle();
            result.First().Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Subtract_SourceStartsAfter(
            bool startIncludedIntervalA, bool startIncludedIntervalB,
            bool endIncludedIntervalA, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var subtraction = _randomHelper.GetInterval(date.AddHours(6), date.AddHours(9), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = Interval.Subtract(source, subtraction);

            //Then
            var expected = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(12), !endIncludedIntervalB, endIncludedIntervalA);
            result.Should().ContainSingle();
            result.First().Should().BeEquivalentTo(expected);
        }
    }
}