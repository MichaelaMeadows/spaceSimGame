using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Components
{
    public interface Entity
    {
        public Tuple<int, int> getLocation();
        public int getSize();
        public String getSprite();
    }
}
