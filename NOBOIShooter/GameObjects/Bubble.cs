using System;
using System.Collections.Generic;
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

		private int minIndex = 0, maxIndex = (Singleton.Instance.GAME_DISPLAY_RIGHT - Singleton.Instance.GAME_DISPLAY_LEFT) / Singleton.Instance.BALL_SHOW_WIDTH;
		private int minLayer = 0, maxLayer = (Singleton.Instance.GAME_DISPLAY_BOTTOM - Singleton.Instance.GAME_DISPLAY_TOP) / Singleton.Instance.BALL_SHOW_WIDTH;


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
					for (int i = 0; i < maxIndex; i++)
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

				if (Position.X - ballDrawWidth + ballObjectWidth <= left) {
					angle = -angle;
					angle += MathHelper.ToRadians(180);
				}

				if (Position.X + ballDrawWidth  >= right) {
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
						if (CheckCollision(gameObjects[iLayer, indexBall]) <= ballObjectWidth)
						{
							// -- MY LOGIC UN SUCCESSFULL
							/*
							float x = Position.X + ballDrawWidth / 2;
							float y = Position.Y + ballDrawWidth / 2;
							

							int gridy = (int) ((y-top) / ballDrawWidth);

							bool hitItemShift = gameObjects[iLayer, indexBall].shiftFloor;
							bool thisFloorShift = false;
							if (iLayer == gridy)
								thisFloorShift = hitItemShift;
							else
								thisFloorShift = ((iLayer - gridy) % 2 == 0);

							// Check for offset
							int xoffset = thisFloorShift  ?  left + (ballDrawWidth / 2) : left ;

							int gridx = (int) ((x - left) / ballDrawWidth);
							// TODO: check is not out side
							//if (gridx * ballDrawWidth + xoffset >= right) continue;

							// TODO : find next nearesed
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
							Position = new Vector2(gridx*ballDrawWidth+xoffset, gridy * ballDrawWidth + top);
							gameObjects[gridy, gridx] = this;
							CheckRemoveBubble(gameObjects, color, new Vector2(gridx, gridy));

                            }


							*/

							// -- OLD LOGIC

							if (Position.X >= gameObjects[iLayer, indexBall].Position.X)
							{

								if (!shiftFloor)
								{
										Position = new Vector2((indexBall * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + top);
										gameObjects[iLayer + 1, indexBall] = this;
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
								if (!shiftFloor)
								{
									Position = new Vector2(((indexBall - 1) * ballDrawWidth) + (((iLayer + 1) % 2) == 0 ? left : left + (ballDrawWidth/2)), ((iLayer + 1) * ballObjectWidth) + 40);
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

		public void CheckRemoveBubble(Bubble[,] bubbleAll, Color colorRemove, Vector2 bubbleInGrid) {

			if (bubbleInGrid.X < minIndex || bubbleInGrid.Y < minLayer ) return;
			if (bubbleInGrid.X > maxIndex - 1 || bubbleInGrid.Y > maxLayer - 1 ) return;

			if (bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X] == null) return;
			if (bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X].color != colorRemove) return;

			Singleton.Instance.removeBubble.Add(bubbleInGrid);
			bubbleAll[(int)bubbleInGrid.Y, (int)bubbleInGrid.X] = null;

			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X + 1, bubbleInGrid.Y)); // Right
			CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y)); // Left
			if (bubbleInGrid.Y % 2 == 0) {
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y - 1)); // Top Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y - 1)); // Top Left
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X, bubbleInGrid.Y + 1)); // Bot Right
				CheckRemoveBubble(bubbleAll, colorRemove, new Vector2(bubbleInGrid.X - 1, bubbleInGrid.Y + 1)); // Bot Left
			} else {
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
				float nx = tile.X + neighborsoffsets[tilerow,i,0];
				float ny = tile.Y + neighborsoffsets[tilerow, i, 1];

				// Make sure the tile is valid
				if (nx >= 0 && nx < finder.Position.X + ballDrawWidth && ny >= 0 && ny < finder.Position.Y + ballDrawWidth)
				{
					neighbors.Add(new Vector2(nx,ny));
				}
			}

			return neighbors;
		}

		private Vector2[] sortNearedNeighbors(List<Vector2> neighbors,Vector2 target)
        {
			//neighbors.Sort((x, y) => (x.X < y.X));
			   
			int size = neighbors.Count;
			Vector2[] sorted = new Vector2[size];
			float[] range = new float[size];
			for (int i = 0; i < size; i++)
				range[i] = (float) Math.Sqrt(Math.Pow(neighbors[i].X - target.X, 2) + Math.Pow(neighbors[i].Y - target.Y, 2));
			return sorted;
        }

	}
}