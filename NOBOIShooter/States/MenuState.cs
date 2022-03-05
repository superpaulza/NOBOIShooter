using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace NOBOIShooter.States
{
    //Menu screen
    public class MenuState : State
    {
        //variables decoration
        private List<Component> _components;
        private Texture2D _buttonTex, _homeTex, _logoTex, _cursorTex;
        private SpriteFont _buttonFont;
        private Button _playBtn, _scoreBtn, _quitGameBtn;
        private Vector2 _position;

        private Vector2 _origin = new Vector2(64, 64);
        
        
        private SoundEffect _homeSoundEffect;
        private SoundEffectInstance _instance;

        //constructor inherit from base class
        public MenuState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            //load content aka. assets files (picture, BG)
            _buttonTex = _content.Load<Texture2D>("Controls/Button");
            _buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            _homeTex = _content.Load<Texture2D>("Backgrouds/wild-west");
            _logoTex = _content.Load<Texture2D>("Item/logo");
            
            //sheriff cursor added
            _cursorTex = _content.Load<Texture2D>("Item/sheriff-cursor");

            // Set cursor mouse
            //Mouse.SetCursor(MouseCursor.FromTexture2D(cursor, 1, 1));
            //Mouse.SetCursor(MouseCursor.Hand);

            //buttons config
            _playBtn = new Button(_buttonTex, _buttonFont)
            {
                Position = new Vector2(800, 100),
                Text = "Play",
            };

            _playBtn.Click += playButton_onClick;

            _scoreBtn = new Button(_buttonTex, _buttonFont)
            {
                Position = new Vector2(800, 200),
                Text = "Leaderboard",
            };

            _scoreBtn.Click += leaderboardButton_onClick;

            _quitGameBtn = new Button(_buttonTex, _buttonFont)
            {
                PenColour = new Color(Color.Red, 1f),
                Position = new Vector2(800, 300),
                Text = "Quit Game",
            };

            _quitGameBtn.Click += QuitGameButton_onClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playBtn,
                _scoreBtn,
                _quitGameBtn,
            };

            //Load BGM
            ControllerBGM(content);
        }

        //Buttons behavior 
        private void playButton_onClick(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            _instance.Dispose();
        }

        private void leaderboardButton_onClick(object sender, EventArgs e)
        {
            Console.WriteLine("Leaderboard click");
        }

        private void QuitGameButton_onClick(object sender, EventArgs e)
        {
            _game.Exit();
        }

        //BGM Controller
        private void ControllerBGM(ContentManager content) 
        {
            _homeSoundEffect = content.Load<SoundEffect>("BGM/BGM");

            _instance = _homeSoundEffect.CreateInstance();
            _instance.IsLooped = true;

            _instance.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_homeTex, new Vector2(0, 0), Color.White);
            
            // resize logo
            Rectangle logoFrame = new Rectangle(115, 100, 500, 200);
            spriteBatch.Draw(_logoTex, logoFrame, Color.White);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            /*
             spriteBatch.Draw(
                 cursor,
                 _position,
                 Color.White
                 );
            
            */
            spriteBatch.Draw(_cursorTex, _position, null,
                Color.White, 0f, new Vector2(_cursorTex.Width / 2, _cursorTex.Height / 2),
                30f / _cursorTex.Width, SpriteEffects.None, 0f);
            
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            //int xPosition = state.X;
            //int yPostition = state.Y;
            _position = new Vector2(state.X, state.Y);

            //if click condition
            foreach (Component component in _components)
                component.Update(gameTime);

        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

    }
}
