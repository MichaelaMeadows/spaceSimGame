using SpaceSimulation.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Ships
{
    interface Ship
    {
        public String name { get; set; }
        public int id { get; set; }
        public List<Tuple<int, int>> cost { get; set; }
        public int buildEffort { get; set; }
        public int speed { get; set; }
        public int fuel { get; set; }
        public int fuelPerUnit { get; set; }
        public int health { get; set; }
        public int armor { get; set; }
        public int shield { get; set; }
        public Tuple<int, int> location { get; set; }
        public Command command { get; set; }
        public List<int[]> getCost();

    }
}
