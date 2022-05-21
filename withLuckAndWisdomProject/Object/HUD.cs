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
        private int _distance;
        private SpriteFont _font;

        public HUD()
        {
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

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, " Score ", new Vector2(100, 20), Color.Black);
            spriteBatch.DrawString(_font, "0", new Vector2(120, 80), Color.Black);
            spriteBatch.DrawString(_font, " Distance ", new Vector2(700, 20), Color.Black);
            spriteBatch.DrawString(_font, "0", new Vector2(120, 80), Color.Black); 
        }
    }
} 
