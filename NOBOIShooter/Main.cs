using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NOBOIShooter.States;

namespace NOBOIShooter
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<GameObject> _gameObjects;
        int _numObject;

        private State _currentState;

        private State _nextState;

        SpriteFont _font;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        //Constructor
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Singleton.Instance.ContentRootDir;
            IsMouseVisible = Singleton.Instance.IsMouseVisible;
        }

        //initialize (run once)
        protected override void Initialize()
        {
            //setting screen size
            _graphics.PreferredBackBufferHeight = Singleton.Instance.ScreenHeight;
            _graphics.PreferredBackBufferWidth = Singleton.Instance.ScreenWidth;
            _graphics.ApplyChanges();

            //load game objects
            _gameObjects = new List<GameObject>();

            base.Initialize();
        }

        //load content (such as assets, picture, music)
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
        }

        //update screen
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);



            base.Update(gameTime);
        }


        //draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
