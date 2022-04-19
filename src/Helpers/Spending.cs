using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Helpers
{
    public static class Spending
    {
        public static Boolean buildIfPossible(int[] storage, List<int[]> costs)
        {
            foreach (var cost in costs)
            {
                if (storage[cost[0]] < cost[1])
                {
                    return false;
                }
            }

            foreach (var cost in costs)
            {
                storage[cost[0]] -= cost[1];
            }
                return true;
         }

    }
}
