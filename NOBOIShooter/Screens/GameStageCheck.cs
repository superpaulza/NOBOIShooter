using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NOBOIShooter.GameObjects;

namespace NOBOIShooter.Screens
{
    class GameStageCheck
    {
        Vector2 _textPosition;
        Rectangle FullDisplay;

        public GameStageCheck(Texture2D _pen, SpriteBatch spriteBatch, SpriteFont _textFront, BallGridManager _bord,  
            SoundEffectInstance BGM, SoundEffectInstance GameEndBGM, SoundEffectInstance GameWinBGM) 
        {
            _textPosition = new Vector2(1000, 180);
            FullDisplay = new Rectangle(0, 0, Singleton.Instance.ScreenWidth, Singleton.Instance.ScreenHeight);

            //Update Score on Screen
            if (!_bord.GamePause)
            {
                spriteBatch.DrawString(_textFront, "Score : " + _bord.GameScore, _textPosition, Color.White);
            }

            // Win/lost Checking
            if (_bord.GameWin || _bord.GameEnd)
            {
                spriteBatch.Draw(_pen, FullDisplay, new Color(Color.Black, .4f));
                BGM.Pause();

                if (_bord.GameEnd)
                {
                    spriteBatch.DrawString(_textFront, "Game Over", _textPosition, Color.White);
                    GameEndBGM.Play();
                }
                else 
                {
                    spriteBatch.DrawString(_textFront, "You Won", _textPosition, Color.White);
                    GameWinBGM.Play();
                }
            }
        }
    }
}
