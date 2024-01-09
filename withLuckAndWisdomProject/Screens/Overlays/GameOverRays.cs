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
    public class GameOverRays
    {
        private List<Component> _components;
        private Texture2D _texture;
        private SpriteFont _font;
        private Button _replayBtn, _homeBtn;

        private Rabbit _player;

        

        public GameOverRays()
        {
            // Load assets
            _texture = ResourceManager.overBackground;
            _font = ResourceManager.font;

            // Create button on Game over screen
            _replayBtn = new Button(ResourceManager.replayBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 50, Singleton.Instance.ScreenHeight / 4 * 3 - 100),
                Text = ""
            };

            _replayBtn.Click += replayBtnOnClick;

            _homeBtn = new Button(ResourceManager.homeBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, Singleton.Instance.ScreenHeight / 4 * 3 - 100),
                Text = ""
            };

            _homeBtn.Click += homeBtnOnClick;


            _components = new List<Component>()
            {
                _replayBtn,
                _homeBtn
            };

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

            //Change Sound When GameOver
            String[] RandomSound = new string[] { "Jumping2", "Jumping3", "GameBGM", "wind" };
            foreach (String st in RandomSound)
            {
                AudioManager.StopSound(st);
            }
            AudioManager.PlaySound("GO");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.Black * 0.5f);
            string gameScore = _player.Score.ToString();
            string gameDistance = "Distance: " + _player.ForwardLenght.ToString("N2");
            string Timer = "Time: " + _player.PlayTime.ToString(@"hh\:mm\:ss");
            spriteBatch.DrawString(_font, "Game Over", new Vector2(Singleton.Instance.ScreenWidth / 2, 225), Color.HotPink, 0f, _font.MeasureString("Game Over") * 0.5f, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, "Score", new Vector2(Singleton.Instance.ScreenWidth / 2, 300), Color.White, 0f, _font.MeasureString("Score") * 0.5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, gameScore, new Vector2(Singleton.Instance.ScreenWidth / 2, 350), Color.White, 0f, _font.MeasureString(gameScore) * 0.5f, 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, gameDistance + "      " + Timer, new Vector2(Singleton.Instance.ScreenWidth / 2, 400), Color.White, 0f, _font.MeasureString(gameDistance + "      " + Timer) * 0.5f, .5f, SpriteEffects.None, 0f);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public void PostUpdate(GameTime gameTime)
        {

        }
    }
}
