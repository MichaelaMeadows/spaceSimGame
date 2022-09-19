using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using SpaceSimulation.src.Ships;
using SpaceSimulation.src.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SpaceSimulation.Vehicles
{
    class Vehicle : Entity
    {
        public String name { get; set; }
        public int id { get; set; }
        public int empire { get; set; }
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
        public int shield_recharge { get; set; }
        public int miningpower { get; set; }
        public Tuple<int, int> location { get; set; }
        public Command command { get; set; }
        public int[] goodsManifest;
        public Weapon[] weapons { get; set; }

        public Vehicle(Tuple<int, int> location, int empire)
        {
            this.location = location;
            goodsManifest = new int[Global.gameGoods.Length];
            this.empire = empire;
        }

        public Tuple<int, int> getLocation()
        {
            return location;
        }

        public virtual int getSize()
        {
            return 10;
        }

        public virtual string getSprite()
        {
            return name;
        }

        public virtual VehicleType getVehicleType()
        {
            throw new Exception();
        }

        public void setLocation(Tuple<int, int> location)
        {
            if (location == null)
            {
                throw new Exception();
            }
            this.location = location;
        }

        public virtual List<int[]> getCost()
        {
            // All ships must override
            return null;
        }
    }
}
