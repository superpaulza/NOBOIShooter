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
        private Texture2D _buttonTexture, _background, _logo, _cursor;
        private SpriteFont buttonFont;
        private Button _playButton, _leaderboardButton, _quitGameButton;
        private Vector2 _position;

        private Vector2 _origin = new Vector2(64, 64);
        
        
        private SoundEffect _soundEffect;
        private SoundEffectInstance _instance;

        //constructor inherit from base class
        public MenuState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            //load content aka. assets files (picture, background)
            _buttonTexture = _content.Load<Texture2D>("Controls/Button");
            buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            _background = _content.Load<Texture2D>("Backgrouds/wild-west");
            _logo = _content.Load<Texture2D>("Item/logo");
            
            //sheriff cursor added
            _cursor = _content.Load<Texture2D>("Item/sheriff-cursor");

            //buttons config
            _playButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(800, 100),
                Text = "Play",
            };

            _playButton.Click += PlayButtonOnClick;

            _leaderboardButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(800, 200),
                Text = "Leaderboard",
            };

            _leaderboardButton.Click += LeaderboardButtonOnClick;

            _quitGameButton = new Button(_buttonTexture, buttonFont)
            {
                PenColour = new Color(Color.Red, 1f),
                Position = new Vector2(800, 300),
                Text = "Quit Game",
            };

            _quitGameButton.Click += QuitGameButtonOnClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playButton,
                _leaderboardButton,
                _quitGameButton,
            };

            //Load BGM
            ControllerBGM(content);
        }

        //Buttons behavior 
        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            _instance.Dispose();
        }

        private void LeaderboardButtonOnClick(object sender, EventArgs e)
        {
            Console.WriteLine("Leaderboard click");
        }

        private void QuitGameButtonOnClick(object sender, EventArgs e)
        {
            _game.Exit();
        }

        //BGM Controller
        private void ControllerBGM(ContentManager content) 
        {
            _soundEffect = content.Load<SoundEffect>("BGM/BGM");

            _instance = _soundEffect.CreateInstance();
            _instance.IsLooped = true;

            _instance.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

            // resize logo
            Rectangle logoFrame = new Rectangle(115, 100, 500, 200);
            spriteBatch.Draw(_logo, logoFrame, Color.White);

            spriteBatch.Draw(
                _cursor,
                _origin,
                Color.White
                );

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();

            int xPosition = state.X;
            int yPostition = state.Y;

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
