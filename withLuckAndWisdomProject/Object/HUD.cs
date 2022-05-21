using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;


// HUD contains the score value and distance value. 
namespace withLuckAndWisdomProject.Object
{
    public class HUD
    {
        private int _score;
        private float _distance;
        private SpriteFont _font;
        private Vector2 _rabbitPosition;
        private Rabbit _rabbit;

        public HUD(Vector2 rabbitPosition)
        {
            // Constructor Parameter Init.
            this._rabbitPosition = rabbitPosition;
            _distance = this._rabbitPosition.X;

            // Font Init.
            _font = ResourceManager.font;
        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            // Draw Score Section.
            
        }

        public void DrawDistance(SpriteBatch spriteBatch)
        {
            // Draw Distance Section.

            // Reference from Rabbit Position.

        }

        public void UpdateScore()
        {

        }

        public void UpdateDistance()
        {
            
        }

        public void update(GameTime gameTime)
        {
            
            Console.WriteLine("Distance: " + _distance);
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Score", new Vector2(100, 20), Color.Black);
            spriteBatch.DrawString(_font, "0", new Vector2(120, 80), Color.Black);
            spriteBatch.DrawString(_font, "Distance", new Vector2(500, 20), Color.Black);
            spriteBatch.DrawString(_font, _distance.ToString(), new Vector2(520, 80), Color.Black);
            spriteBatch.DrawString(_font, "Time", new Vector2(900, 20), Color.Black);
            spriteBatch.DrawString(_font, "0", new Vector2(920, 80), Color.Black);
        }
    }
} 
