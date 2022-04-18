using SpaceSimulation.Commands;
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
        private Random r = new Random();

        public Station(Tuple<int, int> position)
        {
            location = position;
            vehicles = new List<Vehicle>();
            this.goods = new int[100];
            this.goods[0] += 300;
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

        // TODO assign what kind with weight?
        public void buildVehicle(WorldState ws)
        {
            if (this.goods[0] >= 100)
            {
                // TODO clearly make objects with settings. Extract to json someday
                Tuple<int, int> location = ws.findEmptyNeighbor(this.location.Item1, this.location.Item2);
                if (location == null)
                {

                    return;
                }
                else
                {
                    Vehicle v1 = new Vehicle(location);
                    v1.speed = 3;
                    v1.capacity = 10;
                    v1.current_capacity = 0;
                    this.vehicles.Add(v1);
                    this.goods[0] -= 100;
                    ws.placeObject(v1, location.Item1, location.Item2);
                }
            }
        }

        // TODO assign resource with weight?
        public void mine(WorldState ws)
        {
            foreach (Vehicle v in this.vehicles)
            {
                if (v.command == null || v.command.getState().Equals(CommandState.SUCCESS))
                {
                    var n1 = ws.nodes[r.Next(0, ws.nodes.Length)];
                    Command c = new Mine(v, n1, this);
                    v.command = c;
                    v.current_capacity = 0;
                }
                else
                {
                    v.command.execute(ws);
                }
            }
        }

        public void deliverGoods(int type, int quantity)
        {
            this.goods[type] += quantity;
        }
    }
}
