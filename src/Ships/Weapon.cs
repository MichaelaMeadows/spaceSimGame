using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Ships
{
    interface Weapon
    {
        // Add damage type?
        // Splash?
        // Physics model, or hitscan?
        Projectile fireWeapon(Tuple<int, int> currentLocation, Tuple<int, int> target);
    }
}
