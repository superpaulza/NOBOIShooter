using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NOBOIShooter.GameObjects {
	class Gun : GameObject {
		private Random random = new Random();
		private Texture2D bubbleTexture, _line;
		private Bubble BubbleOnGun;
		private Color _color;
		private float angle;
		private float gunScale;

        public SoundEffectInstance _deadSFX, _stickSFX;
        internal Color color;

        public Gun(Texture2D texture, Texture2D bubble, Texture2D line) : base(texture)
        {
            _line = line;
			bubbleTexture = bubble;
			_color = GetRandomColor();
			gunScale = 100f / texture.Width;
		}

		public override void Update(GameTime gameTime, Bubble[,] gameObjects) {
			
			Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
			Singleton.Instance.MouseCurrent = Mouse.GetState();
			if (Singleton.Instance.MouseCurrent.Y < 625) {
				angle = (float) Math.Atan2((Position.Y + 50) - Singleton.Instance.MouseCurrent.Y, (Position.X + 50) - Singleton.Instance.MouseCurrent.X);
				if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released) {
					BubbleOnGun = new Bubble(bubbleTexture) {
						Name = "Bubble",
						Position = new Vector2(Singleton.Instance.ScreenWidth / 2 - 35, Singleton.Instance.ScreenHeight - 120),
						deadSFX = _deadSFX,
						stickSFX = _stickSFX,
						color = _color,
						IsActive = true,
						Angle = angle + MathHelper.Pi, 
						Speed = 1000,
					};
					_color = GetRandomColor();
					Singleton.Instance.Shooting = true;
				}
			}

			if (Singleton.Instance.Shooting)
				BubbleOnGun.Update(gameTime, gameObjects);
			
		}

        public override void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(_texture, Position + new Vector2(50, 50), null, Color.White, angle + MathHelper.ToRadians(-90f), new Vector2(_texture.Width/2, _texture.Height / 2), gunScale, SpriteEffects.None, 0f);
            if (!Singleton.Instance.Shooting)
            {
                spriteBatch.Draw(_line, new Rectangle(Singleton.Instance.ScreenWidth / 2, Singleton.Instance.ScreenHeight - 100, 200, 1), null, Color.AliceBlue, angle + MathHelper.ToRadians(-180f), new Vector2(0,0), SpriteEffects.None, 0);
                spriteBatch.Draw(bubbleTexture, new Rectangle(Singleton.Instance.ScreenWidth / 2 - 35, Singleton.Instance.ScreenHeight - 100, 70, 70), _color);
			}
            else
                BubbleOnGun.Draw(spriteBatch);
			
		}
		public Color GetRandomColor() {
			Color _color = Color.Black;
			switch (random.Next(0, 6)) {
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
