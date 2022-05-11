using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;

namespace withLuckAndWisdomProject.Screens
{
    //Menu screen
    public class MenuScreen
    {
        private List<Component> _components;
        private Texture2D button;
        private SpriteFont font;
        private Button _playButton;

        //Constructor inherit from base class
        public MenuScreen()
        {
            button = ResourceManager.button;
            font = ResourceManager.font;

            _playButton = new Button(button, font)
            {
                PenColour = Color.DarkGreen,
                Position = new Vector2(730, 100),
                Text = "Play",
            };

            _playButton.Click += PlayButtonOnClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _playButton,
            };
        }

        private void PlayButtonOnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen = "game";
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle test = new Rectangle(115, 0, 400, 350);
            spriteBatch.Draw(button, test, Color.White);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public void PostUpdate(GameTime gameTime)
        {

        }
    }
}
