using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Screens;


// HUD contains the score value and distance value. 
namespace withLuckAndWisdomProject.Object
{
    public class HUD
    {
        private int _score;
        private float _distance;
        private SpriteFont _font;
        private Texture2D _background;
        private Vector2 _rabbitPosition;
        private Rabbit _rabbit;
        private Button _pauseBtn;

        public HUD(Vector2 rabbitPosition)
        {
            // Constructor Parameter Init.
            _rabbitPosition = rabbitPosition;
            _distance = _rabbitPosition.X;
            // Font Init.
            _font = ResourceManager.font;
            // Texture Init.
            _background = ResourceManager.hudBackground;
            // Button Init.
            _pauseBtn = new Button(ResourceManager.pauseBtn)
            {
                Position = new Vector2(50, 40),
            };
            _pauseBtn.Click += pauseBtnOnClick;
        }

        private void pauseBtnOnClick(object sender, EventArgs e)
        {
            _rabbit.GamePause = true;
        }

        public void DrawBackGround(SpriteBatch spriteBatch)
        {
            // Draw Background.
            spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, (int)(Singleton.Instance.ScreenHeight * .2f)), Color.White);
        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            // Draw Score Section.
            spriteBatch.DrawString(_font, "Score", new Vector2(200, 20), Color.OrangeRed);
            spriteBatch.DrawString(_font, _rabbit.Score.ToString(), new Vector2(210, 80), Color.Black);

        }

        public void DrawDistance(SpriteBatch spriteBatch)
        {
            // Draw Distance Section.
            spriteBatch.DrawString(_font, "Distance", new Vector2(600, 20), Color.OrangeRed);
            spriteBatch.DrawString(_font, _rabbit.ForwardLenght.ToString("N0"), new Vector2(620, 80), Color.Black);

            // Reference from Rabbit Position.

        }

        public void DrawTime(SpriteBatch spriteBatch)
        {
            // Draw Timer.
            spriteBatch.DrawString(_font, "Time", new Vector2(1000, 20), Color.OrangeRed);
            spriteBatch.DrawString(_font, _rabbit.PlayTime.ToString(@"hh\:mm\:ss"), new Vector2(980, 80), Color.Black);
        }
        public void UpdatePauseBtn(GameTime gameTime)
        {
            // Update pause btn.
            _pauseBtn.Update(gameTime);
        }

        public void DrawPauseBtn(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw pause btn.
            _pauseBtn.Draw(gameTime, spriteBatch);
        }

        public void SetPlayer(object player)
        {
            _rabbit = (Rabbit)player;
        }


        public void update(GameTime gameTime)
        {
            UpdatePauseBtn(gameTime);
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawBackGround(spriteBatch);
            DrawScore(spriteBatch);
            DrawDistance(spriteBatch);
            DrawTime(spriteBatch);
            DrawPauseBtn(gameTime, spriteBatch);
        }
    }
} 
