using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOBOIShooter.GameObjects
{
    class Player
    {

        // Create Variable
        private const int SHOOTER_WIDTH = 100;
        private const int SHOOTER_RADIAN = 50;
        private const int SHOOTER_MOVE_UP = 150;
        private int _aimerLength = 200;
        private int _aimerThick = 10;

        private MouseState MousePrevious;
        private MouseState MouseCurrent;

        private float _shooterAngle;
        private float _shooterScale;
        private float _ballScale;
        


        private Texture2D _shooterTexture;
        private Texture2D _ballTexture;
        private Texture2D _pencilDot;
        private Vector2 _position;
        private Vector2 _shooterCenterPosition;
        private Vector2 _shooterBallPosition;
        private Vector2 _nextBallPosintion;

        private BallGridManager _gameBord;
        private BallMoving _ballShoot;
        private BallDrop _ballEffect;

        private int _currentBall = 0;
        private int _nextBall = 0;

        private bool _isShooting;
        private bool _isSwapping;

        private Random _random = new Random();
        public Player(BallGridManager ballGrid, Texture2D shooterTexture, Texture2D ballTexture, Texture2D pencil)
        {
            _shooterTexture = shooterTexture;
            _ballTexture = ballTexture;
            _gameBord = ballGrid;
            _pencilDot = pencil;

            _position = new Vector2((Singleton.Instance.ScreenWidth - SHOOTER_WIDTH) / 2f, Singleton.Instance.ScreenHeight - SHOOTER_MOVE_UP);
            _shooterCenterPosition = _position + new Vector2(SHOOTER_RADIAN, SHOOTER_RADIAN);
            _shooterBallPosition = _shooterCenterPosition - new Vector2(20, 20);
            _nextBallPosintion = _shooterCenterPosition - new Vector2(20, -30);

            _shooterScale = (float) SHOOTER_WIDTH / _shooterTexture.Width;
            _ballScale = (float)_gameBord.TileWidth / _ballTexture.Width;

            _ballShoot = new BallMoving(_ballTexture, _gameBord);
            _currentBall = _gameBord.RandomBuble();
            _nextBall = _gameBord.RandomBuble();
            _isShooting = false;
            _isSwapping = false;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Shooter Draw
            spriteBatch.Draw(_shooterTexture, _shooterCenterPosition, null,
                Color.White, _shooterAngle + MathHelper.ToRadians(-90f), new Vector2(_shooterTexture.Width / 2, _shooterTexture.Height / 2),
                _shooterScale, SpriteEffects.None, 0f);

            // Draw Aimer
            spriteBatch.Draw(_pencilDot, new Rectangle((int)_shooterCenterPosition.X, (int)_shooterCenterPosition.Y, _aimerLength, _aimerThick), null,
                new Color(Color.Red,1f), _shooterAngle + MathHelper.ToRadians(-180f), new Vector2(0, 0), SpriteEffects.None, 0);

            if(!_isSwapping)
                spriteBatch.Draw(_ballTexture, _nextBallPosintion, null, _gameBord.GetColor(_nextBall), 0f, Vector2.Zero, _ballScale, SpriteEffects.None, 0f);

            spriteBatch.Draw(_ballTexture, _shooterBallPosition, null, _gameBord.GetColor(_currentBall), 0f, Vector2.Zero, _ballScale, SpriteEffects.None, 0f);
            if (_ballShoot != null)
                _ballShoot.Draw(spriteBatch, gameTime);

            if (_isSwapping && _ballEffect.Visible) 
                _ballEffect.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            // Load mouse state
            if(_isSwapping)
            {
                _isSwapping = _ballEffect.Visible;
                _ballEffect.Update(gameTime);
            
            }
            MousePrevious = MouseCurrent;
            MouseCurrent = Mouse.GetState();

            // find angle of shooter
            _shooterAngle = (float)Math.Atan2((_position.Y + SHOOTER_RADIAN) - MouseCurrent.Y, (_position.X + SHOOTER_RADIAN) - MouseCurrent.X);

            if (!_gameBord.GamePause &&!_isShooting && MouseCurrent.LeftButton == ButtonState.Pressed && MousePrevious.LeftButton == ButtonState.Released)
            {
                _ballShoot.setAnimation( _currentBall, _shooterCenterPosition - new Vector2(_gameBord.Radius, _gameBord.Radius), (float)(_shooterAngle + MathHelper.ToRadians(180f)));
                _currentBall = _nextBall;
                _nextBall = _gameBord.nextColorBubble();
                _isShooting = true;
            }
            if (_isShooting)
            {
                _ballShoot.Update(gameTime);
                _isShooting = _ballShoot.Visible;
            }

            //swap ball
            if (!_isShooting && MouseCurrent.RightButton == ButtonState.Pressed &&   MousePrevious.RightButton == ButtonState.Released)
            {
                int tempSwap = _currentBall;
                _ballEffect = new BallDrop(_ballTexture,_gameBord, _currentBall, _shooterBallPosition,_nextBallPosintion);
                _isSwapping = true;
                _currentBall = _nextBall;
                _nextBall = tempSwap;
            }

        }

    }
}
