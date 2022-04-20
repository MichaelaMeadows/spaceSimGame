using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Helpers
{
    static class Distances
    {

        public static Tuple<int, int> findNextPosition(WorldState ws, Tuple<int, int> start, Tuple<int, int> end, int speed)
        {
            var d = distance(start, end);
            if (d == 0)
            {
                return start;
            }

            int dx = end.Item1 - start.Item1;
            int dy = end.Item2 - start.Item2;


            int change_x = (int)Math.Round((dx / d) * speed);
            int change_y = (int)Math.Round((dy / d) * speed);

            int new_x = start.Item1 + change_x;
            int new_y = start.Item2 + change_y;

            return new Tuple<int, int>(new_x, new_y);
/*            if (ws.map[new_x, new_y] == null)
            {
                return new Tuple<int, int>(new_x, new_y);
            } else
            {
                return ws.findEmptyNeighbor(new_x, new_y);
            }

            return null;*/
        }

        public static double distance(Tuple<int, int> p1, Tuple<int, int> p2)
        {
            return Math.Sqrt(Math.Pow(p1.Item1 - p2.Item1, 2) + Math.Pow(p1.Item2 - p2.Item2, 2));
        }
    }
}
