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
        Texture2D whiteRectangle;
        private GameArea gameArea;
        private Texture2D BackImage;
        private Button BackButton;

        public GameState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            myText = _content.Load<SpriteFont>("Fonts/Font");
            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            gameArea = new GameArea(graphicsDevice,30, 30, 800, 650);
            BackImage = _content.Load<Texture2D>("Controls/BackButton");

            BackButton = new Button(BackImage)
            {
                Position = new Vector2(1200, 20),
            };

            BackButton.Click += BackButton_Click;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BackButton.Draw(gameTime, spriteBatch);
            gameArea.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(whiteRectangle, new Rectangle(40, 40, 40, 40), Color.Pink);
            spriteBatch.DrawString(myText, "Can u see me? \n sorry It's too white!", new Vector2(900, 100), Color.Black);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            BackButton.Update(gameTime);
        }
    }
}
