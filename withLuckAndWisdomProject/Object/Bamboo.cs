using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using withLuckAndWisdomProject.Screens;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace withLuckAndWisdomProject.Object
{
    public enum BambooType
    {
        Normal,
        Burning,
        Friable,
        Pandle,
        Slip,
    }
    public enum BambooState
    {
        Living,
        Falling,
        Pankick
    }

    class Bamboo
    {
        public Body Body { get; set; }

        private const int SET_WIDTH_OF_BAMBOOS = 10;
        private const int SET_HEIGHT_OF_JOINT = 50;

        private BambooType _type;
        private BambooState _state;
        private Texture2D _joint1texture;
        private Texture2D _joint2texture;
        private Texture2D _leaftexture;
        private float _height;
        private float _width;
        private bool _drawLeaf;

        private List<Point> _joint;
        private List<float> _leaf;
        private int _jointHeight;
        private Vector2 _pandaPosition;

        private Body _hitter;

        public Bamboo(float height, BambooType bambooType, Body bambooBody)
        {
            _height = height;
            Body = bambooBody;
            _type = bambooType;

            _joint1texture = ResourceManager.BambooJoint1;
            _joint2texture = ResourceManager.BambooJoint2;
            _leaftexture = ResourceManager.BambooLeaf;

            _jointHeight = SET_HEIGHT_OF_JOINT;
            _width = SET_WIDTH_OF_BAMBOOS;
            _drawLeaf = false;

            _state = BambooState.Living;
            _joint = new List<Point>();
            for (int i = 0; i < height ; i+= _jointHeight)
                _joint.Add(new Point(0, i));

            Random random = new Random();
            _leaf = new List<float>();
            for (int i = 0; i < _joint.Count; i ++)
            {
                _leaf.Add( MathHelper.ToRadians(i % 2 == 0 ? random.Next(-60, -30) : random.Next(30,60) ) );

            }

            if (bambooType == BambooType.Pandle)
            {
                _pandaPosition = new Vector2(-_width /2  -ResourceManager.Panda.Width, -height/2);
            }

            Body.OnCollision += CollisionHandler;
        }

        public void LoadContent()
        {
            

        }

        public void update(GameTime gameTime)
        {
            // Update game in some state
            if (_state == BambooState.Falling)
                Body.BodyType = BodyType.Dynamic;

            if (_state == BambooState.Pankick)
            {
                _hitter.LinearVelocity += new Vector2(-50, 0);
                _state = BambooState.Living;
                AudioManager.PlaySound("fall");
            }
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw conlison
            //spriteBatch.Draw(ResourceManager.Pencil, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, (int)_width, (int)_height), null,
            //    Color.Lime, _body.Rotation, new Vector2(.5f, .5f), SpriteEffects.None, 0);

            if(_drawLeaf && _type != BambooType.Friable)
                for (int i = 0; i < _leaf.Count; i++)
                    spriteBatch.Draw(_leaftexture, new Rectangle((int)Body.Position.X + _joint[i].X, (int)(Body.Position.Y - _height / 2 + _joint[i].Y), (int)_width, (int)_jointHeight), null,
                          Color.LightGreen, _leaf[i], new Vector2(0, _leaftexture.Height), SpriteEffects.None, 0);

            if (_type == BambooType.Normal)
            {
                // Draw basic bamboo
                spriteBatch.Draw(ResourceManager.BambooShoot, new Rectangle((int)Body.Position.X , (int)(Body.Position.Y - _height / 2 - _jointHeight / 2), (int)_width, (int)_jointHeight), null,
                         Color.LightGreen, Body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
                

                for (int i = 0; i < _joint.Count; i++)
               
                    spriteBatch.Draw(i % 2 == 0 ? _joint1texture : _joint2texture, new Rectangle((int)Body.Position.X + _joint[i].X, (int)(Body.Position.Y - _height/2 + _jointHeight/2  + _joint[i].Y), (int)_width, (int)_jointHeight), null,
                         i < 2 ? Color.LightGreen : Color.Green, Body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
               
            }

            if (_type == BambooType.Friable)
            {
                // Draw dead bamboo
                for (int i = 0; i < _joint.Count; i++)
                {
                    spriteBatch.Draw(i % 2 == 0 ? _joint1texture : _joint2texture, new Rectangle((int)Body.Position.X + _joint[i].X, (int)(Body.Position.Y - _height / 2 + _jointHeight / 2 + _joint[i].Y), (int)_width, (int)_jointHeight), null,
                         Color.Gold, Body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
                }
            }

            if (_type == BambooType.Pandle)
            {
                // Draw panda bamboo
                for (int i = 0; i < _joint.Count; i++)
                {
                    spriteBatch.Draw(i % 2 == 0 ? _joint1texture : _joint2texture, new Rectangle((int)Body.Position.X + _joint[i].X, (int)(Body.Position.Y - _height / 2 + _jointHeight / 2 + _joint[i].Y), (int)_width, (int)_jointHeight), null,
                         Color.Green, Body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
                }

                // Draw panda
                spriteBatch.Draw(ResourceManager.Panda,Body.Position + _pandaPosition,Color.White);
                //spriteBatch.Draw(ResourceManager.Panda, 
                //    new Rectangle((int)Body.Position.X - ResourceManager.Panda.Width, (int)Body.Position.Y - ResourceManager.Panda.Height, ResourceManager.Panda.Width, ResourceManager.Panda.Height),
                //    null,Color.White, Body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
            }

        }

        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            // Change state after collision
            if (_type == BambooType.Friable)
                _state = BambooState.Falling;

            if (_type == BambooType.Pandle)
                _state = BambooState.Pankick;

            // sent hitting body
            _hitter = other.Body;

            //must always return ture for apply physic after collision
            return true;
        }
    }
}
