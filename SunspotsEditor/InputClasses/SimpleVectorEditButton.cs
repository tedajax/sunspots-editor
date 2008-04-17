using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class SimpleVectorEditButton : SimpleKeyboardEditableButton
    {
        String NonEditableText;
        Vector3 Vector;
        enum SelectedPart { X, Y, Z };
        SelectedPart Selected;

        public SimpleVectorEditButton(Vector2 Position, Vector3 StartingVector, String NonEditableText)
        {
            this.Initialize(Position);
            this.Vector = StartingVector;
            this.DrawText = NonEditableText + StartingVector.ToString();
            this.NonEditableText = NonEditableText;
            Selected = SelectedPart.X;


        }

        public override void Update()
        {
            base.Update();
            if (IsSelected)
            {
               // DrawText = WindowManager.KeyboardMouseManager.GetKeyboardTyping();
                float ReturnFloat;
                if (float.TryParse(this.CurrentKeyboardString, out ReturnFloat))
                {
                    if (Selected == SelectedPart.X)
                        Vector.X = ReturnFloat;
                    if (Selected == SelectedPart.Y)
                        Vector.Y = ReturnFloat;
                    if (Selected == SelectedPart.Z)
                        Vector.Z = ReturnFloat;
                }
                else
                {
                    if (Selected == SelectedPart.X)
                        Vector.X = 0;
                    if (Selected == SelectedPart.Y)
                        Vector.Y = 0;
                    if (Selected == SelectedPart.Z)
                        Vector.Z = 0;
                    GainFocus();
                }
                
                if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Right) == KeyInputType.Pressed)
                {
                    if (Selected == SelectedPart.X)
                    {
                        Selected = SelectedPart.Y;
                    }
                    else if (Selected == SelectedPart.Y)
                    {
                        Selected = SelectedPart.Z;
                    }
                    else
                    {
                        Selected = SelectedPart.X;
                    }
                    GainFocus();
                }
                if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Left) == KeyInputType.Pressed)
                {
                    if (Selected == SelectedPart.X)
                    {
                        Selected = SelectedPart.Z;
                    }
                    else if (Selected == SelectedPart.Y)
                    {
                        Selected = SelectedPart.X;
                    }
                    else
                    {
                        Selected = SelectedPart.Y;
                    }
                    GainFocus();
                }

            }
        }

        public override void Draw2D()
        {
            Color DrawColor = Color.White;
            if (IsSelected)
            {
                Vector2 DistancePassed = WindowManager.EditorFont.MeasureString(NonEditableText+"{");
                WindowManager.TextMngr.DrawText(Position, NonEditableText+"{");
                if (Selected == SelectedPart.X)
                {
                    Vector2 SecondDistancePassed = WindowManager.EditorFont.MeasureString("X:" + Vector.X.ToString() + " ");
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X, 0), "X:"+Vector.X.ToString()+" ",Color.Red);
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X + SecondDistancePassed.X, 0), "Y:"+Vector.Y.ToString()+" "+"Z:"+Vector.Z.ToString()+ "}");
                }
                if (Selected == SelectedPart.Y)
                {
                    Vector2 FirstDistancePassed = WindowManager.EditorFont.MeasureString("X:" + Vector.X.ToString() + " ");
                    Vector2 SecondDistancePassed = WindowManager.EditorFont.MeasureString("Y:" + Vector.Y.ToString() + " ");
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X, 0), "X:" + Vector.X.ToString() + " ");
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X + FirstDistancePassed.X, 0), "Y:" + Vector.Y.ToString() + " ", Color.Red);
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X + FirstDistancePassed.X + SecondDistancePassed.X,0), "Z:" + Vector.Z.ToString() + "}");
                }
                if (Selected == SelectedPart.Z)
                {
                    Vector2 SecondDistancePassed = WindowManager.EditorFont.MeasureString("X:" + Vector.X.ToString() + " " + "Y:" + Vector.Y.ToString() + " ");
                    Vector2 ThirdDistancePassed = WindowManager.EditorFont.MeasureString("Z:" + Vector.Z.ToString());
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X, 0), "X:" + Vector.X.ToString() + " " + "Y:" + Vector.Y.ToString() + " ");
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X + SecondDistancePassed.X, 0), "Z:" + Vector.Z.ToString(), Color.Red);
                    WindowManager.TextMngr.DrawText(Position + new Vector2(DistancePassed.X + SecondDistancePassed.X + ThirdDistancePassed.X, 0), "}");

                }
                
                    
            }
            else
            {
                WindowManager.TextMngr.DrawText(Position, NonEditableText + Vector.ToString(), DrawColor);
            }
        }

        public override void GainFocus()
        {
            base.GainFocus();
            //WindowManager.KeyboardMouseManager.SetKeyboardString(DrawText);
            if (Selected == SelectedPart.X)
            {
                WindowManager.KeyboardMouseManager.SetKeyboardString(Vector.X.ToString());
            }
            if (Selected == SelectedPart.Y)
            {
                WindowManager.KeyboardMouseManager.SetKeyboardString(Vector.Y.ToString());
            }
            if (Selected == SelectedPart.Z)
            {
                WindowManager.KeyboardMouseManager.SetKeyboardString(Vector.Z.ToString());
            }
        }

        public override object getEditText()
        {
            return Vector;
        }

    }
}
