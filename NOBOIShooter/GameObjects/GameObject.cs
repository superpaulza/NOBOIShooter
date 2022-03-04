using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.GameObjects;

namespace NOBOIShooter
{
    //Every game object must inherit from this class
    public class GameObject
    {
        protected Texture2D _texture;

        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        
        

        public string Name;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int) Position.X, (int) Position.Y, _texture.Width, _texture.Height);
            }
        }

        //Constructor
        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
        }

        //Virtual method (parent class) is only accept "override" from child class 
        public virtual void Update(GameTime gameTime, Bubble[,] gameObjects)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Reset()
        {

        }
    }
}
