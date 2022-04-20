using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Helpers
{
    public static class Spending
    {

        public static bool canAfford(int[] storage, List<int[]> costs)
        {
            foreach (var cost in costs)
            {
                if (storage[cost[0]] < cost[1])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool buildIfPossible(int[] storage, List<int[]> costs)
        {
            if (canAfford(storage, costs)) { 

            foreach (var cost in costs)
            {
                storage[cost[0]] -= cost[1];
            }
                return true;
        }   else
            {
                return false;
            }
         }

    }
}
