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

        private float _height,_width;
        private float scale;

        private Vector2 _origin;

        private enum BambooState
        {
            Normal,
            Burning,
            Friable,
            Slip,
        } 
        

        public Bamboo(float height , Body body)
        {
            _texture = ResourceManager.Bamboo;
            _body = body;
            scale = height /(float) ResourceManager.Bamboo.Height;
            _height = height;
            _width = ResourceManager.Bamboo.Width * scale;

            _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _status = BambooState.Normal;
            //System.Diagnostics.Debug.WriteLine(_body.Position.X + " " + _body.Position.Y);
        }

        public void LoadContent()
        {
            

        }

        public void update(GameTime gameTime)
        {
            
        }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
                       
            //spriteBatch.Draw(_texture, _body.Position, Color.White);
            spriteBatch.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0f);

            //spriteBatch.Draw(ResourceManager.Pencil, new Rectangle((int)_body.Position.X, (int)_body.Position.Y, (int)_width, (int)_height), null,
            //    Color.Green, _body.Rotation, new Vector2(.5f,.5f), SpriteEffects.None, 0);

            //.Draw(_texture, _body.Position, null, Color.White, _body.Rotation, _origin, scale, SpriteEffects.None, 0);
        }
    }
}
