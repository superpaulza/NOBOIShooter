using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;

namespace withLuckAndWisdomProject.Screens
{
    //Menu screen
    public class MenuScreen : AScreen
    {
        private List<Component> _components;
        private Texture2D button, mainBackground, logo;
        private SpriteFont font;
        private Button _playButton;
        private Button _testButton;

        //Constructor inherit from base class
        public MenuScreen()
        {
            //load assets
            button = ResourceManager.button;
            mainBackground = ResourceManager.mainBackground;
            logo = ResourceManager.logo;

            font = ResourceManager.font;


            // Create button on main screen
            _playButton = new Button(button, font)
            {
                PenColour = Color.DarkGreen,
                Position = new Vector2(730, 100),
                Text = "Play",
            };

            _playButton.Click += PlayButtonOnClick;

            _testButton = new Button(button, font)
            {
                PenColour = Color.DarkGreen,
                Position = new Vector2(730, 400),
                Text = "Test",
            };

            _testButton.Click += TestButtonOnClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playButton,
                _testButton,
            };
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen. 
            ScreenManager.ChangeScreen = "game";
        }



        private void TestButtonOnClick(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen. 
            ScreenManager.ChangeScreen = "test";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainBackground, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            spriteBatch.Draw(logo, new Rectangle(115, 0, 400, 350), Color.White);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
