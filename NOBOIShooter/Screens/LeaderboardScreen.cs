using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
namespace NOBOIShooter.Screens
{
    public class LeaderboardScreen : AScreen
    {
        private Texture2D _background;
        private SpriteFont _font;

        public LeaderboardScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _font = _content.Load<SpriteFont>("Fonts/Font");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //Screen Drawing Area. 
            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(_font, "Leaderboard", new Vector2(1000, 100), Color.Black);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
