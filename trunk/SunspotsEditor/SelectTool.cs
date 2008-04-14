using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class SelectTool : EditorWindow
    {
        List<Button> Buttons = new List<Button>();
        string ToolToLoad = "";

        public SelectTool()
        {
            Texture2D wtf = WindowManager.Content.Load<Texture2D>("player");
            Buttons.Add(new ImageButton(wtf, "WTF is this shit", new Vector2(500, 50)));
            Buttons.Add(new TextButton("Terrain", new Vector2(5, 35)));
            Buttons.Add(new TextButton("Water", new Vector2(5, 65)));
            Buttons.Add(new TextButton("Enemies", new Vector2(5, 95)));
            Buttons.Add(new TextButton("Scenery", new Vector2(5, 125)));
            Buttons.Add(new TextButton("Save", new Vector2(5, 155)));
            Buttons.Add(new TextButton("Load", new Vector2(5, 185)));
            Buttons.Add(new TextButton("Exit", new Vector2(5, 215)));
        }

        public override void Initialize()
        {
            Mode = "Run";
            Functions.Add("Exit");
        }

        public override void Draw2D()
        {
            if (Mode != "Pause")
            {
                foreach (Button b in Buttons)
                {
                    b.Draw();
                }

                WindowManager.SpriteBatch.Begin();
                WindowManager.SpriteBatch.DrawString(WindowManager.EditorFont,
                                                     ToolToLoad,
                                                     new Vector2(500, 0),
                                                     Color.White);
                WindowManager.SpriteBatch.End();
            }
        }

        public override void Draw3D()
        {
            base.Draw3D();
        }

        public override void Update(GameTime gameTime)
        {
            if (Mode == "Run") Run(gameTime);
            if (Mode == "Die") Die(gameTime);
        }

        private void Run(GameTime gameTime)
        {
            bool clickedbutton = false;
            
            foreach (Button b in Buttons)
            {
                clickedbutton = b.GetClick();
                if (clickedbutton)
                {
                    ToolToLoad = b.Text;
                    Mode = "Die";
                }
            }
        }

        private void Die(GameTime gameTime)
        {
            if (ToolToLoad == "Exit")
            {
                WindowManager.Exit();
                Mode = "Run";
            }
            else if (ToolToLoad == "Terrain")
                Pause();
            else
                Mode = "Run";
        }

        public void Pause() { Mode = "Pause"; }
        public void UnPause() { Mode = "Run"; }
    }
}
