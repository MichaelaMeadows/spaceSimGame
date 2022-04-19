using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    interface Facility
    {
        public int getSize();
        public void setCommand(Command c);
        public void execute(WorldState state);
        public int getWorkPower();
    }
}
