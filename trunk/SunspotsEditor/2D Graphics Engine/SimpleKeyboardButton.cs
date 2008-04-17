using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    public abstract class SimpleKeyboardEditableButton
    {
        protected bool IsSelected = false;
        protected String DrawText;
        protected Vector2 Position;
        protected String CurrentKeyboardString;

        protected void Initialize(Vector2 Position)
        {
            this.Position = Position;
        }

        public virtual void Update()
        {
            if (IsSelected)
            {
                CurrentKeyboardString = WindowManager.KeyboardMouseManager.GetKeyboardTyping();
            }
        }

        public virtual void Draw2D()
        {
            Color DrawColor = Color.White;
            if (IsSelected) DrawColor = Color.Red;
            WindowManager.TextMngr.DrawText(Position, DrawText, DrawColor);
        }

        public virtual void GainFocus()
        {
            WindowManager.KeyboardMouseManager.ClearKeyboardString();
           
            IsSelected = true;
        }

        public void LoseFocus()
        {
            IsSelected = false;
        }

        

    }
}
