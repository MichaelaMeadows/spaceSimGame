﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceSimulation.Bases;
using SpaceSimulation.Commands;
using SpaceSimulation.Components;
using SpaceSimulation.Empires;
using SpaceSimulation.Helpers;
using SpaceSimulation.Ships;
using SpaceSimulation.src.Ships;
using SpaceSimulation.UI;
using SpaceSimulation.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceSimulation
{
    public class Game1 : Game
    {
        private int windowWidth;
        private int windowHeight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private float scale;
        private RenderTarget2D renderTarget;

        private WorldState worldState;
        private List<Empire> empires;

        private Random r;
        private Dictionary<string, Texture2D> textureMap;

        private BaseUI baseUi;

        float tickCount;

        //Viewpoint index using the world state map, not the scaled view.
        private Point viewpoint;
        private int pan_speed = 20;
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
            windowWidth = Math.Min(1920, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            windowHeight = Math.Min(1080, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            baseUi = new BaseUI(windowWidth, windowHeight, 15);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            scale = 1f / (1080f / GraphicsDevice.Viewport.Height);
            renderTarget = new RenderTarget2D(GraphicsDevice, windowWidth, windowHeight);
            viewpoint = new Point(500, 500);

            worldState = new WorldState();

            // Create 12 empires in random starting locations
            empires = new List<Empire>();
            for (int i = 0; i < 50; i++)
            {
                Empire e1 = new Empire(worldState, i);
                empires.Add(e1);
                int x = r.Next(15, worldState.getMapSize() - 10);
                int y = r.Next(15, worldState.getMapSize() - 10);
                Station capital = new Station(new Tuple<int, int>(x, y), worldState, i);
                worldState.placeObject(capital, x, y);
                e1.stations.Add(capital);
            }

            textureMap = new Dictionary<string, Texture2D>();
            textureMap.Add("iron", Content.Load<Texture2D>("iron"));
            textureMap.Add("copper", Content.Load<Texture2D>("copper"));
            textureMap.Add("ship1", Content.Load<Texture2D>("ship1"));
            textureMap.Add("frigate1", Content.Load<Texture2D>("ship1"));
            textureMap.Add("spacestation", Content.Load<Texture2D>("spacestation"));
            textureMap.Add("smallCar", Content.Load<Texture2D>("smallCar"));
            textureMap.Add("hydrogen", Content.Load<Texture2D>("hydrogen"));
            textureMap.Add("KineticRound", Content.Load<Texture2D>("KineticRound"));

            baseUi.load(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            tickCount++;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.W) && viewpoint.Y > 20)
                viewpoint.Y = viewpoint.Y - pan_speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S) && viewpoint.Y < worldState.getMapSize() - 50)
                viewpoint.Y = viewpoint.Y + pan_speed;
            if (Keyboard.GetState().IsKeyDown(Keys.A) && viewpoint.X > 20)
                viewpoint.X = viewpoint.X - pan_speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D) && viewpoint.X < worldState.getMapSize() - 50)
                viewpoint.X = viewpoint.X + pan_speed;
            if (Keyboard.GetState().IsKeyDown(Keys.O) && worldState.mapViewSize > 140)
                worldState.mapViewSize = worldState.mapViewSize - 4;
            if (Keyboard.GetState().IsKeyDown(Keys.P) && worldState.mapViewSize < 2000)
                worldState.mapViewSize = worldState.mapViewSize + 4;


            // Prepare map scaling
            var mapViewSize = worldState.mapViewSize;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Point convertedPoint = WorldSpaceMath.GetWorldPointForMousePoint(
                    viewpoint, 
                    mouseState.Position, 
                    mapViewSize, 
                    GraphicsDevice.Viewport.Height, 
                    GraphicsDevice.Viewport.Width
                );
               
                Entity entityAtLoc = worldState.getEntityAtLocation(convertedPoint.X, convertedPoint.Y);
                // TODO: attach entity directly? Make it subscribe to mouse events? do anything other than this
                baseUi.attachEntity(entityAtLoc);
                
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                // Right mouse button is pressed
                // Do something

            }

            // Nodes refresh available resources.
            if (tickCount % 60 == 0)
            {
                worldState.updateNodes();
            }

            // Nodes refresh available resources.
            if (tickCount % 5 == 0)
            {
                worldState.updateProjectiles();
            }

            // Game runs in 1 minute logical loops so far
            if (tickCount > 600)
            {
                tickCount = 0;
            }

            foreach (Empire e in empires)
            {
                e.executeStrategy(worldState, (int) tickCount);
            }
            //TODO: avoid double worldStateGet
            baseUi.update(
                tickCount, 
                worldState, 
                mouseState, 
                viewpoint
            );
            // TODO: Add your update logic here
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // Prepare map scaling
            var mapViewSize = worldState.mapViewSize;
            var heightScale = (double) GraphicsDevice.Viewport.Height / mapViewSize;
            var widthScale = (double) GraphicsDevice.Viewport.Width / mapViewSize;

            var viewCorner_x = viewpoint.X - (mapViewSize / 2);
            var viewCorner_y = viewpoint.Y - (mapViewSize / 2);
            List<Entity> objectsInView = worldState.GetObjectsInView(viewpoint.X, viewpoint.Y);

            List<Projectile> projectilesInVite = worldState.GetProjectilesInView(viewpoint.X, viewpoint.Y);

            _spriteBatch.Begin();

            foreach (Entity e in objectsInView)
            {
                _spriteBatch.Draw(textureMap.GetValueOrDefault(e.getSprite()), new Rectangle(
                    (int)((e.getLocation().Item1 - viewCorner_x - (e.getSize() / 2)) * widthScale),
                    (int)((e.getLocation().Item2 - viewCorner_y - (e.getSize() / 2)) * heightScale),
                    (int)(e.getSize() * widthScale),
                    (int)(e.getSize() * heightScale)), 
                    Color.White);
            }

            foreach (Projectile p in projectilesInVite)
            {
                _spriteBatch.Draw(textureMap.GetValueOrDefault("KineticRound"), new Rectangle(
                    (int)((p.currentLocation.Item1 - viewCorner_x - (p.size / 2)) * widthScale),
                    (int)((p.currentLocation.Item2 - viewCorner_y - (p.size / 2)) * heightScale),
                    (int)(p.size * widthScale),
                    (int)(p.size * heightScale)),
                    Color.White);
            }


            _spriteBatch.End();
            // Draw a bar at the botom of the screen with buttons of the left, and a unit card on the right
            baseUi.draw(_spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();
            _spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
