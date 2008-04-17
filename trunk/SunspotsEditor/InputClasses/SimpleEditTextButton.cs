using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class SimpleEditTextButton : SimpleKeyboardEditableButton
    {
        String NonEditableText;

        public SimpleEditTextButton(Vector2 Position, String InitialText, String NonEditableText)
        {
            this.Initialize(Position);
            this.DrawText = InitialText;
            this.NonEditableText = NonEditableText;


        }

        public override void Update()
        {
            base.Update();
            if (IsSelected)
            {
                DrawText = WindowManager.KeyboardMouseManager.GetKeyboardTyping();
            }
        }

        public override void Draw2D()
        {
            Color DrawColor = Color.White;
            if (IsSelected) DrawColor = Color.Red;
            WindowManager.TextMngr.DrawText(Position, NonEditableText+ DrawText, DrawColor);
        }

        public override void GainFocus()
        {
            base.GainFocus();
            WindowManager.KeyboardMouseManager.SetKeyboardString(DrawText);
        }

        public String GetEditText() { return this.DrawText; }

    }
}
