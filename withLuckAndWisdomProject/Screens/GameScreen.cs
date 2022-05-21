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
        private HUD _hud;

        private Texture2D gameBackground;

        private GameOverScreen _gameOver;
        //Constructor inherit from base class 
        public GameScreen()
        {
            // World of physic
            _world = new World();
            _world.Gravity = new Vector2(0, _world.Gravity.Y * -1f);

            // load asset
            gameBackground = ResourceManager.gameBackground;

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

            // Load HUD.
            //_hud = new HUD(); // Comment the HUD instance because of the bug will happen. 

            //load game over
            _gameOver = new GameOverScreen();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // Draw game backgriund
            // spriteBatch.Draw(ResourceManager.BackgroundGame, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight),
            //     _rabbit.RabbitState == RabbitState.Ending ? Color.DarkCyan : Color.Cyan);



            // Draw game background
            spriteBatch.Draw(ResourceManager.gameBackground, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight),
                _rabbit.RabbitState == RabbitState.Ending ? Color.DarkCyan : Color.Cyan);

            //when rabbit died
            if (_rabbit.RabbitState == RabbitState.Ending)
            {
                _gameOver.Draw(gameTime, spriteBatch);
            }
            else
            {
                // Draw HUD.
                _hud?.draw(gameTime, spriteBatch);

                // Draw game object
                _rabbit.draw(gameTime, spriteBatch);

                foreach (var bamboo in _bamboos)
                    bamboo.draw(gameTime, spriteBatch);
            }


        }

        public override void Update(GameTime gameTime)
        {

            //world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            //world.ShiftOrigin(new Vector2((float)(gameTime.ElapsedGameTime.TotalMilliseconds * .05f), 0 ));

            //very naive world time update speed up
            //set update 6x
            for (int i = 0; i < 6; i++)
            {
                _world.Step(gameTime.ElapsedGameTime);
            }

            //when rabbit died
            if (_rabbit.RabbitState == RabbitState.Ending)
            {
                _gameOver.Update(gameTime);
            }
            else
            {
                _rabbit.update(gameTime);

                foreach (var bamboo in _bamboos)
                    bamboo.update(gameTime);
                // BGM
                AudioManager.PlaySound("GameBGM", true);
            }

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
