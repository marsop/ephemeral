using System.Collections.Generic;

namespace seasonal
{
    public interface IIntervalSet : ISet<IInterval>, IInterval
    {
         IEnumerable<IInterval> Intervals {get;}

         /// <summary>
         /// Minimum interval that comprises all the intervals of the set.
         /// </summary>
         /// <returns></returns>
         IInterval BoundingInterval {get;}

         /// <summary>
         /// Joins adjacent intervals.
         /// </summary>
         /// <returns> new set with the minimum amount of intervals</returns>
         IIntervalSet Consolidate();


         
    }
}