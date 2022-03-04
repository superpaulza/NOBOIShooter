using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.GameObjects {
	public class Bubble : GameObject {

		public float speed;
		public float angle;
		
		public SoundEffectInstance deadSFX , stickSFX;
        internal Color color;

		public bool isMove;
		private bool shiftFloor;
		private Vector2 velocity;

		readonly int ballDrawWidth = Singleton.Instance.BALL_SHOW_WIDTH;
		readonly int ballObjectWidth = Singleton.Instance.BALL_ART_WIDTH;
		readonly int top = Singleton.Instance.GAME_DISPLAY_TOP;
		readonly int right = Singleton.Instance.GAME_DISPLAY_RIGHT;
		readonly int left = Singleton.Instance.GAME_DISPLAY_LEFT;



        public Bubble(Texture2D texture) : base(texture)
		{
		}

		public Bubble(Texture2D texture,bool isShift) : base(texture) {
			shiftFloor = isShift;
		}

		public override void Update(GameTime gameTime, Bubble[,] gameObjects) {
			if (isMove) {
				velocity.X = (float) Math.Cos(angle) * speed;
				velocity.Y = (float)Math.Sin(angle) * speed;

				Position += velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

				DetectCollision(gameObjects);

				
				
				if (Position.Y <= top) {
					isMove = false;
					for (int i = 0; i < 8; i++)
                    {
						if (Position.X > left + ballDrawWidth * (i) && Position.X < left + ballDrawWidth * (i+1))
						{
							gameObjects[0, i] = this;
							Position = new Vector2((i * ballDrawWidth) + (!shiftFloor ? left : left + (ballDrawWidth/2)), top);
						}
                    }
					Singleton.Instance.Shooting = false;
					//stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
					//stickSFX.Play();
				}

				if (Position.X <= left) {
					angle = -angle;
					angle += MathHelper.ToRadians(180);
				}

				if (Position.X >= right) {
					angle = -angle;
					angle += MathHelper.ToRadians(180);
				}
			}
		}

		private void DetectCollision(Bubble[,] gameObjects)
		{
			int gameLayer = 8, ballStack = 7;
			for (int iLayer = 0; iLayer <= gameLayer; iLayer++)
			{
				for (int indexBall = 0; indexBall <= ballStack; indexBall++)
				{
					if (gameObjects[iLayer, indexBall] != null && !gameObjects[iLayer, indexBall].isMove)
					{
						if (CheckCollision(gameObjects[iLayer, indexBall]) <= ballObjectWidth)
						{
							if (Position.X >= gameObjects[iLayer, indexBall].Position.X)
							{
								if (iLayer % 2 == 0)
								{
									if (indexBall == 7)
									{
										Position = new Vector2(((indexBall - 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + 40);
										gameObjects[iLayer + 1, indexBall - 1] = this;
										CheckRemoveBubble(gameObjects, color, new Vector2(indexBall - 1, iLayer + 1));
									}
									else
									{
										Position = new Vector2((indexBall * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + 40);
										gameObjects[iLayer + 1, indexBall] = this;
										CheckRemoveBubble(gameObjects, color, new Vector2(indexBall, iLayer + 1));
									}
								}
								else
								{
									Position = new Vector2(((indexBall + 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + 40);
									gameObjects[iLayer + 1, indexBall + 1] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall + 1, iLayer + 1));
								}
							}
							else
							{
								if (iLayer % 2 == 0)
								{
									Position = new Vector2(((indexBall - 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + 40);
									gameObjects[iLayer + 1, indexBall - 1] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall - 1, iLayer + 1));
								}
								else
								{
									Position = new Vector2((indexBall * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * 70) + ballObjectWidth);
									gameObjects[(iLayer + 1), indexBall] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall, iLayer + 1));
								}
							}

							isMove = false;

							int minToRemove = 3;
							int scorePerRemovedBall = 100;
							if (Singleton.Instance.removeBubble.Count >= minToRemove)
							{

								Singleton.Instance.Score += Singleton.Instance.removeBubble.Count * scorePerRemovedBall;
								//deadSFX.Volume = Singleton.Instance.SFX_MasterVolume;
								//deadSFX.Play();

							}
							else if (Singleton.Instance.removeBubble.Count > 0)
							{

								//stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
								//stickSFX.Play();

								foreach (Vector2 targetBubble in Singleton.Instance.removeBubble)
								{
									gameObjects[(int)targetBubble.Y, (int)targetBubble.X] = new Bubble(_texture)
									{
										Name = "Bubble",
										Position = new Vector2((targetBubble.X * ballDrawWidth) + ((targetBubble.Y % 2) == 0 ? left : left + (ballDrawWidth/2)),top + (targetBubble.Y * ballDrawWidth)),
										color = color,
										isMove = false,
									};
								}

							}
							Singleton.Instance.removeBubble.Clear();

							Singleton.Instance.Shooting = false;

							return;
						}

					}
				}
			}
		}
		public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(_texture, new Rectangle ((int)Position.X, (int)Position.Y,ballObjectWidth,ballObjectWidth), color);
			base.Draw(spriteBatch);
		}

		public int CheckCollision(Bubble other) {
			return (int)Math.Sqrt(Math.Pow(Position.X - other.Position.X, 2) + Math.Pow(Position.Y - other.Position.Y, 2));
		}

		public void CheckRemoveBubble(Bubble[,] bubbleAll, Color colorRemove, Vector2 thisBubble) {
			int minLayer = 0, maxLayer = 8;
			int minIndex = 0, maxIndex = 7;
			if (thisBubble.X < minIndex || thisBubble.Y < minLayer) return;
			if (thisBubble.X > maxIndex || thisBubble.Y > maxLayer) return;

			if (bubbleAll[(int)thisBubble.Y, (int)thisBubble.X] == null) return;
			if (bubbleAll[(int)thisBubble.Y, (int)thisBubble.X].color != colorRemove) return;

			Singleton.Instance.removeBubble.Add(thisBubble);
			bubbleAll[(int)thisBubble.Y, (int)thisBubble.X] = null;

			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X + 1, thisBubble.Y)); // Right
			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X - 1, thisBubble.Y)); // Left
			if (thisBubble.Y % 2 == 0) {
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X, thisBubble.Y - 1)); // Top Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X - 1, thisBubble.Y - 1)); // Top Left
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X, thisBubble.Y + 1)); // Bot Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X - 1, thisBubble.Y + 1)); // Bot Left
			} else {
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X + 1, thisBubble.Y - 1)); // Top Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X, thisBubble.Y - 1)); // Top Left
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X + 1, thisBubble.Y + 1)); // Bot Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(thisBubble.X, thisBubble.Y + 1)); // Bot 		}
			}
		}
	}
}