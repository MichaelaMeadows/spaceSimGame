using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using SpaceSimulation.src.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    class ResearchStrategy : Strategy
    {
        // Transport goods to fund research?
        // Build new bases?
        public List<Command> getShipCommands(WorldState ws, Empire e)
        {
            throw new NotImplementedException();
        }

        public List<Command> getStationCommands(WorldState ws, Empire e)
        {
            foreach(Station s in e.stations)
            {
                bool canResearch = false;
                foreach(Facility f in s.facilities)
                {
                    // Hard coding is getting hard!
 /*                   Spending.canAfford(s.goods, Global.gameGoods[])
                    if (f.isEligible())
                    {

                    }*/
                }
            }
            List<Command> commands = new List<Command>();
            return commands;
        }

        public List<Command> getVehicleCommands(WorldState ws, Empire e)
        {
            throw new NotImplementedException();
        }
    }
}
