using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Object;

namespace withLuckAndWisdomProject.Screens
{
    class GameScreen : AScreen
    {
        private Bamboo bamboo;
        //Constructor inherit from base class
        public GameScreen()
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            bamboo.draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
