using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace withLuckAndWisdomProject.Screens
{
    class ResourceManager
    {
        // create static fields
        public static Texture2D button;
        public static Texture2D settingBtn;
        public static Texture2D scoreBtn;
        public static Texture2D mainBackground;
        public static Texture2D gameBackground;
        public static Texture2D logo;
        public static Texture2D ball;

        public static Texture2D Bamboo;
        public static Texture2D Rabbit;
        public static Texture2D Pencil;
        public static Texture2D Panda;
        public static Texture2D BambooShoot;
        public static Texture2D BambooJoint1;
        public static Texture2D BambooJoint2;

        public static Texture2D BackgroundMoutain;
        public static Texture2D BackgroundGame;
        public static Texture2D overBackground;

        public static Texture2D BasicBtn;
        
        public static Texture2D BackBtn;
        public static Texture2D checkBoxEmpty;
        public static Texture2D checkBoxSelect;
        public static Texture2D increseBtn;
        public static Texture2D decreseBtn;

        public static SpriteFont font;

        // load content here
        public static void LoadContent(ContentManager content)
        {
            button = content.Load<Texture2D>("Controls/Play");
            settingBtn = content.Load<Texture2D>("Controls/Setting");
            scoreBtn = content.Load<Texture2D>("Controls/Score");
            BasicBtn = content.Load<Texture2D>("Controls/BasicButton");
            mainBackground = content.Load<Texture2D>("Images/background2");
            gameBackground = content.Load<Texture2D>("Images/playedbackground");
            logo = content.Load<Texture2D>("Images/logo-first");
            ball = content.Load<Texture2D>("Images/CircleSprite");
            
            Pencil = content.Load<Texture2D>("Controls/dot");
            Bamboo = content.Load<Texture2D>("Images/Bamboo");
            Rabbit = content.Load<Texture2D>("Images/Rabbit");
            Panda = content.Load<Texture2D>("Images/Panda");
            BambooJoint1 = content.Load<Texture2D>("Images/bamboo-first");
            BambooJoint2 = content.Load<Texture2D>("Images/bamboo-second");
            BambooShoot = content.Load<Texture2D>("Images/bamboo-shoot");

            // BackgroundMoutain = content.Load<Texture2D>("Images/mountain");
            // BackgroundGame = content.Load<Texture2D>("Images/gamebackgroind");
             

            BackBtn = content.Load<Texture2D>("Controls/BackButtonWhite");
            checkBoxEmpty = content.Load<Texture2D>("Controls/CheckboxEmpty");
            checkBoxSelect = content.Load<Texture2D>("Controls/CheckboxSelect");
            increseBtn = content.Load<Texture2D>("Controls/increase");
            decreseBtn = content.Load<Texture2D>("Controls/decrease");

            font = content.Load<SpriteFont>("Fonts/Font");
        }
    }
}
