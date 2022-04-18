using SpaceSimulation.Bases;
using SpaceSimulation.Components;
using SpaceSimulation.Helpers;
using SpaceSimulation.Nodes;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SpaceSimulation.Commands
{
    class Mine : Command
    {
        private Vehicle v;
        private Node n;
        private Station s;
        private CommandState state;
        private int stuckCount;

        public Mine(Vehicle v, Node n, Station s)
        {
            this.v = v;
            this.n = n;
            this.s = s;
            state = CommandState.PROGRESS;
        }

        public void execute(WorldState ws)
        {

            //Debug.WriteLine("Executing");
            if (state.Equals(CommandState.SUCCESS))
            {
                return;
            }
            // Full, so return. Wait if needed.
            if (v.current_capacity >= v.capacity - n.unit_size)
            {
                var position = Distances.findNextPosition(ws, v.location, s.location, v.speed);
                if (position == null)
                {
                    return;
                }
                ws.placeObject(v, position.Item1, position.Item2);

                if (Math.Sqrt((Math.Pow(v.location.Item1 - s.location.Item1, 2) + Math.Pow(v.location.Item2 - s.location.Item2, 2))) < 3)
                {
                    s.goods[0] += v.current_capacity;
                    v.current_capacity = 0;
                    state = CommandState.SUCCESS;
                }
                
                // TODO replace 3 with a "mining distance"
            } else if (Math.Sqrt((Math.Pow(v.location.Item1 - n.location.Item1, 2) + Math.Pow(v.location.Item2 - n.location.Item2, 2))) > 3) // Go to the node
            {
                var position = Distances.findNextPosition(ws, v.location, n.location, v.speed);
                if (position == null)
                {
                    stuckCount++;
                    //Debug.WriteLine("Next position was null");
                    return;
                }
                ws.placeObject(v, position.Item1, position.Item2);
                
            } else // wait and mine
            {
                // TODO make mining sane

                v.current_capacity += n.mine();
            }
        }

        public CommandState getState()
        {
            return state;
        }
    }
}
