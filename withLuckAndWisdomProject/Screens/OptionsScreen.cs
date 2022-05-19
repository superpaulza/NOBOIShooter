using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace withLuckAndWisdomProject.Screens
{
    class OptionsScreen : AScreen
    {
        private Settings _settings;
        public OptionsScreen()
        {
            if (File.Exists("GameSetting.config"))
            {
                _settings = (Settings)FileManager.ReadFromObj("GameSetting.config");
                //load settings
            }
            else
            {
                _settings = new Settings();
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public void OnExiting()
        {
            _settings = new Settings();
            //save setting to file

            FileManager.WriteToObj("GameSetting.config", _settings);
        }
    }
}
