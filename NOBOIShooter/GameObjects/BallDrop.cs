using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOBOIShooter.GameObjects
{
    class BallDrop
    {
        public int TileType { get; set; }
        public bool Visible { get; set; }

        private const float MoveStep = 0.1f;

        private BallGridManager _bord;
        private Texture2D _texture;
        private Vector2 _startPosition;
        private Vector2 _position;
        private Vector2 _targetPosition;
        private Vector2 _moveWay;
        private float _scaleball;
        private Color _color;

        private double _timer = 0;

        public BallDrop(Texture2D texture, BallGridManager bord, int ballType, Vector2 startPosition, Vector2 targetPosition)
        {
            _bord = bord;
            _texture = texture;
            _scaleball = (float)_bord.TileWidth / _texture.Width;
            TileType = ballType;
            _startPosition = startPosition;
            _position = startPosition;
            _moveWay = (targetPosition - startPosition) * MoveStep;
            _targetPosition = targetPosition;
            _color = _bord.GetColor(TileType);
            Visible = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (FadeNow > 0 && FadeNow < 1)
            if (Visible)
            {
                spriteBatch.Draw(_texture, _position, null, _color, 0f, Vector2.Zero, _scaleball, SpriteEffects.None, 0f);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Visible)
            {
                _timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer > 10)
                {
                    _position += _moveWay;
                    _timer = 0;
                }

                if (_position.X - _targetPosition.X  > 0 || _position.Y - _targetPosition.Y > 0)
                    Visible = false;

            }
        }
    }
}
