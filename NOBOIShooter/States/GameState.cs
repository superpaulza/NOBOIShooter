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
        private int BUBBLE_WIDTH = Singleton.Instance.BubbleGridWidth;
        private int top = Singleton.Instance.GameDisplayBorderTop;
        private int right = Singleton.Instance.GameDisplayBorderRight;
        private int left = Singleton.Instance.GameDisplayBorderLeft;
        private int bottom = Singleton.Instance.GameDisplayBorderBottom;

        private static int GAME_GRID_X = (Singleton.Instance.GameDisplayBorderRight - Singleton.Instance.GameDisplayBorderLeft) / Singleton.Instance.BubbleGridWidth;
        private static int GAME_GRID_Y = (Singleton.Instance.GameDisplayBorderBottom - Singleton.Instance.GameDisplayBorderTop) / Singleton.Instance.BubbleGridWidth;

        private int GAME_BUBBLE_START = 2;
        private int GAME_BUBBLE_DEATH = GAME_GRID_Y - 1;

        private Bubble[,] bubbleArea = new Bubble[GAME_GRID_Y + 2, GAME_GRID_X];
        
        private SpriteFont myText;
        private Texture2D BackImage;
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


        
        public GameState(Main game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            myText = _content.Load<SpriteFont>("Fonts/Font");
     
            BackImage = _content.Load<Texture2D>("Controls/BackButton");
            _bubbleTexture = _content.Load<Texture2D>("Item/bubble");
            shooterTexture = _content.Load<Texture2D>("Item/bubble-shoot");

            line = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] {Color.White});

            BackButton = new Button(BackImage)
            {
                Position = new Vector2(1200, 20),
            };

            for (int i = 0; i < GAME_BUBBLE_START; i++)
            {
                for (int j = 0; j < GAME_GRID_X; j++)
                {
                   
                    int BallPositionX = (j * BUBBLE_WIDTH) + ((i % 2) == 0 ? left : left + BUBBLE_WIDTH / 2);
                    if (BallPositionX + BUBBLE_WIDTH  > right) break;
                    int BallPositionY = top + (i * BUBBLE_WIDTH);
                    bubbleArea[i, j] = new Bubble(_bubbleTexture, (i % 2) != 0)
                    {
                        Name = "Bubble",
                        Position = new Vector2(BallPositionX, BallPositionY),
                        _color = GetRandomColor(),
                        IsMoving = false,
                    };
                }
            }
            Player = new Shooter(shooterTexture, _bubbleTexture, line)
            {
                Name = "Shooter",
            };


            line = new Texture2D(graphicsDevice, 1, 1);
            line.SetData(new[] { Color.White });


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
            /*

            spriteBatch.DrawString(myText, "Can u see me? \n sorry It's too white!", new Vector2(900, 100), Color.Black);
            //spriteBatch.DrawString(myText, "Post : " + Mouse.GetState().X + " , " + Mouse.GetState().Y, new Vector2(900, 400), Color.Black);
            */

            BackButton.Draw(gameTime, spriteBatch);
            
            spriteBatch.Draw(line, new Rectangle(left, top, right - left, bottom - top), Color.Chocolate);
            spriteBatch.Draw(line, new Rectangle(left, top, right - left, (GAME_GRID_Y)*BUBBLE_WIDTH), Color.Gray);
            spriteBatch.Draw(line, new Rectangle(left, top, right - left, (GAME_BUBBLE_DEATH)*BUBBLE_WIDTH), Color.Brown);
            spriteBatch.Draw(line, new Rectangle(left, top, right - left, (GAME_BUBBLE_DEATH - 2)*BUBBLE_WIDTH), Color.Orange);
            spriteBatch.Draw(line, new Rectangle(left, top, right - left, (GAME_BUBBLE_START)*BUBBLE_WIDTH), Color.Green);
            


            foreach (Bubble _bubble in bubbleArea) if (_bubble != null) _bubble.Draw(spriteBatch);
        
            Player.Draw(spriteBatch);
            /*
            spriteBatch.DrawString(myText, "Score : " + Singleton.Instance.Score, new Vector2(1060, 260), _Color);
            spriteBatch.DrawString(myText, "Time : " + Timer.ToString("F"), new Vector2(20, 260), _Color);
            spriteBatch.DrawString(myText, "Next Time : " + (tickPerUpdate - __timer).ToString("F"), new Vector2(20, 210), _Color);
            */
            Vector2 textDisply = new Vector2(1000, 200);
            Rectangle FullDisplay = new Rectangle(100, 0, Singleton.Instance.ScreenWidth - 200, Singleton.Instance.ScreenHeight);
            if (gameOver)
            {
                spriteBatch.Draw(line, FullDisplay, new Color(0, 0, 0, 210));
                spriteBatch.DrawString(myText, "Game Over", textDisply, Color.White);
            }

            if (gameWin)
            {
                spriteBatch.Draw(line, FullDisplay, new Color(0, 0, 0, 210));
                spriteBatch.DrawString(myText, "You Won", textDisply, Color.White);
            }

            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(line, FullDisplay, _Color);
            }

            spriteBatch.DrawString(myText, "Time", new Vector2(200, 100), Color.Black);
            spriteBatch.DrawString(myText, "xxx", new Vector2(200, 140), Color.Black);
            spriteBatch.DrawString(myText, "Score", new Vector2(900,100), Color.Black);
            spriteBatch.DrawString(myText, "xxx", new Vector2(900, 140), Color.Black); 
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
                    
                  
                    //Random ball after scroll
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

        }


        public Color GetRandomColor()
        {
            Color _color = Color.Black;
            switch (_random.Next(0, 4))
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
        
        public bool CheckWin(Bubble[,] allBubble)
        {
            // Check all position in null
            foreach(Bubble _bubble in allBubble) if (_bubble != null) return false;
     
            return true;
        }
    }
}
