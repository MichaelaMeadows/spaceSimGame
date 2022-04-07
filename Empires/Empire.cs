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
        private List<Station> stations;
        private List<Vehicle> vehicles;
        private List<Ship> ships;
        private int funds;
        public Empire()
        {
            funds = 0;
            vehicles = new List<Vehicle>();
            stations = new List<Station>();
            ships = new List<Ship>();
        }

    }
}
