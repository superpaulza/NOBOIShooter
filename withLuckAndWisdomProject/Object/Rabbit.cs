using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;

using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using Microsoft.Xna.Framework.Input;

namespace withLuckAndWisdomProject.Object
{
    //Like a catapult state
    public enum RabbitState
    {
        Idle,
        Aiming,
        Firing,
        ProjectileFlying,
        ProjectileHit,
        Hit,
        Reset,
        Stalling
    }

    
    class Rabbit
    {
        // Create defualt variable
        private Texture2D _texture;
        private Rectangle _ractangle;
        private Rectangle _srcRact;
        private float _rotation;
        private float _radian;
        private Vector2 _origin;

        private bool _isClicked, _isReleased, _isCollision;
        private Body _body;
        private Texture2D _pencilDot;
        private Vector2 _relationPositon;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_body.Position.X, (int)_body.Position.Y, _texture.Width, _texture.Height);
            }
        }
        public bool Colliding { get; protected set; }

        private MouseState MousePrevious, MouseCurrent;
        private bool _isMouseDrag;

        public Rabbit (Body body)
        {
            _texture = ResourceManager.Rabbit;
    
            _body = body;
            _body.Mass = 50000f;
            _pencilDot = ResourceManager.Pencil;
            _isMouseDrag = false;
            
            _body.OnCollision += CollisionHandler;
        }

        public void update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine("Rotation = " + body.Rotation);

            // Get mouse action
            MousePrevious = MouseCurrent;
            MouseCurrent = Mouse.GetState();



            var mouseRectangle = new Rectangle(MouseCurrent.X + _texture.Width / 2, MouseCurrent.Y + _texture.Height/2, 1, 1);
            
            if (!_isMouseDrag && mouseRectangle.Intersects(Rectangle) && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released)
            {
                _isMouseDrag = true;
                _relationPositon = new Vector2(MouseCurrent.X - _body.Position.X, MouseCurrent.Y - _body.Position.Y);
                _body.LinearVelocity = Vector2.Zero;
                
            }
            else if (_isMouseDrag && MouseCurrent.LeftButton == ButtonState.Released && MousePrevious.LeftButton == ButtonState.Pressed)
            {
                _isMouseDrag = false;
                
                _body.LinearVelocity = new Vector2(0f, 1200f);
            }


            if (_isMouseDrag)
            {
                _body.Position = new Vector2(MouseCurrent.X, MouseCurrent.Y) - _relationPositon;
            }

            if (_isCollision)
            {
                _body.BodyType = BodyType.Static;
                
            } 
            else
            {
                _body.BodyType = BodyType.Dynamic;
            }
            
            _isCollision = false;



        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //spriteBatch.Draw(_pencilDot, body.Position,Color.White);

            spriteBatch.Draw(_pencilDot, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, _texture.Width, _texture.Height), null,
                    Color.Pink, _body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);

            //spriteBatch.Draw(_texture, body.Position, Color.White);
            //spriteBatch.Draw(_texture, new Rectangle((int)(body.Position.X - _texture.Width/2), (int)(body.Position.Y - _texture.Height/2), _texture.Width, _texture.Height), null,
            //        Color.White, body.Rotation,Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(_texture, _body.Position, null,
                Color.White, _body.Rotation, new Vector2(_texture.Width / 2, _texture.Height / 2),
                1f, SpriteEffects.None, 0f);



        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            _isCollision = true;

            //must always return ture for apply physic after collision
            return true;
        }

    }
}
