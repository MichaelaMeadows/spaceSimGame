using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceSimulation.UI
{
    //TODO: break this class up into multiple pieces. The visual representation of the UI and the data tracked?
    //TODO: is string update actually performant? for changing text every frame should I do it differently?
    public class BaseUI : Components.Loadable, Components.Drawable, Components.Updatable
    {
        private const String UI_FONT_NAME = "UIText";

        private int screenWidth;
        private int screenHeight;
        private SpriteFont font;
        private Matrix scalarMatrix; // calculations should assume 1920x1080
        private Texture2D bottomBar;
        private Vector2 initialBarPos;

        private String debugMessage;

        public BaseUI(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            scalarMatrix = Matrix.CreateScale(1, 1, 1);
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
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null/*samplerState*/, null/*depthStencilState*/, /*rasterizerState*/ null, /*effect*/ null, scalarMatrix);
            spriteBatch.Draw(bottomBar, initialBarPos, Color.Chocolate);
            spriteBatch.DrawString(font, debugMessage, initialBarPos, Color.White);
            spriteBatch.End();
        }

        public void update(GameTime gameTime, int unitCount /*worldState?*/, MouseState mouseState, Point viewpoint, int scaleSize)
        {
            debugMessage = "GameTime: " + gameTime.TotalGameTime + " | units visible: " + unitCount + " | viewport position: " + viewpoint.X + "," + viewpoint.Y + " | mouse position: " + mouseState.X + "," + mouseState.Y + " | current world scale: " + scaleSize; 
        }

        public void resize()
        {
            //TODO
        }
    }
}
