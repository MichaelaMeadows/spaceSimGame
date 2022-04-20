using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src
{
    interface Strategy
    {
        public List<Command> getStationCommands(WorldState ws, Empire e);
        public List<Command> getVehicleCommands(WorldState ws, Empire e);
        public List<Command> getShipCommands(WorldState ws, Empire e);
    }
}
