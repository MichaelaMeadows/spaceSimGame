using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using SpaceSimulation.Helpers;
using SpaceSimulation.src.Ships;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Commands
{
    class SearchAndDestroy : Command
    {
        public Vehicle v;
        public CommandState state;
        private Tuple<int, int> targetPos;

        public SearchAndDestroy(Vehicle v)
        {
            this.v = v;
            this.state = CommandState.CREATED;
        }

        // Move around, shoot things in range I guess?
        public void execute(WorldState ws)
        {
            // This thould throw an exception maybe? Need to make sure they get cleaned up.
            if (v.health <= 0)
            {
                ws.removeObject(v);
                return;
            }
            if (this.targetPos == null)
            {
                this.targetPos = ws.getRandomLocation();
            }

            var position = Distances.findNextPosition(ws, v.location, targetPos, v.speed);
            if (position == null)
            {
                return;
            }
            if (Distances.distance(position, targetPos) < 2)
            {
                this.targetPos = ws.getRandomLocation();
            }
            ws.placeObject(v, position.Item1, position.Item2);

            // Engage Targets
            List<Vehicle> closeEntities = new List<Vehicle>();
            foreach(Entity e in ws.GetObjectsInView(v.location.Item1, v.location.Item2))
            {
                if (e is Frigate && !(e.getLocation().Equals(v.location)))
                {
                    closeEntities.Add((Vehicle)e);
                }
            }
            
            engageTargets(closeEntities);
        }
        private void engageTargets(List<Vehicle> closeEntities)
        {
            foreach (Weapon w in v.weapons)
            {
                // Fire weapons at valid targets. Reload time etc should be calculated in the weapon itself I think.
            }

            foreach(Vehicle ce in closeEntities)
            {
                // Instant boom
                ce.health = 0;
            }
        }

        public CommandState getState()
        {
            return state;
        }
    }
}
