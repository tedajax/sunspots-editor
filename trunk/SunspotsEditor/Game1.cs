using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using SunspotsEditor.LevelData;

namespace SunspotsEditor
{
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum EventType { StartAllRange }

        public static int iWidth = 800;
        public static int iHeight = 600;

        GraphicsDeviceManager graphics;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = iWidth;
            graphics.PreferredBackBufferHeight = iHeight;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            WindowManager windowManager = new WindowManager(this);
            Components.Add(windowManager);

            Level TestLevel = new Level("TestLevel.xml", Content);
            TestLevel.Initialize();
            TestLevel.AddSampleData();
            TestLevel.Save("TestLevel.xml");
            

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }
        
        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
