using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace withLuckAndWisdomProject.Screens
{
    partial class ScreenManager : AScreen
    {
        // Create all value in object
        private AScreen _currentSreen;
        private AScreen _nextScreen;
        private Texture2D _cursor;

        private MenuScreen _menuScreen;
        private OptionScreen _optionScreen;

        public ScreenManager(Main game, GraphicsDevice graphicsDevice, ContentManager content)
           : base(game, graphicsDevice, content)
        {
            // Load image game cursor
            // _cursor = _content.Load<Texture2D>("Item/sheriff-cursor");

            // Change cursor texture
            // Mouse.SetCursor(MouseCursor.FromTexture2D(_cursor, 0, 0));
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
            // Screen select
            switch (screenSelect)
            {
                case ScreenSelect.Setting:
                    _optionScreen = new OptionScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _optionScreen;
                    break;
                default:
                    _menuScreen = new MenuScreen(_game, _graphicsDevice, _content);
                    _nextScreen = _menuScreen;
                    break;
            }
        }

    }
}

