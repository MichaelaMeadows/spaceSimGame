using SpaceSimulation.Bases;
using SpaceSimulation.Ships;
using SpaceSimulation.src.Empires;
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

        private EconomicStrategy e_strategy = new EconomicStrategy();
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
            // TODO saturation should probably live in the strategy level.
            if (tickCount % 60 == 0)
            {
                foreach (Station s in this.stations)
                {
                    s.saturateMines(ws);
                }
            }

            // Move vehicles
            // Move ships
            if (tickCount % 3 == 0)
            {
                e_strategy.getStationCommands(ws, this);
                foreach (Station s in this.stations)
                {
                    s.moveVehicles(ws);
                    s.runFacilities(ws);
                }
            }
        }
    }
}
