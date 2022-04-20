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
        public Vehicle v;
        public Node n;
        public Station s;
        public CommandState state;

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
                if (this.v.current_capacity == 0)
                {
                    state = CommandState.PROGRESS;
                } else
                {
                    return;
                }
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
                    for (int x = 0; x < s.goods.Length; x++)
                    {
                        s.goods[x] += v.goodsManifest[x];
                        v.goodsManifest[x] = 0;
                    }
                    v.current_capacity = 0;
                    state = CommandState.SUCCESS;
                }
                
                // TODO replace 3 with a "mining distance"
            } else if (Math.Sqrt((Math.Pow(v.location.Item1 - n.location.Item1, 2) + Math.Pow(v.location.Item2 - n.location.Item2, 2))) > 3) // Go to the node
            {
                var position = Distances.findNextPosition(ws, v.location, n.location, v.speed);
                if (position == null)
                {
                    return;
                }
                ws.placeObject(v, position.Item1, position.Item2);
                
            } else // wait and mine
            {
                for (int i = 0; i < v.miningpower; i++)
                {
                    if ((v.capacity - v.current_capacity) >= n.unit_size) {
                        int minedUnits = n.mine();
                        if (n.type == 1)
                        {
                            Debug.WriteLine("Copper exists");
                        }
                        v.goodsManifest[n.type] += minedUnits;
                        v.current_capacity += minedUnits * n.unit_size;
                    }
                }
            }
        }

        public CommandState getState()
        {
            return state;
        }
    }
}
