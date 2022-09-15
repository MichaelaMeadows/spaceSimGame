using SpaceSimulation.Bases;
using SpaceSimulation.components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Commands
{
    class Build : Command
    {
        public Station s;
        public int target;
        public Facility facility;
        public CommandState state;
        public int progress;
        public int workPower;
        public int neededWork;

        public Build(Station s, int target, Facility facility)
        {
            this.s = s;
            this.target = target;
            this.facility = facility;
            progress = 0;
            // JAnky decision to allow null facility. Should be refactored
            if (facility != null) { workPower = facility.getWorkPower(); }
            this.neededWork = 0;
        }
        public void execute(WorldState ws)
        {
            if (this.target == 6)
            {
                this.target = this.target;
            }
            if (state.Equals(CommandState.SUCCESS))
            {
                return;
            }
            if (this.neededWork == 0)
            {
                state = CommandState.PROGRESS;
                neededWork = ws.marketplace.goods[target].buildEffort;
            }
            progress += workPower;
            if (progress >= neededWork)
            {
                state = CommandState.SUCCESS;
                s.goods[target] += 1;
            }

        }
        public CommandState getState()
        {
            return state;
        }
    }
}
