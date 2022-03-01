using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.Controls;

namespace NOBOIShooter.States
{
    public class LoadStage : State
    {
        private List<Component> _components;

        public LoadStage(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var LoadButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(550, 550),
                Text = "Load",
            };

            LoadButton.Click += LoadButton_Click;

            var BackButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 550),
                Text = "Cancle",
            };

            BackButton.Click += BackButton_Click;

            _components = new List<Component>()

      {
        LoadButton,
        BackButton
      };
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading");
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Back");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
