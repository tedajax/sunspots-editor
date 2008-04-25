using System;
using System.Collections.Generic;
using System.Text;
using SunspotsEditor.LevelData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows
{
    class EnemyEditor : EditorWindow
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

        static short[] indices;

        protected int SelectedContentItem;

        protected SimpleKeyboardEditableButton[] EditContentButtons;
        protected int SelectedEditButton;

        public override void Initialize()
        {
            base.Initialize();
            
            this.Name = "Enemy";

            CameraMovementSpeed = new Vector3(1, 1, 1);
            CameraRotationSpeed = new Vector3(1, 1, 1);

            PrimitiveBatch = new PrimitiveBatch(WindowManager.GraphicsDevice);

            postprocessEffect = WindowManager.Content.Load<Effect>("Shaders\\postprocessEffect");

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
            //Level.SelectedList.Add();

            CameraClass.setUpCameraClass();
            CameraClass.Position = new Vector3(0, 0, 40);

            EditContentButtons = new SimpleKeyboardEditableButton[8];

            CurrentMode = RunMode.PlaceContent;

            SelectedContentItem = -1;
            SelectedEditButton = 0;

            SwitchedSelectedContent();

            setupIndicies();
        }

        public static void setupIndicies()
        {
            indices = new short[24];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 3;
            indices[7] = 0;

            indices[8] = 4;
            indices[9] = 5;
            indices[10] = 5;
            indices[11] = 6;
            indices[12] = 6;
            indices[13] = 7;
            indices[14] = 7;
            indices[15] = 4;

            indices[16] = 0;
            indices[17] = 4;
            indices[18] = 1;
            indices[19] = 5;
            indices[20] = 2;
            indices[21] = 6;
            indices[22] = 3;
            indices[23] = 7;
        }


        public void UpdateManageContent(GameTime gameTime)
        {
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.A) == KeyInputType.Pressed)
            {
                this.WindowManager.AddWindow(new Windows.LevelPieceWindow.AddNewEnemy());
                this.Pause();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Delete) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    Level.Enemies.RemoveAt(SelectedContentItem);
                    if (SelectedContentItem >= Level.Enemies.Count)
                    {
                        SelectedContentItem = 0;
                    }
                    if (SelectedContentItem < 0) SelectedContentItem = Level.Enemies.Count - 1;
                    if (Level.Enemies.Count == 0) SelectedContentItem = -1;
                    SwitchedSelectedContent();
                }
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.C) == KeyInputType.Pressed)
            {
                if (SelectedContentItem != -1)
                {
                    Enemy Selected = Level.Enemies[SelectedContentItem];
                    Enemy New = new Enemy();
                    New.EnemyObject = new Obj3d(Selected.EnemyObject.GetModel(), Selected.EnemyObject.getContentName(), Selected.EnemyObject.getName());
                    New.EnemyTrigger = new Trigger(Selected.EnemyTrigger.Position, Selected.EnemyTrigger.Scale);
                    New.EnemyType = Selected.EnemyType;
                    New.EnemyObject.setName(Selected.EnemyObject.getName() + "C");
                    New.EnemyObject.setPosition(Selected.EnemyObject.getPosition());
                    New.EnemyObject.setRotation(Selected.EnemyObject.getRotation());
                    New.EnemyObject.setScale(Selected.EnemyObject.getScale());
                    Level.Enemies.Insert(SelectedContentItem + 1, New);
                }
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
            {
                SelectedContentItem++;
                if (Level.Enemies.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem >= Level.Enemies.Count)
                {
                    SelectedContentItem = 0;
                }
                SwitchedSelectedContent();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed)
            {
                SelectedContentItem--;
                if (Level.Enemies.Count == 0)
                {
                    SelectedContentItem = -1;
                }
                else if (SelectedContentItem < 0)
                {
                    SelectedContentItem = Level.Enemies.Count - 1;
                }
                SwitchedSelectedContent();
            }
            if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Back) == KeyInputType.Pressed)
            {
                SelectedContentItem = -1;
                SwitchedSelectedContent();
            }
        }

        public void SwitchedSelectedContent()
        {
            SelectedEditButton = 0;
            if (WindowManager.Level.Enemies.Count > 0)
            {
                if (SelectedContentItem != -1)
                {
                    Obj3d SelectedObj = WindowManager.Level.Enemies[SelectedContentItem].EnemyObject;
                    Enemy SelectedEnemy = WindowManager.Level.Enemies[SelectedContentItem];
                    EditContentButtons = new SimpleKeyboardEditableButton[8];
                    EditContentButtons[0] = new EditTextButton(new Vector2(10, 520), SelectedObj.getName(), "Name : ");
                    EditContentButtons[1] = new EditTextButton(new Vector2(10, 530), SelectedObj.getContentName(), "Content : ");
                    EditContentButtons[2] = new VectorEditButton(new Vector2(10, 540), SelectedObj.getPosition(), "Position : ");
                    EditContentButtons[3] = new VectorEditButton(new Vector2(10, 550), SelectedObj.getRotation(), "Rotation : ");
                    EditContentButtons[4] = new VectorEditButton(new Vector2(10, 560), SelectedObj.getScale(), "Scale : ");

                    EditContentButtons[5] = new EnemyTypeEditButton(new Vector2(10, 570), 0, "Enemy Type : ");
                    int index = 0;
                    if (EditContentButtons[5].GetIntValue(SelectedEnemy.EnemyType, out index))
                        EditContentButtons[5] = new EnemyTypeEditButton(new Vector2(10, 570), index, "Enemy Type : ");

                    EditContentButtons[6] = new VectorEditButton(new Vector2(300, 520), SelectedEnemy.EnemyTrigger.Position, "Trigger Position: ");
                    EditContentButtons[7] = new VectorEditButton(new Vector2(300, 530), SelectedEnemy.EnemyTrigger.Scale, "Trigger Scale : ");

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
            if (WindowManager.Level.Enemies.Count > 0 && SelectedContentItem != -1)
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
                if (WindowManager.Level.Enemies.Count > 0)
                {
                    foreach (SimpleKeyboardEditableButton K in EditContentButtons)
                    {
                        K.Update();
                    }
                    Obj3d SelectedObject = Level.Enemies[SelectedContentItem].EnemyObject;
                    SelectedObject.setName((String)EditContentButtons[0].getEditText());
                    SelectedObject.setPosition((Vector3)EditContentButtons[2].getEditText());
                    SelectedObject.setRotation((Vector3)EditContentButtons[3].getEditText());
                    SelectedObject.setScale((Vector3)EditContentButtons[4].getEditText());
                    Enemy SelectedEnemy = Level.Enemies[SelectedContentItem];
                    string etype = EditContentButtons[5].GetEnemyType();
                    
                    SelectedEnemy.EnemyType = etype;

                    SelectedEnemy.EnemyTrigger.Position = ((Vector3)EditContentButtons[6].getEditText());
                    SelectedEnemy.EnemyTrigger.Scale = ((Vector3)EditContentButtons[7].getEditText());
                }
            }
            else if (SelectedContentItem == -1)
            {
                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Down) == KeyInputType.Pressed)
                {
                    NextButton();
                }
                if (WindowManager.KeyboardMouseManager.getKeyData(Keys.Up) == KeyInputType.Pressed)
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
            for (int i = 0; i < WindowManager.Level.Enemies.Count; i++)
            {
                Obj3d O = WindowManager.Level.Enemies[i].EnemyObject;
                StartPosition += AddValue;
                Color Col = Color.White;
                if (SelectedContentItem == i)
                {
                    Col = Color.Green;
                }
                WindowManager.TextMngr.DrawText(StartPosition, O.getName(), Col);
            }
            DrawEditContent();
            base.Draw2D();
        }

        public void DrawEditContent()
        {
            if ((SelectedContentItem != -1 && WindowManager.Level.Enemies.Count > 0) || SelectedContentItem == -1)
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
            
            device.SetRenderTarget(0, normalDepthRenderTarget);
            device.Clear(Color.Blue);
            DrawLevel("NormalDepth", device);
            Level.Draw("NormalDepth");

            DrawEnemyTrigger();

            device.SetRenderTarget(0, sceneRenderTarget);
            device.Clear(Color.Blue);
            DrawLevel("Toon", device);
            Level.Draw("Toon");

            DrawEnemyTrigger();

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

        public void DrawEnemyTrigger()
        {
            if (SelectedContentItem == -1) return;
            Enemy ene = Level.Enemies[SelectedContentItem];
            BasicEffect basiceffect = new BasicEffect(WindowManager.GraphicsDevice, null);
            basiceffect.VertexColorEnabled = true;

            basiceffect.View = CameraClass.getLookAt();
            basiceffect.Projection = CameraClass.getPerspective();

            Vector3 Pos = ene.EnemyTrigger.Position;
            Vector3 Scl = ene.EnemyTrigger.Scale / 2;

            VertexPositionColor[] Vertices = new VertexPositionColor[8];

            Vertices[0].Position = new Vector3(-Scl.X, -Scl.Y, -Scl.Z);
            Vertices[1].Position = new Vector3(Scl.X, -Scl.Y, -Scl.Z);
            Vertices[2].Position = new Vector3(Scl.X, -Scl.Y, Scl.Z);
            Vertices[3].Position = new Vector3(-Scl.X, -Scl.Y, Scl.Z);

            Vertices[4].Position = new Vector3(-Scl.X, Scl.Y, -Scl.Z);
            Vertices[5].Position = new Vector3(Scl.X, Scl.Y, -Scl.Z);
            Vertices[6].Position = new Vector3(Scl.X, Scl.Y, Scl.Z);
            Vertices[7].Position = new Vector3(-Scl.X, Scl.Y, Scl.Z);

            for (int i = 0; i < 8; i++)
            {
                Vertices[i].Color = Color.Orange;
            }

            Matrix World = Matrix.CreateTranslation(Pos + ene.EnemyObject.getPosition());
            basiceffect.Begin();
            basiceffect.World = World;
            basiceffect.View = CameraClass.getLookAt();
            basiceffect.Projection = CameraClass.getPerspective();
            foreach (EffectPass pass in basiceffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                WindowManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList, Vertices, 0, 8, indices, 0, 12);
                pass.End();
            }
            basiceffect.End();
            
        }

        private void DrawLevel(string Technique, GraphicsDevice device)
        {
            device.RenderState.FillMode = FillMode.WireFrame;
            if (SelectedContentItem == -1) device.RenderState.FillMode = FillMode.Solid;

            for (int i = 0; i < Level.Enemies.Count; i++)
            {
                Obj3d O = Level.Enemies[i].EnemyObject;
                if (SelectedContentItem == i || SelectedContentItem == -1)
                {
                    device.RenderState.FillMode = FillMode.Solid;
                }
                O.DisplayModel(CameraClass.getLookAt(), Technique, Vector3.Zero);
                if (SelectedContentItem == i)
                {
                    device.RenderState.FillMode = FillMode.WireFrame;
                }
            }

            device.RenderState.FillMode = FillMode.Solid;
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
