using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Object;
using tainicom.Aether.Physics2D.Dynamics;

namespace withLuckAndWisdomProject.Screens
{
    //Menu screen
    public class MenuScreen : AScreen
    {
        private List<Component> _components;
        private Texture2D button, mainBackground, logo;
        private SpriteFont font;
        private Button _playButton;
        private Button _testButton;
        private List<Ball> _balls;
        private World world;

        //Constructor inherit from base class
        public MenuScreen()
        {
            //load assets
            button = ResourceManager.button;
            mainBackground = ResourceManager.mainBackground;
            logo = ResourceManager.logo;

            font = ResourceManager.font;

            //world of physic
            world = new World();

            //set world Gravity
            world.Gravity = new Vector2(0, -9.82f);

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

            // Create sample Balls
            System.Random random = new System.Random();
            _balls = new List<Ball>();
            for (int i = 0; i < 20; i++)
            {
                var radius = random.Next(20, 40);
                var position = new Vector2(
                    random.Next(radius, Singleton.Instance.ScreenWidth - radius),
                    random.Next(radius, Singleton.Instance.ScreenHeight - radius)
                );
                var body = world.CreateCircle(radius, .1f, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-100, 100),
                    random.Next(-100, 100)
                    );
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                body.IgnoreGravity = true;
                _balls.Add(new Ball(radius, body));
            }

            // Create button on main screen
            _playButton = new Button(button, font)
            {
                PenColour = Color.DarkGreen,
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 100, Singleton.Instance.ScreenHeight / 4 * 3 - 100),
                Text = "Play",
            };

            _playButton.Click += PlayButtonOnClick;


            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playButton,
            };

        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen. 
            ScreenManager.ChangeScreen = "game";
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainBackground, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            spriteBatch.Draw(logo, new Rectangle(115, 0, 400, 350), Color.White);

            foreach (var ball in _balls)
                ball.Draw(gameTime, spriteBatch);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var ball in _balls)
                ball.Update(gameTime);

            foreach (Component component in _components)
                component.Update(gameTime);
            AudioManager.PlaySound("TestMusic2", true);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
