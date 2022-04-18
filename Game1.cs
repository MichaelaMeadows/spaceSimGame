using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using SpaceSimulation.Empires;
using SpaceSimulation.Ships;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceSimulation
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Collection to hold textures?
        private Texture2D iron_texture;
        private Texture2D base_texture;
        private Texture2D vehicle_texture;

        private float scale;
        private RenderTarget2D renderTarget;

        private WorldState worldState;
        private List<Empire> empires;

        private Random r;

        float tickCount;

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
            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);//60d);

            r = new Random();
            tickCount = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            scale = 1f / (1080f / GraphicsDevice.Viewport.Height);
            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            viewpoint = new Point(500, 500);

            worldState = new WorldState();

            // Create 12 empires in random starting locations
            empires = new List<Empire>();
            for (int i = 0; i < 12; i++)
            {
                Empire e1 = new Empire();
                empires.Add(e1);
                int x = r.Next(15, 1000);
                int y = r.Next(15, 1000);
                Station capital = new Station(new Tuple<int, int>(x, y));
                worldState.placeObject(capital, x, y);
                e1.stations.Add(capital);
            }

            iron_texture = Content.Load<Texture2D>("iron");
            base_texture = Content.Load<Texture2D>("ship1");
            vehicle_texture = Content.Load<Texture2D>("smallCar");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            tickCount++;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.W) && viewpoint.Y > 20)
                viewpoint.Y = viewpoint.Y - 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S) && viewpoint.Y < 1980)
                viewpoint.Y = viewpoint.Y + 1;
            if (Keyboard.GetState().IsKeyDown(Keys.A) && viewpoint.X > 20)
                viewpoint.X = viewpoint.X - 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D) && viewpoint.X < 1980)
                viewpoint.X = viewpoint.X + 1;
            if (Keyboard.GetState().IsKeyDown(Keys.O) && worldState.mapViewSize > 20)
                worldState.mapViewSize = worldState.mapViewSize - 2;
            if (Keyboard.GetState().IsKeyDown(Keys.P) && worldState.mapViewSize < 400)
                worldState.mapViewSize = worldState.mapViewSize + 2;

            // Nodes refresh available resources.
            if (tickCount > 60)
            {
                worldState.updateNodes();
                tickCount = 0;
            }

            foreach (Empire e in empires)
            {
                e.executeStrategy(worldState);
            }

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

            //Debug.WriteLine(widthScale);
            //Debug.WriteLine(heightScale);
            //Random r = new Random();
            foreach (Entity e in objectsInView)
            {
                //worldState.placeObject(e, r.Next(0, 1000), r.Next(0, 1000));
                var tmpTexture = iron_texture;
                if (e.GetType() == typeof(Station)) {
                    tmpTexture = base_texture;
                } else if (e.GetType() == typeof(Vehicle))
                {
                  /*  var xd = r.Next(-2, 3);
                    var yd = r.Next(-2, 3);
                    var nx = e.getLocation().Item1 + xd;
                    nx = Math.Max(0, nx);
                    nx = Math.Min(1998, nx);
                    var ny = e.getLocation().Item2 + yd;
                    ny = Math.Max(0, ny);
                    ny = Math.Min(1998, ny);

                    //e.setLocation(new Tuple<int, int>(e.getLocation().Item1 + xd, e.getLocation().Item2 + yd));
                    worldState.placeObject(e, nx, ny);*/
                    tmpTexture = vehicle_texture;
                }
                _spriteBatch.Draw(tmpTexture, new Rectangle(
                    (e.getLocation().Item1 - viewCorner_x - (e.getSize() / 2)) * widthScale, 
                    (e.getLocation().Item2 - viewCorner_y - (e.getSize() / 2)) * heightScale, 
                    e.getSize() * widthScale, 
                    e.getSize() * heightScale), 
                    Color.White);
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
