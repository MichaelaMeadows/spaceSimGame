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
        public String name { get; set; }
        public int outputVolume { get; set; }
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
            throw new NotImplementedException();
        }
    }
}
