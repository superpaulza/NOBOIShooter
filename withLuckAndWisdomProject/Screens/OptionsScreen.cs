using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;

namespace withLuckAndWisdomProject.Screens
{
    class OptionsScreen : AScreen
    {
        private Settings _settings;

        // Create value
        private List<Component> _components;
        private Texture2D _background, _backIcon, _volumeOn, _volumeOff, _increaseIcon, _decreaseIcon, _volumeBGMState, _volumeSFXState, _checkBoxEmpty, _checkBoxSelected, _checkBoxState;
        private SpriteFont _font, _headerFont;
        private Button _backButton, _increaseSFXButton, _decreaseSFXButton, _increaseBGMButton, _decreaseBGMButton;
        private DynamicButton _volumeSFXControlButton, _volumeBGMControlButton, _guidelineAimerButton;
        private float _sfxVolume = 100, _bgmVolume = 100;
        private int _speed = 5;

        public OptionsScreen()
        {
            //load save settings
            if (File.Exists("GameSetting.config"))
            {
                _settings = (Settings)FileManager.ReadFromObj("GameSetting.config");
                //load settings
            }
            else
            {
                _settings = new Settings();
            }

            _background = ResourceManager.mainBackground;
            _font = ResourceManager.font;
            _headerFont = ResourceManager.font;
            _backIcon = ResourceManager.BackBtn;
            _increaseIcon = ResourceManager.increseBtn;
            _decreaseIcon = ResourceManager.decreseBtn;

            // Create Button
            _backButton = new Button(_backIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth - 80, 20),
            };

            _backButton.Click += BackButtonOnClick;

            _increaseSFXButton = new Button(_increaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 150, 175),
            };

            _increaseSFXButton.Click += IncreaseSFXButtonOnClick;

            _decreaseSFXButton = new Button(_decreaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2, 175),
            };

            _decreaseSFXButton.Click += DecreaseSFXButtonOnClick;

            _increaseBGMButton = new Button(_increaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 150, 275),
            };

            _increaseBGMButton.Click += IncreaseBGMButtonOnClick;

            _decreaseBGMButton = new Button(_decreaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2, 275),
            };

            _decreaseBGMButton.Click += DecreaseBGMButtonOnClick;


            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _backButton,
                _increaseSFXButton,
                _decreaseSFXButton,
                _increaseBGMButton,
                _decreaseBGMButton,
            };

        }

        private void BackButtonOnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen = "menu";
        }

        private void IncreaseSFXButtonOnClick(object sender, EventArgs e)
        {

        }

        private void DecreaseSFXButtonOnClick(object sender, EventArgs e)
        {

        }

        private void IncreaseBGMButtonOnClick(object sender, EventArgs e)
        {

        }

        private void DecreaseBGMButtonOnClick(object sender, EventArgs e)
        {

        }



        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            spriteBatch.DrawString(_headerFont, "Options", new Vector2(Singleton.Instance.ScreenWidth / 2, 50), Color.White, 0f, _font.MeasureString("Options") * 0.5f, 1f, SpriteEffects.None, 0f);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, "SFX", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 200), Color.White, 0f, _font.MeasureString("SFX") * 0.5f, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "BGM", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 300), Color.White, 0f, _font.MeasureString("BGM") * 0.5f, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "Guideline \n\nAimer", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 450), Color.White, 0f, _font.MeasureString("Guideline \n\nAimer") * 0.5f, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, _sfxVolume.ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 170), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, _bgmVolume.ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 270), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);
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
