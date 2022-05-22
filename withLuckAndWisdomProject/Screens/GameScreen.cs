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
        private List<Vector2> _clouds;
        private Texture2D[] _cloudTexture;
        private float _backgroundMove;
        private HUD _hud;
        private Rectangle _backgroundArea;
        private Texture2D _background;
        private int _sceenWidth;
        private int _sceenHeight;

        private GamePauseRays _gamePause;
        private GameOverRays _gameOver;
        private bool _gameEndSaveScore;

        //Constructor inherit from base class 
        public GameScreen()
        {
            // World of physic
            _world = new World();
            _world.Gravity = new Vector2(0, _world.Gravity.Y * -1f);

            // load asset
            _background = ResourceManager.BackgroundGame;
            _sceenWidth = Singleton.Instance.ScreenWidth;
            _sceenHeight = Singleton.Instance.ScreenHeight;
            _backgroundArea = new Rectangle(0, 0, _sceenWidth, _sceenHeight);

            // create cloud
            _clouds = new List<Vector2>();
            _clouds.Add(new Vector2(100,200));
            for (int i = 0; i < 5; i++)
                GetRandomCloud();
            _cloudTexture = new Texture2D[]
            {
                ResourceManager.cloudFirst,
                ResourceManager.cloudSecound,
                ResourceManager.cloudDungo,
                ResourceManager.cloudHammer,

            };
            _backgroundMove = 0;

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
            //_hud = new HUD(rabbitPosition); 
            _hud = new HUD(rabbitPosition);

            _hud.SetPlayer(_rabbit);
            //load game over
            _gameOver = new GameOverRays();
            _gameOver.SetPlayer(_rabbit);
            _gameEndSaveScore = false;

            // load game pause
            _gamePause = new GamePauseRays();
            _gamePause.SetPlayer(_rabbit);
        }

        public void GetRandomCloud()
        {
            // random next cloud
            Random random = new Random();
            int y = random.Next(100, 300);
            int next = random.Next(300, 600);
            int x = (int)(_clouds[_clouds.Count - 1].X + next);
            _clouds.Add(new Vector2(x, y));
        }

        public void MoveClound(GameTime gameTime)
        {
            // find distance
            var distance = _rabbit.ForwardLenght - _backgroundMove;
            if (distance > 0)
            {
                // move cloud
                for (int i = 0; i < _clouds.Count; i++)
                    _clouds[i] = new Vector2(_clouds[i].X - distance * .1f, _clouds[i].Y );
                _backgroundMove += distance;

                // remove cloud
                for (int i = 0; i < _clouds.Count; i++)
                {
                    if(_clouds[i].X < -340)
                    {
                        _clouds.RemoveAt(i);
                        GetRandomCloud();
                    }
                }
                
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
     
            // Draw game background
            spriteBatch.Draw(ResourceManager.gameBackground,_backgroundArea,
                _rabbit.RabbitState == RabbitState.Ending ? Color.DarkCyan : Color.Cyan);

            // Draw Clound
            foreach (var cloud in _clouds)
                spriteBatch.Draw(_cloudTexture[(int)cloud.Y % 4], cloud, Color.LightGray);

            // Draw game object
            _rabbit.draw(gameTime, spriteBatch);

            // Draw bamboo
            foreach (var bamboo in _bamboos)
                bamboo.draw(gameTime, spriteBatch);


            //when rabbit died
            if (_rabbit.RabbitState == RabbitState.Ending)
            {
                _gameOver.Draw(gameTime, spriteBatch);
                if (!_gameEndSaveScore)
                {
                    // Save score
                    _rabbit.SaveGameScore();
                    _gameEndSaveScore = true;
                }
            }
            else if (_rabbit.GamePause)
            {
                _gamePause.Draw(gameTime, spriteBatch);
            }
            else
            {
                // Draw HUD.
                _hud.draw(gameTime, spriteBatch);
            }


        }

        public override void Update(GameTime gameTime)
        {
            //when rabbit died
            if (_rabbit.RabbitState == RabbitState.Ending)
            {
                _gameOver.Update(gameTime);
            }
            else if (_rabbit.GamePause) {
                _gamePause.Update(gameTime);
            }
            else
            {
                // calculate game effect
                MoveClound(gameTime);
                _rabbit.update(gameTime);
                _hud.update(gameTime);

                foreach (var bamboo in _bamboos)
                    bamboo.update(gameTime);

                // Naive way speed up game physic time
                for (int i = 0; i < 6; i++)
                    _world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

                // BGM
                AudioManager.PlaySound("GameBGM", true);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
