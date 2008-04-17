using System;
using System.Collections.Generic;
using System.Text;
using SunspotsEditor.LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SunspotsEditor.Windows
{
    class TestWindow : EditorWindow
    {
        enum RunMode { ManageContent, EditContent, PlaceContent, Pause }

        Level Level;
        Effect postprocessEffect;

        RenderTarget2D sceneRenderTarget;
        RenderTarget2D normalDepthRenderTarget;


        PrimitiveBatch PrimitiveBatch;

        RunMode CurrentMode;
        RunMode BeforePauseMode;

        Texture2D RenderTex;

        Vector3 CameraYawPitchRoll;

        float CameraMoveSpeed = .5f;
       

        public TestWindow()
        {
            this.Name = "Terrain";
        }
        public override void Initialize()
        {

            PrimitiveBatch = new PrimitiveBatch(WindowManager.GraphicsDevice);

           postprocessEffect= WindowManager.Content.Load<Effect>("Shaders\\postprocessEffect");

           PresentationParameters pp = WindowManager.GraphicsDevice.PresentationParameters;

           sceneRenderTarget = new RenderTarget2D(WindowManager.GraphicsDevice,
               pp.BackBufferWidth, pp.BackBufferHeight, 1,
               pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

         
           normalDepthRenderTarget = new RenderTarget2D(WindowManager.GraphicsDevice,
               pp.BackBufferWidth, pp.BackBufferHeight, 1,
               pp.BackBufferFormat, pp.MultiSampleType, pp.MultiSampleQuality);

           Level = WindowManager.Level;
            Level.DrawingMode = Level.DrawMode.DrawNotSelected;

            CameraClass.setUpCameraClass();
            CameraClass.Position = new Vector3(0, 0, 40);


            CurrentMode = RunMode.PlaceContent;
        }
        public void UpdateManageContent(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.A) == KeyInputType.Pressed)
            {
               /* LevelData.LevelData.Generic3DObject NewObject = new SunspotsEditor.LevelData.LevelData.Generic3DObject();
                NewObject.ContentName = "LevelObjects\\demolevel2";
                NewObject.Name = "NewObject";
                NewObject.Position = new Vector3();
                NewObject.Rotation = new Vector3();
                NewObject.Scale = new Vector3(1, 1, 1);

                Level.addLevelPiece(Level.ConvertTo3DObject(NewObject, Level.CartoonEffect));*/
                this.WindowManager.AddWindow(new Windows.LevelPieceWindow.AddNewContent());
                this.Pause();
            }
        }

        public void UpdatePlaceContent()
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.D) == KeyInputType.Held)
            {
                CameraYawPitchRoll.Y-=0.01f;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.A) == KeyInputType.Held)
            {
                CameraYawPitchRoll.Y+=0.01f;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.W) == KeyInputType.Held)
            {
                CameraYawPitchRoll.X += 0.01f;
               
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.S) == KeyInputType.Held)
            {
                CameraYawPitchRoll.X -= 0.01f;
               
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(0, 0, -CameraMoveSpeed);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(0, 0, CameraMoveSpeed);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Right) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(CameraMoveSpeed, 0, 0);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Left) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(-CameraMoveSpeed, 0, 0);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }

        }

        public override void Update(GameTime gameTime)
        {
            Vector3 PointAt = new Vector3(0, 0, -10);
            PointAt = Vector3.Transform(PointAt, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z) * Matrix.CreateTranslation(CameraClass.Position));
            CameraClass.CameraPointingAt = PointAt;
            CameraClass.Update();
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Tab) == KeyInputType.Pressed)
            {
                if (CurrentMode == RunMode.PlaceContent)
                {
                    CurrentMode = RunMode.ManageContent;
                    
                }
                else if (CurrentMode == RunMode.ManageContent)
                {
                    CurrentMode = RunMode.EditContent;
                   
                }
                else if (CurrentMode == RunMode.EditContent)
                {
                    CurrentMode = RunMode.PlaceContent;
                }
            }
            Color PlaceContent = Color.White;
            Color ManageContent = Color.White;
            Color EditContent = Color.White;
            if (CurrentMode == RunMode.PlaceContent)
            {
                PlaceContent = Color.Red;
                UpdatePlaceContent();
            }
            if (CurrentMode == RunMode.ManageContent)
            {
              ManageContent = Color.Red;
              UpdateManageContent(gameTime);
            }
            if (CurrentMode == RunMode.EditContent)
            {
                EditContent = Color.Red;
            }
            WindowManager.TextMngr.DrawText(new Vector2(5, 10), "Place Content", PlaceContent, 0, 1f, SpriteEffects.None);
            WindowManager.TextMngr.DrawText(new Vector2(660, 5), "Manage Content", ManageContent, 0, 1f, SpriteEffects.None);
            WindowManager.TextMngr.DrawText(new Vector2(10, 505), "Edit Content", EditContent, 0, 1f, SpriteEffects.None);
            base.Update(gameTime);
        }

        public override void Draw2D()
        {
            Vector2 StartPosition = new Vector2(660, 20);
            Vector2 AddValue = new Vector2(0, 20);
            foreach (Obj3d O in Level.TerrainPieces)
            {
                StartPosition += AddValue;
                WindowManager.TextMngr.DrawText(StartPosition, O.getName());


            }
            base.Draw2D();
        }

        public override void Draw3D()
        {
           

            WindowManager.GraphicsDevice.RenderState.DepthBufferEnable = true;
            WindowManager.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            WindowManager.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

            RenderState renderState = WindowManager.GraphicsDevice.RenderState;

            renderState.AlphaBlendEnable = true;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;

            GraphicsDevice device = WindowManager.GraphicsDevice;
            
            device.SetRenderTarget(0, normalDepthRenderTarget);
            device.Clear(Color.Blue);
            Level.Draw("NormalDepth");
            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.Blue);
            Level.Draw("Toon");
            device.SetRenderTarget(0, null);
            device.Clear(Color.Black);
            ApplyPostprocess();

            PrimitiveBatch.Begin(PrimitiveType.LineList);
            PrimitiveBatch.AddVertex(new Vector2(650, 0), Color.White);
            PrimitiveBatch.AddVertex(new Vector2(650, 600), Color.White);
            PrimitiveBatch.AddVertex(new Vector2(650, 500), Color.White);
            PrimitiveBatch.AddVertex(new Vector2(0, 500), Color.White);
            PrimitiveBatch.End();


           
           

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

            RenderTex = sceneRenderTarget.GetTexture();
            WindowManager.SpriteBatch.Draw(RenderTex, new Rectangle(0, 0, 650, 500), Color.White);
            WindowManager.SpriteBatch.End();

            postprocessEffect.CurrentTechnique.Passes[0].End();
            postprocessEffect.End();
        }

        public void Pause()
        {
            BeforePauseMode = CurrentMode;
            CurrentMode = RunMode.Pause;
        }

        public void Unpause()
        {
            CurrentMode = BeforePauseMode;
        }

        public override string ToString()
        {
            return "Terrain";
        }

    }
}
