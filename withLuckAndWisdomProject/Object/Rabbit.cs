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
    public enum RabbitState
    {
        Start,
        Ready,
        Idle,
        ProjectileFlying,
        ProjectileHit,
        Ending,

        Falling,
        Aiming,
        Firing,
        Hit,
        Reset,
        Stalling
    }


    class Rabbit
    {
        // Create defualt variable
        private Texture2D _texture;
        private Vector2 _origin;

        private bool _isCollision;
        private Body _body;
        private Fixture _hittingFixture;
        private Texture2D _pencilDot;
        private Vector2 _relationPositon;

        private Point _dragStart, _dragEnd;
        private float _dragAngle, _dragLength;

        private float _worldForward;
        private float _moveForward;

        private Vector2 _projectile;
        List<Bamboo> _bamboos;
        private float _height, _width;
        private float scale;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)_body.Position.X, (int)_body.Position.Y, _texture.Width, _texture.Height);
            }
        }


        //Like a catapult state
        

        
        public bool Colliding { get; protected set; }

        private MouseState MousePrevious, MouseCurrent;
        private bool _isMouseDrag;
        public RabbitState RabbitState { get; set; }

        public Rabbit (Body body, List<Bamboo> bamboos)
        {
            _texture = ResourceManager.Rabbit;
    
            _body = body;
            _body.Mass = 500;
            _height = 100;
            scale = _height / (float)ResourceManager.Rabbit.Height;
            _width = ResourceManager.Rabbit.Width * scale;
            _bamboos = bamboos;
            _pencilDot = ResourceManager.Pencil;
            _isMouseDrag = false;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _body.OnCollision += CollisionHandler;

            _moveForward = 1400;
            RabbitState = RabbitState.Start;
        }

        public void update(GameTime gameTime)
        {
            // System.Diagnostics.Debug.WriteLine("Body = " + _body.LinearDamping);

            // Get mouse action
            MousePrevious = MouseCurrent;
            MouseCurrent = Mouse.GetState();


            // Get Mouse point 
            var mouseRectangle = new Rectangle(MouseCurrent.X + _texture.Width / 2, MouseCurrent.Y + _texture.Height / 2, 1, 1);



            if (RabbitState == RabbitState.Ready)
            {
                // Rabbit Draging 
                if (!_isMouseDrag && mouseRectangle.Intersects(Rectangle) && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released)
                {
                    _isMouseDrag = true;
                    _relationPositon = new Vector2(MouseCurrent.X - _body.Position.X, MouseCurrent.Y - _body.Position.Y);
                    _body.LinearVelocity = Vector2.Zero;
                    _dragStart = MouseCurrent.Position;

                }
                // Rabbit Releasing
                else if (_isMouseDrag && MouseCurrent.LeftButton == ButtonState.Released && MousePrevious.LeftButton == ButtonState.Pressed)
                {
                    _isMouseDrag = false;
                    _dragEnd = MouseCurrent.Position;

                    // Change State
                    RabbitState = RabbitState.ProjectileFlying;

                    // Add Vector and Sound
                    _body.LinearVelocity += _projectile;

                    // Sound when Jumping
                    AudioManager.PlaySound("Jumping2");
                }

                // finding projectile Line
                if (_isMouseDrag)
                {
                    _projectile = new Vector2(-1f * (MouseCurrent.X - _dragStart.X), -.5f * (MouseCurrent.Y - _dragStart.Y));

                }

                if (_isMouseDrag)
                {
                    // find angle of shooter
                    _dragAngle = (float)Math.Atan2(MouseCurrent.Y - _dragStart.Y, MouseCurrent.X - _dragStart.X);
                    _dragEnd = MouseCurrent.Position;

                    _dragLength = (float)Math.Sqrt((Math.Pow(MouseCurrent.X - _dragStart.X, 2) + Math.Pow(MouseCurrent.Y - _dragStart.Y, 2)));
                }
            }

            if (RabbitState == RabbitState.ProjectileHit)
            {
                ////_hittingBody.Awake = false;
                ///
                var h = (int)(Singleton.Instance.ScreenHeight - _hittingFixture.Body.Position.Y);
                _body.Position = new Vector2(_hittingFixture.Body.Position.X, _hittingFixture.Body.Position.Y - h - _height / 2);
                _body.LinearVelocity = Vector2.Zero;
                _worldForward = _hittingFixture.Body.Position.X - 250;

                RabbitState = RabbitState.Ready;
                System.Diagnostics.Debug.WriteLine(_body.Position.X + " " + _body.Position.Y);
                //if(_isCollision)
                //    _body.Position += new Vector2(0,10);
                //else 

                // Add Sound when hit the tree
                AudioManager.StopSound("Jumping2");
                AudioManager.PlaySound("ThreeHit");
            }
            _isCollision = false;

            if (_worldForward > 0)
            {
                float forwardStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * .5f);
                if (_isMouseDrag) forwardStep *= .5f;
                _body.World.ShiftOrigin(new Vector2(forwardStep, 0));
                _worldForward -= forwardStep;
                _moveForward += forwardStep;
            }

            if (_moveForward > 250)
            {
                System.Random random = new System.Random();
                float h = random.Next(200, 400);
                int BambooCenter = Singleton.Instance.ScreenHeight - (int)(h / 2);

                var Vertical = _bamboos[_bamboos.Count - 1].Body.Position.X + 250;
                _moveForward -= 250;
                var bodyBaboo = _body.World.CreateRectangle(5, h, 1f, new Vector2(Vertical, BambooCenter), 0f, BodyType.Static);
                _bamboos.Add(new Bamboo(h, bodyBaboo));

                for (int i = 0; i < _bamboos.Count; i++)
                {
                    if (_bamboos[i].Body.Position.X < 0)
                        _bamboos.RemoveAt(i);
                    else
                        break;
                }
            }

            if (_body.Position.Y > Singleton.Instance.ScreenHeight || _body.Position.X < 0)
            {
                RabbitState = RabbitState.Ending;
                this._body.Enabled = false;
            }



        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //spriteBatch.Draw(_pencilDot, body.Position,Color.White);

            // Draw Collision Box
            //spriteBatch.Draw(_pencilDot, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, (int)_width, (int)_height), null,
            //        Color.Pink, _body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);
            if (RabbitState != RabbitState.Ending)
            {
                if (_isMouseDrag)
                {
                    // Draw Draging
                    spriteBatch.Draw(_pencilDot, new Rectangle((int)_dragStart.X, (int)_dragStart.Y, 3, (int)_dragLength), null,
                        Color.Red, _dragAngle + MathHelper.ToRadians(-90f), Vector2.Zero, SpriteEffects.None, 0);

                    // Draw Pathing
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

            

        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            _isCollision = true;
            if (RabbitState == RabbitState.Start)
                   RabbitState = RabbitState.Ready;

            if(RabbitState == RabbitState.ProjectileFlying)
            {
                RabbitState = RabbitState.ProjectileHit;
                _hittingFixture = other;
            }

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
            return g * (.5f * time * time) + v0 * time + x0 ;
        }
    }
}
