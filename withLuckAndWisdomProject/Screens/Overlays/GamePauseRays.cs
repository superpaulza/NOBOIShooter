using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Object;

namespace withLuckAndWisdomProject.Screens
{
    public class GamePauseRays
    {
        private List<Component> _components;
        private Texture2D _texture;
        private SpriteFont _font;
        private Button _replayBtn, _homeBtn, _playButton;

        private Rabbit _player;

        

        public GamePauseRays()
        {
            // Load assets
            _texture = ResourceManager.overBackground;
            _font = ResourceManager.font;

            float centerOfWidth = Singleton.Instance.ScreenWidth / 2;
            float positionBtnHeight = Singleton.Instance.ScreenHeight / 4 * 3 - 250;

            // Create button on Game to replay
            _replayBtn = new Button(ResourceManager.replayBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(centerOfWidth + 150, positionBtnHeight),
                Text = ""
            };

            _replayBtn.Click += replayBtnOnClick;

            // Create button to continuous 
            _playButton = new Button(ResourceManager.button, _font)
            {
                PenColour = Color.DarkGreen,
                Position = new Vector2(centerOfWidth - 50, positionBtnHeight),
                Text = "",
            };

            _playButton.Click += PlayButtonOnClick;

            // Create back to home screen
            _homeBtn = new Button(ResourceManager.homeBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(centerOfWidth - 200, positionBtnHeight),
                Text = ""
            };

            _homeBtn.Click += homeBtnOnClick;

            // Add to list contain
            _components = new List<Component>()
            {
                _replayBtn,
                _homeBtn,
                _playButton
            };

        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            _player.GamePause = false;
            AudioManager.StopSound("GO");
        }

        public void SetPlayer(object player)
        {
            _player = (Rabbit) player;
            
        }

        private void replayBtnOnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen = "game";
        }

        private void homeBtnOnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen = "menu";
        }

        public void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);

            //Disable sound effect
            String[] RandomSound = new string[] { "Jumping2", "Jumping3", "GameBGM", "wind" };
            foreach (String st in RandomSound)
            {
                AudioManager.StopSound(st);
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.Black * 0.5f);
            spriteBatch.DrawString(_font, "Game Pause", new Vector2(Singleton.Instance.ScreenWidth / 2 + 30, 225), Color.HotPink, 0f, _font.MeasureString("Game Pause") * 0.5f, 1.5f, SpriteEffects.None, 0f);
           
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public void PostUpdate(GameTime gameTime)
        {

        }
    }
}
