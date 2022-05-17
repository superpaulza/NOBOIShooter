using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using withLuckAndWisdomProject.Screens;

namespace withLuckAndWisdomProject.Object
{
    class Bamboo
    {
        Texture2D bamboo;
        Vector2 bambooPosition;
        private BambooState _status;
        private Texture2D _texture;

        private enum BambooState
        {
            Normal,
            Burning,
            Friable,
            Slip,
        } 
        

        public Bamboo(Vector2 bambooPosition)
        {
            this.bambooPosition = bambooPosition;
            _status = BambooState.Normal;
            _texture = ResourceManager.Bamboo;
        }

        public void LoadContent()
        {
            

        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this.bambooPosition, Color.White);
        }
    }
}
