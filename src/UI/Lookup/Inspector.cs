using SpaceSimulation.Bases;
using SpaceSimulation.Components;
using SpaceSimulation.Nodes;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spaceSimGame.src.UI.Lookup
{
    static class Inspector
    {
        //TODO: reflection? factory? Better yet, observer pattern? That way if any values change I can update this in real time via event.
        public static List<String> inspect(Entity entity)
        {
            List<String> inspectedAtts = new List<String>();
            if (entity != null && entity is Vehicle)
            {
                Vehicle vehicle = entity as Vehicle;
                inspectedAtts.Add("Name: " + vehicle.name);
                inspectedAtts.Add("id: " + vehicle.id.ToString());
                inspectedAtts.Add("empireId: " + vehicle.empire.ToString());
                inspectedAtts.Add("currentCapacity: " + vehicle.current_capacity.ToString());
                inspectedAtts.Add("speed: " + vehicle.speed.ToString());
                inspectedAtts.Add("shield: " + vehicle.shield.ToString());
            }
            else if (entity != null && entity is Station)
            {
                Station station = entity as Station;
                inspectedAtts.Add("Name: " + station.name);
                inspectedAtts.Add("i: " + station.id.ToString());
                inspectedAtts.Add("health: " + station.health.ToString());
                inspectedAtts.Add("vehicles: " + station.vehicles.ToString());
                inspectedAtts.Add("combatShips: " + station.health.ToString());
            }
            else if (entity != null && entity is Node)
            {
                Node node = entity as Node;
                inspectedAtts.Add("Type: " + node.type);
                inspectedAtts.Add("id: " + node.id);
                inspectedAtts.Add("unit size: " + node.unit_size);
                inspectedAtts.Add("output volume: " + node.outputVolume);
            }
            return inspectedAtts;
        }
    }
}
