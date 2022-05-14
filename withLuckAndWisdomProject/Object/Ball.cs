using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Screens;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace withLuckAndWisdomProject.Object
{
    class Ball
    {
        // private variables
        Texture2D texture;
        float radius;
        float scale;
        Vector2 origin;
        private Body body;

        // A boolean indicating if this ball is colliding with another
        public bool Colliding { get; protected set; }

        public Ball(float radius, Body body)
        {
            this.body = body;
            this.radius = radius;
            scale = radius / 49;
            origin = new Vector2(49, 49);

            // Loads the ball's texture
            texture = ResourceManager.ball;

            //When ball OnCollision (CollisionHandler)
            this.body.OnCollision += CollisionHandler;
        }

        // Updates the ball
        // gameTime: An object representing time in the game
        public void Update(GameTime gameTime)
        {
            // Clear the colliding flag 
            Colliding = false;
        }

        // Draws the ball using the provided spritebatch
        // spriteBatch: The spritebatch to render with
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Use Green for visual collision indication
            Color color = (Colliding) ? Color.Red : Color.White;

            spriteBatch.Draw(texture, body.Position, null, color, body.Rotation, origin, scale, SpriteEffects.None, 0);
        }

        //Physic collision handler
        bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            contact.Restitution = 1f;
            Colliding = true;

            //must always return ture for apply physic after collision
            return true;
        }
    }
}
