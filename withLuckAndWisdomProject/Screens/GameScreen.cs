using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Object;
using tainicom.Aether.Physics2D.Dynamics; 

namespace withLuckAndWisdomProject.Screens
{
    class GameScreen : AScreen
    {
        private Button _backButton;
        private Rabbit testRabbit;
        private World world;
        private List<Bamboo> _bamboos;
        private List<Ball> _balls;
        List<Component> _components;
        //Constructor inherit from base class 
        public GameScreen()
        {
            
    

            //world of physic
            world = new World();
            world.Gravity = new Vector2(0, world.Gravity.Y * -1);
                        
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
                var body = world.CreateCircle(radius, 1f, position, BodyType.Dynamic);
                body.LinearVelocity = new Vector2(
                    random.Next(-100, 100),
                    random.Next(-100, 100)
                    );
                body.AngularVelocity = (float)random.NextDouble() * MathHelper.Pi - MathHelper.PiOver2;
                _balls.Add(new Ball(radius, body));
            }
            
            // Create Bamboo Object and Give a position as parameter. 
            _bamboos = new List<Bamboo>();
            for (int Vertical = 100; Vertical < 1200; Vertical += 150)
            {
                
                float h = random.Next(200,400);
                int BambooHeight = Singleton.Instance.ScreenHeight - (int)(h/2); 
                var bodyBaboo = world.CreateRectangle(ResourceManager.Bamboo.Width * h / ResourceManager.Bamboo.Height, h, 1f, new Vector2(Vertical, BambooHeight), 0f, BodyType.Static);

                _bamboos.Add(new Bamboo(h, bodyBaboo));
            }


            // Add Rabbit in to world
            Vector2 PositionRabbit = new Vector2(600, 200);
            Body x = new Body();
            var Rabbitheight = 50;
            var scale = Rabbitheight / (float)ResourceManager.Rabbit.Height;
            var RabbitWidth = ResourceManager.Rabbit.Width * scale;
            var bodyRabbit = world.CreateRectangle(RabbitWidth, Rabbitheight, 1f, PositionRabbit, 0f, BodyType.Dynamic);
            bodyRabbit.FixedRotation = true;

            testRabbit = new Rabbit(bodyRabbit);



            // Create back to main menu button
            _backButton = new Button(ResourceManager.BasicBtn, ResourceManager.font)
            {
                PenColour = Color.Red,
                Position = new Vector2(1000, 40),
                Text = "Back",
            };

            _backButton.Click += BackToMainMenu;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _backButton,
            };

            AudioManager.PlaySound("BG", true);

        }

        private void BackToMainMenu(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen. 
            ScreenManager.ChangeScreen = "menu";
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.mainBackground, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            

            // Test texture in game
            testRabbit.draw(gameTime, spriteBatch);

            foreach (var bamboo in _bamboos)
                bamboo.draw(gameTime, spriteBatch);
            
            foreach (var ball in _balls)
                ball.Draw(gameTime, spriteBatch);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var bamboo in _bamboos)
                bamboo.update(gameTime);
            foreach (var ball in _balls)
                ball.Update(gameTime);

            testRabbit.update(gameTime);

            foreach (Component component in _components)
                component.Update(gameTime);

            AudioManager.PlaySound("BG", true);


            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
