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
        private Texture2D _gameBackground;

        private int _sceenWidth;
        private int _sceenHeight;
        private Rectangle _backgroundTile1;
        private Rectangle _backgroundTile2;
        //Constructor inherit from base class 
        public GameScreen()
        {
            // World of physic
            _world = new World();
            _world.Gravity = new Vector2(0, _world.Gravity.Y * -1f);

            // load asset
            _gameBackground = ResourceManager.BackgroundGame;
            _sceenWidth = Singleton.Instance.ScreenWidth;
            _sceenHeight = Singleton.Instance.ScreenHeight;

            //world of physic
            _world = new World();
            _world.Gravity = new Vector2(0, _world.Gravity.Y * -1f);

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
            // Draw game background
            if (_rabbit.RabbitState != RabbitState.Ending)
            {
                spriteBatch.Draw(_gameBackground, _backgroundTile1, Color.White);
                spriteBatch.Draw(_gameBackground, _backgroundTile2, Color.White);
            }

            // Draw game object
            _rabbit.draw(gameTime, spriteBatch);

            foreach (var bamboo in _bamboos)
                bamboo.draw(gameTime, spriteBatch);

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
            //_backgroundNow = (int) _rabbit.ForwardLenght;
            //_backgroundMove = (int)(_rabbit.ForwardLenght * gameTime.ElapsedGameTime.TotalSeconds * 5);
            //System.Diagnostics.Debug.WriteLine(_backgroundMove);
            //_backgroundTile1 = new Rectangle(-_backgroundMove, 0, _sceenWidth, _sceenHeight);
            //_backgroundTile2 = new Rectangle(_sceenWidth - _backgroundMove, 0, _sceenWidth, _sceenHeight);
            _backgroundTile1 = new Rectangle(0, 0, _sceenWidth, _sceenHeight);

            //when rabbit died
            if (_rabbit.RabbitState == RabbitState.Ending)
            {
                ScreenManager.ChangeScreen = "menu";
            }
            else
            {
                // BGM
                AudioManager.PlaySound("GameBGM", true);
            }

            //world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            //world.ShiftOrigin(new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .05f), 0 ));

            //very naive world time update speed up
            //set update 5x
            for (int i = 0; i < 6; i++)
            {
                _world.Step(gameTime.ElapsedGameTime);
            }
            
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
