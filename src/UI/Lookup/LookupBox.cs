using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Components;
using SpaceSimulation;
using spaceSimGame.src.UI.Lookup;
using System.Diagnostics;

namespace SpaceSimulation.UI.Lookup
{
    public class LookupBox : Components.Loadable, Components.Drawable, Components.Updatable
    {
        private const string UI_FONT_NAME = "UIText";

        private int screenWidth;
        private int screenHeight;
        private int tickRefreshRate;
        private SpriteFont font;
        private Matrix scalarMatrix; // calculations should assume 1920x1080
        private Texture2D infoBox;
        private Vector2 initialBoxPos;
        private Entity entity;
        private Boolean entityChanged;
        private List<String> inspectedAtts;

        public LookupBox(int screenWidth, int screenHeight, int tickRefreshRate)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.tickRefreshRate = tickRefreshRate;
            scalarMatrix = Matrix.CreateScale(1, 1, 1);
            entityChanged = false;
            inspectedAtts = new List<String>();
        }

        public void load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            font = contentManager.Load<SpriteFont>(UI_FONT_NAME);
            int boxHeight = screenHeight / 3;
            int boxWidth = screenWidth / 8;
            int boxXPos = screenWidth - boxWidth - screenWidth / 192;
            int boxYPos = screenHeight / 108;


            infoBox = new Texture2D(graphicsDevice, boxWidth, boxHeight);
            Color[] colorData = new Color[boxWidth * boxHeight];
            for (int i = 0; i < colorData.Length; ++i) colorData[i] = Color.Chocolate;
            infoBox.SetData(colorData);
            initialBoxPos = new Vector2(boxXPos, boxYPos);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (entity != null)
            {
                spriteBatch.Draw(infoBox, initialBoxPos, Color.Chocolate);
                for (int i = 0; i < inspectedAtts.Count; i++)
                {
                    Vector2 computedPos = new Vector2(initialBoxPos.X, initialBoxPos.Y + (i * (screenHeight / 54)));
                    spriteBatch.DrawString(font, inspectedAtts[i], computedPos, Color.White);
                }
            }
        }

        public void update(float tick, WorldState worldState, MouseState mouseState, Point viewpoint)
        {
            if (entityChanged && entity != null && tick % tickRefreshRate == 0)
            {
                this.inspectedAtts = Inspector.inspect(entity);
                entityChanged = false;
            }

        }

        public void attachEntity(Entity entity)
        {
            this.entity = entity;
            entityChanged = true;
        }
    }
}
