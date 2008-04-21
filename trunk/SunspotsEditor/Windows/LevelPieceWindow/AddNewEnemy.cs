using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows.LevelPieceWindow
{
    class AddNewEnemy : AddNewContent
    {

        new public void DrawSelectContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Select Enemy Item";
            Vector2 DrawLength = WindowManager.EditorFont.MeasureString(draw);
            WindowManager.TextMngr.DrawText(TextStartPosition - new Vector2(DrawLength.X / 2, 0), draw);
            TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
            Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);
            for (int i = 0; i < Game1.LevelObjects.Length; i++)
            {
                draw = Game1.LevelObjects[i];
                TextStartPosition += TextAddVector;
                Color DrawColor = Color.White;
                if (i == SelectedContent)
                    DrawColor = Color.Red;
                WindowManager.TextMngr.DrawText(TextStartPosition, draw, DrawColor);
            }
        }

        new public void DrawDefineContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Define Enemy";
            Vector2 DrawLength = WindowManager.EditorFont.MeasureString(draw);
            WindowManager.TextMngr.DrawText(TextStartPosition - new Vector2(DrawLength.X / 2, 0), draw);

            foreach (SimpleKeyboardEditableButton S in EditButtons)
            {
                S.Draw2D();
            }
        }

        new public void updateSelectContent()
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Pressed)
            {
                SelectedContent++;
                if (SelectedContent >= Game1.LevelObjects.Length) SelectedContent = 0;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
            {
                SelectedContent--;
                if (SelectedContent < 0) SelectedContent = Game1.LevelObjects.Length - 1;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Enter) == KeyInputType.Pressed)
            {
                CurrentMode = RunMode.DefineContent;
                EditButtons = new SimpleKeyboardEditableButton[6];
                Vector2 StartPosition = Position - (Size / 2);
                Vector2 TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
                Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);

                TextStartPosition += TextAddVector;
                EditButtons[0] = new EditTextButton(TextStartPosition, "NewModel", "Name : ");
                TextStartPosition += TextAddVector;
                EditButtons[1] = new EditTextButton(TextStartPosition, Game1.LevelObjects[SelectedContent], "Content Item : ");
                TextStartPosition += TextAddVector;
                EditButtons[2] = new VectorEditButton(TextStartPosition, Vector3.Zero, "Position : ");
                TextStartPosition += TextAddVector;
                EditButtons[3] = new VectorEditButton(TextStartPosition, Vector3.Zero, "Rotation : ");
                TextStartPosition += TextAddVector;
                EditButtons[4] = new VectorEditButton(TextStartPosition, Vector3.One, "Scale : ");
                TextStartPosition += TextAddVector;
                EditButtons[5] = new SimpleBooleanEditButton(TextStartPosition, false, "Waypoint Enemy : ");

                EditButtons[0].GainFocus();
                SelectedButton = 0;
            }
        }
    }
}
