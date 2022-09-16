using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using SpaceSimulation.src.Empires;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src
{
    interface Strategy
    {
        public int getScore();
        public List<EmpireCommand> executeStrategy();
        //public List<EmpireCommand> getVehicleCommands();
        //public List<EmpireCommand> getShipCommands();
    }
}
