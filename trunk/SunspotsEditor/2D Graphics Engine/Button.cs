using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class Button
    {
        protected string text;
        protected Vector2 position;
        protected Vector2 drawoffset;
        protected Rectangle clickRectangle;
        protected ButtonState OldLeftClick;
        protected bool buttonSelected;
        protected bool buttonPressed;
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

        public Vector2 DrawOffset
        {
            get { return drawoffset; }
            set { drawoffset = value; }
        }

        public bool ButtonSelected
        {
            get { return buttonSelected; }
            set { buttonSelected = value; }
        }

        public virtual void Draw()
        {
            /*
            WindowManager.SpriteBatch.Begin();
            WindowManager.SpriteBatch.DrawString(WindowManager.EditorFont,
                                                 text,
                                                 drawoffset + position,
                                                 textColor);
            WindowManager.SpriteBatch.End();*/

            WindowManager.TextMngr.DrawText(drawoffset + position, text, textColor);
        }

        public virtual bool GetClick()
        {
            MouseState mouse = Mouse.GetState();
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Enter) == KeyInputType.Pressed)
                buttonPressed = true;
            else
                buttonPressed = false;
            if (buttonSelected)
            {
                textColor = Color.Red;
                if (buttonPressed)
                    textColor = Color.Lime;
                else
                    textColor = Color.Red;

                if (buttonPressed)
                    return true;
            }
            else
                textColor = Color.White;

            OldLeftClick = mouse.LeftButton;
            
            return false;
        }
    }
}
