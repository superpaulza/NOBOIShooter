using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace NOBOIShooter.Screens
{
    class OptionScreen : AScreen
    {
        private Texture2D _background;
        private SpriteFont _font;

        public OptionScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _font = _content.Load<SpriteFont>("Fonts/Font");
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(_font, "Game Options", new Vector2(Singleton.Instance.ScreenWidth / 2 - 100, 20), Color.White, 0f, new Vector2(0, 0), new Vector2(1), SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
