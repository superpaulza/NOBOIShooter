using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.Controls;

namespace NOBOIShooter.States
{
    public class MenuState : State
    {

        private Texture2D menuBackGroundTexture;

        public MenuState(Main game, ContentManager content) : base(game, content)
        {

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
      {
        newGameButton,
        loadGameButton,
        quitGameButton,
      };
        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("");
            var buttonFont = _content.Load<Texture2D>("");
            menuBackGroundTexture = _content.Load<Texture2D>("");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime game, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
