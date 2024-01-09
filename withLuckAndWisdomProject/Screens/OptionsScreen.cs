﻿using System;
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
        private Texture2D _background, _backIcon, _volumeOn, _volumeOff, _increaseIcon, _decreaseIcon, _volumeBGMState, _volumeSFXState, _checkBoxEmpty, _checkBoxSelected, _checkBoxState, _applyBtn, _checkBoxShareState;
        private SpriteFont _font, _headerFont;
        private Button _backButton, _increaseSFXButton, _decreaseSFXButton, _increaseBGMButton, _decreaseBGMButton, _applyButton;
        private DynamicButton _volumeSFXControlButton, _volumeBGMControlButton, _guidelineAimerButton, _uploadScoreButton;
        private float _sfxVolume = Singleton.Instance.SFXVolume * 100, _bgmVolume = Singleton.Instance.BGMVolume * 100;
        private int _speed = 5;
        private Texture2D _pen;
        private Vector2 _underLine;

        public OptionsScreen()
        {
            //load save settings
            if (File.Exists("GameSetting.config"))
            {
                _settings = (Settings)FileManager.ReadFromObj("GameSetting.config");
                //load settings
                Singleton.Instance.SFXVolume = _settings.gameSFXSound;
                Singleton.Instance.BGMVolume = _settings.gameMainSound;
                Singleton.Instance.IsEnableAimer = _settings.IsEnableAimer;
                Singleton.Instance.IsEnableSFX = _settings.IsEnableSFX;
                Singleton.Instance.IsEnableBGM = _settings.IsEnableBGM;
                Singleton.Instance.IsShareDataToDev = _settings.IsShareDataToDev;

            }
            else
            {
                _settings = new Settings();
            }

            _background = ResourceManager.mainBackground;
            _font = ResourceManager.font;
            _headerFont = ResourceManager.font;
            _backIcon = ResourceManager.homeBtn;
            _applyBtn = ResourceManager.applyBtn;
            _increaseIcon = ResourceManager.increseBtn;
            _decreaseIcon = ResourceManager.decreseBtn;
            _volumeOn = ResourceManager.volumeOnIcon;
            _volumeOff = ResourceManager.volumeOffIcon;
            _checkBoxEmpty = ResourceManager.checkBoxEmpty;
            _checkBoxSelected = ResourceManager.checkBoxSelect;
            _pen = ResourceManager.Pencil;

            // Get sound default
            _volumeBGMState = Singleton.Instance.IsEnableBGM ? _volumeOn : _volumeOff;
            _volumeSFXState = Singleton.Instance.IsEnableSFX ? _volumeOn : _volumeOff;
            _checkBoxState = Singleton.Instance.IsEnableAimer ? _checkBoxSelected : _checkBoxEmpty;
            _checkBoxShareState = Singleton.Instance.IsShareDataToDev ? _checkBoxSelected : _checkBoxEmpty;

            // Create Button
            _backButton = new Button(_backIcon)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth - 120, 20),
            };

            _backButton.Click += BackButtonOnClick;

            //SFXVolume
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

            //BGMVolume
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

            _volumeSFXControlButton = new DynamicButton(_volumeSFXState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 250, 180),
                colour = Color.Orange

            };

            _volumeSFXControlButton.Click += VolumeSFXControlButtonOnClick;

            _volumeBGMControlButton = new DynamicButton(_volumeBGMState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 250, 280),
                colour = Color.Orange
            };

            _volumeBGMControlButton.Click += VolumeBGMControlButtonOnClick;

            _guidelineAimerButton = new DynamicButton(_checkBoxState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 85, 350),
            };

            _guidelineAimerButton.Click += GuidelineAimerOnClick;

            _uploadScoreButton = new DynamicButton(_checkBoxShareState)
            {
                PenColour = new Color(Color.White, 1f),
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 + 85, 400),
            };

            _uploadScoreButton.Click += uploadScoreButtonOnClick;

            //apply
            _applyButton = new Button(_applyBtn)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 70, Singleton.Instance.ScreenHeight / 2 + 180),
            };

            _applyButton.Click += ApplyBtnOnClick;

            //load buttons onto component aka. dynamic drawing list
            _components = new List<Component>()
            {
                _backButton,
                _increaseSFXButton,
                _decreaseSFXButton,
                _increaseBGMButton,
                _decreaseBGMButton,
                _volumeSFXControlButton,
                _volumeBGMControlButton,
                _guidelineAimerButton,
                _uploadScoreButton,
                _applyButton,
            };

        }

        private void BackButtonOnClick(object sender, EventArgs e)
        {
            ScreenManager.ChangeScreen = "menu";
        }

        private void IncreaseSFXButtonOnClick(object sender, EventArgs e)
        {
            if (_sfxVolume < 100 && Singleton.Instance.IsEnableSFX)
            {
                _sfxVolume += _speed;
                Singleton.Instance.SFXVolume = (float)_sfxVolume / 100;
            }
            else if (_sfxVolume == 0)
            {
                Singleton.Instance.IsEnableSFX = true;
                _volumeSFXControlButton.Texture = _volumeOn;
                _sfxVolume += _speed;
                Singleton.Instance.SFXVolume = (float)_sfxVolume / 100;
            }
        }

        private void DecreaseSFXButtonOnClick(object sender, EventArgs e)
        {
            if (_sfxVolume > _speed && Singleton.Instance.IsEnableSFX)
            {
                _sfxVolume -= _speed;
                Singleton.Instance.SFXVolume = (float)_sfxVolume / 100;
            }
            else if (_sfxVolume == _speed)
            {
                _sfxVolume -= _speed;
                Singleton.Instance.IsEnableSFX = false;
                _volumeSFXControlButton.Texture = _volumeOff;
                Singleton.Instance.SFXVolume = 0.0f;
            }
        }

        private void IncreaseBGMButtonOnClick(object sender, EventArgs e)
        {
            if (_bgmVolume < 100 && Singleton.Instance.IsEnableBGM)
            {
                _bgmVolume += _speed;
                Singleton.Instance.BGMVolume = (float)_bgmVolume / 100;
            }
            else if (_bgmVolume == 0)
            {
                Singleton.Instance.IsEnableBGM = true;
                _volumeBGMControlButton.Texture = _volumeOn;
                _bgmVolume += _speed;
                Singleton.Instance.BGMVolume = (float)_bgmVolume / 100;
            }
        }

        private void DecreaseBGMButtonOnClick(object sender, EventArgs e)
        {
            if (_bgmVolume > _speed && Singleton.Instance.IsEnableBGM)
            {
                _bgmVolume -= _speed;
                Singleton.Instance.BGMVolume = (float)_bgmVolume / 100;
            }
            else if (_bgmVolume == _speed)
            {
                _bgmVolume -= _speed;
                Singleton.Instance.IsEnableBGM = false;
                _volumeBGMControlButton.Texture = _volumeOff;
                Singleton.Instance.BGMVolume = 0.0f;
            }
        }

        private void VolumeSFXControlButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.IsEnableSFX)
            {
                _volumeSFXControlButton.Texture = _volumeOff;
                Singleton.Instance.IsEnableSFX = false;
                _sfxVolume = 0;
                Singleton.Instance.SFXVolume = 0.0f;
            }
            else if (!Singleton.Instance.IsEnableSFX)
            {
                _volumeSFXControlButton.Texture = _volumeOn;
                Singleton.Instance.IsEnableSFX = true;
                _sfxVolume = 100;
                Singleton.Instance.SFXVolume = 1.0f;
            }
        }

        private void VolumeBGMControlButtonOnClick(object sender, EventArgs e)
        {
            if (Singleton.Instance.IsEnableBGM)
            {
                _volumeBGMControlButton.Texture = _volumeOff;
                Singleton.Instance.IsEnableBGM = false;
                _bgmVolume = 0;
                Singleton.Instance.BGMVolume = 0.0f;
            }
            else if (!Singleton.Instance.IsEnableBGM)
            {
                _volumeBGMControlButton.Texture = _volumeOn;
                Singleton.Instance.IsEnableBGM = true;
                _bgmVolume = 100;
                Singleton.Instance.BGMVolume = 1.0f;
            }
        }

        private void GuidelineAimerOnClick(object sender, EventArgs e)
        {
            Singleton.Instance.IsEnableAimer = !Singleton.Instance.IsEnableAimer;
            _guidelineAimerButton.Texture = Singleton.Instance.IsEnableAimer ? _checkBoxSelected : _checkBoxEmpty;
        }

        private void uploadScoreButtonOnClick(object sender, EventArgs e)
        {
            Singleton.Instance.IsShareDataToDev = !Singleton.Instance.IsShareDataToDev;
            _uploadScoreButton.Texture = Singleton.Instance.IsShareDataToDev ? _checkBoxSelected : _checkBoxEmpty;
        }

        private void ApplyBtnOnClick(object sender, EventArgs e)
        {
            OnExiting();
            ScreenManager.ChangeScreen = "menu";
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Component component in _components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            var titlePosition = new Vector2(Singleton.Instance.ScreenWidth / 2, 100);
            spriteBatch.DrawString(_headerFont, "Options", titlePosition, Color.DarkGreen, 0f, _font.MeasureString("Options") * 0.5f, 1.2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_pen, new Rectangle((int)titlePosition.X - 100, (int)titlePosition.Y + 25, 200, 5), Color.DarkSeaGreen);
            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, "SFX", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 200), Color.DarkSlateGray, 0f, _font.MeasureString("SFX") * 0.5f, .8f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "BGM", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 300), Color.DarkSlateGray, 0f, _font.MeasureString("BGM") * 0.5f, .8f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "Guideline Aimer", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 375), Color.DarkSlateGray, 0f, _font.MeasureString("Guideline Aimer") * 0.5f, .6f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "Auto upload score", new Vector2(Singleton.Instance.ScreenWidth / 2 - 150, 425), Color.DarkSlateGray, 0f, _font.MeasureString("Auto upload score") * 0.5f, .6f, SpriteEffects.None, 0f);
            
            spriteBatch.DrawString(_font, _sfxVolume.ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 160), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, _bgmVolume.ToString("N0"), new Vector2(Singleton.Instance.ScreenWidth / 2 + 70, 260), Color.White, 0f, new Vector2(0), 1f, SpriteEffects.None, 0f);

            foreach (Component component in _components)
                component.Draw(gameTime, spriteBatch);

            AudioManager.PlaySound("Opbg", true);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public void OnExiting()
        {
            _settings = new Settings();
            //save setting to file
            _settings.gameMainSound = Singleton.Instance.BGMVolume;
            _settings.gameSFXSound = Singleton.Instance.SFXVolume;
            _settings.IsEnableAimer = Singleton.Instance.IsEnableAimer;
            _settings.IsEnableSFX = Singleton.Instance.IsEnableSFX;
            _settings.IsEnableBGM = Singleton.Instance.IsEnableBGM;
            _settings.IsShareDataToDev = Singleton.Instance.IsShareDataToDev;
            FileManager.WriteToObj("GameSetting.config", _settings);
        }
    }
}
