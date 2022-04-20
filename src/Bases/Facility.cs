using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    interface Facility
    {
        public List<Command> executingCommands();
        public int getSize();
        public bool canTakeCommand(Command c);
        public bool isEligible(Command c);
        public void addCommand(Command c);
        public void execute(WorldState state);
        public int getWorkPower();
    }
}
