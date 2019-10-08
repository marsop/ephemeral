using System;
using Xunit;
using FluentAssertions;

namespace Marsop.Ephemeral.Tests.Implementation
{
    /// <summary>
    ///     Testing class for <see cref="IntervalStartComparer"/>
    /// </summary>
    public class IntervalStartComparerTests
    {
        public class TheCompareMethod
        {
            private RandomHelper _randomHelper = new RandomHelper();

            [Theory]
            [InlineData(true, true)]            
            [InlineData(false, false)]
            private void WhenBothIntervalsHaveTheSameStartDateAndSameStartIncludedValue_ThenReturnsZero(bool startIncludedIntervalA, bool startIncludedIntervalB) 
            {
                //arrange
                DateTime now = _randomHelper.GetDateTime();

                var intervalA = _randomHelper.GetInterval(now, null, startIncludedIntervalA);
                var intervalB = _randomHelper.GetInterval(now, null, startIncludedIntervalB);

                var sut = new IntervalStartComparer();

                //act
                int result = sut.Compare(intervalA,intervalB);

                //assert
                result.Should().Be(0,"start dates are the same");
            }


            [Theory]
            [InlineData(true, false, -1)]
            [InlineData(false,true, 1)]
            private void WhenBothIntervalsHaveTheSameStartDateButDifferentStartIncludedValue_ThenReturnsDifferentThanZero(bool startIncludedIntervalA, bool startIncludedIntervalB, int expectedResult)
            {
                //arrange                
                DateTime now = _randomHelper.GetDateTime();

                var intervalA = _randomHelper.GetInterval(now,null, startIncludedIntervalA); 
                var intervalB = _randomHelper.GetInterval(now, null, startIncludedIntervalB); 

                var sut = new IntervalStartComparer();

                //act
                int result = sut.Compare(intervalA, intervalB);

                //assert
                result.Should().Be(expectedResult);
            }

            [Fact]
            private void WhenFirstIntervalStartDateIsEarlierThanSecondIntervalStartDate_ThenReturnsMinusOne()
            {
                //arrange                
                DateTime now = _randomHelper.GetDateTime();

                var intervalA = _randomHelper.GetInterval(now);
                var intervalB = _randomHelper.GetInterval(now.AddTicks(1));

                var sut = new IntervalStartComparer();

                //act
                int result = sut.Compare(intervalA, intervalB);

                //assert
                result.Should().Be(-1);
            }

            [Fact]
            private void WhenFirstIntervalStartDateIsLaterThanSecondIntervalStartDate_ThenReturnsOne()
            {
                //arrange                
                DateTime now = _randomHelper.GetDateTime();

                var intervalA = _randomHelper.GetInterval(now);
                var intervalB = _randomHelper.GetInterval(now.AddTicks(-1));

                var sut = new IntervalStartComparer();

                //act
                int result = sut.Compare(intervalA, intervalB);

                //assert
                result.Should().Be(1);
            }
        }
    }
}
