using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace NOBOIShooter
{
    //apply for singleton design pattern
    class Singleton
    {
        //Store default value or parameter here!
        public int ScreenHeight = 720;
        public int ScreenWidth = 1280;
        public bool IsMouseVisible = true;
        public String ContentRootDir = "Content";
        public MouseState MousePrevious, MouseCurrent;
        public List<Vector2> removeBubble = new List<Vector2>();
        public bool Shooting = false;
        public int Score = 0;
        public string BestTime, BestScore;

        public readonly int BALL_SHOW_WIDTH = 80;
        public readonly int BALL_ART_WIDTH = 70;
        public readonly int GAME_DISPLAY_TOP = 40;
        public readonly int GAME_DISPLAY_RIGHT = 880;
        public readonly int GAME_DISPLAY_LEFT = 320;
        //Base of singleton
        private static Singleton instance;

        //Constructor
        private Singleton()
        {

        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }

                return instance;
            }
        }
    }
}
