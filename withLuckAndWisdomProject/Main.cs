﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;


namespace withLuckAndWisdomProject
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenManager _screenManager;

        // Main Constructure
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Singleton.Instance.ContentRootDir;
            IsMouseVisible = Singleton.Instance.IsMouseVisible;

            //Make it full screen
            // _graphics.IsFullScreen = true;
            AudioManager.Initialize(this);
        }

        // Initialize (Run on start)
        protected override void Initialize()
        {
            //setting screen size
            _graphics.PreferredBackBufferHeight = Singleton.Instance.ScreenHeight;
            _graphics.PreferredBackBufferWidth = Singleton.Instance.ScreenWidth;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        // Load content (such as assets, picture, music)
        // contentManager: The content manager to use
        protected override void LoadContent()
        {
            // Create a spriteBatch
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all contents
            ResourceManager.LoadContent(Content);

            //Load all sound
            AudioManager.LoadSounds();

            // Load screen manager
            _screenManager = new ScreenManager();

        }

        // Update program logic everytime
        protected override void Update(GameTime gameTime)
        {

            // Update screen
            _screenManager.Update(gameTime);

            if(ScreenManager.Quit)
                Exit();

            base.Update(gameTime);
        }

        // Draw Main Method 
        protected override void Draw(GameTime gameTime)
        {
            // Clear programpage with white colour
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            
            // Draw program current screen
            _screenManager.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {

            base.OnExiting(sender, args);
        }

    }
}
