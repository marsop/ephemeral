using FluentAssertions;
using Marsop.Ephemeral.Extensions;
using Marsop.Ephemeral.Interfaces;
using Xunit;

namespace Marsop.Ephemeral.Tests.Extensions
{
    public class IntervalExtensionsTests
    {
        private readonly RandomHelper _randomHelper = new RandomHelper();

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Covers(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var other = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = source.Covers(other);

            //Then
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, false)]
        public void Test_Intersect(bool startIncludedIntervalA, bool endIncludedIntervalA, bool startIncludedIntervalB, bool endIncludedIntervalB)
        {
            //Given
            var date = _randomHelper.GetDateTime();

            var source = _randomHelper.GetInterval(date.AddHours(8), date.AddHours(12), startIncludedIntervalA, endIncludedIntervalA);
            var other = _randomHelper.GetInterval(date.AddHours(9), date.AddHours(11), startIncludedIntervalB, endIncludedIntervalB);

            //When
            var result = source.Intersect(other).Match(x => x.ToInterval(), () => default(IInterval));

            //Then
            result.Should().BeEquivalentTo(other);
        }

    }
}