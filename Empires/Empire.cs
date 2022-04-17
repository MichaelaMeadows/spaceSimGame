using SpaceSimulation.Bases;
using SpaceSimulation.Ships;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Empires
{
    class Empire
    {
        public List<Station> stations;
        public List<Ship> ships;
        public int funds;
        public int playerId;
        public Empire()
        {
            funds = 100;
            stations = new List<Station>();
            ships = new List<Ship>();
        }
        // Each empire independantly assigns tasks to resources it controls. After task assignment, the game executes each step.
        public void executeStrategy()
        {

        }

    }
}
