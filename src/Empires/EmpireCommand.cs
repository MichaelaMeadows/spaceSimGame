using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    interface EmpireCommand
    {
        // These are looped, so status doesn't need to be tracked?
        public void execute();
    }
}
