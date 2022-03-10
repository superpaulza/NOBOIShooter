using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.Controls;
using System;
using NOBOIShooter.Data;
using System.Collections.Generic;

namespace NOBOIShooter.Screens
{
    class ScoreScreen : AScreen
    {
        private const int LIMIT_SHOW_SCORE = 7;

        private Texture2D _backIcon, _background, _leftIcon, _rightIcon, _pen;

        private Button _backButton;
        private Button _leftButton;
        private Button _rightButton;
        
        private SpriteFont _font;
        private ScoreData _scoreBord;

        List<Component> _components;
        private Rectangle _areaBackGround;
        private int _scorePage = 0;
        bool _hasNextPage;

        public ScoreScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _backIcon = _content.Load<Texture2D>("Controls/BackButton");
            _leftIcon = _content.Load<Texture2D>("Icons/ArrowLeft");
            _rightIcon = _content.Load<Texture2D>("Icons/ArrowRight");
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _font = _content.Load<SpriteFont>("Fonts/Font");
            _scoreBord = new ScoreData();

            _backButton = new Button(_backIcon, content)
            {
                Position = new Vector2(1200, 20),
            };

            _backButton.Click += _backButtonOnClick;

            _leftButton = new Button(_leftIcon, content)
            {
                Position = new Vector2(550, 550),
            };

            _leftButton.Click += _leftButtonOnClick;

            _rightButton = new Button(_rightIcon, content)
            {
                Position = new Vector2(700, 550),
            };

            _rightButton.Click += _rightButtonOnClick;

            _components = new List<Component>()
            {
                _backButton,
                _leftButton,
                _rightButton,
            };


            _pen = new Texture2D(graphicsDevice, 1, 1);
            _pen.SetData(new[] { Color.White });
            int width = Singleton.Instance.ScreenWidth / 5;
            int height = Singleton.Instance.ScreenHeight / 15;
            _areaBackGround = new Rectangle(width, height, width * 3 , height * 10);
        }

        private void _backButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeScreen(ScreenSelect.Menu);
        }

        private void _leftButtonOnClick(object sender, EventArgs e)
        {
            if (_scorePage > 0)
                _scorePage--;
        }

        private void _rightButtonOnClick(object sender, EventArgs e)
        {
            if (_hasNextPage)
                _scorePage++;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_pen, _areaBackGround, new Color(Color.Black,.5f));

            string scoretext = "Score List";
            spriteBatch.DrawString(_font, scoretext,
                new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(scoretext).X) / 2, 100f), Color.White);

            
            for (int i = 0; i < LIMIT_SHOW_SCORE; i++)
            {
                int pos = _scorePage * LIMIT_SHOW_SCORE + i;
                if(pos < _scoreBord.ScoresTables.Count)
                {
                    scoretext = (pos + 1) +  ". Score : "+ _scoreBord.ScoresTables[pos].ScoreGet.ToString() + " | " + _scoreBord.ScoresTables[pos].ScoreDate.ToString("g");
                    spriteBatch.DrawString(_font, scoretext, new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(scoretext).X) / 2, 150f + i* 50), Color.White);
                }

            }

            _backButton.Draw(gameTime, spriteBatch);
            if(_scorePage > 0)
                _leftButton.Draw(gameTime, spriteBatch);
            if(_hasNextPage)
                _rightButton.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _hasNextPage = _scorePage < _scoreBord.ScoresTables.Count / LIMIT_SHOW_SCORE;
            //if click condition
            _backButton.Update(gameTime);
            _leftButton.Update(gameTime);
            _rightButton.Update(gameTime);
        }
    }
}
