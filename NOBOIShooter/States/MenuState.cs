using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using System;
using System.Collections.Generic;

namespace NOBOIShooter.States
{
    //Menu screen
    public class MenuState : State
    {
        //variables decoration
        private List<Component> _components;
        private Texture2D buttonTexture, BG;
        private SpriteFont buttonFont;
        private Button playButton, leaderboardButton, quitGameButton;

        private SoundEffect soundEffect;

        //constructor inherit from base class
        public MenuState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            //load content aka. assets files (picture, BG)
            buttonTexture = _content.Load<Texture2D>("Controls/Button");
            buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            BG = _content.Load<Texture2D>("Backgrouds/wild-west");

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
        }

        private void leaderboardButton_onClick(object sender, EventArgs e)
        {
            Console.WriteLine("Leaderboard click");
        }

        private void QuitGameButton_onClick(object sender, EventArgs e)
        {
            _game.Exit();
        }

        //BGM behavior
        private void ControllerBGM(ContentManager content) 
        {
            soundEffect = content.Load<SoundEffect>("BGM/BGM");

            var Instance = soundEffect.CreateInstance();
            Instance.IsLooped = true;

            Instance.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(BG, new Vector2(0, 0), Color.White);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

    }
}
