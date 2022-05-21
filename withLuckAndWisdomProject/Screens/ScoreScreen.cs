﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using withLuckAndWisdomProject.Controls;
using withLuckAndWisdomProject.Manager;

namespace withLuckAndWisdomProject.Screens
{
    class ScoreScreen : AScreen
    {
        private const int LIMIT_SCORE_PER_PAGE = 10;

        private Texture2D _mainBackground;
        private Texture2D _pen;

        private SpriteFont _font;
        private Button _backButton;
        private Button _leftButton;
        private Button _rightButton;

        private ScoreData _scoreBord;
        private int _scorePage;
        private bool _hasNextPage;
        private Rectangle _areaBackGround;

        private string _titleText = "Score List";
        private string _scoretext;
        private Vector2 _titlePosion;

        public ScoreScreen()
        {
            _mainBackground = ResourceManager.mainBackground;
            _pen = ResourceManager.Pencil;
            _font = ResourceManager.font;
            _scoreBord = new ScoreData();
            _scorePage = 0;
            // Create back to main menu button
            _backButton = new Button(ResourceManager.BackBtn)
            {
                Position = new Vector2(Singleton.Instance.ScreenWidth - 80, 20),
            };
            _backButton.Click += BackToMainMenu;

            _leftButton = new Button(ResourceManager.increseBtn)
            {
                Position = new Vector2(550, 550),
            };

            _leftButton.Click += LeftButtonOnClick;

            _rightButton = new Button(ResourceManager.decreseBtn)
            {
                Position = new Vector2(700, 550),
            };

            _rightButton.Click += RightButtonOnClick;
            // Setting value
            int width = Singleton.Instance.ScreenWidth / 5;
            int height = Singleton.Instance.ScreenHeight / 15;
            _areaBackGround = new Rectangle(width, height, width * 3, height * 10);
            _titlePosion = new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(_titleText).X) / 2, 100f);
        }

        public override void Update(GameTime gameTime)
        {
            _backButton.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_mainBackground, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            

            // Draw text title
            spriteBatch.DrawString(_font, _titleText, _titlePosion, Color.White);

            // Write score 
            for (int i = 0; i < LIMIT_SCORE_PER_PAGE; i++)
            {
                int index = _scorePage * LIMIT_SCORE_PER_PAGE + i;
                if (index < _scoreBord.ScoresTables.Count)
                {
                    _scoretext = (index + 1) + ". Score : " + _scoreBord.ScoresTables[index].ScoreGet.ToString() + " | " + _scoreBord.ScoresTables[index].ScoreDate.ToString("g");
                    spriteBatch.DrawString(_font, _scoretext, new Vector2((float)(Singleton.Instance.ScreenWidth - _font.MeasureString(_scoretext).X) / 2, 150f + i * 50), (i % 2 == 0) ? Color.White : Color.LightGray);
                }

            }

            // Show button
            _backButton.Draw(gameTime, spriteBatch);
            if (_scorePage > 0)
                _leftButton.Draw(gameTime, spriteBatch);
            if (_hasNextPage)
                _rightButton.Draw(gameTime, spriteBatch);

            _backButton.Draw(gameTime,spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
        private void CheckHaveNextPage()
        {
            _hasNextPage = _scorePage < _scoreBord.ScoresTables.Count / LIMIT_SCORE_PER_PAGE;
        }

        private void BackToMainMenu(object sender, EventArgs e)
        {
            // Change to Screen when Clicked on Play button in Menu Screen.
            ScreenManager.ChangeScreen = "menu";
        }
        private void LeftButtonOnClick(object sender, EventArgs e)
        {
            if (_scorePage > 0)
                _scorePage--;
            CheckHaveNextPage();
        }

        private void RightButtonOnClick(object sender, EventArgs e)
        {
            if (_hasNextPage)
                _scorePage++;
            CheckHaveNextPage();
        }
    }
}