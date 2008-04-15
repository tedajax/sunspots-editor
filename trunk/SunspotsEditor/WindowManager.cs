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
        protected static TextManager textMngr;

        List<EditorWindow> WindowList;

        protected static Cursor cursor;

        public WindowManager(Game game)
            : base(game)
        {
            graphics = game.GraphicsDevice;
            content = game.Content;
            spriteBatch = new SpriteBatch(graphics);
            editorFont = content.Load<SpriteFont>("EditorFont");
        }

        public void Exit()
        {
            Game.Exit();
        }

        //Just include load content in here as well because it's easier
        public override void Initialize()
        {
            WindowList = new List<EditorWindow>();
            cursor = new Cursor();

            textMngr = new TextManager();

            base.Initialize();

            AddWindow(new SelectTool());
        }

        public override void Update(GameTime gameTime)
        {
            foreach (EditorWindow w in WindowList)
            {
                w.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            graphics.Clear(Color.Black);

            foreach (EditorWindow w in WindowList)
            {
                w.Draw3D();
                w.Draw2D();
            }

            textMngr.DrawAllText();

            cursor.Draw();
        }

        public void AddWindow(EditorWindow add)
        {
            add.WindowManager = this;

            WindowList.Add(add);
            add.Initialize();
        }

        public void AddWindow(EditorWindow add, int pos)
        {
            add.WindowManager = this;

            WindowList.Insert(pos, add);
            add.Initialize();
        }

        public void RemoveWindow(EditorWindow remove)
        {
            WindowList.Remove(remove);
        }

        public EditorWindow FindWindow(string name)
        {
            foreach (EditorWindow w in WindowList)
            {
                if (w.WindowName().Equals(name))
                    return w;
            }

            return null;
        }

        public EditorWindow FindWindow(string name, string mode)
        {
            foreach (EditorWindow w in WindowList)
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

        public static TextManager TextMngr
        {
            get { return textMngr; }
        }
    }
}
