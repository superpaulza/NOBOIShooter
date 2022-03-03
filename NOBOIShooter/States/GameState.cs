using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Bubble[,] bubble = new Bubble[9, 8];
        private Color _Color;
        private Random random = new Random();
        private Gun Gun;
        private bool gameOver = false, gameWin = false, fadeFinish = false;
        private float _timer = 0f;
        private float __timer = 0f;
        private float Timer = 0f;
        private int alpha = 255;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 5f; // 30f

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
                    bubble[i, j] = new Bubble(ballBubble)
                    {
                        Name = "Bubble",
                        Position = new Vector2((j * 80) + ((i % 2) == 0 ? 320 : 360), (i * 70) + 40),
                        color = GetRandomColor(),
                        IsActive = false,
                    };
                }
            }
            Gun = new Gun(gun, ballBubble, line)
            {
                Name = "Gun",
                Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 50, Singleton.Instance.ScreenHeight - 120),
                color = Color.White,
                IsActive = true,
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
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Singleton.Instance.removeBubble.Clear();
            Singleton.Instance.Shooting = false;
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }




        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            BackButton.Draw(gameTime, spriteBatch);
            
            spriteBatch.Draw(whiteRectangle, new Rectangle(320, 0, 630, Singleton.Instance.ScreenHeight), Color.Chocolate);
            
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
                    if (bubble[i, j] != null)
                        bubble[i, j].Draw(spriteBatch);
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
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (bubble[i, j] != null)
                            bubble[i, j].Update(gameTime, bubble);
                    }
                }
                Gun.Update(gameTime, bubble);
                Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                for (int i = 0; i < 8; i++)
                {
                    if (bubble[8, i] != null)
                    {
                        gameOver = true;
                        Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        Singleton.Instance.BestTime = Timer.ToString("F");
                    }
                }
                //Check ball flying
                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 7 - (i % 2); j++)
                    {
                        if (i % 2 != 0)
                        {
                            if (bubble[i - 1, j] == null && bubble[i - 1, j + 1] == null)
                            {
                                bubble[i, j] = null;
                            }
                            if (bubble[i, 1] == null && bubble[i - 1, 0] == null && bubble[i - 1, 1] == null)
                            {
                                bubble[i, 0] = null;
                            }
                            if (bubble[i, 5] == null && bubble[i - 1, 7] == null && bubble[i - 1, 6] == null)
                            {
                                bubble[i, 6] = null;
                            }
                        }
                        else
                        {
                            if (bubble[i - 1, j - 1] == null && bubble[i - 1, j] == null)
                            {
                                bubble[i, j] = null;
                            }
                            if (bubble[i - 1, 0] == null && bubble[i, 1] == null)
                            {
                                bubble[i, 0] = null;
                            }
                            if (bubble[i - 1, 6] == null && bubble[i, 6] == null)
                            {
                                bubble[i, 7] = null;
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
                            if (bubble[i, j] != null)
                            {
                                gameOver = true;
                                Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                                Singleton.Instance.BestTime = Timer.ToString("F");
                            }
                        }
                    }
                    // Scroll position 
                    for (int i = 5; i >= 0; i--)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubble[i + 2, j] = bubble[i, j];
                        }
                    }
                    // Draw new scroll position
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            if (bubble[i, j] != null)
                            {
                                bubble[i, j].Position = new Vector2((j * 80) + ((i % 2) == 0 ? 320 : 360), (i * 70) + 40);
                            }
                        }
                    }
                    //Random ball after scroll
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubble[i, j] = new Bubble(ballBubble)
                            {
                                Name = "Bubble",
                                Position = new Vector2((j * 80) + ((i % 2) == 0 ? 320 : 360), (i * 70) + 40),
                                color = GetRandomColor(),
                                IsActive = false,
                            };
                        }
                    }

                    __timer -= tickPerUpdate;
                }

                gameWin = CheckWin(bubble);

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
