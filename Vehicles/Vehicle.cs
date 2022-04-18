using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Vehicles
{
    class Vehicle : Entity
    {
        public String name { get; set; }
        public int id { get; set; }
        public List<Tuple<int, int>> cost { get; set; }
        public int buildEffort { get; set; }
        public int speed { get; set; }
        public int capacity { get; set; }
        public int current_capacity { get; set; }
        public int fuel { get; set; }
        public int fuelPerUnit { get; set; }
        public int health { get; set; }
        public int armor { get; set; }
        public int shield { get; set; }
        public Tuple<int, int> location { get; set; }
        public Command command { get; set;}

        public Vehicle(Tuple<int, int> location)
        {
            this.location = location;
        }

        public Tuple<int, int> getLocation()
        {
            return location;
        }

        public int getSize()
        {
            return 2;
        }

        public string getSprite()
        {
            throw new NotImplementedException();
        }

        public void setLocation(Tuple<int, int> location)
        {
            if (location == null)
            {
                throw new Exception();
            }
            this.location = location;
        }
    }
}
