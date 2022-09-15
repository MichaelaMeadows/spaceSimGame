using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    class EconomicStrategy : Strategy
    {
        public int getScore()
        {
            throw new NotImplementedException();
        }

        public List<EmpireCommand> getShipCommands(WorldState ws, Empire e)
        {
            throw new NotImplementedException();
        }

        public List<EmpireCommand> getStationCommands(WorldState ws, Empire e)
        {
            throw new NotImplementedException();
        }

        public List<EmpireCommand> getVehicleCommands(WorldState ws, Empire e)
        {
            throw new NotImplementedException();
        }
    }
}
