using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NOBOIShooter.Controls;
using System;

namespace NOBOIShooter.States
{
    //Game screen
    public class GameState : State
    {
        private SpriteFont myText;
        Texture2D whiteRectangle;
        private Texture2D BackImage;
        private Button BackButton;
        private int posX, posY;
        private Vector2 Position, gunPoint;
        private int width, height;
        Texture2D ballBubble;

        public GameState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            myText = _content.Load<SpriteFont>("Fonts/Font");
            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            BackImage = _content.Load<Texture2D>("Controls/BackButton");
            ballBubble = _content.Load<Texture2D>("Controls/bubble");
            BackButton = new Button(BackImage)
            {
                Position = new Vector2(1200, 20),
            };

            posX = 30;
            posY = 30;
            Position = new Vector2(posX, posY);
            width = 800;
            height = 650;

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            gunPoint = new Vector2(posX + width / 2 - 20, posY + height - 20);


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
            spriteBatch.Draw(whiteRectangle, new Rectangle(posX, posY, width, height), Color.Chocolate);
            int size = 40;
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (x % 2 == 0)
                        spriteBatch.Draw(ballBubble, new Rectangle(posX + y * size, posY + x * size, size - 2, size - 2), Color.Pink);
                    else if (y != 19) spriteBatch.Draw(ballBubble, new Rectangle(posX + y * size + 20, posY + x * size, size - 2, size - 2), Color.Green);
                }
            }

            spriteBatch.Draw(whiteRectangle, new Rectangle((int)gunPoint.X, (int)gunPoint.Y, size - 2, size - 2), Color.Red);
            //spriteBatch.Draw(whiteRectangle, new Vector2(posX, posY), null, Color.Red, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(whiteRectangle, new Vector2(posX, posY), null, Color.Red, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);

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

        private double radToDeg(double angle)
        {
            return angle * (180 / Math.PI);
        }

        // On mouse movement
        private double getShooterAngle()
        {
            // Get the mouse position
            MouseState mouse = Mouse.GetState();
            int x = mouse.X, y = mouse.Y;

            // Get the mouse angle
            var mouseangle = radToDeg(Math.Atan2(gunPoint.X - y, x - gunPoint.Y));

            // Convert range to 0, 360 degrees
            if (mouseangle < 0)
            {
                mouseangle = 180 + (180 + mouseangle);
            }

            // (...)

            // Set the player angle
            return mouseangle;
        }
    }
}
