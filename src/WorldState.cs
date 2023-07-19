using SpaceSimulation.Bases;
using SpaceSimulation.components;
using SpaceSimulation.Components;
using SpaceSimulation.Empires;
using SpaceSimulation.Helpers;
using SpaceSimulation.Nodes;
using SpaceSimulation.src.Ships;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace SpaceSimulation
{
    public class WorldState
    {
        // Map is used to quickly identify neighbors instead of searching all entities
        public HashSet<Entity>[,] map { get; set; }
        public Node[] nodes {get;}
        public List<Projectile> projectiles { get; set; }

        public int mapViewSize { get; set; }
        public static int BOX_SIZE = 100;
        // I want map size to be a clean multiple of box size
        public static int MAP_SIZE = BOX_SIZE * 500;

        // All goods are pre-determined, and exist in a fixed-sized array for performance.
        public Marketplace marketplace;
        public static int RESOURCE_COUNT = 4;
        Random r = new Random();

        internal Tuple<int, int> getRandomLocation()
        {
            return new Tuple<int, int>(r.Next(MAP_SIZE), r.Next(MAP_SIZE));
        }

        public WorldState()
        {
            projectiles = new List<Projectile>();
            loadMarketplace();
            map = new HashSet<Entity>[MAP_SIZE / BOX_SIZE, MAP_SIZE / BOX_SIZE];
            var rand = new Random();
            for (int i = 0; i < MAP_SIZE / BOX_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE / BOX_SIZE; j++)
                {
                    map[i, j] = new HashSet<Entity>();
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
                        n1.unit_size = g.size;
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
            Global.gameGoods = marketplace.goods;
        }

        public void placeObject(Entity entity, int x, int y)
        {
            // TODO check for collision???
            map[entity.getLocation().Item1 / BOX_SIZE, entity.getLocation().Item2 / BOX_SIZE].Remove(entity);
            entity.setLocation(new Tuple<int, int>(x, y));
            map[x / BOX_SIZE, y / BOX_SIZE].Add(entity);
        }
        public void removeObject(Entity entity)
        {
            // TODO check for collision???
            map[entity.getLocation().Item1 / BOX_SIZE, entity.getLocation().Item2 / BOX_SIZE].Remove(entity);
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

        public List<Projectile> GetProjectilesInView(int x, int y)
        {
            List<Entity> found = new List<Entity>();
            int half = (mapViewSize / 2);
            var left = Math.Max(0, x - half);
            var right = Math.Min(MAP_SIZE, x + half);
            var top = Math.Max(0, y - half);
            var bot = Math.Min(MAP_SIZE, y + half);

            var imax = Math.Min(MAP_SIZE / BOX_SIZE - 1, (right / BOX_SIZE) + 1);
            var jmax = Math.Min(MAP_SIZE / BOX_SIZE - 1, (bot / BOX_SIZE) + 1);

            List<Projectile> foundProjectiles = new List<Projectile>();

            foreach (Projectile p in projectiles)
            {
                int xpos = p.currentLocation.Item1;
                int ypos = p.currentLocation.Item2;

                if (xpos >= left && xpos <= right)
                {
                    if(ypos >= top && ypos <= bot)
                    {
                        foundProjectiles.Add(p);
                    }
                }
            }

            return foundProjectiles;
        }

        // Return the object whos location is closest to X, Y, with an error range of 3
        public Entity getEntityAtLocation(int x, int y)
        {
            var i = Math.Min(MAP_SIZE / BOX_SIZE - 1, (x / BOX_SIZE) );
            var j = Math.Min(MAP_SIZE / BOX_SIZE - 1, (y / BOX_SIZE) );
            // For entities in this box, find the closest one +- 3 units
            Entity closest = null;

            foreach (Entity e in map[i, j])
            {
                var dist = Math.Sqrt(Math.Pow(e.getLocation().Item1 - x, 2) + Math.Pow(e.getLocation().Item2 - y, 2));
                if (closest == null && dist < 8) {
                    closest = e;
                }
                else if (closest != null) {
                    var dist2 = Math.Sqrt(Math.Pow(closest.getLocation().Item1 - x, 2) + Math.Pow(closest.getLocation().Item2 - y, 2));

                    if (dist < dist2) {
                        closest = e;
                    }
                }
            }
            return closest;

        }

        public List<Entity> getEntitiesWithinDistance(int x, int y, int z)
        {
            var i = Math.Min(MAP_SIZE / BOX_SIZE - 1, (x / BOX_SIZE) + 1);
            var j = Math.Min(MAP_SIZE / BOX_SIZE - 1, (y / BOX_SIZE) + 1);

            List<Entity> found = new List<Entity>();

            foreach (Entity e in map[i, j])
            {
                var dist = Math.Sqrt(Math.Pow(e.getLocation().Item1 - x, 2) + Math.Pow(e.getLocation().Item2 - y, 2));

                if (dist < z)
                {
                    found.Add(e);
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

        public void updateProjectiles()
        {
            List<Projectile> newProjectiles = new List<Projectile>();
            foreach(Projectile p in projectiles)
            {
                bool result = p.update(this);
                if (result)
                {
                    newProjectiles.Add(p);
                }
            }
            this.projectiles = newProjectiles;
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
