using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using NOBOIShooter.Controls;
using NOBOIShooter.GameObjects;
using System;

namespace NOBOIShooter.States
{
    //Game screen
    public class GameState : State
    {
        private SpriteFont myText;
        Texture2D whiteRectangle;
        private Texture2D BackImage;
        private Button BackButton;
        private int posX, posY;
        private Vector2 Position, gunPoint;
        private int width, height;
        Texture2D ballBubble, gun, line;
        private Bubble[,] bubbleArea = new Bubble[9, 8];
        private Color _Color;
        private Random random = new Random();
        private Shooter Gun;
        private bool gameOver = false, gameWin = false, fadeFinish = false;
        private float _timer = 0f;
        private float __timer = 0f;
        private float PlayTime = 0f;
        private int alpha = 255;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 5f; // 30f
        private SoundEffect Effect1, Effect2;
        private SoundEffectInstance Instance1, Instance2; 
        private int count = 0;


        int ballDrawWidth = Singleton.Instance.BALL_SHOW_WIDTH;
        int top = Singleton.Instance.GAME_DISPLAY_TOP;
        int right = Singleton.Instance.GAME_DISPLAY_RIGHT;
        int left = Singleton.Instance.GAME_DISPLAY_LEFT;


        public GameState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            myText = _content.Load<SpriteFont>("Fonts/Font");
            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            BackImage = _content.Load<Texture2D>("Controls/BackButton");
            ballBubble = _content.Load<Texture2D>("Item/bubble");
            //gun = _content.Load<Texture2D>("Item/Gun");
            gun = _content.Load<Texture2D>("Item/bubble-shoot");
            line = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] {Color.White});

            BackButton = new Button(BackImage)
            {
                Position = new Vector2(1200, 20),
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8 - (i % 2); j++)
                {
                    bubbleArea[i, j] = new Bubble(ballBubble, (i % 2) == 0)
                    {
                        Name = "Bubble",
                        Position = new Vector2((j * ballDrawWidth) + ((i % 2) == 0 ? left : left + ballDrawWidth / 2),top + (i * ballDrawWidth)),
                        color = GetRandomColor(),
                        isMove = false,
                    };
                }
            }
            Gun = new Shooter(gun, ballBubble, line)
            {
                Name = "Shooter",
            };

            //
            posX = 30;
            posY = 30;
            Position = new Vector2(posX, posY);
            width = 800;
            height = 650;

            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            gunPoint = new Vector2(posX + width / 2 - 20, posY + height - 20);


            BackButton.Click += BackButton_Click;

            // BG
            ControllerBGM(content);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Singleton.Instance.removeBubble.Clear();
            Singleton.Instance.Shooting = false;
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            Instance1.Dispose();
            Instance2.Dispose();
        }

        private void ControllerBGM(ContentManager content)
        {
            Effect1 = content.Load<SoundEffect>("BGM/BACK");
            Effect2 = content.Load<SoundEffect>("BGM/GameOverBGM");

            Instance1 = Effect1.CreateInstance();
            Instance2 = Effect2.CreateInstance();
            Instance1.IsLooped = true;

            Instance1.Play();
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BackButton.Draw(gameTime, spriteBatch);
            
            spriteBatch.Draw(whiteRectangle, new Rectangle(320, 40, 640, 560), Color.Chocolate);
            
/*
            //spriteBatch.Draw(gun, new Rectangle((int)gunPoint.X, (int)gunPoint.Y, size - 2, size - 2), Color.Red);
            //spriteBatch.Draw(whiteRectangle, new Vector2(posX, posY), null, Color.Red, MathHelper.Pi / 2, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(gun, gunPoint, null, Color.Red, getShooterAngle(), Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(myText, "Can u see me? \n sorry It's too white!", new Vector2(900, 100), Color.Black);
            //spriteBatch.DrawString(myText, "Post : " + Mouse.GetState().X + " , " + Mouse.GetState().Y, new Vector2(900, 400), Color.Black);

            */

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bubbleArea[i, j] != null)
                        bubbleArea[i, j].Draw(spriteBatch);
                }
            }
            Gun.Draw(spriteBatch);
            /*
            spriteBatch.DrawString(myText, "Score : " + Singleton.Instance.Score, new Vector2(1060, 260), _Color);
            spriteBatch.DrawString(myText, "Time : " + Timer.ToString("F"), new Vector2(20, 260), _Color);
            spriteBatch.DrawString(myText, "Next Time : " + (tickPerUpdate - __timer).ToString("F"), new Vector2(20, 210), _Color);
            */
            Vector2 textDisply = new Vector2(1000, 200);
            Rectangle FullDisplay = new Rectangle(100, 0, Singleton.Instance.ScreenWidth - 200, Singleton.Instance.ScreenHeight);
            if (gameOver)
            {
                spriteBatch.Draw(whiteRectangle, FullDisplay, new Color(0, 0, 0, 210));
                spriteBatch.DrawString(myText, "Game Over", textDisply, Color.White);
            }

            if (gameWin)
            {
                spriteBatch.Draw(whiteRectangle, FullDisplay, new Color(0, 0, 0, 210));
                spriteBatch.DrawString(myText, "You Won", textDisply, Color.White);
            }

            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(whiteRectangle, FullDisplay, _Color);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            BackButton.Update(gameTime);
            if (!gameOver && !gameWin)
            {
                //ObjeCT UPDATE
                foreach (Bubble bubble in bubbleArea) if (bubble != null) bubble.Update(gameTime, bubbleArea); 
                Gun.Update(gameTime, bubbleArea);


                PlayTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                for (int i = 0; i < 8; i++)
                {
                    if (bubbleArea[6, i] != null)
                    {
                        gameOver = true;
                        Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        Singleton.Instance.BestTime = PlayTime.ToString("F");
                    }
                }

                //Check ball flying
                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 7 - (i % 2); j++)
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
                    }
                }
                
                __timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (__timer >= tickPerUpdate)
                {
                    // Check game over before scroll
                    for (int i = 6; i < 9; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
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
                    for (int i = 5; i >= 0; i--)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubbleArea[i + 2, j] = bubbleArea[i, j];
                        }
                    }
                    // Draw new scroll position
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            if (bubbleArea[i, j] != null)
                            {
                                bubbleArea[i, j].Position = new Vector2((j * ballDrawWidth) + ((i % 2) == 0 ? left : left + ballDrawWidth/2),top + (i * ballDrawWidth));
                            }
                        }
                    }
                    //Random ball after scroll
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubbleArea[i, j] = new Bubble(ballBubble)
                            {
                                Name = "Bubble",
                                Position = new Vector2((j * ballDrawWidth) + ((i % 2) == 0 ? left : left + ballDrawWidth / 2),top + (i * ballDrawWidth)),
                                color = GetRandomColor(),
                                isMove = false,
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

        }


        public Color GetRandomColor()
        {
            Color _color = Color.Black;
            switch (random.Next(0, 6))
            {
                case 0:
                    _color = Color.White;
                    break;
                case 1:
                    _color = Color.Blue;
                    break;
                case 2:
                    _color = Color.Yellow;
                    break;
                case 3:
                    _color = Color.Red;
                    break;
                case 4:
                    _color = Color.Green;
                    break;
                case 5:
                    _color = Color.Purple;
                    break;
            }
            return _color;
        }
        
        public bool CheckWin(Bubble[,] bubble)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8 - (i % 2); j++)
                {
                    if (bubble[i, j] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
