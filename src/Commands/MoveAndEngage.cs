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
    class MoveAndEngage : Command
    {
        public Vehicle v;
        public CommandState state;
        private Tuple<int, int> targetPos;

        public MoveAndEngage(Vehicle v, Tuple<int, int> destination)
        {
            this.v = v;
            this.state = CommandState.CREATED;
            this.targetPos = destination;
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

            var position = Distances.findNextPosition(ws, v.location, targetPos, v.speed);
            if (position == null)
            {
                return;
            }
            if (Distances.distance(position, targetPos) < 2)
            {
                this.state = CommandState.PROGRESS;
            }
            ws.placeObject(v, position.Item1, position.Item2);

            // Engage Targets
            List<Vehicle> closeEntities = new List<Vehicle>();
            foreach (Entity e in ws.GetObjectsInView(v.location.Item1, v.location.Item2))
            {
                if (e is Frigate && !(e.getLocation().Equals(v.location)))
                {
                    closeEntities.Add((Vehicle)e);
                }
            }

            engageTargets(closeEntities, v.empire, ws);
        }
        private void engageTargets(List<Vehicle> closeEntities, int empire, WorldState ws)
        {
            KineticWeapon weapon = new KineticWeapon(10, 16, 600);
            // foreach (Weapon w in v.weapons)
            // {
            // Fire weapons at valid targets. Reload time etc should be calculated in the weapon itself I think.
            // TODO the weapon need to be built and loaded

            //}

            foreach (Vehicle ce in closeEntities)
            {
                if (ce.empire != empire)
                {
                    // Instant boom
                    //ce.health = 0;
                    // TODO Really need to add reload time.
                    ws.projectiles.Add(weapon.fireWeapon(v.location, ce.location));
                }
            }
        }

        public CommandState getState()
        {
            return state;
        }
    }
}
