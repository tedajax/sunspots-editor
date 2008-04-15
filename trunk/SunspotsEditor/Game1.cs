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
        public static float FieldOfView = 45f;

        GraphicsDeviceManager graphics;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
        }

        protected override void Initialize()
        {


            WindowManager windowManager = new WindowManager(this);
            windowManager.Initialize();
            Components.Add(windowManager);

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
            graphics.GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.RenderState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RenderState.DepthBufferEnable = true;
            graphics.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            base.Draw(gameTime);
        }
    }
}
