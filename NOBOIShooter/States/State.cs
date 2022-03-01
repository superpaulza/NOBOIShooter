using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.States
{
    //every screen must inheritance from this class 
    public abstract class State
    {
        protected Main _game;

        protected ContentManager _content;

        public State(Main game, ContentManager content)
        {
            _game = game;
            _content = content;
        }

        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Draw(GameTime game, SpriteBatch spriteBatch);

    }
}
