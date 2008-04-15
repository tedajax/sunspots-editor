using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    abstract class EditorWindow
    {
        protected string mode;
        WindowManager windowManager;

        protected List<string> Functions = new List<string>();

        public string Mode
        {
            get { return mode; }
            internal set { mode = value; }
        }

        public WindowManager WindowManager
        {
            get { return windowManager; }
            internal set { windowManager = value; }
        }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw3D() { }
        public virtual void Draw2D() { }

        public void DrawFunctions()
        {

            WindowManager.SpriteBatch.Begin();

            int count = 0;
            foreach (string s in Functions)
            {
                WindowManager.SpriteBatch.DrawString(WindowManager.EditorFont,
                                                     count.ToString() + ". " + s,
                                                     new Vector2(5, count * 20),
                                                     Color.White
                                                     );
                count++;
            }

            WindowManager.SpriteBatch.End();
        }

        public string WindowName() { return GetType().Name; }

        public override string ToString()
        {
            return GetType().Name + " : " + Mode;
        }

    }
}
