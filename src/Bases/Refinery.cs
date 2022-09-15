using SpaceSimulation.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    class Refinery : Facility
    {
        // turn into a list to avoid constant construction in the getter
        public Command command;

        public void addCommand(Command c)
        {
            this.command = c;
        }

        public bool isEligible(Command c)
        {
            if (c.GetType() == typeof(Build))
            {
                // Check for all valid smelting targets
                if (((Build)c).target == 3 || ((Build)c).target == 4 || ((Build)c).target == 6)
                {
                    return true;
                }
            }
            return false;
        }
        public bool canTakeCommand(Command c)
        {
            if (this.command == null || (this.command.getState() != CommandState.PROGRESS && this.command.getState() != CommandState.CREATED)) {
                return isEligible(c);
            }
            return false;
        }

        public void execute(WorldState ws)
        {
            if(command == null)
            {
                return;
            }
            command.execute(ws);
        }

        public List<Command> executingCommands()
        {
            List<Command> c = new List<Command>();
            c.Add(this.command);
            return c;
        }

        public int getSize()
        {
            return 5;
        }

        public int getWorkPower()
        {
            return 2;
        }
    }
}
