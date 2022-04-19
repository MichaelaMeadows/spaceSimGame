using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Vehicles
{
    class BasicMiner : Vehicle
    {
        public BasicMiner(Tuple<int, int> location) : base(location)
        {
            this.speed = 3;
            this.capacity = 10;
            this.current_capacity = 0;
            this.name = "smallCar";
        }

        override public string getSprite()
        {
            return "smallCar";
        }

        public List<int[]> getCost()
        {
            List<int[]> cost = new List<int[]>();
            int[] c = new int[2];
            c[0] = 0;
            c[1] = 100;
            cost.Add(c);
            return cost;
        }

    }
}
