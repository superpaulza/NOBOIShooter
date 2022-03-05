using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NOBOIShooter.GameObjects {
	class Shooter : GameObject {

		// Create Variable
		private Random _random = new Random();

		private readonly static int SHOOTER_SIZE = 100;
		private readonly static int SHOOTER_RADIAN = SHOOTER_SIZE / 2;
		private readonly static int BUBBLE_WIDTH = Singleton.Instance.BALL_ART_WIDTH;
		private readonly static int BUBBLE_RADIAN = BUBBLE_WIDTH / 2;
		private int AIMER_LENGHT = 200;
		private int AIMER_THICK = 1;


		private static int BORDER_UNDER = Singleton.Instance.GAME_DISPLAY_BOTTOM;

		private Bubble _shooterBubble;
		private Texture2D _bubbleTexture, _lineTexture;
		private Color shooterBubbleColor;
		private Color nextBubbleColor;

		private float shooterAngle;
		private float scaleShooter;

		// Create Effect Sound Value
        public SoundEffectInstance _deadSFX, _stickSFX;

        public Shooter(Texture2D shooterTexture, Texture2D bubbleTexture, Texture2D aimerTexture) 
			: base(shooterTexture)
        {
            _lineTexture = aimerTexture;
			_bubbleTexture = bubbleTexture;

			//Set details
			shooterBubbleColor = GetRandomColor();
			nextBubbleColor = GetRandomColor();
			scaleShooter = (float)SHOOTER_SIZE / shooterTexture.Width;
			Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - SHOOTER_RADIAN , Singleton.Instance.ScreenHeight - SHOOTER_SIZE - 20);
			
		}

		public override void Update(GameTime gameTime, Bubble[,] gameObjects) {
			
			// Load mouse state
			Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
			Singleton.Instance.MouseCurrent = Mouse.GetState();

			//Check mouse over Shooter
			if (Singleton.Instance.MouseCurrent.Y < BORDER_UNDER) {

				// find angle of shooter
				shooterAngle = (float) Math.Atan2((Position.Y + SHOOTER_RADIAN) - Singleton.Instance.MouseCurrent.Y,
										(Position.X + SHOOTER_RADIAN) - Singleton.Instance.MouseCurrent.X);
				//Check mouse click
				if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && 
					Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released) {
					// Create new bubble went mouse release
					_shooterBubble = new Bubble(_bubbleTexture) {
						Name = "Bubble",
						Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - BUBBLE_RADIAN, 
							Singleton.Instance.ScreenHeight - SHOOTER_SIZE + (SHOOTER_SIZE - BUBBLE_RADIAN)/2 - 20),
						deadSFX = _deadSFX,
						stickSFX = _stickSFX,
						color = shooterBubbleColor,
						isMove = true,
						angle = shooterAngle + MathHelper.Pi, 
						speed = 1000,
					};

					//Creagte 
					shooterBubbleColor = nextBubbleColor;
					nextBubbleColor = GetRandomColor();

					Singleton.Instance.Shooting = true;
				}
			}

			if (Singleton.Instance.Shooting)
				_shooterBubble.Update(gameTime, gameObjects);
			
		}

        public override void Draw(SpriteBatch spriteBatch) {
			// Shooter Draw
			spriteBatch.Draw(_texture, Position + new Vector2(SHOOTER_RADIAN, SHOOTER_RADIAN), null,
				Color.White, shooterAngle + MathHelper.ToRadians(-90f), new Vector2(_texture.Width/2, _texture.Height / 2),
				scaleShooter, SpriteEffects.None, 0f);

			// Draw Aimer
			spriteBatch.Draw(_lineTexture, new Rectangle(Singleton.Instance.ScreenWidth / 2, Singleton.Instance.ScreenHeight - SHOOTER_RADIAN - 20, AIMER_LENGHT, AIMER_THICK), null,
				Color.AliceBlue, shooterAngle + MathHelper.ToRadians(-180f), new Vector2(0, 0), SpriteEffects.None, 0);

			if (!Singleton.Instance.Shooting)
			{
				
				// Draw Bubble on Gun
				spriteBatch.Draw(_bubbleTexture, new Rectangle(Singleton.Instance.ScreenWidth / 2 - BUBBLE_RADIAN,
							Singleton.Instance.ScreenHeight - (SHOOTER_RADIAN + BUBBLE_RADIAN + 20), BUBBLE_WIDTH, BUBBLE_WIDTH),
					shooterBubbleColor);
				// Draw Bubble next
				spriteBatch.Draw(_bubbleTexture, new Rectangle(Singleton.Instance.ScreenWidth / 2 - BUBBLE_RADIAN - 150, Singleton.Instance.ScreenHeight - SHOOTER_SIZE, BUBBLE_WIDTH, BUBBLE_WIDTH),
					nextBubbleColor);
			}
			else {
				// Draw Bubble on Fly
				_shooterBubble.Draw(spriteBatch);

				// Draw Drawer Bubble next
				spriteBatch.Draw(_bubbleTexture, new Rectangle(Singleton.Instance.ScreenWidth / 2 - BUBBLE_RADIAN - 150, Singleton.Instance.ScreenHeight - SHOOTER_SIZE, BUBBLE_WIDTH, BUBBLE_WIDTH),
					nextBubbleColor);
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
