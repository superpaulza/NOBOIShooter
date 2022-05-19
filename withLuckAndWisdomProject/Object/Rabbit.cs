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
        private RabbitStatus _rabbitStatus;
        public enum RabbitStatus
        {
            Start,
            Ready,
            Jumping,
            Falling
        }

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

            _rabbitStatus = RabbitStatus.Start;
        }

        public void update(GameTime gameTime)
        {
            //System.Diagnostics.Debug.WriteLine("Body = " + _body.LinearDamping);

            // Get mouse action
            MousePrevious = MouseCurrent;
            MouseCurrent = Mouse.GetState();



            var mouseRectangle = new Rectangle(MouseCurrent.X + _texture.Width / 2, MouseCurrent.Y + _texture.Height/2, 1, 1);

            // Rabbit Physics 
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
                
                _body.LinearVelocity  += _projectile;
                AudioManager.PlaySound("Re");
                //System.Diagnostics.Debug.WriteLine((MouseCurrent.X - _dragStart.X) + " " + (MouseCurrent.Y - _dragStart.Y));
            }

            if (_isMouseDrag)
            {
                _projectile  = new Vector2( 1000f * - 1f *(MouseCurrent.X - _dragStart.X), 1000f * -1f * (MouseCurrent.Y - _dragStart.Y));
                //_body.Position = new Vector2(MouseCurrent.X, MouseCurrent.Y) - _relationPositon;
            }

            
            if (_isCollision)
            {
                if (_rabbitStatus == RabbitStatus.Start)
                    _rabbitStatus = RabbitStatus.Ready;

                if (_rabbitStatus == RabbitStatus.Ready)
                    _rabbitStatus = RabbitStatus.Falling;

                _body.BodyType = BodyType.Static;
                
            } 
            else
            {
                _rabbitStatus = RabbitStatus.Jumping;
                _body.BodyType = BodyType.Dynamic;
                //_body.LinearVelocity += new Vector2(0, 100000);
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

                
                for (int i = 0; i < 100; i++)
                {
                    float t0 = .1f * (float) i;
                    float t1 = .1f * (float) (i+1);
                    Vector2 x0 = PredictProjectileAtTime(t0, _projectile, _body.Position , _body.World.Gravity);
                    Vector2 x1 = PredictProjectileAtTime(t1, _projectile, _body.Position , _body.World.Gravity);
                    DrawLine(x0, x1, spriteBatch, Color.LightGreen);
                }
            }

            //spriteBatch.Draw(_texture, _body.Position, Color.White);
            spriteBatch.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0f);

            //DrawLine(new Vector2(200,200), new Vector2(400, 400) , spriteBatch);



        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            _isCollision = true;
            
            //must always return ture for apply physic after collision
            return true;
        }

        void DrawLine (Vector2 p1, Vector2 p2, SpriteBatch spriteBatch, Color color)
        {
            var angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
            var length = (float)Math.Sqrt((Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)));
            spriteBatch.Draw(_pencilDot, new Rectangle((int)p1.X, (int)p1.Y, 3, (int)length), null,
                    color, angle + MathHelper.ToRadians(-90f), Vector2.Zero, SpriteEffects.None, 0);
        }

        Vector2 PredictProjectileAtTime (float time, Vector2 v0, Vector2 x0, Vector2 g)
        {
            return g * (.5f * time * time) + v0* .001f * time + x0 ;
        }
    }
}
