using SpaceSimulation.Components;
using SpaceSimulation.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SpaceSimulation
{
    class WorldState
    {
        public Entity[,] map { get; set; }
        public int mapViewSize { get; set; }

        public static int MAP_SIZE = 1000;
        public WorldState()
        {
            map = new Entity[MAP_SIZE, MAP_SIZE];
            var rand = new Random();
            for (int i = 0; i< 1000; i++) {
                for(int j = 0; j < 1000; j++) {
                    Node iron = new Node();
                    iron.name = "iron";
                    iron.id = 1;
                    iron.outputVolume = 2;
                    iron.location = new Tuple<int, int>(i, j);

                    if (rand.NextDouble() > .95) {
                        map[i, j] = iron;
                        //Debug.WriteLine("added iron");
                    }
                }
            }
            var stn = new Bases.Station();
            //stn.
            //map[500,500] = 
            mapViewSize = 80;

    }
        public List<Entity> GetObjectsInView(int x, int y)
        {
            List<Entity> found = new List<Entity>();
            var left = Math.Max(0, x - (mapViewSize / 2));
            var right = Math.Min(MAP_SIZE, x + (mapViewSize / 2));
            var top = Math.Max(0, y - (mapViewSize / 2));
            var bot = Math.Min(MAP_SIZE, y + (mapViewSize / 2));

            Debug.WriteLine("" + left + "," + right + "," + top + "," + bot);

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
    }

}
