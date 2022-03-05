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
			for (int iLayer = 0; iLayer < maxLayer; iLayer++)
			{
				for (int indexBall = 0; indexBall < maxIndex; indexBall++)
				{
					if (gameObjects[iLayer, indexBall] != null && !gameObjects[iLayer, indexBall].isMove)
					{
						if (CheckCollision(gameObjects[iLayer, indexBall]) < ballDrawWidth)
						{
							// -- MY LOGIC UN SUCCESSFULL
							snapBubble(gameObjects, gameObjects[iLayer, indexBall], false);
							/*
						int gridy = (int)Math.Ceiling((Position.Y - top) / ballDrawWidth);

						bool hitItemShift = gameObjects[iLayer, indexBall].shiftFloor;
						bool thisFloorShift = (iLayer == gridy) ? hitItemShift : !hitItemShift;

						// Check for offset
						int xoffset = thisFloorShift ? left + (ballDrawWidth / 2) : left;


						int gridx = (int)Math.Round((Position.X - xoffset) / ballDrawWidth);
						//if (!thisFloorShift) gridx++;
						// TODO: check is not out side
						//if (gridx * ballDrawWidth + xoffset >= right) continue;

						// TODO : find next nearesed
						if (gridx >= 0 && gridy >= 0 && gridx < gameObjects.GetLength(1) && gridy < gameObjects.GetLength(0) && gameObjects[gridy, gridx] == null)
						{
							isMove = false;
							//shiftFloor = thisFloorShift;
							//Position = new Vector2(gridx * ballDrawWidth, gridy * ballDrawWidth + top);
							gameObjects[gridy, gridx] = this;
							//CheckRemoveBubble(gameObjects, color, new Vector2(gridx, gridy));
						}
						else
						{
							if (iLayer == gridy && indexBall == gridx)
							{

								int newgridx = Position.X >= gameObjects[iLayer, indexBall].Position.X ? (!thisFloorShift ? gridx + 1 : gridx) : ( !thisFloorShift ?  gridx - 1 : gridx - 1);
								//int newgridx = Position.X >= gameObjects[iLayer, indexBall].Position.X ? gridx + 1 : gridx - 1;
								if (newgridx > 0 && newgridx < gameObjects.GetLength(1) && gameObjects[gridy, newgridx] == null)
								{
									Position = new Vector2((newgridx) * ballDrawWidth + xoffset, gridy * ballDrawWidth + top);
									isMove = false;
									gameObjects[gridy, (newgridx)] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2((newgridx), gridy));
								}

							}
							else
							{
								int newgridx = iLayer == gridy ? indexBall <= gridx ? gridx - 1 : gridx + 1 : gridx;// thisFloorShift ? gridx - 1: gridx ;
								int nextgridy = gridy + 1;
								int nextgridx = gridx;//!thisFloorShift ? gridx - 2 : gridx - 3;
								int nextoffset = !thisFloorShift ? left + (ballDrawWidth / 2) : left;

								if (gameObjects[gridy, newgridx] == null && newgridx < gameObjects.GetLength(1))
								{
									Position = new Vector2((newgridx) * ballDrawWidth + xoffset, gridy * ballDrawWidth + top);
									isMove = false;
									gameObjects[gridy, (newgridx)] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2((newgridx), gridy));
								} 

								else if (gameObjects[nextgridy, nextgridx] == null && nextgridx < gameObjects.GetLength(1) && nextgridy < gameObjects.GetLength(0))
								{
									Position = new Vector2((nextgridx) * ballDrawWidth + nextoffset, nextgridy * ballDrawWidth + top);
									isMove = false;
									gameObjects[nextgridy, (nextgridx)] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2((nextgridx), nextgridy));
								}
							}
						}

						if (gameObjects[gridy, gridx] != null) { 


							List<Vector2> neighbors =  getNeighbors(this,gameObjects);
							foreach (Vector2 n in neighbors) {
								int newgridx = gridx + (int)n.X, newgridy = gridy + (int)n.Y;
								if (newgridx < 0 || newgridx > gameObjects.GetLength(1) || newgridy < 0 || newgridy > gameObjects.GetLength(0)) continue;

								if (gameObjects[newgridy, newgridx] == null) { 							
									Position = new Vector2(newgridx * ballDrawWidth + xoffset, newgridy * ballDrawWidth + top);
									gameObjects[newgridy, gridx] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(newgridx, newgridy));
									break;
								}
							}

						} else
						{


						}
*/


							/*
							// -- OLD LOGIC
							bool checkShift = gameObjects[iLayer, indexBall].shiftFloor;
							if (Position.X >= gameObjects[iLayer, indexBall].Position.X)
							{

								if (!checkShift)
								{
										Position = new Vector2(((indexBall+1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + top);
										gameObjects[iLayer + 1, indexBall + 1] = this;
										CheckRemoveBubble(gameObjects, color, new Vector2(indexBall, iLayer + 1));
									
								}
								else
								{
									shiftFloor = true;
									Position = new Vector2(((indexBall + 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + top);
									gameObjects[iLayer + 1, indexBall + 1] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall + 1, iLayer + 1));
								}
							}
							else
							{
								if (!checkShift)
								{
									Position = new Vector2(((indexBall - 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + top);
									gameObjects[iLayer + 1, indexBall - 1] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall - 1, iLayer + 1));
								}
								else
								{
									shiftFloor = true;
									Position = new Vector2((indexBall * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * 70) + ballObjectWidth);
									gameObjects[(iLayer + 1), indexBall] = this;
									CheckRemoveBubble(gameObjects, color, new Vector2(indexBall, iLayer + 1));
								}
							}
							*/


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
										Position = new Vector2((targetBubble.X * ballDrawWidth) + ((targetBubble.Y % 2) == 0 ? left : left + (ballDrawWidth / 2)), top + (targetBubble.Y * ballDrawWidth)),
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
		
		private void snapBubble(Bubble[,] bubbleGrid,Bubble target, bool firstFloorIsShift)
		{

			float centerx = Position.X + ballDrawWidth / 2 - left;
			float centery = Position.Y + ballDrawWidth / 2 - top;
			var gridpos = getGridPosition(centerx, centery);
			if (gridpos.Y < 0)
			{
				gridpos.Y = 0;

			}
			if (gridpos.Y >= bubbleGrid.GetLength(0))
			{
				gridpos.Y = bubbleGrid.GetLength(0) - 1;

			}


			if (gridpos.X < 0)
			{
				gridpos.X = 0;

			}
			if (gridpos.X >= bubbleGrid.GetLength(1))
			{
				gridpos.X = bubbleGrid.GetLength(1) - 1;
				if (gridpos.Y % 2 != 0) gridpos.X -= 1;

			}
		

			var an = false;
			if (bubbleGrid[(int)gridpos.Y, (int)gridpos.X] != null)
			{
				for (int checkY = (int)gridpos.Y ; checkY < bubbleGrid.GetLength(0); checkY++)
				{
					//int xrange = 1; // Position.X <= target.Position.X ? -1 : 1; 
					int xoffset = left;
						if (checkY % 2 == 0 && shiftFloor) xoffset += ballDrawWidth / 2; 
						if (checkY % 2 != 0 && !shiftFloor) xoffset += ballDrawWidth / 2; 

						gridpos.Y = checkY;
					if (bubbleGrid[ checkY , (int)gridpos.X] == null)
					{
						isMove = false;
						Position = new Vector2(((int)gridpos.X) * ballDrawWidth + xoffset, ((int)gridpos.Y) * ballDrawWidth + top);
						bubbleGrid[(int)gridpos.Y, (int)gridpos.X] = this;
						an = true;
						break;
					}/* else if (bubbleGrid[checkY, (int)gridpos.X + xrange] == null)
                    {
						isMove = false;
						Position = new Vector2(((int)gridpos.X + xrange) * ballDrawWidth + xoffset, ((int)gridpos.Y) * ballDrawWidth + top);
						bubbleGrid[(int)gridpos.Y, (int)gridpos.X + xrange] = this;

						an = true;
						break;
					}*/
				}
			}
			else
			{
				an = true;
				int xoffset = left;
				if ((int)gridpos.Y % 2 == 0 && firstFloorIsShift) xoffset += ballDrawWidth / 2;
				if ((int)gridpos.Y % 2 != 0 && !firstFloorIsShift) xoffset += ballDrawWidth / 2;

				isMove = false;
				Position = new Vector2(((int)gridpos.X) * ballDrawWidth + xoffset, (int)gridpos.Y * ballDrawWidth + top);
				bubbleGrid[(int)gridpos.Y, (int)gridpos.X] = this;
			}

			/*
			if (an)
			{
				//ah.bubble.visible = false;
				bubbleGrid[(int)gridpos.Y, (int)gridpos.X]  = this;
				if (U())
				{
					return;
				}
			}
				M = G(gridpos.X, gridpos.Y, true, true, false);
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
			if (gridpos.Y > 0 && bubbleGrid[(int)gridpos.Y, (int)gridpos.X] == null)
            {
				positionLass = gridpos;
				return false;
			}
			if(positionLass != null && bubbleGrid[(int)gridpos.Y, (int)gridpos.X] != null)
            {
				bubbleGrid[(int)gridpos.Y, (int)gridpos.X] = this;
				isMove = false;
				return true;
            }
			return false;*/

		}

		private Vector2 getGridPosition(float x, float y)
		{
			float gridy = (float)Math.Floor((y) / ballDrawWidth);

			// Check for offset
			float xoffset = 0;
			if ((gridy) % 2 == 0)
			{
				xoffset = ballDrawWidth / 2;
			}
			float gridx = (float)Math.Floor((x - xoffset) / ballDrawWidth);

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