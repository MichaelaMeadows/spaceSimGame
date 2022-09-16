using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Commands
{
    interface Command
    {
        public void execute(WorldState ws);
        public CommandState getState();

    }
}
