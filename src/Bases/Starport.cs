using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.src.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Bases
{
    class Starport : Facility
    {
        // turn into a list to avoid constant construction in the getter
        public Command command;
        // TODO Expand starports for concurrent builds!!!
        public void addCommand(Command c)
        {
            this.command = c;
        }
        public bool isEligible(Command c)
        {
            if (c.GetType() == typeof(BuildVehicle))
            {
                return true;
            }
            return false;
        }
        public bool canTakeCommand(Command c)
        {
            if (this.command == null || this.command.getState() != CommandState.PROGRESS)
            {
                return isEligible(c);
            }
            return false;
        }

        public void execute(WorldState ws)
        {
            if (command == null)
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
            return 20;
        }

        public int getWorkPower()
        {
            return 2;
        }
    }
}
