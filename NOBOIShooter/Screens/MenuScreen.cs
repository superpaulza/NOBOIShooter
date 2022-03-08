﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace NOBOIShooter.Screens
{
    //Menu screen
    public class MenuScreen : AScreen
    {
        //variables decoration
        private List<Component> _components;
        private Texture2D _buttonTexture, _background, _logo, _volumeOn, _volumeOff, _volumeState, _options, _human, _leftEye, _rightEye;
        private SpriteFont _buttonFont;
        private Button _playButton, _leaderboardButton, _quitGameButton, _gameOptionsButton;
        private DynamicButton _volumeControlButton;

        private SoundEffect _soundEffect;
        private SoundEffectInstance _instance;

        //Constructor inherit from base class
        public MenuScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            //load content eg. assets files (picture, background)
            _buttonTexture = _content.Load<Texture2D>("Controls/Button");
            _buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _human = _content.Load<Texture2D>("Item/human/body");
            _leftEye = _content.Load<Texture2D>("Item/human/left-eye");
            _rightEye = _content.Load<Texture2D>("Item/human/right-eye");
            _logo = _content.Load<Texture2D>("Item/logo");
            _volumeOn = _content.Load<Texture2D>("Item/volume-on");
            _volumeOff = _content.Load<Texture2D>("Item/volume-off");
            _options = _content.Load<Texture2D>("Icons/Setting");

            _volumeState = _volumeOn;

            //buttons config
            _playButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(800, 100),
                Text = "Play",
            };

            _playButton.Click += PlayButtonOnClick;

            _leaderboardButton = new Button(_buttonTexture, _buttonFont)
            {
                Position = new Vector2(800, 200),
                Text = "Leaderboard",
            };

            _leaderboardButton.Click += LeaderboardButtonOnClick;

            _quitGameButton = new Button(_buttonTexture, _buttonFont)
            {
                PenColour = new Color(Color.Red, 1f),
                Position = new Vector2(800, 300),
                Text = "Quit Game",
            };

            _quitGameButton.Click += QuitGameButtonOnClick;

            _volumeControlButton = new DynamicButton(_volumeState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(1220, 20),
                Text = "",
                
            };

            _volumeControlButton.Click += VolumeControlButtonOnClick;

            _gameOptionsButton = new Button(_options)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(1150, 20),
                Text = "",

            };

            _gameOptionsButton.Click += GameOptionsButtonOnClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playButton,
                _leaderboardButton,
                _quitGameButton,
                _volumeControlButton,
                _gameOptionsButton,
            };

            //Load BGM
            ControllerBGM(content);
        }

        //Buttons behavior 
        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeScreen(ScreenSelect.Game);
            // _instance.Stop();
            _instance.Dispose();
        }

        private void LeaderboardButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeScreen(ScreenSelect.Leaderboard);
            Console.WriteLine("Leaderboard click");
        }

        private void QuitGameButtonOnClick(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void VolumeControlButtonOnClick(object sender, EventArgs e) 
        {
            switch (_instance.State) {
                case SoundState.Playing:
                    _instance.Pause();
                    _volumeControlButton.Texture = _volumeOff;
                    break;
                case SoundState.Paused:
                    _instance.Resume();
                    _volumeControlButton.Texture = _volumeOn;
                    break;
            }
        }

        private void GameOptionsButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeScreen(ScreenSelect.Setting);
        }

        //BGM Controller
        private void ControllerBGM(ContentManager content) 
        {
            _soundEffect = content.Load<SoundEffect>("BGM/MainMenuBGM");

            _instance = _soundEffect.CreateInstance();
            _instance.IsLooped = true;

            _instance.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);

            // Draw Noboi logo
            Rectangle logoFrame = new Rectangle(115, 0, 400, 350);
            spriteBatch.Draw(_logo, logoFrame, Color.White);

            // Draw human
            Rectangle humanFrame = new Rectangle(494, 349, 171, 316);
            spriteBatch.Draw(_human, humanFrame, Color.White);

            // Draw eyes of human
            Rectangle leftEyeFrame = new Rectangle(535, 439, 29, 29);
            spriteBatch.Draw(_leftEye, leftEyeFrame, Color.White);

            Rectangle rightEyeFrame = new Rectangle(600, 442, 29, 29);
            spriteBatch.Draw(_rightEye, rightEyeFrame, Color.White);

           
            // draw On-Off Volume Icon
            /* Rectangle resized_volume_icon = new Rectangle(1210, 10, 60, 60);
               spriteBatch.Draw(volume_state, resized_volume_icon, Color.White);
            */

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

                _volumeControlButton.Draw(gameTime, spriteBatch);

                spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        { 
            //if click condition
            foreach (Component component in _components)
                component.Update(gameTime);

            _volumeControlButton.Update(gameTime);

            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                System.Diagnostics.Debug.WriteLine(mouse.X.ToString() + " , " + mouse.Y.ToString());
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }
    }
}
