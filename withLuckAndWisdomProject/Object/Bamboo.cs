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
        

        public Bamboo()
        {
            _status = BambooState.Normal;
            _texture = ResourceManager.Bamboo;
            bambooPosition = new Vector2(100, 200);
        }

        public void LoadContent()
        {
            

        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, bambooPosition, Color.White);
        }
    }
}
