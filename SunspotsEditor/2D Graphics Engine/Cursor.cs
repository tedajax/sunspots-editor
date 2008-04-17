using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SunspotsEditor
{
    class Cursor
    {
        Texture2D image;
        Vector2 position;
        bool hide;

        public Cursor()
        {
            image = WindowManager.Content.Load<Texture2D>("Cursor");
            position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            hide = false;
        }

        public void Draw()
        {
            if (!hide)
            {
                position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                WindowManager.SpriteBatch.Begin();
                WindowManager.SpriteBatch.Draw(image, position, Color.White);
                WindowManager.SpriteBatch.End();
            }
        }

        public void HideCursor() { hide = true; }
        public void ShowCursor() { hide = false; }
    }
}
