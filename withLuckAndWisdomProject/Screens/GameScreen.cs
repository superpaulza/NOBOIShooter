using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Object;
using tainicom.Aether.Physics2D.Dynamics; 

namespace withLuckAndWisdomProject.Screens
{
    class GameScreen : AScreen
    {
        private const int BAMBOO_START_POSITION = 250;
        private const int BAMBOO_START_HEGIHT = 350;
        private const int RABBIT_START_DROP = 100;
        private const int RABBIT_HEIGHT = 100;
        private World _world;
        private Rabbit _rabbit;
        private List<Bamboo> _bamboos;

        private Button _backButton;
        private List<Component> _components;
        //Constructor inherit from base class 
        public GameScreen()
        {
            // World of physic
            _world = new World();
            _world.Gravity = new Vector2(0, _world.Gravity.Y * -1f);

            // load asset
            gameBackground = ResourceManager.gameBackground;

            //world of physic
            world = new World();
            world.Gravity = new Vector2(0, world.Gravity.Y * -1f);

            //Create game border (set edges line foreach edge)
            //var top = 0;
            //var bottom = Singleton.Instance.ScreenHeight; //720
            //var left = 0;
            //var right = Singleton.Instance.ScreenWidth; //1280
            //var edges = new Body[]
            //{
            //    world.CreateEdge(new Vector2(left, top), new Vector2(right, top)),
            //    //world.CreateEdge(new Vector2(right, top), new Vector2(right, bottom)),
            //    world.CreateEdge(new Vector2(left, top), new Vector2(left, bottom)),
            //    world.CreateEdge(new Vector2(left, bottom), new Vector2(right, bottom))
            //};

            //foreach (var edge in edges)
            //{
            //    edge.BodyType = BodyType.Static;
            //    // edge.SetRestitution(1);
            //}

            // Create Bamboo Object and Give a position as parameter. 
            _bamboos = new List<Bamboo>(); 
            int BambooCenter = Singleton.Instance.ScreenHeight - BAMBOO_START_HEGIHT / 2; 
            var bodyBaboo = _world.CreateRectangle(5, BAMBOO_START_HEGIHT, 1f, new Vector2(BAMBOO_START_POSITION, BambooCenter), 0f, BodyType.Static);
             _bamboos.Add(new Bamboo(BAMBOO_START_HEGIHT, BambooType.Normal, bodyBaboo));
            
            // Add Rabbit in to the world
            Vector2 rabbitPosition = new Vector2(BAMBOO_START_POSITION, RABBIT_START_DROP);
            var rabbitWidth = ResourceManager.Rabbit.Width * RABBIT_HEIGHT / (float)ResourceManager.Rabbit.Height;
            var bodyRabbit = _world.CreateRectangle(rabbitWidth, RABBIT_HEIGHT, 1.5f, rabbitPosition, 0f, BodyType.Dynamic);
            bodyRabbit.FixedRotation = true;
            _rabbit = new Rabbit(bodyRabbit , RABBIT_HEIGHT, _bamboos);
            

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

        }

        private void BackToMainMenu(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen.
            ScreenManager.ChangeScreen = "menu";
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.BackgroundGame, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight),
                _rabbit.RabbitState == RabbitState.Ending ? Color.DarkCyan : Color.Cyan);
            
            //if(_rabbit.RabbitState == RabbitState.Ending)
            //    spriteBatch.Draw(ResourceManager.Pencil, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight),
            //     new Color(Color.Black,.2f));

            
            
            // Test texture in game
            _rabbit.draw(gameTime, spriteBatch);

            foreach (var bamboo in _bamboos)
                bamboo.draw(gameTime, spriteBatch);

            //foreach (var mout in _mountains)
            //    spriteBatch.Draw(ResourceManager.BackgroundMoutain, mout, Color.Green);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

        }

        public override void Update(GameTime gameTime)
        {
            _rabbit.update(gameTime);
            foreach (var bamboo in _bamboos)
                bamboo.update(gameTime);

            foreach (Component component in _components)
                component.Update(gameTime);

            // BGM
            AudioManager.PlaySound("GameBGM", true);

            //world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            //world.ShiftOrigin(new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .05f), 0 ));

            //very naive world time update
            _world.Step(gameTime.ElapsedGameTime);
            _world.Step(gameTime.ElapsedGameTime);
            _world.Step(gameTime.ElapsedGameTime);
            _world.Step(gameTime.ElapsedGameTime);
            _world.Step(gameTime.ElapsedGameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
