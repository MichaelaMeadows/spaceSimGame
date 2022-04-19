using SpaceSimulation.Bases;
using SpaceSimulation.components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Commands
{
    class Build : Command
    {
        private Station s;
        private int target;
        private Facility facility;
        private CommandState state;
        int progress;
        int workPower;
        int neededWork;

        public Build(Station s, int target, Facility facility)
        {
            this.s = s;
            this.target = target;
            this.facility = facility;
            progress = 0;
            workPower = facility.getWorkPower();
        }
        public void execute(WorldState ws)
        {
            if (state.Equals(CommandState.SUCCESS))
            {
                return;
            }

            if (state.Equals(CommandState.CREATED))
            {
                int effort = ws.marketplace.goods[target].buildEffort;
                List<int[]> costs = ws.marketplace.goods[target].cost;
                Boolean canBuild = true;
                foreach (var cost in costs)
                {
                    if (!(s.goods[cost[0]] >= cost[1]))
                    {
                        canBuild = false;
                    }
                }
                if (canBuild)
                {
                    foreach (var cost in costs)
                    {
                        s.goods[cost[0]] -= cost[1];
                    }
                    // Resources have been deducted
                    state = CommandState.PROGRESS;
                    neededWork = ws.marketplace.goods[target].buildEffort;
                }
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
