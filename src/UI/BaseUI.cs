using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Components;
using SpaceSimulation.UI.Lookup;
using SpaceSimulation.Helpers;

namespace SpaceSimulation.UI
{
    //TODO: break this class up into multiple pieces. The visual representation of the UI and the data tracked?
    //TODO: is string update actually performant? for changing text every frame should I do it differently?
    public class BaseUI : Components.Loadable, Components.Drawable, Components.Updatable
    {
        private const String UI_FONT_NAME = "UIText";

        private int screenWidth;
        private int screenHeight;
        private int tickRefreshRate;
        private SpriteFont font;
        private Matrix scalarMatrix; // calculations should assume 1920x1080
        private Texture2D bottomBar;
        private Vector2 initialBarPos;
        private LookupBox lookupBox;

        private String debugMessage;

        public BaseUI(int screenWidth, int screenHeight, int tickRefreshRate)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.tickRefreshRate = tickRefreshRate;
            this.scalarMatrix = Matrix.CreateScale(1, 1, 1);
            this.lookupBox = new LookupBox(screenWidth, screenHeight, tickRefreshRate);
            this.debugMessage = "";
        }

        public void load(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            font = contentManager.Load<SpriteFont>(UI_FONT_NAME);
            int barHeight = screenHeight / 54;
            int barPos = screenHeight - barHeight;
            bottomBar = new Texture2D(graphicsDevice, screenWidth, barHeight);
            Color[] colorData = new Color[screenWidth * barHeight];
            for (int i = 0; i < colorData.Length; ++i) colorData[i] = Color.Chocolate;
            bottomBar.SetData(colorData);
            initialBarPos = new Vector2(0, barPos);
            lookupBox.load(contentManager, graphicsDevice);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null/*samplerState*/, null/*depthStencilState*/, /*rasterizerState*/ null, /*effect*/ null, scalarMatrix);
            spriteBatch.Draw(bottomBar, initialBarPos, Color.Chocolate);
            spriteBatch.DrawString(font, debugMessage, initialBarPos, Color.White);
            lookupBox.draw(spriteBatch);
            spriteBatch.End();
        }

        public void update(float tick, WorldState worldState, MouseState mouseState, Point viewpoint)
        {
            if (tick % tickRefreshRate == 0)
            {
                debugMessage = "CurrentTick: " + tick
                    + " | viewport position in world space: " + viewpoint.X + "," + viewpoint.Y
                    + " | mouse position in screen space: " + mouseState.X + "," + mouseState.Y
                    + " | current world scale: " + worldState.mapViewSize
                    ;
                lookupBox.update(tick, worldState, mouseState, viewpoint);
            }

        }

        public void attachEntity(Entity entity)
        {
            this.lookupBox.attachEntity(entity);
        }

        public void resize()
        {
            //TODO
        }
    }
}
