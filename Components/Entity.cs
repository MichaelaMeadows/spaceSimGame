using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Components
{
    public interface Entity
    {
        public Tuple<int, int> getLocation();
        public void setLocation(Tuple<int, int> location);
        public int getSize();
        public String getSprite();
    }
}
