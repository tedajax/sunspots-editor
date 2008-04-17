using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SunspotsEditor.LevelData;

namespace SunspotsEditor
{
    class SelectTool : EditorWindow   
    {
        List<Button> Buttons = new List<Button>();
        string ToolToLoad = "";
        Level TestLevel;
        RenderTarget2D sceneRenderTarget;
        RenderTarget2D normalDepthRenderTarget;
        Effect postprocessEffect;

        Vector3 Rotation = new Vector3();

        Vector2 ItemOffset;

        public SelectTool()
        {
            
        }

        public override void Initialize()
        {
            postprocessEffect = WindowManager.Content.Load<Effect>("Shaders\\PostprocessEffect");

            // Create two custom rendertargets.
            PresentationParameters pp = WindowManager.GraphicsDevice.PresentationParameters;

            sceneRenderTarget = new RenderTarget2D(WindowManager.GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

            normalDepthRenderTarget = new RenderTarget2D(WindowManager.GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, 1,
                pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);


            Texture2D wtf = WindowManager.Content.Load<Texture2D>("player");
            Buttons.Add(new ImageButton(wtf, "WTF is this shit", new Vector2(500, 50)));
            Buttons.Add(new TextButton("Terrain", new Vector2(5, 35)));
            Buttons.Add(new TextButton("Water", new Vector2(5, 65)));
            Buttons.Add(new TextButton("Enemies", new Vector2(5, 95)));
            Buttons.Add(new TextButton("Scenery", new Vector2(5, 125)));
            Buttons.Add(new TextButton("Save", new Vector2(5, 155)));
            Buttons.Add(new TextButton("Load", new Vector2(5, 185)));
            Buttons.Add(new TextButton("Exit", new Vector2(5, 215)));

            ItemOffset = Vector2.Zero;
        }

            TestLevel = new LevelData.Level();
            TestLevel.Initialize();

            CameraClass.setUpCameraClass();
            CameraClass.Position = new Vector3(0, 20, -50);

            Mode = "Run";
            Functions.Add("Exit");
        }

        public override void Draw2D()
        {

            foreach (Button b in Buttons)
            {
                b.Draw();
            }
        }

        public override void Draw3D()
        {
           

            GraphicsDevice device = WindowManager.GraphicsDevice;
            device.SetRenderTarget(0, normalDepthRenderTarget);
            TestLevel.Draw("NormalDepth");
            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.Black);
            TestLevel.Draw("Toon");
            device.SetRenderTarget(0, null);
            device.Clear(Color.Black);
            ApplyPostprocess();

            
            
            base.Draw3D();
        }

        void ApplyPostprocess()
        {
            EffectParameterCollection parameters = postprocessEffect.Parameters;
            string effectTechniqueName;


            Vector2 resolution = new Vector2(sceneRenderTarget.Width,
                                             sceneRenderTarget.Height);

            Texture2D normalDepthTexture = normalDepthRenderTarget.GetTexture();

            parameters["EdgeWidth"].SetValue(1);
            parameters["EdgeIntensity"].SetValue(1);
            parameters["ScreenResolution"].SetValue(resolution);
            parameters["NormalDepthTexture"].SetValue(normalDepthTexture);

            // Choose which effect technique to use.

            effectTechniqueName = "EdgeDetect";

            // Activate the appropriate effect technique.
            postprocessEffect.CurrentTechnique =
                                    postprocessEffect.Techniques[effectTechniqueName];

            // Draw a fullscreen sprite to apply the postprocessing effect.
            WindowManager.SpriteBatch.Begin(SpriteBlendMode.None,
                              SpriteSortMode.Immediate,
                              SaveStateMode.None);

            postprocessEffect.Begin();
            postprocessEffect.CurrentTechnique.Passes[0].Begin();

            WindowManager.SpriteBatch.Draw(sceneRenderTarget.GetTexture(), Vector2.Zero, Color.White);

            WindowManager.SpriteBatch.End();

            postprocessEffect.CurrentTechnique.Passes[0].End();
            postprocessEffect.End();
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
            Rotation.Y += 0.001f;
            CameraClass.Position = Vector3.Transform(new Vector3(0, 10, -40), Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z));

            CameraClass.Update();

            
            bool clickedbutton = false;

            ItemOffset = Vector2.Lerp(ItemOffset, Vector2.Zero, 0.1f);

            foreach (Button b in Buttons)
            {
                b.DrawOffset = ItemOffset;
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
                //WindowManager.Exit();
                Mode = "FinalSave";
            }
            else if (ToolToLoad == "Terrain")
            {
                Pause();
                //WindowManager.AddWindow(new TerrainEditor());
            }
            else if (ToolToLoad == "Water")
            {
                Pause();
                //WindowManager.AddWindow(new WaterEditor());
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
                //Save();
            }
            else if (ToolToLoad == "Load")
            {
                Mode = "Run";
                //Load();
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                UnPause();
        }

        private void FinalSave()
        {
            ItemOffset = Vector2.Lerp(ItemOffset, new Vector2(-Game1.iWidth, 0), 0.1f);

            foreach (Button b in Buttons)
            {
                b.DrawOffset = ItemOffset;
            }

            string exitString = "Do You Want To Save?";
            Vector2 midstring = WindowManager.EditorFont.MeasureString(exitString);
            WindowManager.TextMngr.DrawText(new Vector2(Game1.iWidth / 2, Game1.iHeight / 2) - midstring, exitString);

        }

        public void Pause() { Mode = "Pause"; }
        public void UnPause() { Mode = "Run"; }
    }
}
