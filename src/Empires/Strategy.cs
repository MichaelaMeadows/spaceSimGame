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
        public List<EmpireCommand> getStationCommands(WorldState ws, Empire e);
        public List<EmpireCommand> getVehicleCommands(WorldState ws, Empire e);
        public List<EmpireCommand> getShipCommands(WorldState ws, Empire e);
    }
}
