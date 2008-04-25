using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows.Waypoint
{
  class AddWaypoint : EditorWindow
    {
        protected Vector2 Position;
        protected Vector2 Size;

        protected PrimitiveBatch PrimitiveBatch;

        protected SimpleKeyboardEditableButton[] EditButtons;
        protected int SelectedButton;

        LevelData.LevelData.WaypointFolder WaypointFolder;

        public AddWaypoint(LevelData.LevelData.WaypointFolder Folder)
        {
            this.WaypointFolder = Folder;
        }

        public override void Initialize()
        {
            this.Name = "AddWaypoint";
            PrimitiveBatch = new PrimitiveBatch(WindowManager.GraphicsDevice);
            Position = new Vector2(400, 300);
            Size = new Vector2(450, 300);
        
            EditButtons = new SimpleKeyboardEditableButton[2];
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + (new Vector2(Size.X * .05f, Size.Y * .08f));
            Vector2 TextAddVector = new Vector2(0, Size.Y * .08f);

            TextStartPosition += TextAddVector;
            EditButtons[0] = new EditTextButton(TextStartPosition, "-", "Name : ");
            TextStartPosition += TextAddVector;
            EditButtons[1] = new VectorEditButton(TextStartPosition, Vector3.Zero, "Position : ");
            
            EditButtons[0].GainFocus();
            SelectedButton = 0;

            base.Initialize();
        }

        public void DrawDefineContent()
        {
            Vector2 StartPosition = Position - (Size / 2);
            Vector2 TextStartPosition = StartPosition + new Vector2((Size.X * .5f), Size.Y * .03f);
            String draw = "Define Waypoint";
            Vector2 DrawLength = WindowManager.EditorFont.MeasureString(draw);
            WindowManager.TextMngr.DrawText(TextStartPosition - new Vector2(DrawLength.X / 2, 0), draw);

            foreach (SimpleKeyboardEditableButton S in EditButtons)
            {
                S.Draw2D();
            }
        }

        public override void Draw2D()
        {
           
            DrawDefineContent();
            
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
                LevelData.LevelData.WaypointData NewWaypoint = new SunspotsEditor.LevelData.LevelData.WaypointData();
                NewWaypoint.Name = (String)EditButtons[0].getEditText();
                NewWaypoint.Position = (Vector3)EditButtons[1].getEditText();
                WaypointFolder.WaypointData.Add(NewWaypoint);
                Waypoints Window = (Waypoints)WindowManager.FindWindow("Waypoints");
                Window.Unpause();
                WindowManager.RemoveWindow(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Escape) == KeyInputType.Pressed)
            {
                Waypoints Window = (Waypoints)WindowManager.FindWindow("Waypoints");
                Window.Unpause();
                WindowManager.RemoveWindow(this);
            }

            UpdateDefineContent();
            
            
            base.Update(gameTime);
        }

    }
}

