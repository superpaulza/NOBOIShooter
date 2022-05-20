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

namespace withLuckAndWisdomProject.Object
{
    class Bamboo
    {
        private Body _body;
        private BambooState _status;
        private Texture2D _texture;
        private Texture2D _joint1texture, _joint2texture;

        private float _height,_width;
        private float scale;

        private Vector2 _origin;
        private List<Point> _joint;
        private int _jointHeight;

        private enum BambooState
        {
            Normal,
            Burning,
            Friable,
            Slip,
        } 
        

        public Bamboo(float height, Body body)
        {
            _texture = ResourceManager.Bamboo;
            _joint1texture = ResourceManager.BambooJoint1;
            _joint2texture = ResourceManager.BambooJoint2;
            _body = body;
            scale = height /(float) ResourceManager.Bamboo.Height;
            _height = height;
            _width = 10;
            _jointHeight = 50;
            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _status = BambooState.Normal;
            //System.Diagnostics.Debug.WriteLine(_body.Position.X + " " + _body.Position.Y);
            _joint = new List<Point>();
            for (int i = 0; i < height ; i+= _jointHeight)
            {
                _joint.Add(new Point(0, i));
            }
        }

        public void LoadContent()
        {
            

        }

        public void update(GameTime gameTime)
        {
            
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw conlison
            //spriteBatch.Draw(ResourceManager.Pencil, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, (int)_width, (int)_height), null,
            //    Color.Lime, _body.Rotation, new Vector2(.5f, .5f), SpriteEffects.None, 0);

         
            for (int i = 0; i < _joint.Count; i++)
            {
                spriteBatch.Draw(i % 2 == 0 ? _joint1texture : _joint2texture, new Rectangle((int)_body.Position.X + _joint[i].X, (int)(_body.Position.Y - _height/2 + _jointHeight/2  + _joint[i].Y), (int)_width, (int)_jointHeight), null,
                     Color.White, _body.Rotation, new Vector2(_joint1texture.Width / 2, _joint1texture.Height / 2), SpriteEffects.None, 0);
            }
            
            //spriteBatch.Draw(_texture, _body.Position, Color.White);
            //spriteBatch.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0f);



            //.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0);
        }
    }
}
