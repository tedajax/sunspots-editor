using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class EnemyTypeEditButton : SimpleKeyboardEditableButton
    {
        String NonEditable;
        EnemyType EnemyType;

        Color SelectedColor;

        public EnemyTypeEditButton(Vector2 Position, EnemyType StartingType, String NonEditable)
        {
            this.NonEditable = NonEditable;
            this.EnemyType = StartingType;
            this.Initialize(Position);
            this.DrawText = NonEditable + "<- " + StartingType.ToString() + " ->";
        }

        public override void Update()
        {
            if (IsSelected)
            {
                SelectedColor = Color.Red;

                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Right) == KeyInputType.Pressed)
                {
                    if (EnemyType == EnemyType.Block)
                        EnemyType = EnemyType.Chase;
                    else if (EnemyType == EnemyType.Chase)
                        EnemyType = EnemyType.Waypoint;
                    else if (EnemyType == EnemyType.Waypoint)
                        EnemyType = EnemyType.Block;

                    this.DrawText = NonEditable + "<- " + EnemyType.ToString() + " ->";
                }
                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Left) == KeyInputType.Pressed)
                {
                    if (EnemyType == EnemyType.Block)
                        EnemyType = EnemyType.Waypoint;
                    else if (EnemyType == EnemyType.Waypoint)
                        EnemyType = EnemyType.Chase;
                    else if (EnemyType == EnemyType.Chase)
                        EnemyType = EnemyType.Block;

                    this.DrawText = NonEditable + "<- " + EnemyType.ToString() + " ->";
                }
            }
            else
            {
                SelectedColor = Color.White;
            }
        }


        public override void Draw2D()
        {
            int firsthyphen = DrawText.IndexOf("-");
            string firststring = DrawText.Substring(0, firsthyphen + 2);
            string secondstring = DrawText.Substring(firsthyphen + 2, DrawText.IndexOf(">") - firsthyphen - 3);
            Vector2 secondstrpos = new Vector2(Position.X + WindowManager.EditorFont.MeasureString(firststring).X, Position.Y);
            string laststring = DrawText.Substring(DrawText.IndexOf(">") - 1);
            Vector2 laststrpos = new Vector2(secondstrpos.X + WindowManager.EditorFont.MeasureString(secondstring).X, Position.Y);
            WindowManager.TextMngr.DrawText(Position, firststring, Color.White);
            WindowManager.TextMngr.DrawText(secondstrpos, secondstring, SelectedColor);
            WindowManager.TextMngr.DrawText(laststrpos, laststring);
        }
    }
}
