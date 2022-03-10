using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.Controls;
using System;
using NOBOIShooter.Data;

namespace NOBOIShooter.Screens
{
    class ScoreScreen : AScreen
    {
        private const int LIMIT_SHOW_SCORE = 7;

        private Texture2D _backIcon, _background, _pen;
        private Button _backButton;
        private SpriteFont _font;
        private ScoreData _scoreBord;

        private Rectangle _areaBackGround;

        public ScoreScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _backIcon = _content.Load<Texture2D>("Controls/BackButton");
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _font = _content.Load<SpriteFont>("Fonts/Font");
            _scoreBord = new ScoreData();

            _backButton = new Button(_backIcon, content)
            {
                Position = new Vector2(1200, 20),
            };

            _backButton.Click += _backButtonOnClick;

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _backButton.Draw(gameTime,spriteBatch);
            spriteBatch.Draw(_pen, _areaBackGround, new Color(Color.Black,.5f));

            string scoretext = "Score List";
            spriteBatch.DrawString(_font, scoretext,
                new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(scoretext).X) / 2, 100f), Color.White);

            int i = 1;
            foreach (var score in _scoreBord.ScoresTables)
            {
                scoretext = i +  ". Score : "+score.ScoreGet.ToString() + " | " +score.ScoreDate.ToString("g");
                spriteBatch.DrawString(_font, scoretext,
                    new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(scoretext).X) / 2, 100f + i++ * 50), Color.White);
                if (i >= LIMIT_SHOW_SCORE + 1) break;
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _backButton.Update(gameTime);
        }
    }
}
