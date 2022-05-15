using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;

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

        public Rabbit ()
        {
            _texture = ResourceManager.Rabbit;
            Position = new Vector2(100, 200);
        }

        public void update(GameTime gameTime)
        {

        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

    }
}
