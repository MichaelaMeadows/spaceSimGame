using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Components;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceSimulation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private WorldState worldState;
        private Texture2D texture;

        private float scale;
        private RenderTarget2D renderTarget;

        //Viewpoint index using the world state map, not the scaled view.
        private Point viewpoint;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            scale = 1f / (1080f / GraphicsDevice.Viewport.Height);
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            viewpoint = new Point(500, 500);

            worldState = new WorldState();

            texture = Content.Load<Texture2D>("iron");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            // Prepare map scaling
            var mapViewSize = worldState.mapViewSize;
            var heightScale = GraphicsDevice.Viewport.Height / mapViewSize;
            var widthScale = GraphicsDevice.Viewport.Width / mapViewSize;


            var viewCorner_x = viewpoint.X - (mapViewSize / 2);
            var viewCorner_y = viewpoint.Y - (mapViewSize / 2);
            List<Entity> objectsInView = worldState.GetObjectsInView(viewpoint.X, viewpoint.Y);
            Debug.WriteLine(objectsInView.Count);


            _spriteBatch.Begin();
            //var position = new Vector2(0, 0);
            //_spriteBatch.Draw(texture, position, new Rectangle(new Point(50, 50), new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
           // _spriteBatch.Draw(texture, new Rectangle(0, 0, 15, 15), Color.White);


            foreach (Entity e in objectsInView)
            {
                _spriteBatch.Draw(texture, new Rectangle((e.getLocation().Item1 - viewCorner_x) * widthScale, (e.getLocation().Item2 - viewCorner_y) * heightScale, 20, 20), Color.White);
            }

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
