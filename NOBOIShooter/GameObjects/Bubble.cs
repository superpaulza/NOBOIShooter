using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.GameObjects {
	public class Bubble : GameObject
	{

		public float speed;
		public float angle;

		public SoundEffectInstance deadSFX, stickSFX;
		internal Color color;

		public bool isMove;
		private bool shiftFloor;
		private Vector2 velocity;

		readonly int ballDrawWidth = Singleton.Instance.BALL_SHOW_WIDTH;
		readonly int ballObjectWidth = Singleton.Instance.BALL_ART_WIDTH;
		readonly int top = Singleton.Instance.GAME_DISPLAY_TOP;
		readonly int right = Singleton.Instance.GAME_DISPLAY_RIGHT;
		readonly int left = Singleton.Instance.GAME_DISPLAY_LEFT;

		private int minIndex = 0, maxIndex = (Singleton.Instance.GAME_DISPLAY_RIGHT - Singleton.Instance.GAME_DISPLAY_LEFT) / Singleton.Instance.BALL_SHOW_WIDTH;
		private int minLayer = 0, maxLayer = (Singleton.Instance.GAME_DISPLAY_BOTTOM - Singleton.Instance.GAME_DISPLAY_TOP) / Singleton.Instance.BALL_SHOW_WIDTH;

		private Vector2 positionLass;


		public Bubble(Texture2D texture) : base(texture)
		{
		}

		public Bubble(Texture2D texture, bool isShift) : base(texture)
		{
			shiftFloor = isShift;
		}

		public override void Update(GameTime gameTime, Bubble[,] gameObjects)
		{
			if (isMove)
			{
				velocity.X = (float)Math.Cos(angle) * speed;
				velocity.Y = (float)Math.Sin(angle) * speed;

				Position += velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

				DetectCollision(gameObjects);



				if (Position.Y <= top)
				{
					isMove = false;
					for (int i = 0; i < maxIndex; i++)
					{
						if (Position.X > left + ballDrawWidth * (i) && Position.X < left + ballDrawWidth * (i + 1))
						{
							gameObjects[0, i] = this;
							Position = new Vector2((i * ballDrawWidth) + (!shiftFloor ? left : left + (ballDrawWidth / 2)), top);
						}
					}
					Singleton.Instance.Shooting = false;
					//stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
					//stickSFX.Play();
				}

				if (Position.X - ballDrawWidth + ballObjectWidth <= left)
				{
					angle = -angle;
					angle += MathHelper.ToRadians(180);
				}

				if (Position.X + ballDrawWidth >= right)
				{
					angle = -angle;
					angle += MathHelper.ToRadians(180);
				}
			}
		}
		private void DetectCollision(Bubble[,] gameObjects)
		{
			for (int i = 0; i < maxLayer; i++)
			{
				for (int j = 0; j < maxIndex; j++)
				{
					if (gameObjects[i, j] != null && !gameObjects[i, j].isMove)
					{
						if (CheckCollision(gameObjects[i, j]) <= ballObjectWidth)
						{
							if (Position.X >= gameObjects[i, j].Position.X)
							{
								if (i % 2 == 0)
								{
									if (j == maxIndex - 1)
									{
										gameObjects[i + 1, j - 1] = this;
										gameObjects[i + 1, j - 1].Position = new Vector2(((j - 1) * ballDrawWidth) + (((i + 1) % 2) == 0 ? left : left + ballDrawWidth/2), ((i + 1) * ballDrawWidth) + top);
										CheckRemoveBubble(gameObjects, color, new Vector2(j - 1, i + 1));
									}
									else
									{
										gameObjects[i + 1, j] = this;
										gameObjects[i + 1, j].Position = new Vector2((j * ballDrawWidth) + (((i + 1) % 2) == 0 ? left : left + ballDrawWidth / 2), ((i + 1) * ballDrawWidth) + top);
										CheckRemoveBubble(gameObjects, color, new Vector2(j, i + 1));
									}
								}
								else
								{
									gameObjects[i + 1, j + 1] = this;
									gameObjects[i + 1, j + 1].Position = new Vector2(((j + 1) * ballDrawWidth) + (((i + 1) % 2) == 0 ? left : left + ballDrawWidth / 2), ((i + 1) * ballDrawWidth) + top);
									CheckRemoveBubble(gameObjects, color, new Vector2(j + 1, i + 1));
								}
							}
							else
							{
								if (i % 2 == 0)
								{
									gameObjects[i + 1, j - 1] = this;
									gameObjects[i + 1, j - 1].Position = new Vector2(((j - 1) * ballDrawWidth) + (((i + 1) % 2) == 0 ? left : left + ballDrawWidth / 2), ((i + 1) * ballDrawWidth) + top);
									CheckRemoveBubble(gameObjects, color, new Vector2(j - 1, i + 1));
								}
								else
								{
									gameObjects[(i + 1), j] = this;
									gameObjects[(i + 1), j].Position = new Vector2((j * ballDrawWidth) + (((i + 1) % 2) == 0 ? left : left + ballDrawWidth / 2), ((i + 1) * ballDrawWidth) + top);
									CheckRemoveBubble(gameObjects, color, new Vector2(j, i + 1));
								}
							}
							isMove = false;
							if (Singleton.Instance.removeBubble.Count >= 3)
							{
								Singleton.Instance.Score += Singleton.Instance.removeBubble.Count * 100;
								//deadSFX.Volume = Singleton.Instance.SFX_MasterVolume;
								//deadSFX.Play();
							}
							else if (Singleton.Instance.removeBubble.Count > 0)
							{
								//stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
								//stickSFX.Play();
								foreach (Vector2 v in Singleton.Instance.removeBubble)
								{
									gameObjects[(int)v.Y, (int)v.X] = new Bubble(_texture)
									{
										Name = "Bubble",
										Position = new Vector2((v.X * ballDrawWidth) + ((v.Y % 2) == 0 ? left : left + ballDrawWidth  / 2), (v.Y * ballDrawWidth) + top),
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
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, ballObjectWidth, ballObjectWidth), color);
			base.Draw(spriteBatch);
		}

		public int CheckCollision(Bubble other)
		{
			return (int)Math.Sqrt(Math.Pow(Position.X - other.Position.X, 2) + Math.Pow(Position.Y - other.Position.Y, 2));
		}

		public void CheckRemoveBubble(Bubble[,] bubbleAll, Color colorRemove, Vector2 bubbleInGrid)
		{

			if (bubbleInGrid.X < minIndex || bubbleInGrid.Y < minLayer) return;
			if (bubbleInGrid.X > maxIndex - 1 || bubbleInGrid.Y > maxLayer - 1) return;

			if (bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X] == null) return;
			if (bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X].color != colorRemove) return;

			Singleton.Instance.removeBubble.Add(bubbleInGrid);
			bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X] = null;

			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X + 1, bubbleInGrid.Y)); // Right
			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y)); // Left
			if (bubbleInGrid.Y % 2 == 0)
			{
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y - 1)); // Top Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y - 1)); // Top Left
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y + 1)); // Bot Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y + 1)); // Bot Left
			}
			else
			{
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X + 1, bubbleInGrid.Y - 1)); // Top Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y - 1)); // Top Left
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X + 1, bubbleInGrid.Y + 1)); // Bot Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y + 1)); // Bot 		}
			}
		}

		// Get the neighbors of the specified tile
		private List<Vector2> getNeighbors(Bubble finder, Bubble[,] gameObjects)
		{
			int tilerow = finder.shiftFloor ? 1 : 0; // Even or odd row
			List<Vector2> neighbors = new List<Vector2>();
			Vector2 tile = new Vector2();

			int[,,] neighborsoffsets = new int[,,] { { { 1, 0 }, {0, 1}, {-1, 1}, {-1, 0}, {-1, -1}, {0, -1}}, // Even row tiles
						{{1, 0}, {1, 1}, {0, 1}, {-1, 0}, {0, -1}, {1, -1}}};  // Odd row tiles

			// Get the neighbor offsets for the specified tile
			//int[,] n = neighborsoffsets[tilerow];

			// Get the neighbors
			for (var i = 0; i < neighborsoffsets.GetLength(2); i++)
			{
				// Neighbor coordinate
				float nx = tile.X + neighborsoffsets[tilerow, i, 0];
				float ny = tile.Y + neighborsoffsets[tilerow, i, 1];

				// Make sure the tile is valid
				if (nx >= 0 && nx < finder.Position.X + ballDrawWidth && ny >= 0 && ny < finder.Position.Y + ballDrawWidth)
				{
					neighbors.Add(new Vector2(nx, ny));
				}
			}

			return neighbors;
		}

		private Vector2[] sortNearedNeighbors(List<Vector2> neighbors, Vector2 target)
		{
			//neighbors.Sort((x, y) => (x.X < y.X));

			int size = neighbors.Count;
			Vector2[] sorted = new Vector2[size];
			float[] range = new float[size];
			for (int i = 0; i < size; i++)
				range[i] = (float)Math.Sqrt(Math.Pow(neighbors[i].X - target.X, 2) + Math.Pow(neighbors[i].Y - target.Y, 2));
			return sorted;
		}
		// Get the tile coordinate
		private Vector2 getTileCoordinate(int x, int y)
		{
			float tilex = maxLayer * ballDrawWidth + left;

			// X offset for odd rows
			if (x % 2 != 0)
			{
				tilex += ballDrawWidth / 2;
			}

			float tiley = y * ballDrawWidth + top;
			return new Vector2(tilex, tiley);
		}
		
		private void snapBubble(Bubble[,] bubbleGrid,Bubble target)
		{
			float centerx = Position.X + ballDrawWidth / 2 - left;
			float centery = Position.Y + ballDrawWidth / 2 - top;
			var gridpos = getGridPosition(centerx, centery);

			var targetpos = target.getGridPosition();
			bool thisFloorIsShift = ((int)targetpos.Y - (int)gridpos.Y) % 2 == 0 ? target.shiftFloor : !target.shiftFloor;

			int gridx = (int)Math.Floor(gridpos.X);
			int gridy = (int)Math.Floor(gridpos.Y);
			if (gridy < 0)
			{
				gridy = 0;
			}
			if (gridy >= bubbleGrid.GetLength(0))
			{
				gridy = bubbleGrid.GetLength(0) - 1;
			}

			if (gridx < 0)
			{
				gridx = 0;
			}
			if (gridx >= bubbleGrid.GetLength(1))
			{
				gridx = bubbleGrid.GetLength(1) - 1;
				//if (gridy % 2 != 0) gridx -= 1;
			}
			if (bubbleGrid[gridy, gridx] != null)
			{
				for (int checkY = gridy ; checkY < bubbleGrid.GetLength(0); checkY++)
				{
					int moveX = gridx;
					moveX += Position.X <= target.Position.X ? -1 : 1;

					bool moveFloorIsShift = (checkY - gridy) % 2 == 0 ? thisFloorIsShift : !thisFloorIsShift;
					//if (checkY % 2 == 0 && firstFloorIsShift) moveX--;

					if (moveX < 0) moveX = 0;
					if (moveX >= bubbleGrid.GetLength(1)) _ = bubbleGrid.GetLength(1) - 1;


					int xoffset = left;
					if (moveFloorIsShift) xoffset += ballDrawWidth / 2;
					

					gridy = checkY;
					if (bubbleGrid[checkY, gridx] == null)
					{
						thisFloorIsShift = moveFloorIsShift;
						isMove = false;
						Position = new Vector2(gridx * ballDrawWidth + xoffset, checkY * ballDrawWidth + top);
						bubbleGrid[checkY, gridx] = this;
						CheckRemoveBubble(bubbleGrid, color, new Vector2(gridx, checkY));
						break;
					}
					/*
					else if (bubbleGrid[checkY, moveX] == null)
                    {
						isMove = false;
						Position = new Vector2((moveX) * ballDrawWidth + xoffset, checkY * ballDrawWidth + top);
						bubbleGrid[checkY, moveX] = this;

						an = true;
						break;
					} */

				}
			}
			else
			{
                int xoffset = left;
				if (thisFloorIsShift) xoffset += ballDrawWidth / 2;
				shiftFloor = thisFloorIsShift;
				isMove = false;
				Position = new Vector2(gridx * ballDrawWidth + xoffset, gridy * ballDrawWidth + top);
				CheckRemoveBubble(bubbleGrid, color, new Vector2(gridx, gridy));
				bubbleGrid[gridy, gridx] = this;
			}

			/*
			if (an)
			{
				//ah.bubble.visible = false;
				bubbleGrid[(int)gridy, (int)gridx]  = this;
				if (U())
				{
					return;
				}
			}
				M = G(gridx, gridy, true, true, false);
				if (M.length >= 3)
				{
					m(v.removecluster);
				return;
	
			}
			}
			q++;
			if (q >= 5)
			{
				ab();
				q = 0;
				u = (u + 1) % 2;
				if (U())
				{
					return
	
			}
			}
			o();
			m(v.ready)
			*/

	/*
			if (gridy > 0 && bubbleGrid[(int)gridy, (int)gridx] == null)
            {
				positionLass = gridpos;
				return false;
			}
			if(positionLass != null && bubbleGrid[(int)gridy, (int)gridx] != null)
            {
				bubbleGrid[(int)gridy, (int)gridx] = this;
				isMove = false;
				return true;
            }
			return false;*/

		}

		private Vector2 getGridPosition(float x, float y)
		{
			float gridy = (float) ((y) / ballDrawWidth);

			// Check for offset
			float xoffset = 0;
			if ((gridy) % 2 == 0)
			{
				xoffset = ballDrawWidth / 2;
			}
			float gridx = (float) ((x - xoffset) / ballDrawWidth);

			return new Vector2(gridx, gridy);
		}

		private Vector2 getGridPosition()
		{
			float gridy = (float)Math.Floor((Position.Y - top) / ballDrawWidth);

			// Check for offset
			float xoffset = left;
			if ((gridy) % 2 == 0)
			{
				xoffset = ballDrawWidth / 2;
			}

			float gridx = (float)Math.Floor((Position.X - xoffset) / ballDrawWidth);

			return new Vector2(gridx, gridy);
		}

		
	}
}