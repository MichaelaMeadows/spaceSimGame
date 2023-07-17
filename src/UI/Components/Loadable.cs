﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSimulation.UI.Components
{
    interface Loadable
    {
        public void load(ContentManager contentManager, GraphicsDevice graphicsDevice);
    }
}
