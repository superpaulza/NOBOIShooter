using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using NOBOIShooter.Controls;

namespace NOBOIShooter.Screens
{
    class OptionScreen : AScreen
    {
        private List<Component> _components;
        private Texture2D _background, _backIcon, _volumeOn, _volumeOff, _increaseIcon, _decreaseIcon, _volumeBGMState, _volumeSFXState;
        private SpriteFont _font, _publicsans;
        private Button _backButton, _increaseSFXButton, _decreaseSFXButton, _increaseBGMButton, _decreaseBGMButton;
        private DynamicButton _volumeSFXControlButton, _volumeBGMControlButton;
        private int _sfxVolume = 100, _bgmVolume = 100;

        public OptionScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            _background = _content.Load<Texture2D>("Backgrouds/background");
            _font = _content.Load<SpriteFont>("Fonts/Font");
            _publicsans = _content.Load<SpriteFont>("Fonts/PublicSans");
            _backIcon = _content.Load<Texture2D>("Controls/BackButton");
            _volumeOn = _content.Load<Texture2D>("Item/volume-on");
            _volumeOff = _content.Load<Texture2D>("Item/volume-off");
            _increaseIcon = _content.Load<Texture2D>("Controls/increase");
            _decreaseIcon = _content.Load<Texture2D>("Controls/decrease");

            _volumeBGMState = Singleton.Instance.IsBGMEnable ? _volumeOn : _volumeOff;

            _volumeSFXState = Singleton.Instance.IsSFXEnable ? _volumeOn : _volumeOff;

            _backButton = new Button(_backIcon)
            {
                Position = new Vector2(1200, 20),
            };
            
            _backButton.Click += _backButtonOnClick;

            _increaseSFXButton = new Button(_increaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 150, 175),
            };

            _increaseSFXButton.Click += _increaseSFXButtonOnClick;

            _decreaseSFXButton = new Button(_decreaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2, 175),
            };

            _decreaseSFXButton.Click += _decreaseSFXButtonOnClick;

            _increaseBGMButton = new Button(_increaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 150, 275),
            };

            _increaseBGMButton.Click += _increaseBGMButtonOnClick;

            _decreaseBGMButton = new Button(_decreaseIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2, 275),
            };

            _decreaseBGMButton.Click += _decreaseBGMButtonOnClick;

            _volumeSFXControlButton = new DynamicButton(_volumeSFXState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 250, 180),
                Text = "",

            };

            _volumeSFXControlButton.Click += _volumeSFXControlButtonOnClick;

            _volumeBGMControlButton = new DynamicButton(_volumeBGMState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 250, 280),
                Text = "",

            };

            _volumeBGMControlButton.Click += _volumeBGMControlButtonOnClick;

            
            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _backButton,
                _volumeBGMControlButton,
                _volumeSFXControlButton,
                _increaseSFXButton,
                _decreaseSFXButton,
                _increaseBGMButton,
                _decreaseBGMButton,
            };
        }

        private void _backButtonOnClick(object sender, EventArgs e)
        {
            _game.ChangeScreen(ScreenSelect.Menu);
        }

        private void _increaseSFXButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.SFXVolume < 1.0f)
            {
                Singleton.Instance.SFXVolume += 0.01f;
            }
            
        }
        
        private void _decreaseSFXButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.SFXVolume > 0.0f)
            {
                Singleton.Instance.SFXVolume -= 0.01f;
            }
            
        }

        private void _increaseBGMButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.BGMVolume < 1.0f)
            {
                Singleton.Instance.BGMVolume += 0.01f;
            }
        }

        private void _decreaseBGMButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.BGMVolume > 0.0f)
            {
                Singleton.Instance.BGMVolume -= 0.01f;
            }
        }

        private void _volumeSFXControlButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.IsSFXEnable)
            {
                _volumeSFXControlButton.Texture = _volumeOff;
                Singleton.Instance.IsSFXEnable = false;
                Singleton.Instance.SFXVolume = 0.0f;
            }
            else
            {
                _volumeSFXControlButton.Texture = _volumeOn;
                Singleton.Instance.IsSFXEnable = true;
                Singleton.Instance.SFXVolume = 1.0f;
            }
        }

        private void _volumeBGMControlButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.IsBGMEnable)
            {
                _volumeBGMControlButton.Texture = _volumeOff;
                Singleton.Instance.IsBGMEnable = false;
                Singleton.Instance.BGMVolume = 0.0f;
            }
            else
            {
                _volumeBGMControlButton.Texture = _volumeOn;
                Singleton.Instance.IsBGMEnable = true;
                Singleton.Instance.BGMVolume = 1.0f;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(_font, "Game Options", new Vector2(Singleton.Instance.ScreenWidth / 2, 50), Color.White, 0f, _font.MeasureString("Game Options") * 0.5f, 2f, SpriteEffects.None, 0f);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, "SFX", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 200), Color.White, 0f, _font.MeasureString("SFX") * 0.5f, 1.5f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "BGM", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 300), Color.White, 0f, _font.MeasureString("BGM") * 0.5f, 1.5f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_publicsans, (Singleton.Instance.SFXVolume * 100).ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 170), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_publicsans, (Singleton.Instance.BGMVolume * 100).ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 270), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);


            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }
    }
}
