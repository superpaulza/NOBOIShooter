using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NOBOIShooter.Screens
{
    partial class ScreenManager : AScreen
    {
        private AScreen _currentSreen, _nextScreen;
        private Texture2D _cursor;

        private MenuScreen _menuScreen;
        private GameScreen _gameScreen;
        private OptionScreen _optionScreen;
        private LeaderboardScreen _leaderboardScreen;

        public ScreenManager(Main game, GraphicsDevice graphicsDevice, ContentManager content)
           : base(game, graphicsDevice, content)
        {
            // Load image game cursor
            _cursor = _content.Load<Texture2D>("Item/sheriff-cursor");

            // Change cursor texture
            Mouse.SetCursor(MouseCursor.FromTexture2D(_cursor, 0, 0));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw current screen
            _currentSreen.Draw(gameTime,spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // post update current screen
            _currentSreen.PostUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // If press ( "esc" key or close button ) then exit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            if(_currentSreen == null)
            {
                _currentSreen = new MenuScreen(_game, _graphicsDevice, _content);
            }

            // Change screen when next update
            if (_nextScreen != null)
            {
                _currentSreen = _nextScreen;
                _nextScreen = null;
            }

            // Update current screen
            _currentSreen.Update(gameTime);
            _currentSreen.PostUpdate(gameTime);
        }

        // Method to change sceen
        public void ChangeScreen(ScreenSelect screenSelect)
        {
            switch (screenSelect)
            {
                case ScreenSelect.Game:
                    _gameScreen = new GameScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _gameScreen;
                    break;
                case ScreenSelect.Setting:
                    _optionScreen = new OptionScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _optionScreen;
                    break;
                case ScreenSelect.Leaderboard:
                    _leaderboardScreen = new LeaderboardScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _leaderboardScreen;
                    break;
                default:
                    _menuScreen = new MenuScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _menuScreen;
                    break;
            }
        }

    }
}

