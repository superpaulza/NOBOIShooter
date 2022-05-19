using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace withLuckAndWisdomProject.Screens
{
    partial class ScreenManager
    {
        // Create all value in object
        private MenuScreen _menuScreen;
        private GameScreen _gameScreen;

        private string[] _menuList = {"menu", "game"};
        private static string _currentSreen;
        private static bool quit;
        private static bool reset;

        public ScreenManager()
        {
            _currentSreen = _menuList[0];
            reset = true;
            quit = false;
        }

        public static string ChangeScreen
        {
            set
            {
                _currentSreen = value;
                reset = true;
            }
        }

        public static bool Quit
        {
            get { return quit; }
            set { quit = value; }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw current screen
            if (_currentSreen == "menu")
                _menuScreen.Draw(gameTime, spriteBatch);
            else if (_currentSreen == "game")
                _gameScreen.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (reset)
            {
                _menuScreen = new MenuScreen();
                _gameScreen = new GameScreen();
                reset = false;
            }

            // If press ( "esc" key or close button ) then exit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Quit = true;

            if (_currentSreen == "menu")
                _menuScreen.Update(gameTime);
            else if (_currentSreen == "game")
                _gameScreen.Update(gameTime);
        }

    }
}

