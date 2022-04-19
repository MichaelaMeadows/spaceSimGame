using SpaceSimulation.Bases;
using SpaceSimulation.components;
using SpaceSimulation.Components;
using SpaceSimulation.Empires;
using SpaceSimulation.Helpers;
using SpaceSimulation.Nodes;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SpaceSimulation
{
    class WorldState
    {
        // Map is used to quickly identify neighbors instead of searching all entities
        public List<Entity>[,] map { get; set; }
        public Node[] nodes {get;}

        public int mapViewSize { get; set; }
        public static int BOX_SIZE = 400;
        // I want map size to be a clean multiple of box size
        public static int MAP_SIZE = BOX_SIZE * 100;

        // All goods are pre-determined, and exist in a fixed-sized array for performance.
        public Marketplace marketplace;
        public static int RESOURCE_COUNT = 4;

        public WorldState()
        {
            loadMarketplace();
            map = new List<Entity>[MAP_SIZE / BOX_SIZE, MAP_SIZE / BOX_SIZE];
            var rand = new Random();
            for (int i = 0; i < MAP_SIZE / BOX_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE / BOX_SIZE; j++)
                {
                    map[i, j] = new List<Entity>();
                }

            }
            var nodeList = new List<Node>();


            List<NodeSetting> settings;
            using (StreamReader r = new StreamReader("Content/nodeSettings.json"))
            {
                string json = r.ReadToEnd();
                settings = JsonSerializer.Deserialize<List<NodeSetting>>(json);
            }
            foreach (Goods g in marketplace.goods)
            {
                // It's a type of ore
                if (g.cost.Count == 0)
                {
                    for (int i = 0; i < settings[g.id].quantity; i++)
                    {
                        Node n1 = new Node();
                        n1.type = g.id;
                        n1.id = g.id;
                        n1.outputVolume = (settings[g.id].averageVolume + rand.Next(-1 * settings[g.id].variance, settings[g.id].variance));
                        n1.location = new Tuple<int, int>(rand.Next(1, MAP_SIZE), rand.Next(1, MAP_SIZE));
                        map[n1.location.Item1 / BOX_SIZE, n1.location.Item2 / BOX_SIZE].Add(n1);
                        nodeList.Add(n1);
                    }
                }
            }

            nodes = new Node[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodes[i] = nodeList[i];
            }
            nodeList = null;
            mapViewSize = 140;
    }

        private void loadMarketplace()
        {
            marketplace = new Marketplace();
        }

        public void placeObject(Entity entity, int x, int y)
        {
            // TODO check for collision???
            map[entity.getLocation().Item1 / BOX_SIZE, entity.getLocation().Item2 / BOX_SIZE].Remove(entity);
            entity.setLocation(new Tuple<int, int>(x, y));
            map[x / BOX_SIZE, y / BOX_SIZE].Add(entity);
        }
        public List<Entity> GetObjectsInView(int x, int y)
        {
            List<Entity> found = new List<Entity>();
            int half = (mapViewSize / 2);
            var left = Math.Max(0, x - half);
            var right = Math.Min(MAP_SIZE, x + half);
            var top = Math.Max(0, y - half);
            var bot = Math.Min(MAP_SIZE, y + half);

            var imax = Math.Min(MAP_SIZE / BOX_SIZE - 1, (right / BOX_SIZE) + 1);
            var jmax = Math.Min(MAP_SIZE / BOX_SIZE - 1, (bot / BOX_SIZE) + 1);

            for (int i = left / BOX_SIZE; i < imax; i++)
            {
                for (int j = top / BOX_SIZE; j < jmax; j++)
                {
                    if (map[i, j] != null) {
                        found.AddRange(map[i, j]);
                    }
                }
            }
            return found;
        }

        public void updateNodes()
        {
            foreach (Node n in nodes) {
                n.refresh();
            }
        }

        // TODO Convert to use new map boxed system
        public Tuple<int, int> findEmptyNeighbor(int x, int y)
        {
            var left = Math.Max(0, x - 1);
            var right = Math.Min(MAP_SIZE, x + 1);
            var top = Math.Max(0, y - 1);
            var bot = Math.Min(MAP_SIZE, y + 1);
            for (int i = left / BOX_SIZE; i < right / BOX_SIZE; i++)
            {
                for (int j = top / BOX_SIZE; j < bot / BOX_SIZE; j++)
                {
                    if (map[i, j].Count == 0)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }

            return null;
        }
        public int getMapSize()
        {
            return MAP_SIZE;
        }
    }

}
