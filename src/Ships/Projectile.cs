using SpaceSimulation.Components;
using SpaceSimulation.Helpers;
using SpaceSimulation.Ships;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Ships
{
    class Projectile
    {
        public Tuple<int, int> currentLocation { get; set; }
        public Tuple<int, int> targetLocation { get; set; }
        public int speed { get; set; }
        public int damage { get; set; }
        public int range { get; set; }

        // Constructor for a projectile.
        public Projectile(Tuple<int, int> currentLocation, Tuple<int, int> targetLocation, int speed, int damage, int range)
        {
            this.speed = speed;
            this.currentLocation = currentLocation;
            this.targetLocation = targetLocation;
            this.damage = damage;
            this.range = range;
        }

        // Moves projectile, checks for collision, does damage, removes itself if out of range or hits something.
        // Returns false if the projectile can be deleted.
        public bool update(WorldState ws)
        {
            // Move
            Tuple<int, int> nextLocation = Distances.findNextPosition(ws, currentLocation, targetLocation, speed);
            this.currentLocation = nextLocation;
            range -= speed;

            // Check collision
            Entity e = ws.getEntityAtLocation(nextLocation.Item1, nextLocation.Item2);
            if (e != null)
            {
                // See if e is a Ship
                if(e is Ship)
                {
                    Ship ship = (Ship)e;
                    ship.health -= damage;
                    return false;
                }
            }

            if (range <= 0)
            {
                return false;
            } else {
                return true;
            }
        }

    }
}
