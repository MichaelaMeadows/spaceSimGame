using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Bases;
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
        private Texture2D iron_texture;
        private Texture2D base_texture;

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
            Station capital = new Station();
            worldState.placeObject(capital, 500, 500);

            iron_texture = Content.Load<Texture2D>("iron");
            base_texture = Content.Load<Texture2D>("ship1");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                viewpoint.Y = viewpoint.Y - 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                viewpoint.Y = viewpoint.Y + 1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                viewpoint.X = viewpoint.X - 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                viewpoint.X = viewpoint.X + 1;
            if (Keyboard.GetState().IsKeyDown(Keys.O))
                worldState.mapViewSize = worldState.mapViewSize - 2;
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                worldState.mapViewSize = worldState.mapViewSize + 2;

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
            //Debug.WriteLine(objectsInView.Count);

            _spriteBatch.Begin();
            //_spriteBatch.Draw(texture, position, new Rectangle(new Point(50, 50), new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            // _spriteBatch.Draw(texture, new Rectangle(0, 0, 15, 15), Color.White);

            Debug.WriteLine(widthScale);
            Debug.WriteLine(heightScale);
            foreach (Entity e in objectsInView)
            {
                var tmpTexture = iron_texture;
                if (e.GetType() == typeof(Station)) {
                    tmpTexture = base_texture;
                }
                _spriteBatch.Draw(tmpTexture, new Rectangle((e.getLocation().Item1 - viewCorner_x) * widthScale, (e.getLocation().Item2 - viewCorner_y - (e.getSize() / 2)) * heightScale, e.getSize() * widthScale, e.getSize() * heightScale), Color.White);
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
