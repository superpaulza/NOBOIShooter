using System;
using System.Collections.Generic;
using System.Text;

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

        //Base of singleton
        private static Singleton instance;

        //Constructor
        private Singleton()
        {
            //Leave it blank
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
