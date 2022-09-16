using SpaceSimulation.src.Vehicles;
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
            this.capacity = 50;
            this.current_capacity = 0;
            this.name = "ship1";
            this.miningpower = 2;
        }

        override public string getSprite()
        {
            return "ship1";
        }

        override public VehicleType getVehicleType()
        {
            return VehicleType.MINER;
        }

        public override List<int[]> getCost()
        {
            // TODO fix cost.
            List<int[]> cost = new List<int[]>();
            int[] c = new int[2];
            c[0] = 3;
            c[1] = 3;
            cost.Add(c);
            return cost;
        }

    }
}
