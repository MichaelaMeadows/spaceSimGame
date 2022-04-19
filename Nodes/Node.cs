﻿using Microsoft.Xna.Framework;
using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Nodes
{
    class Node : Entity
    {
        public int id { get; set; }
        public int type { get; set; }
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
            return outputVolume * 2;
        }

        public string getSprite()
        {
            switch (this.type)
            {
                case 0:
                    return "iron";
                case 1:
                    return "copper";
                default:
                    return "iron";
            }
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
