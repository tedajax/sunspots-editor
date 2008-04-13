using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SunspotsEditor
{
    class WindowManager : DrawableGameComponent
    {
        protected static GraphicsDevice graphics;
        protected static ContentManager content;
        protected static SpriteBatch spriteBatch;
        protected static SpriteFont editorFont;

        List<Window> WindowList;

        public WindowManager(Game game)
            : base(game)
        {
            graphics = game.GraphicsDevice;
            content = game.Content;
            spriteBatch = new SpriteBatch(graphics);
            editorFont = content.Load<SpriteFont>("EditorFont");
        }

        //Just include load content in here as well because it's easier
        public override void Initialize()
        {
            WindowList = new List<Window>();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);
        }

        public void AddWindow(Window add)
        {
            add.WindowManager = this;

            WindowList.Add(add);
            add.Initialize();
        }

        public void AddWindow(Window add, int pos)
        {
            add.WindowManager = this;

            WindowList.Insert(pos, add);
            add.Initialize();
        }

        public void RemoveWindow(Window remove)
        {
            WindowList.Remove(remove);
        }

        public Window FindWindow(string name)
        {
            foreach (Window w in WindowList)
            {
                if (w.WindowName().Equals(name))
                    return w;
            }

            return null;
        }

        public Window FindWindow(string name, string mode)
        {
            foreach (Window w in WindowList)
            {
                if (w.ToString().Equals(name + " : " + mode))
                    return w;
            }

            return null;
        }

        public static ContentManager Content
        {
            get { return content; }
        }

        public static SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public static SpriteFont EditorFont
        {
            get { return editorFont; }
        }
    }
}
