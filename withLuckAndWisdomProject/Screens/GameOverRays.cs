using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Data;
using withLuckAndWisdomProject.Object;

namespace withLuckAndWisdomProject.Screens
{
    public class GameOverRays
    {
        private List<Component> _components;
        private Texture2D _texture, _textureBtn;
        private SpriteFont _font;
        private Button _replayBtn, _homeBtn;

        private Rabbit _player;

        

        public GameOverRays()
        {
            _texture = ResourceManager.overBackground;
            _textureBtn = ResourceManager.BasicBtn;
            _font = ResourceManager.font;
            _replayBtn = new Button(_textureBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 100, Singleton.Instance.ScreenHeight / 4 * 3 - 75),
                Text = "Replay"
            };

            _replayBtn.Click += replayBtnOnClick;

            _homeBtn = new Button(_textureBtn, _font)
            {
                PenColour = Color.Yellow,
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 250, Singleton.Instance.ScreenHeight / 4 * 3 - 75),
                Text = "Home"
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
            AudioManager.StopSound("GameBGM");
            AudioManager.PlaySound("GO");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.Black * 0.5f);
            string gameScore = "Distance: " + _player.ForwardLenght.ToString("N2") + " , Score: " + _player.Score.ToString();
            spriteBatch.DrawString(_font, "Game Over", new Vector2(Singleton.Instance.ScreenWidth / 2, 150), Color.White, 0f, _font.MeasureString("Game Over") * 0.5f, 3f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(_font, gameScore, new Vector2(Singleton.Instance.ScreenWidth / 2, 350), Color.White, 0f, _font.MeasureString(gameScore) * 0.5f, 1f, SpriteEffects.None, 0f);
            
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public void PostUpdate(GameTime gameTime)
        {

        }
    }
}
