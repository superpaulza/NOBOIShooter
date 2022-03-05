using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NOBOIShooter.GameObjects {
	class Shooter : GameObject {

		// Create Variable
		private Random _random = new Random();

		private readonly static int s_shooterWidth = 100;
		private readonly static int s_shooterRadian = s_shooterWidth / 2;
		private readonly static int s_bubbleWidth = Singleton.Instance.BubblePictureWidth;
		private readonly static int s_bubbleRadian = s_bubbleWidth / 2;
		private int _aimerAngle = 200;
		private int _aimerThick = 1;


		private static int s_underBubbleField = Singleton.Instance.GameDisplayBorderBottom;

		private Bubble _shooterBubble;
		private Texture2D _bubbleTex, _lineTex;
		private Color _bubbleInShooterColor;
		private Color _nextbubbleColor;

		private float shooterAngle;
		private float scaleShooter;

		// Create Effect Sound Value
        private SoundEffectInstance _deadSFX, _stickSFX;

        public Shooter(Texture2D shooterTexture, Texture2D bubbleTexture, GraphicsDevice graphicsDevice) 
			: base(shooterTexture)
        {
			// Build Aiming Line
			_lineTex = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
			_lineTex.SetData(new[] { Color.White });

			_bubbleTex = bubbleTexture;

			//Set details
			_bubbleInShooterColor = GetRandomColor();
			_nextbubbleColor = GetRandomColor();
			scaleShooter = (float)s_shooterWidth / shooterTexture.Width;
			Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - s_shooterRadian , Singleton.Instance.ScreenHeight - s_shooterWidth - 20);
			
		}

		public override void Update(GameTime gameTime, Bubble[,] gameObjects) {
			
			// Load mouse state
			Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
			Singleton.Instance.MouseCurrent = Mouse.GetState();

			//Check mouse over Shooter
			if (Singleton.Instance.MouseCurrent.Y < s_underBubbleField) {

				// find angle of shooter
				shooterAngle = (float) Math.Atan2((Position.Y + s_shooterRadian) - Singleton.Instance.MouseCurrent.Y,
										(Position.X + s_shooterRadian) - Singleton.Instance.MouseCurrent.X);
				//Check mouse click
				if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && 
					Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released) {
					// Create new bubble went mouse release
					_shooterBubble = new Bubble(_bubbleTex) {
						Name = "Bubble",
						Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - s_bubbleRadian, 
							Singleton.Instance.ScreenHeight - s_shooterWidth + (s_shooterWidth - s_bubbleRadian) /2 - 20),
						DeadSFX = _deadSFX,
						StickSFX = _stickSFX,
						_color = _bubbleInShooterColor,
						IsMoving = true,
						Angle = shooterAngle + MathHelper.Pi, 
						Speed = 1000,
					};

					//Creagte 
					_bubbleInShooterColor = _nextbubbleColor;
					_nextbubbleColor = GetRandomColor();

					Singleton.Instance.Shooting = true;
				}
			}

			if (Singleton.Instance.Shooting)
				_shooterBubble.Update(gameTime, gameObjects);
			
		}

        public override void Draw(SpriteBatch spriteBatch) {
			// Shooter Draw
			spriteBatch.Draw(_texture, Position + new Vector2(s_shooterRadian, s_shooterRadian), null,
				Color.White, shooterAngle + MathHelper.ToRadians(-90f), new Vector2(_texture.Width/2, _texture.Height / 2),
				scaleShooter, SpriteEffects.None, 0f);

			// Draw Aimer
			spriteBatch.Draw(_lineTex, new Rectangle(Singleton.Instance.ScreenWidth / 2, Singleton.Instance.ScreenHeight - s_shooterRadian - 20, _aimerAngle, _aimerThick), null,
				Color.AliceBlue, shooterAngle + MathHelper.ToRadians(-180f), new Vector2(0, 0), SpriteEffects.None, 0);

			if (!Singleton.Instance.Shooting)
			{
				
				// Draw Bubble on Gun
				spriteBatch.Draw(_bubbleTex, new Rectangle(Singleton.Instance.ScreenWidth / 2 - s_bubbleRadian,
							Singleton.Instance.ScreenHeight - (s_shooterRadian + s_bubbleRadian + 20), s_bubbleWidth, s_bubbleWidth),
					_bubbleInShooterColor);
				// Draw Bubble next
				spriteBatch.Draw(_bubbleTex, new Rectangle(Singleton.Instance.ScreenWidth / 2 - s_bubbleRadian - 150, Singleton.Instance.ScreenHeight - s_shooterWidth, s_bubbleWidth, s_bubbleWidth),
					_nextbubbleColor);
			}
			else {
				// Draw Bubble on Fly
				_shooterBubble.Draw(spriteBatch);

				// Draw Drawer Bubble next
				spriteBatch.Draw(_bubbleTex, new Rectangle(Singleton.Instance.ScreenWidth / 2 - s_bubbleRadian - 150, Singleton.Instance.ScreenHeight - s_shooterWidth, s_bubbleWidth, s_bubbleWidth),
					_nextbubbleColor);
			}
			
		}

		public Color GetRandomColor() {
			Color _color = Color.Black;
			switch (_random.Next(0, 4)) {
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
    }
}
