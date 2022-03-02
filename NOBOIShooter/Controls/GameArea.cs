using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NOBOIShooter.Controls
{
    public class GameArea : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Rectangle mouseRectangle;

        private Color background;

        private int posX;

        private int posY;

        private int width;

        private int height;

        Texture2D whiteRectangle;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Vector2 Position { get; set; }

        private int[,] bubble = new int[20, 8];
        #endregion

        #region Methods

        Vector2 gunPoint;

        public GameArea(GraphicsDevice graphicsDevice, int x, int y, int Width,int Height)
        {
            posX = x;
            posY = y;
            Position = new Vector2(x,y);
            width = Width;
            height = Height;

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            gunPoint = new Vector2(posX + width / 2 - 20, posY + height - 20);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whiteRectangle, new Rectangle(posX, posY,width, height), Color.Chocolate);
            int size = 40;
            for (int y = 0; y < 20; y++) {
                for (int x = 0; x < 8; x++) { 
                    if(x % 2 == 0)
                        spriteBatch.Draw(whiteRectangle, new Rectangle(posX+y*size, posY+x * size, size-2, size-2), Color.Pink);
                    else if(y != 19) spriteBatch.Draw(whiteRectangle, new Rectangle(posX + y * size + 20, posY + x * size, size - 2, size - 2), Color.Green);
                }
            }

            spriteBatch.Draw(whiteRectangle, new Rectangle((int)gunPoint.X , (int)gunPoint.Y , size - 2, size - 2), Color.Red);
            //spriteBatch.Draw(whiteRectangle, new Vector2(posX, posY), null, Color.Red, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(whiteRectangle, new Vector2(posX, posY), null, Color.Red, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }



        public override void Update(GameTime gameTime)
        {
            
            
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
            int x = mouse.X , y = mouse.Y;

            // Get the mouse angle
            var mouseangle = radToDeg(Math.Atan2(gunPoint.X - y,x - gunPoint.Y));

            // Convert range to 0, 360 degrees
            if (mouseangle < 0)
            {
                mouseangle = 180 + (180 + mouseangle);
            }

            // (...)

            // Set the player angle
            return mouseangle;
        }


        #endregion
    }
}
