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
    // Create state like a catapult state
    public enum RabbitState
    {
        Start,
        Ready,
        Idle,
        Aiming,
        ProjectileFlying,
        ProjectileHit,
        Ending,

        Falling,
        Firing,
        Hit,
        Reset,
        Stalling
    }


    class Rabbit
    {
        // Public variable
        public int Score { get; set; }
        public bool VisibleProjectile{ get; set; }
        public float Forwarding { get; set; }
        public float Forwarder { get; set; }
        public float ForwardLenght { get; set; }
        public Body Body { get; set; }

        public TimeSpan PlayTime { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Body.Position.X, (int)Body.Position.Y, _texture.Width, _texture.Height);
            }
        }

        private const int START_BAMBOO_CREATE_LENGTH = 1600;
        private const int MIN_HEIGHT_OF_BAMBOO = 200;
        private const int MAX_HEIGHT_OF_BAMBOO = 400;

        private const int MIN_DISTAN_OF_BAMBOO = 250;
        private const int MAX_DISTAN_OF_BAMBOO = 300;

        // Create defualt variable
        private Fixture _hitting;
        private Texture2D _pencilDot;
        private Texture2D _texture;
        private Vector2 _origin;
        private float _scale;
        private float _height, _width;

        // Create optional variable
        private Vector2 _projectile;
        private Point _dragStart, _dragEnd;
        private float _dragAngle, _dragLength;

        public bool IsCollision { get; set; }

        List<Bamboo> _bamboos;

        private MouseState MousePrevious, MouseCurrent;
        public RabbitState RabbitState { get; set; }

        public Rabbit (Body body, int height, List<Bamboo> bamboos)
        {
            Body = body;
            _height = height;
            _bamboos = bamboos;
            
            _texture = ResourceManager.Rabbit;
            _pencilDot = ResourceManager.Pencil;
            _scale = (float)height / ResourceManager.Rabbit.Height;
            _width = ResourceManager.Rabbit.Width * _scale;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            Score = 0;
            VisibleProjectile = true;
            Body.OnCollision += CollisionHandler;

            Forwarder = START_BAMBOO_CREATE_LENGTH;
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

            // Jumping Sound list
            String[] RandomSound = new string[] { "Jumping2", "Jumping3" };

            if (RabbitState == RabbitState.Ready)
            {

                // Rabbit Draging 
                if (mouseRectangle.Intersects(Rectangle) && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released)
                {
                    RabbitState = RabbitState.Aiming;
                    _dragStart = MouseCurrent.Position;

                }
            }
            if (RabbitState == RabbitState.Aiming) 
            { 
                // Rabbit Releasing
                if (MouseCurrent.LeftButton == ButtonState.Released && MousePrevious.LeftButton == ButtonState.Pressed)
                {
                    _dragEnd = MouseCurrent.Position;

                    // Change State
                    RabbitState = RabbitState.ProjectileFlying;

                    // Add Vector and Sound
                    Body.LinearVelocity += _projectile;

                    // Add Sound when jumping
                    Random randomSound = new Random();
                    int index = randomSound.Next(RandomSound.Length);
                    AudioManager.PlaySound(RandomSound[index]);
                    AudioManager.PlaySound("wind");
                }

                // finding projectile Line
                _projectile = new Vector2(-1f * (MouseCurrent.X - _dragStart.X), -.5f * (MouseCurrent.Y - _dragStart.Y));

                // find angle of shooter
                _dragAngle = (float)Math.Atan2(MouseCurrent.Y - _dragStart.Y, MouseCurrent.X - _dragStart.X);
                _dragEnd = MouseCurrent.Position;
                _dragLength = (float)Math.Sqrt((Math.Pow(MouseCurrent.X - _dragStart.X, 2) + Math.Pow(MouseCurrent.Y - _dragStart.Y, 2)));
                    
            }

            if (RabbitState == RabbitState.ProjectileFlying)
            {
                int limitRight = Singleton.Instance.ScreenWidth - 250;
                if(Body.Position.X > limitRight)
                {
                    Forwarding = Body.Position.X - limitRight;
                }

            }


            if (RabbitState == RabbitState.ProjectileHit)
            {
                // Calculate after hitting
                var centerBamboo = (int)(Singleton.Instance.ScreenHeight - _hitting.Body.Position.Y);
                var topBamboo = new Vector2(_hitting.Body.Position.X, _hitting.Body.Position.Y - centerBamboo - _height / 2);
                var lenght2top = (int) (Body.Position.Y - topBamboo.Y);
                if (lenght2top < 150)
                {
                    Body.Position = topBamboo;
                    Score += (100 - lenght2top) * 10;
                    Body.LinearVelocity = Vector2.Zero;
                }

                // Adding world move forward
                Forwarding = _hitting.Body.Position.X - 250;

                RabbitState = RabbitState.Ready;
                
                // Add Sound when hit the tree
                foreach(String st in RandomSound) {
                    AudioManager.StopSound(st);    
                }
                AudioManager.StopSound("wind");
                AudioManager.PlaySound("ThreeHit");
            }
            if (RabbitState != RabbitState.Ending)
            {
                PlayTime += gameTime.ElapsedGameTime;
                if (Body.Position.Y > Singleton.Instance.ScreenHeight || Body.Position.X < 0)
                {
                    RabbitState = RabbitState.Ending;
                    Body.Enabled = false;
                }
            }


            if (Forwarding > 0)
            {
                float forwardStep = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * .5f);
                if (RabbitState == RabbitState.Aiming) forwardStep *= .5f;
                Body.World.ShiftOrigin(new Vector2(forwardStep, 0));
                Forwarding -= forwardStep;
                Forwarder += forwardStep;
                ForwardLenght += forwardStep;
            }

            if (Forwarder > 250)
            {
                RandomNextBamboo();

                for (int i = 0; i < _bamboos.Count; i++)
                {
                    if (_bamboos[i].Body.Position.X < 0)
                        _bamboos.RemoveAt(i);
                    else
                        break;
                }
            }
            
            
            

            IsCollision = false;
        }

        

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // Draw Collision Box
            //spriteBatch.Draw(_pencilDot, new Rectangle((int)Body.Position.X, (int)Body.Position.Y, (int)_width, (int)_height), null,
            //        Color.Pink, Body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);


            if (RabbitState != RabbitState.Ending)
            {
                if (RabbitState == RabbitState.Aiming)
                {
                    // Draw Draging
                    
                    spriteBatch.Draw(_pencilDot, new Rectangle((int)_dragStart.X, (int)_dragStart.Y, 3, (int)_dragLength), null,
                        Color.Red, _dragAngle + MathHelper.ToRadians(-90f), Vector2.Zero, SpriteEffects.None, 0);

                    // Draw Pathing
                    if (VisibleProjectile)
                        DrawProjectile(spriteBatch);
                }

                //spriteBatch.Draw(_texture, _body.Position, Color.White);
                spriteBatch.Draw(_texture, Body.Position, null, Color.White, Body.Rotation, _origin, _scale, SpriteEffects.None, 0f);

                //DrawLine(new Vector2(200,200), new Vector2(400, 400) , spriteBatch);
            }
        }

        private void DrawProjectile (SpriteBatch spriteBatch)
        {
            int i = 0;
            while (true)
            {
                float t0 = .1f * i;
                float t1 = .1f * (i + 1);
                Vector2 x0 = PredictProjectileAtTime(t0, _projectile, Body.Position, Body.World.Gravity);
                Vector2 x1 = PredictProjectileAtTime(t1, _projectile, Body.Position, Body.World.Gravity);
                DrawLine(x0, x1, spriteBatch, Color.LightGreen);
                if (x1.Y > Singleton.Instance.ScreenHeight) break;
                i++;
            }
        }

        private void RandomNextBamboo()
        {
            System.Random random = new System.Random();
            float nextHeight = random.Next(MIN_HEIGHT_OF_BAMBOO, MAX_HEIGHT_OF_BAMBOO);
            int BambooCenter = Singleton.Instance.ScreenHeight - (int)(nextHeight / 2);

            float nextDistan = random.Next(MIN_DISTAN_OF_BAMBOO, MAX_DISTAN_OF_BAMBOO);
            var Vertical = _bamboos[_bamboos.Count - 1].Body.Position.X + nextDistan;
            Forwarder -= nextDistan;
            var bodyBaboo = Body.World.CreateRectangle(5, nextHeight, 1f, new Vector2(Vertical, BambooCenter), 0f, BodyType.Static);

            var nextType = random.NextDouble() > .2f ?
                BambooType.Normal :
                    random.NextDouble() > .4f ?
                          BambooType.Friable :
                          BambooType.Pandle;

            _bamboos.Add(new Bamboo(nextHeight, nextType, bodyBaboo));
        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            IsCollision = true;
            if (RabbitState == RabbitState.Start)
                RabbitState = RabbitState.Ready;

            if(RabbitState == RabbitState.ProjectileFlying)
            {
                RabbitState = RabbitState.ProjectileHit;
                _hitting = other;
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
