﻿using Microsoft.Xna.Framework.Audio;
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
        public static Texture2D homeBtn;
        public static Texture2D pauseBtn;
        public static Texture2D replayBtn;
        public static Texture2D applyBtn;

        public static Texture2D mainBackground;
        public static Texture2D gameBackground;
        public static Texture2D hudBackground;
        public static Texture2D logo;
        public static Texture2D ball;

        public static Texture2D cloudFirst;
        public static Texture2D cloudSecound;
        public static Texture2D cloudDungo;
        public static Texture2D cloudHammer;

        public static Texture2D Bamboo;
        public static Texture2D Rabbit;
        public static Texture2D RabbitHug;
        public static Texture2D Pencil;
        public static Texture2D Panda;
        public static Texture2D BambooShoot;
        public static Texture2D BambooJoint1;
        public static Texture2D BambooJoint2;
        public static Texture2D BambooLeaf;

        public static Texture2D BackgroundMoutain;
        public static Texture2D BackgroundGame;
        public static Texture2D overBackground;

        public static Texture2D BasicBtn;
        public static Texture2D BackBtn;

        public static Texture2D checkBoxEmpty;
        public static Texture2D checkBoxSelect;
        public static Texture2D increseBtn;
        public static Texture2D decreseBtn;
        public static Texture2D volumeOnIcon;
        public static Texture2D volumeOffIcon;
        public static Texture2D checkboxEmpty;
        public static Texture2D checkboxSelect;

        public static SpriteFont font;

        // load content here
        public static void LoadContent(ContentManager content)
        {
            button = content.Load<Texture2D>("Controls/Play");
            settingBtn = content.Load<Texture2D>("Controls/Setting");
            scoreBtn = content.Load<Texture2D>("Controls/Score");
            homeBtn = content.Load<Texture2D>("Controls/Home");
            pauseBtn = content.Load<Texture2D>("Controls/Pause");
            replayBtn = content.Load<Texture2D>("Controls/Replay");
            applyBtn = content.Load<Texture2D>("Controls/Apply");
            BasicBtn = content.Load<Texture2D>("Controls/BasicButton");
            
            mainBackground = content.Load<Texture2D>("Images/background2");
            gameBackground = content.Load<Texture2D>("Images/playedbackground");
            overBackground = content.Load<Texture2D>("Images/over_background");

            hudBackground = content.Load<Texture2D>("Images/hud-background");
            logo = content.Load<Texture2D>("Images/logo-first");
            ball = content.Load<Texture2D>("Images/CircleSprite");

            cloudFirst = content.Load<Texture2D>("Images/cloud-first");
            cloudSecound = content.Load<Texture2D>("Images/cloud-second");
            cloudDungo = content.Load<Texture2D>("Images/cloud-dungo");
            cloudHammer = content.Load<Texture2D>("Images/cloud-hammer");
            
            Pencil = content.Load<Texture2D>("Controls/dot");
            Bamboo = content.Load<Texture2D>("Images/Bamboo");
            Rabbit = content.Load<Texture2D>("Images/Rabbit");
            RabbitHug = content.Load<Texture2D>("Images/Rabbit-hugbamboo");
            Panda = content.Load<Texture2D>("Images/Panda");
            BambooJoint1 = content.Load<Texture2D>("Images/bamboo-first");
            BambooJoint2 = content.Load<Texture2D>("Images/bamboo-second");
            BambooShoot = content.Load<Texture2D>("Images/bamboo-shoot");
            BambooLeaf = content.Load<Texture2D>("Images/bamboo-leaf");

            BackBtn = content.Load<Texture2D>("Controls/BackButtonWhite");
            checkBoxEmpty = content.Load<Texture2D>("Controls/CheckboxEmpty");
            checkBoxSelect = content.Load<Texture2D>("Controls/CheckboxSelect");
            increseBtn = content.Load<Texture2D>("Controls/increase");
            decreseBtn = content.Load<Texture2D>("Controls/decrease");
            volumeOnIcon = content.Load<Texture2D>("Controls/volume-on");
            volumeOffIcon = content.Load<Texture2D>("Controls/volume-off");
            checkBoxEmpty = content.Load<Texture2D>("Controls/CheckboxEmpty");
            checkboxSelect = content.Load<Texture2D>("Controls/CheckboxSelect");

            font = content.Load<SpriteFont>("Fonts/Itim"); 
        } 
    }
}
