using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using NOBOIShooter.GameObjects;
using System;

namespace NOBOIShooter.States
{
    public class LeaderboardState : State
    {
        private Texture2D _bg;
        private SpriteFont _font;

        public LeaderboardState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _bg = _content.Load<Texture2D>("Backgrounds/wild-west");
            _font = _content.Load<SpriteFont>("Fonts/Font");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Drawing Sprite Batch.
            //spriteBatch.Draw(_bg, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(_font, "Leaderboard", new Vector2(300, 10), Color.Black);

            spriteBatch.End();
        }
    }
}
