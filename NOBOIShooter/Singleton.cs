using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace NOBOIShooter
{
    //apply for singleton design pattern
    class Singleton
    {
        //Store default value or parameter here!
        public int ScreenHeight = 720, ScreenWidth = 1280;
        public int Score = 0;
        public bool IsMouseVisible = true, Shooting = false;
        public string ContentRootDir = "Content";
        public string BestTime, BestScore;
        public MouseState MousePrevious, MouseCurrent;
        public List<Vector2> removeBubble = new List<Vector2>();

        public readonly int BubbleGridWidth = 60;
        public readonly int BubblePictureWidth = 58;
        public readonly int GameDisplayBorderTop = 40;
        public readonly int GameDisplayBorderRight = 880;
        public readonly int GameDisplayBorderLeft = 320;
        public readonly int GameDisplayBorderBottom = 600;

        //Base of singleton
        private static Singleton s_instance;

        //Constructor
        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new Singleton();
                }
                return s_instance;
            }
        }
    }
}
