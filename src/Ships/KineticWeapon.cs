using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Ships
{
    class KineticWeapon : Weapon
    {
        int damage { get; set; }
        int speed { get; set; }
        int range { get; set; }

        public KineticWeapon(int damage, int speed, int range)
        {
            this.damage = damage;
            this.speed = speed;
            this.range = range;
        }

        public Projectile fireWeapon(Tuple<int, int> currentLocation, Tuple<int, int> target)
        {
            return new Projectile(currentLocation, target, speed, damage, range);  
        }
    }
}
