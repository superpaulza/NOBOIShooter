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
        // STATIC FIELDS
        public static Texture2D button;
        public static SpriteFont font;

        // LOAD CONTENT
        public static void LoadContent(ContentManager content)
        {
            button = content.Load<Texture2D>("Controls/button");
            font = content.Load<SpriteFont>("Fonts/Font");
        }
    }
}
