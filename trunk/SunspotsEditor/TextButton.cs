using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SunspotsEditor
{
    class TextButton : Button
    {
        public TextButton(string txt, Vector2 pos)
        {
            text = txt;
            position = pos;

            Vector2 measuretext = WindowManager.EditorFont.MeasureString(text);
            clickRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)measuretext.X, (int)measuretext.Y);

            textColor = Color.White;
        }
    }
}
