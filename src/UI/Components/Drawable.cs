﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSimulation.UI.Components
{
    public interface Drawable
    {
        public void draw(SpriteBatch spriteBatch);
    }
}
