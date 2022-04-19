using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Vehicles
{
    class BasicMiner : Vehicle
    {
        public BasicMiner(Tuple<int, int> location) : base(location)
        {
        }

        override public string getSprite()
        {
            return "smallCar";
        }

    }
}
