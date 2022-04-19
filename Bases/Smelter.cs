using SpaceSimulation.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    class Smelter : Facility
    {
        public Command command;
        public void execute(WorldState ws)
        {
            if(command == null)
            {
                return;
            }
            command.execute(ws);
        }

        public int getSize()
        {
            return 5;
        }

        public int getWorkPower()
        {
            return 2;
        }

        public void setCommand(Command c)
        {
            this.command = c;
        }
    }
}
