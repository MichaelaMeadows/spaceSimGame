using SpaceSimulation.Commands;
using SpaceSimulation.components;
using SpaceSimulation.Components;
using SpaceSimulation.Helpers;
using SpaceSimulation.Nodes;
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
        public int facilitySpace;
        public List<Facility> facilities { get; set; }
        public int health { get; set; }
        public Tuple<int, int> location;
        public List<Vehicle> vehicles;
        private Random r = new Random();

        private List<Node>[] closeNodes;
        public int CLOSE_ORE_COUNT = 10;
        
        public Station(Tuple<int, int> position, WorldState state)
        {
            location = position;
            vehicles = new List<Vehicle>();
            this.goods = new int[state.marketplace.goods.Length];
            this.goods[0] += 30000;

            closeNodes = new List<Node>[WorldState.RESOURCE_COUNT];
            populateCloseNodes(state);
            facilitySpace = 100;
        }

        private void populateCloseNodes(WorldState state)
        {
            SortedList<double, Node>[] tmp_nodes = new SortedList<double, Node>[WorldState.RESOURCE_COUNT];
            for (int i =0; i< WorldState.RESOURCE_COUNT; i++)
            {
                SortedList<double, Node> node_list = new SortedList<double, Node>();
                tmp_nodes[i] = node_list;
            }

            foreach (Node n in state.nodes)
            {
                int type = n.type;
                Double distance = Distances.distance(this.location, n.location);
                if (distance < 1000)
                {
                    if(tmp_nodes[type].Count < CLOSE_ORE_COUNT)
                    {
                        tmp_nodes[type].Add(distance, n);
                    } else
                    {
                        if (distance < tmp_nodes[type].Keys[CLOSE_ORE_COUNT - 1])
                        {
                            tmp_nodes[type].Remove(tmp_nodes[type].Keys[CLOSE_ORE_COUNT - 1]);
                            try
                            {
                                tmp_nodes[type].Add(distance, n);
                            } catch (Exception e)
                            {
                                // TODO. More elegant soluton could be good!
                                tmp_nodes[type].Add(distance + .000000001, n);
                            }
                        }
                        
                    }
                }
            }
            for (int i = 0; i < tmp_nodes.Length; i++)
            {
                closeNodes[i] = new List<Node>();
                SortedList<double, Node> items = tmp_nodes[i];
                foreach (var x in items) {
                    closeNodes[i].Add(x.Value);
                }
            }
        }
        public Tuple<int, int> getLocation()
        {
            return location;
        }
        public int getSize()
        {
            return 16;
        }

        public string getSprite()
        {
            return "spacestation";
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
                Tuple<int, int> location = new Tuple<int, int>(this.location.Item1 + r.Next(-1, 1), this.location.Item2 + r.Next(-1, 1));
/*                if (location == null)
                {
                    return;
                }*/
                
                Vehicle v1 = new Vehicle(location);
                v1.speed = 3;
                v1.capacity = 10;
                v1.current_capacity = 0;
                v1.name = "smallCar";
                this.vehicles.Add(v1);
                this.goods[0] -= 100;
                ws.placeObject(v1, location.Item1, location.Item2); 
            }
        }

        // TODO assign resource with weight?
        public void mine(WorldState ws)
        {
            foreach (Vehicle v in this.vehicles)
            {
                if (v.command == null || v.command.getState().Equals(CommandState.SUCCESS))
                {
                    var n1 = closeNodes[0][r.Next(0, closeNodes[0].Count)];
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
