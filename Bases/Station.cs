using SpaceSimulation.components;
using SpaceSimulation.Components;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    class Station : Entity
    {
        public String name { get; set; }
        public int id { get; set; }
        public int[] goods { get; set; }
        public List<Facility> facilities { get; set; }
        public int health { get; set; }
        public Tuple<int, int> location;
        public List<Vehicle> vehicles;

        public Station(Tuple<int, int> position)
        {
            location = position;
            vehicles = new List<Vehicle>();
        }

        public Tuple<int, int> getLocation()
        {
            return location;
        }
        public int getSize()
        {
            return 8;
        }

        public string getSprite()
        {
            throw new NotImplementedException();
        }

        public void setLocation(Tuple<int, int> location)
        {
            this.location = location;
        }
    }
}
