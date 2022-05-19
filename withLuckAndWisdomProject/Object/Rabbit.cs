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

        private bool _isCollision;
        private Body _body;
        private Texture2D _pencilDot;
        private Vector2 _relationPositon;

        private Point _dragStart, _dragEnd;
        private float _dragAngle, _dragLength;

        private bool _isTrolling;
        private Vector2 _projectile;

        private float _height, _width;
        private float scale;

        
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
            _body.Mass = 500;
            _height = 50;
            scale = _height / (float)ResourceManager.Rabbit.Height;
            _width = ResourceManager.Rabbit.Width * scale;

            _pencilDot = ResourceManager.Pencil;
            _isMouseDrag = false;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _body.OnCollision += CollisionHandler;
        }

        public void update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine("Body = " + _body.LinearDamping);

            // Get mouse action
            MousePrevious = MouseCurrent;
            MouseCurrent = Mouse.GetState();



            var mouseRectangle = new Rectangle(MouseCurrent.X + _texture.Width / 2, MouseCurrent.Y + _texture.Height/2, 1, 1);
            
            if (!_isMouseDrag && mouseRectangle.Intersects(Rectangle) && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released)
            {
                _isMouseDrag = true;
                _relationPositon = new Vector2(MouseCurrent.X - _body.Position.X, MouseCurrent.Y - _body.Position.Y);
                _body.LinearVelocity = Vector2.Zero;
                _dragStart = MouseCurrent.Position;
        

            }
            else if (_isMouseDrag && MouseCurrent.LeftButton == ButtonState.Released && MousePrevious.LeftButton == ButtonState.Pressed)
            {
                _isMouseDrag = false;
                _dragEnd = MouseCurrent.Position;
                _projectile  = new Vector2( (float)Math.Pow(10f, 4.5f) * - 1f *(MouseCurrent.X - _dragStart.X), (float)Math.Pow(10f, 4.5f) * -1f * (MouseCurrent.Y - _dragStart.Y));
                _body.LinearVelocity  += _projectile;
                AudioManager.PlaySound("Re");
                //System.Diagnostics.Debug.WriteLine((MouseCurrent.X - _dragStart.X) + " " + (MouseCurrent.Y - _dragStart.Y));
            }

            if (_isMouseDrag)
            {
                //_body.Position = new Vector2(MouseCurrent.X, MouseCurrent.Y) - _relationPositon;
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

            if (_isMouseDrag)
            {
                // find angle of shooter
                _dragAngle = (float) Math.Atan2(MouseCurrent.Y - _dragStart.Y, MouseCurrent.X - _dragStart.X);
                _dragEnd = MouseCurrent.Position;
                
                _dragLength = (float) Math.Sqrt((Math.Pow(MouseCurrent.X - _dragStart.X, 2) + Math.Pow(MouseCurrent.Y - _dragStart.Y, 2)));
            }



        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //spriteBatch.Draw(_pencilDot, body.Position,Color.White);

            //spriteBatch.Draw(_pencilDot, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, (int)_width, (int)_height), null,
            //        Color.Pink, _body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);

            if (_isMouseDrag)
            {
                // Draw Aimer
                spriteBatch.Draw(_pencilDot, new Rectangle((int)_dragStart.X, (int)_dragStart.Y, 3, (int)_dragLength), null,
                    Color.Red, _dragAngle + MathHelper.ToRadians(-90f), Vector2.Zero, SpriteEffects.None, 0);
            }

            //spriteBatch.Draw(_texture, _body.Position, Color.White);
            spriteBatch.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0f);



        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            _isCollision = true;
            contact.Restitution = 1f;
            //must always return ture for apply physic after collision
            return true;
        }

    }
}
