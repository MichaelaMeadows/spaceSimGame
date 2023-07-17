using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSimulation.UI.Components
{
    // This interface is dubious at best. I think BaseUI needs specialized information fed to it
    interface Updatable
    {
        public void update(GameTime gameTime, int unitCount /*worldState?*/, MouseState mouseState, Point viewPoint, int scaleSize);
    }
}
