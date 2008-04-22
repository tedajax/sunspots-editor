using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows.LevelPieceWindow
{
    class AddNewContent : EditorWindow
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
            base.Initialize();
        }

        public void DrawSelectContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Select Content Item";
            Vector2 DrawLength = WindowManager.EditorFont.MeasureString(draw);
            WindowManager.TextMngr.DrawText(TextStartPosition - new Vector2(DrawLength.X / 2, 0), draw);
            TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
            Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);
            for (int i =0; i<Game1.LevelObjects.Length;i++)
            {
                draw = Game1.LevelObjects[i];
                TextStartPosition += TextAddVector;
                Color DrawColor = Color.White;
                if (i == SelectedContent)
                    DrawColor = Color.Red;
                WindowManager.TextMngr.DrawText(TextStartPosition, draw,DrawColor);
            }
        }

        public void DrawDefineContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Define Content Item";
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

            for (int i = (int)StartPosition.X+1; i < StartPosition.X + Size.X-1; i++)
            {
                PrimitiveBatch.AddVertex(new Vector2(i, StartPosition.Y+1), new Color(99,153,255,150));
                PrimitiveBatch.AddVertex(new Vector2(i, StartPosition.Y + Size.Y-1), new Color(99, 153, 255, 150));
            }
            PrimitiveBatch.End();
            base.Draw3D();
        }

        public void updateSelectContent()
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down)== KeyInputType.Pressed)
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
                EditButtons[5] = new EnemyTypeEditButton(TextStartPosition, 0, "LAME : ");

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
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Pressed)
            {
                EditButtons[SelectedButton].LoseFocus();
                SelectedButton++;
                if (SelectedButton >= EditButtons.Length)
                {
                    SelectedButton = 0;
                }
                EditButtons[SelectedButton].GainFocus();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
            {
                EditButtons[SelectedButton].LoseFocus();
                SelectedButton--;
                if (SelectedButton < 0)
                {
                    SelectedButton = EditButtons.Length-1;
                }
                EditButtons[SelectedButton].GainFocus();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Enter) == KeyInputType.Pressed)
            {
                LevelData.LevelData.Generic3DObject NewObject = new SunspotsEditor.LevelData.LevelData.Generic3DObject();
                NewObject.ContentName = "LevelObjects\\" + (String)EditButtons[1].getEditText();
                NewObject.Name = (String)EditButtons[0].getEditText();
                NewObject.Position = (Vector3)EditButtons[2].getEditText();
                Vector3 Degrees = (Vector3)EditButtons[3].getEditText();
                //NewObject.Rotation = new Vector3(MathHelper.ToRadians(Degrees.X), MathHelper.ToRadians(Degrees.Y), MathHelper.ToRadians(Degrees.Z));
                NewObject.Rotation = Degrees;
                NewObject.Scale = (Vector3)EditButtons[4].getEditText();
                WindowManager.Level.addLevelPiece(WindowManager.Level.ConvertTo3DObject(NewObject, WindowManager.Level.CartoonEffect));
                TestWindow Window = (TestWindow)WindowManager.FindWindow("Terrain");
                Window.Unpause();
                WindowManager.RemoveWindow(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Escape) == KeyInputType.Pressed)
            {
                TestWindow Window = (TestWindow)WindowManager.FindWindow("Terrain");
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
