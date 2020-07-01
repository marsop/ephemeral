using System;
using Marsop.Ephemeral.Implementation;

namespace Marsop.Ephemeral.Tests
{
    /// <summary>
    ///     Helper class to handle tipical random methods
    /// </summary>
    public class RandomHelper
    {
        private Random _random = new Random();

        /// <summary>
        ///     Gets a random between a threshold
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>        
        public int GetIntInRanger(int min, int max) {            
            return _random.Next(min, max);            
        }

        /// <summary>
        ///     Gets a random bool
        /// </summary>        
        public bool GetBool() {
            return _random.Next(2) == 0;
        }

        /// <summary>
        ///     Get a random datetime
        ///     From: https://stackoverflow.com/questions/194863/random-date-in-c-sharp
        /// </summary>        
        public DateTime GetDateTime()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_random.Next(range));
        }

        /// <summary>
        ///     Gets a random interval based on a start date
        /// </summary>
        /// <param name="startDate">Start date (mandatory)</param>
        /// <param name="endDate">End date (optional)</param>
        /// <param name="startIncluded">Start included (optional)</param>
        /// <param name="endIncluded">End included (optional)</param>        
        public Interval GetInterval(DateTime startDate, DateTime? endDate = null, bool? startIncluded=null, bool? endIncluded=null)
        {
            DateTime end = endDate.HasValue ? endDate.Value : startDate.AddDays(GetIntInRanger(1, 10));
            bool startIncludedBool = startIncluded.HasValue ? startIncluded.Value : GetBool();
            bool endIncludedBool = endIncluded.HasValue ? endIncluded.Value : GetBool();

            DateTimeOffset intervalStartDate = new DateTimeOffset(startDate);
            DateTimeOffset intervalEndDate = new DateTimeOffset(end);

            return new Interval(intervalStartDate, intervalEndDate, startIncludedBool, endIncludedBool);
        }
    }
}
