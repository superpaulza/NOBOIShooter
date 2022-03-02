using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.Controls;
using System;

namespace NOBOIShooter.States
{
    //Game screen
    public class GameState : State
    {
        private SpriteFont myText;
        private Texture2D BackImage;
        private Button BackButton;

        public GameState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            myText = _content.Load<SpriteFont>("Fonts/Font");
            BackImage = _content.Load<Texture2D>("Controls/BackButton");

            BackButton = new Button(BackImage)
            {
                Position = new Vector2(100, 100),
            };

            BackButton.Click += BackButton_Click;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        private void BackButton_Onclick(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //BackButton.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(myText, "Can u see me? \n sorry It's too white!", new Vector2(300,300), Color.Black);
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
