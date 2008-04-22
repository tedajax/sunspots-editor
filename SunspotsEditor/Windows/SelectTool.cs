using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows
{
    class SelectTool : EditorWindow
    {
        List<Button> Buttons = new List<Button>();
        string ToolToLoad = "";

        Button Yes;
        Button No;

        Vector2 ItemOffset;

        PrimitiveBatch Prims2D;

        int SelectedButton = 0;

        bool holdingEnter = false;

        public SelectTool()
        {
            Texture2D wtf = WindowManager.Content.Load<Texture2D>("player");
            Buttons.Add(new TextButton("Terrain", new Vector2(5, 35)));
            Buttons.Add(new TextButton("Waypoints", new Vector2(5, 65)));
            Buttons.Add(new TextButton("Enemies", new Vector2(5, 95)));
            Buttons.Add(new TextButton("Scenery", new Vector2(5, 125)));
            Buttons.Add(new TextButton("Save", new Vector2(5, 155)));
            Buttons.Add(new TextButton("Load", new Vector2(5, 185)));
            Buttons.Add(new TextButton("Exit", new Vector2(5, 215)));

            ItemOffset = Vector2.Zero;

            Yes = new TextButton("Yes", new Vector2(-1000, -1000));
            No = new TextButton("No", new Vector2(-1000, -1000));

        }

        public override void Initialize()
        {
            Mode = "Run";
            Functions.Add("Exit");
            Prims2D = new PrimitiveBatch(WindowManager.GraphicsDevice);
            this.Name = "Select";
        }

        public override void Draw2D()
        {
            foreach (Button b in Buttons)
            {
                b.Draw();
            }

            Yes.Draw();
            No.Draw();
        }

        public override void Draw3D()
        {
            Color TransBlack = new Color(0, 0, 0, 100);
            //Prims2D.DrawBox(Vector2.Zero + ItemOffset, new Vector2(6490, 500), TransBlack);

            if (Mode == "FinalSave")
            {
                Prims2D.DrawBox(new Rectangle((int)Game1.CenterScreen.X - 100, (int)Game1.CenterScreen.Y - 50, (int)Game1.CenterScreen.X, (int)Game1.CenterScreen.Y + 50), Color.Black);
            }

            base.Draw3D();
        }

        public override void Update(GameTime gameTime)
        {
            if (Mode == "Run") Run(gameTime);
            if (Mode == "Pause") PauseWindow();
            if (Mode == "Die") Die(gameTime);
            if (Mode == "FinalSave") FinalSave();
        }

        private void Run(GameTime gameTime)
        {
            bool clickedbutton = false;

            ItemOffset = Vector2.Lerp(ItemOffset, Vector2.Zero, 0.1f);

            foreach (Button b in Buttons)
            {
                b.ButtonSelected = false;
            }

            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                SelectedButton += 1;
                if (SelectedButton >= Buttons.Count)
                    SelectedButton = 0;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed)
            {
                SelectedButton -= 1;
                if (SelectedButton < 0)
                    SelectedButton = Buttons.Count - 1;
            }

            Buttons[SelectedButton].ButtonSelected = true;

            foreach (Button b in Buttons)
            {
                b.DrawOffset = ItemOffset;
                clickedbutton = b.GetClick();
                if (clickedbutton)
                {
                    ToolToLoad = b.Text;
                    Mode = "Die";
                    Buttons[SelectedButton].ButtonSelected = false;
                }
            }
        }

        private void Die(GameTime gameTime)
        {
            if (ToolToLoad == "Exit")
            {
                //WindowManager.Exit();
                Mode = "FinalSave";
                Yes.ButtonSelected = true;
            }
            else if (ToolToLoad == "Terrain")
            {
                Pause();
                if(WindowManager.FindWindow("Terrain")==null) WindowManager.AddWindow(new TestWindow(), 0);
                ((TestWindow)WindowManager.FindWindow("Terrain")).Unpause();
                holdingEnter = true;
            }
            else if (ToolToLoad == "Waypoints")
            {
                Pause();
                if (WindowManager.FindWindow("Waypoints") == null) WindowManager.AddWindow(new Waypoints(), 0);
                ((Waypoints)WindowManager.FindWindow("Waypoints")).Unpause();
                holdingEnter = true;
            }
            else if (ToolToLoad == "Enemies")
            {
                Pause();
                //WindowManager.AddWindow(new EnemyEditor());
            }
            else if (ToolToLoad == "Scenery")
            {
                Pause();
                //WindowManager.AddWindow(new SceneryEditor());
            }
            else if (ToolToLoad == "Save")
            {
                Mode = "Run";
                WindowManager.Level.Save("TestLevel.XML");
            }
            else if (ToolToLoad == "Load")
            {
                Mode = "Run";
                WindowManager.Level = new LevelData.Level("TestLevel.XML", WindowManager.Content);
                WindowManager.Level.Initialize();
            }
            else
                Mode = "Run";
        }

        private void PauseWindow()
        {
            ItemOffset = Vector2.Lerp(ItemOffset, new Vector2(-Game1.iWidth, 0), 0.1f);

            foreach (Button b in Buttons)
            {
                b.DrawOffset = ItemOffset;
            }

           /* if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Escape) == KeyInputType.Pressed)
                UnPause();*/
        }

        private void FinalSave()
        {
            ItemOffset = Vector2.Lerp(ItemOffset, new Vector2(-Game1.iWidth, 0), 0.1f);

            Yes.Position = new Vector2(5, 65);
            No.Position = new Vector2(5, 95);

            foreach (Button b in Buttons)
            {
                b.DrawOffset = ItemOffset;
            }

            KeyboardState keyboard = Keyboard.GetState();

            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Enter) == KeyInputType.Released)
                holdingEnter = false;

            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed
                || WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                Yes.ButtonSelected = !Yes.ButtonSelected;
                No.ButtonSelected = !No.ButtonSelected;
            }

            if (!holdingEnter)
            {
                if (Yes.GetClick())
                {
                    //WindowManager.Level.Save("Filename");
                    WindowManager.Exit();
                }

                if (No.GetClick())
                {
                    WindowManager.Exit();
                }
            }

            string exitString = "Do You Want To Save?";
            Vector2 midstring = WindowManager.EditorFont.MeasureString(exitString);
            midstring /= 2;
            WindowManager.TextMngr.DrawText(new Vector2(5, 35), exitString);

        }

        public void Pause()
        { 
            Mode = "Pause";
        }
        public void UnPause()
        {
            Mode = "Run";
        }
    }
}
