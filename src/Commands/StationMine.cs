using SpaceSimulation.Bases;
using SpaceSimulation.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Commands
{
    class StationMine : Command
    {
        private Station s;
        private int resource;
        private CommandState state;
        
        // Given a target resource and a miner, dispatch the worker.
        public StationMine(Station s, int resource)
        {
            this.s = s;
            this.resource = resource;
        }
        public void execute(WorldState ws)
        {
            
        }

        public CommandState getState()
        {
            return state;
        }
    }
}
