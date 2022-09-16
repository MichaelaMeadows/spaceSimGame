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
    class MilitaryStrategy : Strategy
    {
        WorldState ws;
        Empire empire;

        public MilitaryStrategy(WorldState ws, Empire e)
        {
            this.ws = ws;
            this.empire = e;
        }

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
            return null;
        }
        public List<EmpireCommand> getVehicleCommands()
        {
            throw new NotImplementedException();
        }
    }
}
