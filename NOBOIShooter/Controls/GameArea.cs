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


        #endregion

        #region Methods

        public GameArea(GraphicsDevice graphicsDevice, int x, int y, int Width,int Height)
        {
            posX = x;
            posY = y;
            Position = new Vector2(x,y);
            width = Width;
            height = Height;

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whiteRectangle, new Rectangle(posX, posY,width, height), Color.Chocolate);
        }



        public override void Update(GameTime gameTime)
        {
            
        }

        #endregion
    }
}
