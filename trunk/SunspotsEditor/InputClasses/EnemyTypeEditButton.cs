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
        String EnemyType;

        int selected = 0;

        Color SelectedColor;

        string[] EnemyTypes = { "Block", "Chase", "Waypoint" };

        public EnemyTypeEditButton(Vector2 Position, int Starting, String NonEditable)
        {
            this.NonEditable = NonEditable;
            this.EnemyType = EnemyTypes[Starting];
            this.Initialize(Position);
            this.DrawText = NonEditable + "<- " + EnemyType + " ->";
        }

        public string GetEnemyType() { return EnemyType; }

        public override void Update()
        {
            if (IsSelected)
            {
                SelectedColor = Color.Red;

                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Right) == KeyInputType.Pressed)
                {
                    selected++;

                    if (selected > EnemyTypes.Length - 1)
                        selected = 0;

                    EnemyType = EnemyTypes[selected];

                    this.DrawText = NonEditable + "<- " + EnemyTypes[selected] + " ->";
                }
                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Left) == KeyInputType.Pressed)
                {
                    selected--;

                    if (selected < 0)
                        selected = EnemyTypes.Length - 1;

                    EnemyType = EnemyTypes[selected];

                    this.DrawText = NonEditable + "<- " + EnemyTypes[selected] + " ->";
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
