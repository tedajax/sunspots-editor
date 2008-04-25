using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows.LevelPieceWindow
{
    class AddNewEnemy : EditorWindow
    {
        protected Vector2 Position;
        protected Vector2 Size;
        protected int SelectedContent;
        protected enum RunMode { SelectContent, DefineContent }

        protected RunMode CurrentMode;

        protected PrimitiveBatch PrimitiveBatch;

        protected SimpleKeyboardEditableButton[] EditButtons;
        protected int SelectedButton;

        public override void Initialize()
        {
            PrimitiveBatch = new PrimitiveBatch(WindowManager.GraphicsDevice);
            Position = new Vector2(400, 300);
            Size = new Vector2(450, 300);
            CurrentMode = RunMode.SelectContent;
            SelectedContent = 0;
            this.Name = "AddEnemy";
            base.Initialize();
        }

        public void DrawSelectContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Select Enemy Item";
            Vector2 DrawLength = WindowManager.EditorFont.MeasureString(draw);
            WindowManager.TextMngr.DrawText(TextStartPosition - new Vector2(DrawLength.X / 2, 0), draw);
            TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
            Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);
            for (int i = 0; i < Game1.EnemyObjects.Length; i++)
            {
                draw = Game1.EnemyObjects[i];
                TextStartPosition += TextAddVector;
                Color DrawColor = Color.White;
                if (i == SelectedContent)
                    DrawColor = Color.Red;
                WindowManager.TextMngr.DrawText(TextStartPosition, draw, DrawColor);
            }
        }

        public void DrawDefineContent()
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

        public override void Draw2D()
        {
            //DrawBox
            if (CurrentMode == RunMode.SelectContent)
            {
                DrawSelectContent();
            }
            if (CurrentMode == RunMode.DefineContent)
            {
                DrawDefineContent();
            }
            base.Draw2D();
        }
        public override void Draw3D()
        {
            Vector2 StartPosition = Position - (Size / 2);
            PrimitiveBatch.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList);

            PrimitiveBatch.AddVertex(StartPosition, Color.White);
            PrimitiveBatch.AddVertex(StartPosition + new Vector2(0, Size.Y), Color.White);

            PrimitiveBatch.AddVertex(StartPosition, Color.White);
            PrimitiveBatch.AddVertex(StartPosition + new Vector2(Size.X, 0), Color.White);

            PrimitiveBatch.AddVertex(StartPosition + new Vector2(Size.X, 0), Color.White);
            PrimitiveBatch.AddVertex(StartPosition + new Vector2(Size.X, Size.Y), Color.White);

            PrimitiveBatch.AddVertex(StartPosition + new Vector2(0, Size.Y), Color.White);
            PrimitiveBatch.AddVertex(StartPosition + new Vector2(Size.X, Size.Y), Color.White);

            for (int i = (int)StartPosition.X + 1; i < StartPosition.X + Size.X - 1; i++)
            {
                PrimitiveBatch.AddVertex(new Vector2(i, StartPosition.Y + 1), new Color(99, 153, 255, 150));
                PrimitiveBatch.AddVertex(new Vector2(i, StartPosition.Y + Size.Y - 1), new Color(99, 153, 255, 150));
            }
            PrimitiveBatch.End();
            base.Draw3D();
        }

        public void updateSelectContent()
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                SelectedContent++;
                if (SelectedContent >= Game1.EnemyObjects.Length) SelectedContent = 0;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed)
            {
                SelectedContent--;
                if (SelectedContent < 0) SelectedContent = Game1.EnemyObjects.Length - 1;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Enter) == KeyInputType.Pressed)
            {
                CurrentMode = RunMode.DefineContent;
                EditButtons = new SimpleKeyboardEditableButton[6];
                Vector2 StartPosition = Position - (Size / 2);
                Vector2 TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
                Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);

                TextStartPosition += TextAddVector;
                EditButtons[0] = new EditTextButton(TextStartPosition, "NewEnemy", "Name : ");
                TextStartPosition += TextAddVector;
                EditButtons[1] = new EditTextButton(TextStartPosition, Game1.EnemyObjects[SelectedContent], "Content Item : ");
                TextStartPosition += TextAddVector;
                EditButtons[2] = new VectorEditButton(TextStartPosition, Vector3.Zero, "Position : ");
                TextStartPosition += TextAddVector;
                EditButtons[3] = new VectorEditButton(TextStartPosition, Vector3.Zero, "Rotation : ");
                TextStartPosition += TextAddVector;
                EditButtons[4] = new VectorEditButton(TextStartPosition, new Vector3(0.5f, 0.5f, 0.5f), "Scale : ");
                TextStartPosition += TextAddVector;
                EditButtons[5] = new EnemyTypeEditButton(TextStartPosition, 0, "Enemy Type : ");

                EditButtons[0].GainFocus();
                SelectedButton = 0;
            }
        }

        public void UpdateDefineContent()
        {
            foreach (SimpleKeyboardEditableButton S in EditButtons)
            {
                S.Update();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                EditButtons[SelectedButton].LoseFocus();
                SelectedButton++;
                if (SelectedButton >= EditButtons.Length)
                {
                    SelectedButton = 0;
                }
                EditButtons[SelectedButton].GainFocus();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed)
            {
                EditButtons[SelectedButton].LoseFocus();
                SelectedButton--;
                if (SelectedButton < 0)
                {
                    SelectedButton = EditButtons.Length - 1;
                }
                EditButtons[SelectedButton].GainFocus();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Enter) == KeyInputType.Pressed)
            {
                LevelData.LevelData.EnemyData NewEnemy = new SunspotsEditor.LevelData.LevelData.EnemyData();
                
                NewEnemy.ContentName = "" + (String)EditButtons[1].getEditText();
                NewEnemy.Name = (String)EditButtons[0].getEditText();
                NewEnemy.Position = (Vector3)EditButtons[2].getEditText();
                Vector3 Degrees = (Vector3)EditButtons[3].getEditText();
                //NewEnemy.EnemyObject.Rotation = new Vector3(MathHelper.ToRadians(Degrees.X), MathHelper.ToRadians(Degrees.Y), MathHelper.ToRadians(Degrees.Z));
                NewEnemy.Rotation = Degrees;
                NewEnemy.Scale = (Vector3)EditButtons[4].getEditText();
                WindowManager.Level.addEnemy(WindowManager.Level.ConvertEnemyDataToEnemy(NewEnemy, WindowManager.Level.CartoonEffect));
                EnemyEditor Window = (EnemyEditor)WindowManager.FindWindow("Enemy");
                Window.Unpause();
                WindowManager.RemoveWindow(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Escape) == KeyInputType.Pressed)
            {
                EnemyEditor Window = (EnemyEditor)WindowManager.FindWindow("Enemy");
                Window.Unpause();
                WindowManager.RemoveWindow(this);
            }
            if (CurrentMode == RunMode.DefineContent)
            {
                UpdateDefineContent();
            }
            if (CurrentMode == RunMode.SelectContent)
            {
                updateSelectContent();
            }
            
            base.Update(gameTime);
        }
    }
}
