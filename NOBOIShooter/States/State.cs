using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.States
{
    //every screen must inheritance from this class 
    public abstract class State
    {
        #region Fields

        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Main _game;

        #endregion

        #region Methods

        //constructor
        public State(Main game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        
        #endregion
    }
}
