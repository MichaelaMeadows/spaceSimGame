using SpaceSimulation.Bases;
using SpaceSimulation.Ships;
using System;
using System.Collections.Generic;

namespace SpaceSimulation.Empires
{
    class Empire
    {
        public List<Station> stations;
        public List<Ship> ships;
        public int funds;
        public int playerId;
        private Random r = new Random();
        public Empire()
        {
            funds = 100;
            stations = new List<Station>();
            ships = new List<Ship>();
        }
        // Each empire independantly assigns tasks to resources it controls. After task assignment, the game executes each step.
        public void executeStrategy(WorldState ws, int tickCount)
        {
            // Create strategy
            // Order stations
            // Move vehicles
            // Move ships
            if (tickCount % 15 == 0)
            {
                foreach (Station s in this.stations)
                {
                    s.buildVehicle(ws);
                    // TODO don't only mine iron
                    s.mine(ws, 0);
                }
            }
            if (tickCount % 3 == 0)
            {
                foreach (Station s in this.stations)
                {
                    s.moveVehicles(ws);
                }
            }
        }
    }
}
