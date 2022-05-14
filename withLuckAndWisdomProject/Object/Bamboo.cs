using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace withLuckAndWisdomProject.Object
{
    class Bamboo
    {
        Texture2D _bamboo;
        Vector2 _bambooPosition;
        public Bamboo()
        {

        }

        public void LoadContent()
        {
            _bambooPosition = new Vector2(100, 0);
        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bamboo, _bambooPosition, Color.Yellow);
        }
    }
}
