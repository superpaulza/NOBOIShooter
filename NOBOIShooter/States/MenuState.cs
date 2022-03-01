using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.States
{
    public class MenuState : State
    {

        private Texture2D menuBackGroundTexture;

        public MenuState(Main game, ContentManager content) : base(game, content)
        {

        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("");
            var buttonFont = _content.Load<Texture2D>("");
            menuBackGroundTexture = _content.Load<Texture2D>("");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime game, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
