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
        public static Texture2D mainBackground;
        public static Texture2D logo;
        public static Texture2D ball;
        public static Texture2D _bamboo;

        public static SpriteFont font;

        // load content here
        public static void LoadContent(ContentManager content)
        {
            button = content.Load<Texture2D>("Controls/Play");
            mainBackground = content.Load<Texture2D>("Images/background");
            logo = content.Load<Texture2D>("Images/logo");
            ball = content.Load<Texture2D>("Images/CircleSprite");
            _bamboo = content.Load<Texture2D>("Images/Bamboo");

            font = content.Load<SpriteFont>("Fonts/Font");
        }
    }
}
