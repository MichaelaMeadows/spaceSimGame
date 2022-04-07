using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Vehicles
{
    class Vehicle
    {
        public String name { get; set; }
        public int id { get; set; }
        public List<Tuple<int, int>> cost { get; set; }
        public int buildEffort { get; set; }
        public int speed { get; set; }
        public int capacity { get; set; }
        public int fuelPerUnit { get; set; }
        public int health { get; set; }
        public int armor { get; set; }
        public int shield { get; set; }
    }
}
