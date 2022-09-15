using SpaceSimulation.Bases;
using SpaceSimulation.Ships;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceSimulation.Empires
{
    class Empire
    {
        public List<Station> stations;
        public List<Ship> ships;
        public int funds;
        public int playerId;
        private int[] researchPoints;
        private Random r = new Random();
        public Empire()
        {
            funds = 100;
            stations = new List<Station>();
            ships = new List<Ship>();
            // 10 tiers of research points?
            researchPoints = new int[10];
        }
        // Each empire independantly assigns tasks to resources it controls. After task assignment, the game executes each step.
        public void executeStrategy(WorldState ws, int tickCount)
        {
            // Create strategy
            // Order stations
            // Move vehicles
            // Move ships
            if (tickCount % 60 == 0)
            {
/*               // Debug.WriteLine("Station Dump");
                foreach (Station s in this.stations)
                {
                   // Debug.WriteLine("Goods");
                    foreach (int x in s.goods)
                    {
                        
                    }
                    //Debug.WriteLine("Goods " + s.pendingCommands.Count);
                }*/


                foreach (Station s in this.stations)
                {
                    s.saturateMines(ws);
                    // These are tests to build specific things?
                    s.build(ws, 5);
                    s.build(ws, 6);
                }
            }

            if (tickCount % 3 == 0)
            {
                foreach (Station s in this.stations)
                {
                    s.moveVehicles(ws);
                    s.build(ws, 3);
                    s.build(ws, 4);
                    s.runFacilities(ws);
                }
            }
        }
    }
}
