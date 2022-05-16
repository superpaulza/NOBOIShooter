using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Object;
using tainicom.Aether.Physics2D.Dynamics;

namespace withLuckAndWisdomProject.Screens
{
    class TestPhysicScreen : AScreen
    {
        private List<Ball> balls;
        private World world;

        public TestPhysicScreen()
        {
            //world of physic
            world = new World();
            world.Gravity = new Vector2(0, 80f);

            //Create game border (set edges line foreach edge)
            var top = 0;
            var bottom = Singleton.Instance.ScreenHeight; //720
            var left = 0;
            var right = Singleton.Instance.ScreenWidth; //1280
            var edges = new Body[]
            {
                world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
                world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom)),
                world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
                world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom))
            };

            foreach (var edge in edges)
            {
                edge.BodyType = BodyType.Static;
                // edge.SetRestitution(1);
            }

            System.Random random = new System.Random();
            balls = new List<Ball>();
            for (int i = 0; i < 20; i++)
            {
                var radius = random.Next(30, 50);
                var position = new Vector2(
                    random.Next(radius, Singleton.Instance.ScreenWidth - radius),
                    random.Next(radius, Singleton.Instance.ScreenHeight - radius)
                );
                var body = world.CreateCircle(radius, 1f, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-1000, 1000),
                    random.Next(-1000, 1000)
                    );
                body.AngularVelocity = (float) random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                balls.Add(new Ball(radius, body));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var ball in balls)
            {
                ball.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime, bool isActive)
        {
            foreach (var ball in balls)
            {
                ball.Update(gameTime);
            }
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
