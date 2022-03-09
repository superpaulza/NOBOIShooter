using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using NOBOIShooter.GameObjects;
using System;

namespace NOBOIShooter.Screens
{
    //Game screen
    public class GameScreen : AScreen
    {
        private Texture2D _bubbleImg, _shooterImg, _backIcon, _background, _pen;

        private SpriteFont _textFront;

        private SoundEffect _gameSfxBg, _gameSfxEnd;
        private SoundEffectInstance _sfxBgInstance, _sfxEndInstance2;

        private BallGridManager _bord;
        private Player _player;

        private Button _backButton;

        public GameScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            //State = GameState.init;
            _textFront = _content.Load<SpriteFont>("Fonts/Font");
            _backIcon = _content.Load<Texture2D>("Controls/BackButton");
            _background = content.Load<Texture2D>("Backgrouds/gameBackground");
            _bubbleImg = _content.Load<Texture2D>("Item/bubble");

            _gameSfxBg = content.Load<SoundEffect>("BGM/GameScreenBGM");
            _gameSfxEnd = content.Load<SoundEffect>("BGM/GameOverBGM");

            _shooterImg = _content.Load<Texture2D>("Item/bubble-shoot");

            _sfxBgInstance = _gameSfxBg.CreateInstance();
            _sfxEndInstance2 = _gameSfxEnd.CreateInstance();
            _sfxBgInstance.IsLooped = true;

            _sfxBgInstance.Play();

            _pen = new Texture2D(graphicsDevice, 1, 1);
            _pen.SetData(new[] { Color.White });

            _backButton = new Button(_backIcon)
            {
                Position = new Vector2(1200, 20),
            };
            _backButton.Click += BackButton_Click;

            _bord = new BallGridManager(_bubbleImg);
            _player = new Player(_bord, _shooterImg, _bubbleImg, _pen);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw Backgrounds
            spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            // Draw Backgrounds
            spriteBatch.Draw(_pen, new Rectangle((int)_bord.Position.X, (int)_bord.Position.Y, _bord.Width, _bord.Height), new Color(Color.Black, 0.2f));

            _backButton.Draw(gameTime, spriteBatch);
            _player.Draw(spriteBatch, gameTime);
            _bord.Draw(spriteBatch, gameTime);

            new GameStageCheck(_pen, spriteBatch, _textFront, _bord, _sfxBgInstance, _sfxEndInstance2, _sfxEndInstance2);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _backButton.Update(gameTime);
            _bord.Update(gameTime);
            _player.Update(gameTime);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            //Singleton.Instance.removeBubble.Clear();
            //Singleton.Instance.Shooting = false;
            _bord.ClearGame();
            _sfxBgInstance.Dispose();
            _sfxEndInstance2.Dispose();
            _game.ChangeScreen(ScreenSelect.Menu);
        }
    }
}
