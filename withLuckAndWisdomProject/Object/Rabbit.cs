using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;

using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

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

        private Vector2 Position;
        private Body body;
        private Texture2D _pencilDot;
        public bool Colliding { get; protected set; }
        public Rabbit (Vector2 position, Body body)
        {
            _texture = ResourceManager.Rabbit;
            Position = position;
            this.body = body;
            _pencilDot = ResourceManager.Pencil;
            this.body.OnCollision += CollisionHandler;
        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //spriteBatch.Draw(_pencilDot, body.Position,Color.White);

            spriteBatch.Draw(_pencilDot, new Rectangle((int)body.Position.X, (int)body.Position.Y, _texture.Width, _texture.Height), null,
                    Color.Green, body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);

            //spriteBatch.Draw(_texture, body.Position, Color.White);
            //spriteBatch.Draw(_texture, new Rectangle((int)(body.Position.X - _texture.Width/2), (int)(body.Position.Y - _texture.Height/2), _texture.Width, _texture.Height), null,
            //        Color.White, body.Rotation,Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(_texture, body.Position, null,
                Color.White, body.Rotation + MathHelper.ToRadians(-90f), new Vector2(_texture.Width / 2, _texture.Height / 2),
                1f, SpriteEffects.None, 0f);



        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            contact.Restitution = 1f;
            Colliding = true;

            //must always return ture for apply physic after collision
            return true;
        }

    }
}
