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

        public SelectTool()
        {
            Buttons.Add(new Button("TESTING", Vector2.Zero));
        }

        public override void Initialize()
        {
            Mode = "Run";
            Functions.Add("Exit");
        }

        public override void Draw2D()
        {
            foreach (Button b in Buttons)
            {
                b.Draw();
            }

            WindowManager.SpriteBatch.Begin();
            WindowManager.SpriteBatch.DrawString(WindowManager.EditorFont,
                                                 "\\",
                                                 new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                                                 Color.White);
            WindowManager.SpriteBatch.End();
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
                    b.Text = "SUCCESS!";
            }
        }

        private void Die(GameTime gameTime)
        {
        }
    }
}
