using System;
using System.Collections.Generic;
using System.Text;
using SunspotsEditor.LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SunspotsEditor.Windows
{
    class Waypoints : EditorWindow
    {
        
        Level Level;
        Effect postprocessEffect;

        RenderTarget2D sceneRenderTarget;
        RenderTarget2D normalDepthRenderTarget;

        PrimitiveBatch PrimitiveBatch;

        RunMode CurrentMode;
        RunMode BeforePauseMode;

        Texture2D RenderTex;

        Vector3 CameraYawPitchRoll;

        Vector3 CameraMovementSpeed;
        Vector3 CameraRotationSpeed;

        int SelectedContentItem;

        SimpleKeyboardEditableButton[] EditContentButtons;
        int SelectedEditButton;

        Obj3d WaypointObject;
       
        public override void Initialize()
        {
            this.Name = "Waypoints";

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
                Level.SelectedList.Add(Level.DrawTypes.WaypointData);

             Model Waypoint = WindowManager.Content.Load<Model>("testship2-2-3ds");
             Level.ChangeEffectUsedByModel(Waypoint, Level.CartoonEffect);
             WaypointObject = new Obj3d(Waypoint, "waypoint", "waypoint");
             WaypointObject.setRotation(new Vector3(-90, 0, 0));
             WaypointObject.setScale(.2f);

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
                this.WindowManager.AddWindow(new Windows.Waypoint.AddWaypoint(Level.Waypoints));
                this.Pause();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Delete) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    Level.Waypoints.WaypointData.RemoveAt(SelectedContentItem);
                    if (SelectedContentItem >= Level.Waypoints.WaypointData.Count)
                    {
                        SelectedContentItem = 0;
                    }
                    if (SelectedContentItem < 0) SelectedContentItem = Level.Waypoints.WaypointData.Count - 1;
                    if (Level.Waypoints.WaypointData.Count == 0) SelectedContentItem = -1;
                    SwitchedSelectedContent();
                }
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.PageUp) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    LevelData.LevelData.WaypointData Selected = Level.Waypoints.WaypointData[SelectedContentItem];
                    LevelData.LevelData.WaypointData New = new SunspotsEditor.LevelData.LevelData.WaypointData();
                    New.Name = Selected.Name;
                    New.Position = Selected.Position;
                    if (SelectedContentItem != 0)
                    {
                        Level.Waypoints.WaypointData.Insert(SelectedContentItem - 1, New);
                        Level.Waypoints.WaypointData.Remove(Selected);
                        SelectedContentItem = Level.Waypoints.WaypointData.IndexOf(New);
                    }
                }
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.PageDown) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    LevelData.LevelData.WaypointData Selected = Level.Waypoints.WaypointData[SelectedContentItem];
                    LevelData.LevelData.WaypointData New = new SunspotsEditor.LevelData.LevelData.WaypointData();
                    New.Name = Selected.Name;
                    New.Position = Selected.Position;
                    if (SelectedContentItem != Level.Waypoints.WaypointData.Count-1)
                    {
                        Level.Waypoints.WaypointData.Insert(SelectedContentItem + 2, New);
                        Level.Waypoints.WaypointData.Remove(Selected);
                        SelectedContentItem = Level.Waypoints.WaypointData.IndexOf(New);
                    }
                }

            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.C) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    LevelData.LevelData.WaypointData Selected = Level.Waypoints.WaypointData[SelectedContentItem];
                    LevelData.LevelData.WaypointData New = new SunspotsEditor.LevelData.LevelData.WaypointData();
                    New.Name = Selected.Name + "C";
                    New.Position = Selected.Position;
                    Level.Waypoints.WaypointData.Insert(SelectedContentItem+1,New);
                }
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                SelectedContentItem++;
                if (Level.Waypoints.WaypointData.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem >= Level.Waypoints.WaypointData.Count)
                {
                    SelectedContentItem = 0;
                }
                SwitchedSelectedContent();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Microsoft.Xna.Framework.Input.Keys.Up) == KeyInputType.Pressed)
            {
                SelectedContentItem--;
                if (Level.Waypoints.WaypointData.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem < 0)
                {
                    SelectedContentItem = Level.Waypoints.WaypointData.Count - 1;
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
            if (WindowManager.Level.Waypoints.WaypointData.Count > 0)
            {
                if (SelectedContentItem != -1)
                {
                    LevelData.LevelData.WaypointData SelectedWaypoint = Level.Waypoints.WaypointData[SelectedContentItem];
                    EditContentButtons = new SimpleKeyboardEditableButton[2];
                    EditContentButtons[0] = new EditTextButton(new Vector2(10, 520), SelectedWaypoint.Name, "Name : ");
                    EditContentButtons[1] = new VectorEditButton(new Vector2(10, 540), SelectedWaypoint.Position, "Position : ");
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
            if (SelectedContentItem != -1 && WindowManager.Level.Waypoints.WaypointData.Count > 0)
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
               
                foreach (SimpleKeyboardEditableButton K in EditContentButtons)
                {
                    K.Update();
                }
                LevelData.LevelData.WaypointData SelectedWaypoint = Level.Waypoints.WaypointData[SelectedContentItem];
                SelectedWaypoint.Name = (String)EditContentButtons[0].getEditText();
                SelectedWaypoint.Position = (Vector3)EditContentButtons[1].getEditText();
            }
            else
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
            int min = 0 ;
            int max=0;
            if (SelectedContentItem == -1)
            {
                min = 0;
                max = 30;
            }
            else
            {
                if (SelectedContentItem > 10)
                {

                    min = SelectedContentItem - 10;
                    max = SelectedContentItem + 20;
                }
                else
                {
                    min = 0;
                    max = 30;
                }
            }
            for (int i =0; i<WindowManager.Level.Waypoints.WaypointData.Count;i++)
            {
                if (i >= min && i <= max)
                {
                    LevelData.LevelData.WaypointData SelectedWaypoint = Level.Waypoints.WaypointData[i];
                    StartPosition += AddValue;
                    Color Col = Color.White;
                    if (SelectedContentItem == i)
                    {
                        Col = Color.Green;
                    }
                    WindowManager.TextMngr.DrawText(StartPosition, SelectedWaypoint.Name, Col);
                }
            }
            DrawEditContent();
            base.Draw2D();
        }

        public void DrawEditContent()
        {
            if ((SelectedContentItem != -1 && WindowManager.Level.Waypoints.WaypointData.Count>0) || SelectedContentItem == -1)
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
            WindowManager.GraphicsDevice.RenderState.CullMode = CullMode.None;

            RenderState renderState = WindowManager.GraphicsDevice.RenderState;

            renderState.AlphaBlendEnable = true;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;

            GraphicsDevice device = WindowManager.GraphicsDevice;
            device.RenderState.FillMode = FillMode.Solid;
            
            device.SetRenderTarget(0, normalDepthRenderTarget);
            device.Clear(Color.Blue);
            Level.Draw("NormalDepth");
            DrawWaypoints("NormalDepth",device);
            DrawLine(device, "NormalDepth");
            
            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.Blue);
            Level.Draw("Toon");
            DrawWaypoints("Toon",device);
            DrawLine(device, "Toon");


            
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

        public void DrawLine(GraphicsDevice Device, string Technique)
        {
            if (this.Level.Waypoints.WaypointData.Count <= 1)
            {
                return;
            }
            else
            {
                Effect Cartoon = Level.CartoonEffect;
                Cartoon.CurrentTechnique = Cartoon.Techniques[Technique];
                Cartoon.Parameters["World"].SetValue(Matrix.Identity);
                Cartoon.Parameters["View"].SetValue(CameraClass.getLookAt());
                Cartoon.Parameters["Projection"].SetValue(CameraClass.getPerspective());

                VertexPositionColor[] VertList = new VertexPositionColor[Level.Waypoints.WaypointData.Count];
                short[] Indicies = new short[(Level.Waypoints.WaypointData.Count * 2) - 2];

                for (int i = 0; i < Level.Waypoints.WaypointData.Count; i++)
                {
                    LevelData.LevelData.WaypointData D = Level.Waypoints.WaypointData[i];
                    VertList[i] = new VertexPositionColor(D.Position, Color.Red);
                }
                for (int i = 0; i < Level.Waypoints.WaypointData.Count - 1; i++)
                {
                    Indicies[i * 2] = (short)i;
                    Indicies[(i * 2) + 1] = (short)(i + 1);
                }

                Device.RenderState.PointSize = 10f;
                Cartoon.Begin();
                foreach (EffectPass Pass in Cartoon.CurrentTechnique.Passes)
                {
                    Pass.Begin();
                    Device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, VertList, 0, VertList.Length, Indicies, 0, VertList.Length - 1);
                    Pass.End();
                }
                Cartoon.End();
            }
        }

        public void DrawWaypoints(String technique, GraphicsDevice device)
        {
            
            for (int i =0; i<Level.Waypoints.WaypointData.Count;i++)
            {
                LevelData.LevelData.WaypointData D = Level.Waypoints.WaypointData[i];
                if (SelectedContentItem != -1 && technique == "Toon")
                {
                    if (SelectedContentItem == i)
                    {

                        WaypointObject.setPosition(D.Position);
                        WaypointObject.DisplayModel(CameraClass.getLookAt(), technique, Vector3.Zero);
                    }
                }
                else
                {
                    WaypointObject.setPosition(D.Position);
                    WaypointObject.DisplayModel(CameraClass.getLookAt(), technique, Vector3.Zero);
                }
                device.RenderState.FillMode = FillMode.Solid;
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
            this.Level = WindowManager.Level;
        }
    }
}
