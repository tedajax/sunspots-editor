using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SunspotsEditor
{
    class ImageButton : Button
    {
        protected Texture2D image;

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public ImageButton(Texture2D img, string identifier, Vector2 pos)
        {
            image = img;
            text = identifier;
            position = pos;

            clickRectangle = new Rectangle((int)pos.X, (int)pos.Y, image.Width, image.Height);

            textColor = Color.White;
        }

        public override void Draw()
        {
            WindowManager.SpriteBatch.Begin();

            WindowManager.SpriteBatch.Draw(image,
                                           position,
                                           textColor);

            WindowManager.SpriteBatch.End();
        }
    }
}
