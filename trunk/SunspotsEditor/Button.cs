using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SunspotsEditor
{
    class Button
    {
        protected string text;
        protected Vector2 position;
        protected Rectangle clickRectangle;
        protected ButtonState OldLeftClick;
        protected Color textColor;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Vector2 measuretext = WindowManager.EditorFont.MeasureString(text);
                clickRectangle = new Rectangle((int)position.X, (int)position.Y, (int)measuretext.X, (int)measuretext.Y);
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Button(string txt, Vector2 pos)
        {
            text = txt;
            position = pos;

            Vector2 measuretext = WindowManager.EditorFont.MeasureString(text);
            clickRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)measuretext.X, (int)measuretext.Y);

            textColor = Color.White;
        }

        public void Draw()
        {
            WindowManager.SpriteBatch.Begin();
            WindowManager.SpriteBatch.DrawString(WindowManager.EditorFont,
                                                 text,
                                                 position,
                                                 textColor);
            WindowManager.SpriteBatch.End();
        }

        public bool GetClick()
        {
            MouseState mouse = Mouse.GetState();
            if (clickRectangle.Contains(mouse.X, mouse.Y))
            {
                textColor = Color.Red;
                if (mouse.LeftButton == ButtonState.Pressed)
                    textColor = Color.Lime;
                else
                    textColor = Color.Red;
                
                if (OldLeftClick == ButtonState.Pressed && mouse.LeftButton == ButtonState.Released)
                    return true;
            }
            else
                textColor = Color.White;

            OldLeftClick = mouse.LeftButton;

            return false;
        }
    }
}
