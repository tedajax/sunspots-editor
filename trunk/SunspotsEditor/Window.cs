using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    abstract class Window
    {
        protected string mode;
        WindowManager windowManager;

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

        public string WindowName() { return GetType().Name; }

        public override string ToString()
        {
            return GetType().Name + " : " + Mode;
        }
    }
}
