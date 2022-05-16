using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Object;

namespace withLuckAndWisdomProject.Screens
{
    class GameScreen : AScreen
    {
        private Button _backButton;
        private Bamboo testBamboo;
        private Rabbit testRabbit;

        List<Component> _components;
        //Constructor inherit from base class 
        public GameScreen()
        {
            testBamboo = new Bamboo();

            testRabbit = new Rabbit();



            // Create back to main menu button
            _backButton = new Button(ResourceManager.BasicBtn, ResourceManager.font)
            {
                PenColour = Color.Red,
                Position = new Vector2(1000, 40),
                Text = "Back",
            };

            _backButton.Click += BackToMainMenu;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _backButton,
            };

        }

        private void BackToMainMenu(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen. 
            ScreenManager.ChangeScreen = "menu";
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Test texture in game
            testBamboo.draw(gameTime, spriteBatch);
            testRabbit.draw(gameTime, spriteBatch);



            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime, bool isActive)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
    }
}
