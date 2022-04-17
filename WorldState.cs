using SpaceSimulation.Bases;
using SpaceSimulation.Components;
using SpaceSimulation.Empires;
using SpaceSimulation.Nodes;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SpaceSimulation
{
    class WorldState
    {
        public Entity[,] map { get; set; }
        public Node[] nodes {get;}

        public int mapViewSize { get; set; }
        

        public static int MAP_SIZE = 2000;
        public WorldState()
        {
            var nodeList = new List<Node>();

            map = new Entity[MAP_SIZE, MAP_SIZE];
            var rand = new Random();
            for (int i = 0; i< 1000; i++) {
                for(int j = 0; j < 1000; j++) {
                    Node iron = new Node();
                    iron.type = "iron";
                    iron.id = 1;
                    iron.outputVolume = 2;
                    iron.location = new Tuple<int, int>(i, j);

                    if (rand.NextDouble() > .995) {
                        map[i, j] = iron;
                        nodeList.Add(iron);
                        //Debug.WriteLine("added iron");
                    }
                }
            }
            //BasicMiner miner = new BasicMiner();
            //miner.
            nodes = new Node[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodes[i] = nodeList[i];
            }
            nodeList = null;
            mapViewSize = 80;
    }

        public void placeObject(Entity entity, int x, int y)
        {
            // TODO check for collision???
            entity.setLocation(new Tuple<int, int>(x, y));
            map[x, y] = entity;
        }
        public List<Entity> GetObjectsInView(int x, int y)
        {
            List<Entity> found = new List<Entity>();
            int half = (mapViewSize / 2);
            var left = Math.Max(0, x - half);
            var right = Math.Min(MAP_SIZE, x + half);
            var top = Math.Max(0, y - half);
            var bot = Math.Min(MAP_SIZE, y + half);

            //Debug.WriteLine("" + left + "," + right + "," + top + "," + bot);

            for (int i = left; i < right; i++)
            {
                for (int j = top; j < bot; j++)
                {
                    if (map[i, j] != null) {
                        found.Add(map[i, j]);
                    }
                }
            }
            return found;
        }

        // Move things as they have been instructed.
        public void moveAssets()
        {

        }

        public void updateNodes()
        {
            foreach (Node n in nodes) {
                n.refresh();
            }
        }

        public Tuple<int, int> findEmptyNeighbor(int x, int y)
        {
            var left = Math.Max(0, x - 1);
            var right = Math.Min(MAP_SIZE, x + 1);
            var top = Math.Max(0, y - 1);
            var bot = Math.Min(MAP_SIZE, y + 1);
            for (int i = left; i < right; i++)
            {
                for (int j = top; j < bot; j++)
                {
                    if (map[i, j] == null)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }

            return null;
        }
    }

}
