using Microsoft.Xna.Framework;
using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Nodes
{
    class Node : Entity
    {
        public int id { get; set; }
        public String type { get; set; }
        public int unit_size { get; set; }
        public int outputVolume { get; set; }
        private int store;
        public Tuple<int, int> location { get; set; }

        public Tuple<int, int> getLocation()
        {
            return location;
        }

        public int getSize()
        {
            return 1;
        }

        public string getSprite()
        {
            throw new NotImplementedException();
        }

        public void setLocation(Tuple<int, int> location)
        {
            this.location = location;
        }

        public int mine()
        {
            if (store > 0)
            {
                store = store - 1;
                return 1;
            }
            return 0;
        }

        public void refresh()
        {
            store = store + outputVolume;
            store = Math.Min(store, (outputVolume * 3));
        }
    }
}
