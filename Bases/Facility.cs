using SpaceSimulation.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Bases
{
    class Facility : Entity
    {
        public Tuple<int, int> getLocation()
        {
            throw new NotImplementedException();
        }

        public int getSize()
        {
            return 20;
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
