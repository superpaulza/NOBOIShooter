using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using NOBOIShooter.GameObjects;
using System;

namespace NOBOIShooter.Screens
{
    //Game screen
    public class GameScreen : AScreen
    {
        private Texture2D _bubbleImg, _shooterImg, _backIcon, _background, _pen;

        private SpriteFont _textFront;
        private SpriteFont _numberFront;

        private SoundEffect _gameSfxBg, _gameSfxEnd, _gameSfxWin;
        private SoundEffectInstance _sfxBgInstance, _sfxEndInstance2, _sfxEndInstance3;

        private Bubble[,] bubbleArea = new Bubble[GAME_GRID_Y + 2, GAME_GRID_X];
        
        private SpriteFont myText;
        private Texture2D BackIcon, _background;
        private Button BackButton;
        Texture2D _bubbleTexture, shooterTexture, line;
        
        private Color _Color;
        private Random _random = new Random();
        private Shooter Player;
        private bool gameOver = false, gameWin = false, fadeFinish = false;
        private float _timer = 0f;
        private float __timer = 0f;
        private float PlayTime = 0f;
        private int alpha = 255;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 15f; // 30f
        private SoundEffect Effect1, Effect2;
        private SoundEffectInstance Instance1, Instance2; 
        private int count = 0;
        private int bubblenull = 0;
        private BallGridManager _bord;
        private Player _player;

        private Button _backButton;

        public GameScreen(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            //State = GameState.init;
            _textFront = _content.Load<SpriteFont>("Fonts/Font");

            _backIcon = _content.Load<Texture2D>("Controls/BackButton");
            _background = content.Load<Texture2D>("Backgrouds/gameBackground");
            _bubbleImg = _content.Load<Texture2D>("Item/bubble");

            _gameSfxBg = content.Load<SoundEffect>("BGM/GameScreenBGM");
            _gameSfxEnd = content.Load<SoundEffect>("BGM/GameOverBGM");
            _gameSfxWin = content.Load<SoundEffect>("BGM/VictoryBGM");

            _shooterImg = _content.Load<Texture2D>("Item/bubble-shoot");

            _sfxBgInstance = _gameSfxBg.CreateInstance();
            _sfxBgInstance.Volume = Singleton.Instance.BGMVolume;
            _sfxEndInstance2 = _gameSfxEnd.CreateInstance();
            _sfxEndInstance2.Volume = Singleton.Instance.SFXVolume;
            _sfxBgInstance.IsLooped = true;
            _sfxEndInstance3 = _gameSfxWin.CreateInstance();
            _sfxEndInstance3.Volume = Singleton.Instance.SFXVolume;

            _sfxBgInstance.Play();

            _pen = new Texture2D(graphicsDevice, 1, 1);
            _pen.SetData(new[] { Color.White });

            _backButton = new Button(_backIcon)
            {
                Position = new Vector2(1200, 20),
            };
            _backButton.Click += BackButton_Click;

            _bord = new BallGridManager(_bubbleImg);
            _player = new Player(_bord, _shooterImg, _bubbleImg, _pen);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Draw Backgrounds
            spriteBatch.Draw(_background, new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight), Color.White);
            // Draw Backgrounds
            spriteBatch.Draw(_pen, new Rectangle((int)_bord.Position.X, (int)_bord.Position.Y, _bord.Width, _bord.Height), new Color(Color.Black, 0.2f));

            _backButton.Draw(gameTime, spriteBatch);
            _player.Draw(spriteBatch, gameTime);
            _bord.Draw(spriteBatch, gameTime);

            new GameStageCheck(_pen, spriteBatch, _textFront, _bord, _sfxBgInstance, _sfxEndInstance2, _sfxEndInstance3);

            // Score Display. 
            spriteBatch.DrawString(myText, "Score", new Vector2(900, 100), Color.Black);
            spriteBatch.DrawString(myText, "10", new Vector2(900, 180), Color.Black);

            if (gameOver)
=======
            if (!_bord.GamePause)
            {
                spriteBatch.DrawString(_textFront, "Score : " + _bord.GameScore, _textPosition, Color.White);
                //Debug.WriteLine("Score : " + _bord.GameScore);
            }

            if (_bord.GameEnd)
            {
                spriteBatch.DrawString(_textFront, "Game Over", _textPosition, Color.White);
            }

            if (_bord.GameWin)
            {
                spriteBatch.DrawString(_textFront, "You Won", _textPosition, Color.White);
            }

            // Draw fade out
            if (_bord.GameWin || _bord.GameEnd)
            {

                spriteBatch.Draw(_pen, FullDisplay, new Color(Color.Black, .4f));
                _sfxBgInstance.Pause();
                _sfxEndInstance2.Play();
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
<<<<<<< HEAD
            BackButton.Update(gameTime);
            if (!gameOver && !gameWin)
            {
                //Object Update
                foreach (Bubble bubble in bubbleArea) if (bubble != null) bubble.Update(gameTime, bubbleArea); 
                Player.Update(gameTime, bubbleArea);

                PlayTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                for (int i = 0; i < GAME_GRID_X; i++)
                {
                    if (bubbleArea[GAME_BUBBLE_DEATH - 2 , i] != null)
                    {
                        gameOver = true;
                        Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        Singleton.Instance.BestTime = PlayTime.ToString("F");
                    }
                }

                //Check ball flying
                for (int i = 1; i < GAME_GRID_Y; i++)
                {
                    for (int j = 1; j < GAME_GRID_X - 1; j++)
                    {
                        if (i % 2 != 0)
                        {
                            if (bubbleArea[i - 1, j] == null && bubbleArea[i - 1, j + 1] == null)
                            {
                                bubbleArea[i, j] = null;
                            }
                            if (bubbleArea[i, 1] == null && bubbleArea[i - 1, 0] == null && bubbleArea[i - 1, 1] == null)
                            {
                                bubbleArea[i, 0] = null;
                            }
                            if (bubbleArea[i, 5] == null && bubbleArea[i - 1, 7] == null && bubbleArea[i - 1, 6] == null)
                            {
                                bubbleArea[i, 6] = null;
                            }
                        }
                        else
                        {
                            if (bubbleArea[i - 1, j - 1] == null && bubbleArea[i - 1, j] == null)
                            {
                                bubbleArea[i, j] = null;
                            }
                            if (bubbleArea[i - 1, 0] == null && bubbleArea[i, 1] == null)
                            {
                                bubbleArea[i, 0] = null;
                            }
                            if (bubbleArea[i - 1, 6] == null && bubbleArea[i, 6] == null)
                            {
                                bubbleArea[i, 7] = null;
                            }
                        }
                        bubblenull++;
                    }
                }
                
                __timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (__timer >= tickPerUpdate && !Singleton.Instance.Shooting)
                {
                    // Check game over before scroll
                    for (int i = GAME_BUBBLE_DEATH; i < GAME_GRID_Y; i++)
                    {
                        for (int j = 0; j < GAME_GRID_Y; j++)
                        {
                            if (bubbleArea[i, j] != null)
                            {
                                gameOver = true;
                                Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                                Singleton.Instance.BestTime = PlayTime.ToString("F");
                            }
                        }
                    }

                    // Scroll position 
                    for (int layer = GAME_GRID_Y - 2; layer >= 0; layer--)
                    {
                        for (int index = 0; index < GAME_GRID_X - (layer % 2); index++)
                        {
                            if (bubbleArea[layer, index] != null)
                            {
                                // Draw new scroll position
                                bubbleArea[layer, index].Position += new Vector2(0,2*BUBBLE_WIDTH);
                                bubbleArea[layer + 2, index] = bubbleArea[layer, index];
                            }
                        }
                    }
                    
                    //Random ball After Scroll
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < GAME_GRID_X - (i % 2); j++)
                        {
                            bubbleArea[i, j] = new Bubble(_bubbleTexture)
                            {
                                Name = "Bubble",
                                Position = new Vector2((j * BUBBLE_WIDTH) + ((i % 2) == 0 ? left : left + BUBBLE_WIDTH / 2),top + (i * BUBBLE_WIDTH)),
                                _color = GetRandomColor(),
                                IsMoving = false,
                            };
                        }
                    }

                    __timer -= tickPerUpdate;
                }

                gameWin = CheckWin(bubbleArea);

            }
            else
            {
                Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
                Singleton.Instance.MouseCurrent = Mouse.GetState();
                if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.Score = 0;
                    //ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                }

                if (gameWin)
                {
                    if (count == 0)
                    {
                        count++;
                        Instance1.Dispose();
                        Instance2.Play();
                    }
                }
                else if (gameOver)
                {
                    if (count == 0)
                    {
                        count++;
                        Instance1.Dispose();
                        Instance2.Play();
                    }
                }

            }

            // fade out
            if (!fadeFinish)
            {
                _timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (_timer >= timerPerUpdate)
                {
                    alpha -= 5;
                    _timer -= timerPerUpdate;
                    if (alpha <= 5)
                    {
                        fadeFinish = true;
                    }
                    _Color.A = (byte)alpha;
                }
            }
=======
            _backButton.Update(gameTime);
            _bord.Update(gameTime);
            _player.Update(gameTime);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            //Singleton.Instance.removeBubble.Clear();
            //Singleton.Instance.Shooting = false;
            _bord.ClearGame();
            _sfxBgInstance.Dispose();
            _sfxEndInstance2.Dispose();
            _game.ChangeScreen(ScreenSelect.Menu);
        }
    }
}
