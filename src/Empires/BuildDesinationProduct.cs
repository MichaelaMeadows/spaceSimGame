using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Empires;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.src.Empires
{
    // Works towards an end product, building intermediates if needed.
    // Is a short hand to be used in strategies.
    class BuildDesinationProduct : EmpireCommand
    {
        // Identify candidate stations, or create a facility in a station if needed.
        // Of the candidates, pick the one closest to fulfilling.
        // Create / trade for ingrediants, and reserve them.
        // Build the stuff.
        public BuildDesinationProduct(WorldState ws, Empire e, int target)
        {
            List<int[]> targetCost = Global.gameGoods[target].cost;
            Build b = new Build(null, target, null);
            List<Station> validStations = new List<Station>();
            foreach (var s in e.stations)
            {
                if (hasMatchingFacility(s, b))
                {
                    validStations.Add(s);
                }
            }

            foreach (var s in validStations)
            {

            }
        }

        public bool hasMatchingFacility(Station s, Command b)
        {
            foreach (var f in s.facilities)
            {
                if (f.isEligible(b))
                {
                    return true;
                }
            }
            return false;

        }

        public void execute()
        {
            throw new NotImplementedException();
        }
    }
}
