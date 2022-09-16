using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using SpaceSimulation.src.Helpers;
using SpaceSimulation.src.Ships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    class EconomicStrategy : Strategy
    {
        WorldState ws;
        Empire empire;
        public EconomicStrategy(WorldState ws, Empire e)
        {
            this.ws = ws;
            this.empire = e;
        }
        /*
         * Economic strategy considers requests from research and military. 
         * It also reviews current resource and tech level to get a sense of waht to go after.
         */
        // Stateful capture of goals from other layers? Perhaps a heap of values items?

        public int getScore()
        {
            throw new NotImplementedException();
        }

        public List<EmpireCommand> getShipCommands()
        {
            throw new NotImplementedException();
        }


        public List<EmpireCommand> executeStrategy()
        {
            // TODO Don't do this per request in the future;
            this.ws = ws;
            // Stations make resources in priority from simplest to most advanced.
            // When there are multiple stations in later game, they learn to assign specializations and then transport. (Big TODO)
            List<EmpireCommand> commands = new List<EmpireCommand>();
            foreach (Station s in empire.stations)
            {
                // Base resource storage
                // TODO - It assumes that this can over shoot with work queing in the facility/station list of things to do.

                //Iron
                buildToThresholdIfPossible(s, 3, 100);
                // Copper
                buildToThresholdIfPossible(s, 4, 100);
                // Circuits
                buildToThresholdIfPossible(s, 5, 50);
                // Hydro cells
                buildToThresholdIfPossible(s, 6, 100);

                // Building complex products
                // First identify the station that will do it, then slowly build towards it until done. Do not thrash.
                Frigate f1 = new Frigate(s.getLocation());
                // If we can't build, make progress towards it.
                // There's probably some checks to make sure it's even possible or something.
                if (s.goods[3] > 75 && !s.buildVehicle(ws, f1))
                {
                    buildRecursiveRequirements(s, f1.getCost());
                }

            }
            return commands;
        }

        // Tracks the build items, sorta like stack size
        private int buildRecursiveRequirements(Station s, List<int[]> costs)
        {
            // It's a base resource
            if (costs.Count < 1)
            {
                return 0;
            }

            int built = 0;
            foreach (int[] cost in costs)
            {
                // We need more of the thing
                if (!(s.goods[cost[0]] > cost[1]))
                {
                    // Build the thing we need if we can.
                    if (Spending.canAfford(s.goods, this.ws.marketplace.goods[cost[0]].cost)) {
                        s.build(ws, cost[0]);
                        built += 1;
                    } else {
                        // Build the dependency
                        built += buildRecursiveRequirements(s, this.ws.marketplace.goods[cost[0]].cost);
                    }
                }
            }
            return built;
        }

        public void buildToThresholdIfPossible(Station s, int item, int cap)
        {
            if (Spending.canAfford(s.goods, ws.marketplace.goods[item].cost) && s.goods[item] < cap)
            {
                s.build(ws, item);
            }

        }

        public List<EmpireCommand> getVehicleCommands()
        {
            throw new NotImplementedException();
        }
    }
}
