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
        private Texture2D buttonTexture, BG, logo, cursor;
        private SpriteFont buttonFont;
        private Button playButton, leaderboardButton, quitGameButton;
        private Vector2 _position;

        private Vector2 _origin = new Vector2(64, 64);
        
        
        private SoundEffect soundEffect;
        private SoundEffectInstance Instance;

        //constructor inherit from base class
        public MenuState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            //load content aka. assets files (picture, BG)
            buttonTexture = _content.Load<Texture2D>("Controls/Button");
            buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            BG = _content.Load<Texture2D>("Backgrouds/wild-west");
            logo = _content.Load<Texture2D>("Item/logo");
            
            //sheriff cursor added
            cursor = _content.Load<Texture2D>("Item/sheriff-cursor");

            // Set cursor mouse
            //Mouse.SetCursor(MouseCursor.FromTexture2D(cursor, 1, 1));
            Mouse.SetCursor(MouseCursor.Hand);

            //buttons config
            playButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(800, 100),
                Text = "Play",
            };

            playButton.Click += playButton_onClick;

            leaderboardButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(800, 200),
                Text = "Leaderboard",
            };

            leaderboardButton.Click += leaderboardButton_onClick;

            quitGameButton = new Button(buttonTexture, buttonFont)
            {
                PenColour = new Color(Color.Red, 1f),
                Position = new Vector2(800, 300),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_onClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                playButton,
                leaderboardButton,
                quitGameButton,
            };

            //Load BGM
            ControllerBGM(content);
        }

        //Buttons behavior 
        private void playButton_onClick(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            Instance.Dispose();
        }

        private void leaderboardButton_onClick(object sender, EventArgs e)
        {
            _game.ChangeState(new LeaderboardState(_game, _graphicsDevice, _content));
            //Console.WriteLine("Leaderboard click");
        }

        private void QuitGameButton_onClick(object sender, EventArgs e)
        {
            _game.Exit();
        }

        //BGM Controller
        private void ControllerBGM(ContentManager content) 
        {
            soundEffect = content.Load<SoundEffect>("BGM/BGM");

            Instance = soundEffect.CreateInstance();
            Instance.IsLooped = true;

            Instance.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(BG, new Vector2(0, 0), Color.White);
            
            // resize logo
            Rectangle logoFrame = new Rectangle(115, 100, 500, 200);
            spriteBatch.Draw(logo, logoFrame, Color.White);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            /*
             spriteBatch.Draw(
                 cursor,
                 _position,
                 Color.White
                 );
            
            */
            spriteBatch.Draw(cursor, _position, null,
                Color.White, 0f, new Vector2(cursor.Width / 2, cursor.Height / 2),
                30f / cursor.Width, SpriteEffects.None, 0f);
            
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
