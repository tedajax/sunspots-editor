using System;
using System.Collections.Generic;
using System.Text;
using SunspotsEditor.LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SunspotsEditor.Windows
{
    class TestWindow : EditorWindow
    {
        protected Level Level;
        protected Effect postprocessEffect;

        protected RenderTarget2D sceneRenderTarget;
        protected RenderTarget2D normalDepthRenderTarget;

        protected PrimitiveBatch PrimitiveBatch;

        protected RunMode CurrentMode;
        protected RunMode BeforePauseMode;

        protected Texture2D RenderTex;

        protected Vector3 CameraYawPitchRoll;

        protected Vector3 CameraMovementSpeed;
        protected Vector3 CameraRotationSpeed;

        protected int SelectedContentItem;

        protected SimpleKeyboardEditableButton[] EditContentButtons;
        protected int SelectedEditButton;
       
        public override void Initialize()
        {
            this.Name = "Terrain";

            CameraMovementSpeed = new Vector3(1, 1, 1);
            CameraRotationSpeed = new Vector3(1, 1, 1);

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
                Level.SelectedList = new List<Level.DrawTypes>();
                Level.SelectedList.Add(Level.DrawTypes.LevelPieces);

            CameraClass.setUpCameraClass();
            CameraClass.Position = new Vector3(0, 0, 40);

            EditContentButtons = new SimpleKeyboardEditableButton[5];

            CurrentMode = RunMode.PlaceContent;

            SelectedContentItem = -1;
            SelectedEditButton = 0;

            SwitchedSelectedContent();
        }
        public void UpdateManageContent(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.A) == KeyInputType.Pressed)
            {
                this.WindowManager.AddWindow(new Windows.LevelPieceWindow.AddNewContent());
                this.Pause();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                SelectedContentItem++;
                if (Level.TerrainPieces.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem >= Level.TerrainPieces.Count)
                {
                    SelectedContentItem = 0;
                }
                SwitchedSelectedContent();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
            {
                SelectedContentItem--;
                if (Level.TerrainPieces.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem < 0)
                {
                    SelectedContentItem = Level.TerrainPieces.Count - 1;
                }
                SwitchedSelectedContent();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Back) == KeyInputType.Pressed)
            {
                SelectedContentItem = -1;
                SwitchedSelectedContent();
            }
           
        }

        public void SwitchedSelectedContent()
        {
            SelectedEditButton = 0;
            if (WindowManager.Level.TerrainPieces.Count > 0)
            {
                if (SelectedContentItem != -1)
                {
                    Obj3d SelectedObj = WindowManager.Level.TerrainPieces[SelectedContentItem];
                    EditContentButtons = new SimpleKeyboardEditableButton[5];
                    EditContentButtons[0] = new EditTextButton(new Vector2(10, 520), SelectedObj.getName(), "Name : ");
                    EditContentButtons[1] = new EditTextButton(new Vector2(10, 530), SelectedObj.getContentName(), "Content : ");
                    EditContentButtons[2] = new VectorEditButton(new Vector2(10, 540), SelectedObj.getPosition(), "Position : ");
                    EditContentButtons[3] = new VectorEditButton(new Vector2(10, 550), SelectedObj.getRotation(), "Rotation : ");
                    EditContentButtons[4] = new VectorEditButton(new Vector2(10, 560), SelectedObj.getScale(), "Scale : ");
                    EditContentButtons[SelectedEditButton].GainFocus();
                }
            }
            if (SelectedContentItem == -1)
            {
                EditContentButtons = new SimpleKeyboardEditableButton[2];
                EditContentButtons[0] = new VectorEditButton(new Vector2(10, 520), CameraMovementSpeed, "Camera Move Speed : ");
                EditContentButtons[1] = new VectorEditButton(new Vector2(10, 535), CameraRotationSpeed, "Camera Rotation Speed : ");
                EditContentButtons[SelectedEditButton].GainFocus();
            }
           
            
        }

        public void UpdatePlaceContent()
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.D) == KeyInputType.Held)
            {
                CameraYawPitchRoll.Y -= MathHelper.ToRadians(CameraRotationSpeed.Y);
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.A) == KeyInputType.Held)
            {
                CameraYawPitchRoll.Y += MathHelper.ToRadians(CameraRotationSpeed.Y);
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.W) == KeyInputType.Held)
            {
                CameraYawPitchRoll.X += MathHelper.ToRadians(CameraRotationSpeed.X);
               
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.S) == KeyInputType.Held)
            {
                CameraYawPitchRoll.X -= MathHelper.ToRadians(CameraRotationSpeed.X);
               
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(0, 0, -CameraMovementSpeed.Z);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(0, 0, CameraMovementSpeed.Z);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Right) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(CameraMovementSpeed.X, 0, 0);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Left) == KeyInputType.Held)
            {
                Vector3 Move = new Vector3(-CameraMovementSpeed.X, 0, 0);
                Move = Vector3.Transform(Move, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z));
                CameraClass.Position += Move;
            }

        }

        public void NextButton()
        {
            EditContentButtons[SelectedEditButton].LoseFocus();
            SelectedEditButton++;
            if (SelectedEditButton >= EditContentButtons.Length) SelectedEditButton = 0;
            EditContentButtons[SelectedEditButton].GainFocus();
        }

        public void PreviousButton()
        {
            EditContentButtons[SelectedEditButton].LoseFocus();
            SelectedEditButton--;
            if (SelectedEditButton < 0) SelectedEditButton = EditContentButtons.Length - 1;
            EditContentButtons[SelectedEditButton].GainFocus();
        }

        public void UpdateEditContent()
        {
            if (WindowManager.Level.TerrainPieces.Count > 0 && SelectedContentItem != -1)
            {
                if (!EditContentButtons[SelectedEditButton].GetScrolling())
                {
                    if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Pressed)
                    {
                        NextButton();
                    }
                    if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
                    {
                        PreviousButton();
                    }
                }
                if (WindowManager.Level.TerrainPieces.Count > 0)
                {
                    foreach (SimpleKeyboardEditableButton K in EditContentButtons)
                    {
                        K.Update();
                    }
                    Obj3d SelectedObject = Level.TerrainPieces[SelectedContentItem];
                    SelectedObject.setName((String)EditContentButtons[0].getEditText());
                    SelectedObject.setPosition((Vector3)EditContentButtons[2].getEditText());
                    SelectedObject.setRotation((Vector3)EditContentButtons[3].getEditText());
                    SelectedObject.setScale((Vector3)EditContentButtons[4].getEditText());
                }
            }
            else if (SelectedContentItem == -1)
            {
                if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Down) == KeyInputType.Pressed)
                {
                    NextButton(); 
                }
                if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
                {
                    PreviousButton();
                }
                            
                foreach (SimpleKeyboardEditableButton K in EditContentButtons)
                {
                    K.Update();
                }
                CameraMovementSpeed = (Vector3)EditContentButtons[0].getEditText();
                CameraRotationSpeed = (Vector3)EditContentButtons[1].getEditText();
                
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 PointAt = new Vector3(0, 0, -10);
            PointAt = Vector3.Transform(PointAt, Matrix.CreateFromYawPitchRoll(CameraYawPitchRoll.Y, CameraYawPitchRoll.X, CameraYawPitchRoll.Z) * Matrix.CreateTranslation(CameraClass.Position));
            CameraClass.CameraPointingAt = PointAt;
            CameraClass.Update();
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Escape) == KeyInputType.Pressed)
            {
                if (CurrentMode != RunMode.Pause)
                {
                    WindowManager.RemoveWindow(this);
                    SelectTool Window = (SelectTool)WindowManager.FindWindow("Select");
                    Window.UnPause();
                }

            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Tab) == KeyInputType.Pressed)
            {
                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.LeftShift) == KeyInputType.Released)
                {
                    if (CurrentMode == RunMode.PlaceContent)
                    {
                        CurrentMode = RunMode.ManageContent;

                    }
                    else if (CurrentMode == RunMode.ManageContent)
                    {
                        CurrentMode = RunMode.EditContent;
                        EditContentButtons[SelectedEditButton].GainFocus();

                    }
                    else if (CurrentMode == RunMode.EditContent)
                    {
                        CurrentMode = RunMode.PlaceContent; 
                       
                    }
                }
                else
                {
                    if (CurrentMode == RunMode.PlaceContent)
                    {
                        CurrentMode = RunMode.EditContent;
                        EditContentButtons[SelectedEditButton].GainFocus();
                    }
                    else if (CurrentMode == RunMode.EditContent)
                    {
                        CurrentMode = RunMode.ManageContent;

                    }
                    else if (CurrentMode == RunMode.ManageContent)
                    {
                        CurrentMode = RunMode.PlaceContent;
                    }
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
                UpdateEditContent();
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
            for (int i =0; i<WindowManager.Level.TerrainPieces.Count;i++)
            {
                Obj3d O = WindowManager.Level.TerrainPieces[i];
                StartPosition += AddValue;
                Color Col = Color.White;
                if (SelectedContentItem == i)
                {
                    Col = Color.Green;
                }
                WindowManager.TextMngr.DrawText(StartPosition, O.getName(),Col);
            }
            DrawEditContent();
            base.Draw2D();
        }

        public void DrawEditContent()
        {
            if ((SelectedContentItem != -1 && WindowManager.Level.TerrainPieces.Count > 0) || SelectedContentItem == -1)
            {
                foreach (SimpleKeyboardEditableButton k in EditContentButtons)
                {
                    k.Draw2D();
                }
            }
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
            device.RenderState.FillMode = FillMode.WireFrame;
            if (SelectedContentItem == -1) device.RenderState.FillMode = FillMode.Solid;

            device.SetRenderTarget(0, normalDepthRenderTarget);
            device.Clear(Color.Blue);
            DrawLevel("NormalDepth",device);
            Level.Draw("NormalDepth");
            
            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.Blue);
            DrawLevel("Toon",device);
            Level.Draw("Toon");
            device.RenderState.FillMode = FillMode.Solid;
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

        private void DrawLevel(string Technique, GraphicsDevice device)
        {
            for (int i = 0; i < Level.TerrainPieces.Count; i++)
            {
                Obj3d O = Level.TerrainPieces[i];
                if (SelectedContentItem == i||SelectedContentItem == -1)
                {
                    device.RenderState.FillMode = FillMode.Solid;
                }
                O.DisplayModel(CameraClass.getLookAt(), Technique, Vector3.Zero);
                if (SelectedContentItem == i)
                {
                    device.RenderState.FillMode = FillMode.WireFrame;
                }
            }
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
            
            
            /*postprocessEffect.Begin();
            postprocessEffect.CurrentTechnique.Passes[0].Begin();*/
            
            RenderTex = sceneRenderTarget.GetTexture();
            WindowManager.SpriteBatch.Draw(RenderTex, new Rectangle(0, 0, 650, 500), Color.White);
            WindowManager.SpriteBatch.End();
            
            /*postprocessEffect.CurrentTechnique.Passes[0].End();
            postprocessEffect.End();*/
        }

        public void Pause()
        {
            BeforePauseMode = CurrentMode;
            CurrentMode = RunMode.Pause;
        }

        public void Unpause()
        {
            CurrentMode = BeforePauseMode;
            this.Level = WindowManager.Level;
        }
    }
}
