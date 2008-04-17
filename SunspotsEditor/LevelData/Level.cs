using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SunspotsEditor.LevelData
{
    public class Real3DObject
    {
        
        Model Model;

        public LevelData.Generic3DObject GenericObject;
        
        public Real3DObject(ContentManager Content, String ContentName, Vector3 Position, Vector3 Rotation, Vector3 Scale)
        {
            GenericObject = new LevelData.Generic3DObject();
            GenericObject.ContentName = ContentName;
            GenericObject.Position = Position;
            GenericObject.Rotation = Rotation;
            GenericObject.Scale = Scale;

            Model = null;

            this.Initialize(Content);
        }

        public Real3DObject(ContentManager Content, LevelData.Generic3DObject GenericObject)
        {
            this.GenericObject = GenericObject;
            Model = null;
        }

        public bool Initialize(ContentManager Content)
        {
            try
            {
                Content.Load<Model>(GenericObject.ContentName);
            }
            catch
            {
                Model = null;
                return false;
            }
            return true;
        }


        public Model getModel() { return this.Model; }



    }

    public class Level
    {
        LevelData LevelData;

        ContentManager ContentManager;

        public enum DrawMode { DrawOnlySelected, DrawNotSelected }

        public enum DrawTypes { LevelPieces }

        public List<DrawTypes> SelectedList;
        public DrawMode DrawingMode;

        List<Obj3d> LevelPieces;

        public List<Obj3d> TerrainPieces
        {
            get
            {
                return LevelPieces;
            }
        }

        public Effect CartoonEffect;

        public Level(string Filename, ContentManager Content)
        {
            LevelData = LevelData.Load(Filename);
            ContentManager = Content;
            SelectedList = new List<DrawTypes>();
            DrawingMode = DrawMode.DrawNotSelected;
            
        }

        public void addLevelPiece(Obj3d Object)
        {
            LevelPieces.Add(Object);
        }

        public void Initialize()
        {
            CartoonEffect = ContentManager.Load<Effect>("Shaders\\CartoonEffect");
            if (LevelData != null)
            {
                LevelPieces = new List<Obj3d>();
                foreach (LevelData.Generic3DObject GenericObject in LevelData.LevelObjects)
                {
                    Obj3d Object = ConvertTo3DObject(GenericObject,CartoonEffect);
                    LevelPieces.Add(Object);
                    
                }
            }

        }

        public static void ChangeEffectUsedByModel(Model model, Effect replacementEffect)
        {

            // Table mapping the original effects to our replacement versions.
            Dictionary<Effect, Effect> effectMapping = new Dictionary<Effect, Effect>();

            foreach (ModelMesh mesh in model.Meshes)
            {
             
                // Scan over all the effects currently on the mesh.
                if (mesh.Effects[0].ToString() == "Microsoft.Xna.Framework.Graphics.BasicEffect")
                {
                    foreach (BasicEffect oldEffect in mesh.Effects)
                    {
                        // If we haven't already seen this effect...
                        if (!effectMapping.ContainsKey(oldEffect))
                        {
                            // Make a clone of our replacement effect. We can't just use
                            // it directly, because the same effect might need to be
                            // applied several times to different parts of the llmodel using
                            // a different texture each time, so we need a fresh copy each
                            // time we want to set a different texture into it.
                            Effect newEffect = replacementEffect.Clone(
                                                        replacementEffect.GraphicsDevice);

                            // Copy across the texture from the original effect.
                            newEffect.Parameters["Texture"].SetValue(oldEffect.Texture);

                            newEffect.Parameters["TextureEnabled"].SetValue(
                                                                oldEffect.TextureEnabled);

                            effectMapping.Add(oldEffect, newEffect);
                        }
                    }

                    // Now that we've found all the effects in use on this mesh,
                    // update it to use our new replacement versions.
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = effectMapping[meshPart.Effect];
                    }
                }
            }
        }

        public Obj3d ConvertTo3DObject(LevelData.Generic3DObject GenericObject, Effect CartoonEffect)
        {
            Model Model = ContentManager.Load<Model>(GenericObject.ContentName);
            ChangeEffectUsedByModel(Model, CartoonEffect);
            Obj3d Object = new Obj3d(Model, GenericObject.Position, GenericObject.Rotation, GenericObject.ContentName, GenericObject.Name);
            Object.setScale(GenericObject.Scale);
            return Object;
        }

        public LevelData.Generic3DObject ConvertToGeneric3DObject(Obj3d Object)
        {
            LevelData.Generic3DObject GenericObject = new LevelData.Generic3DObject();
            GenericObject.ContentName = Object.getContentName();
            GenericObject.Position = Object.getPosition();
            GenericObject.Rotation = Object.getRotation();
            GenericObject.Scale = Object.getScale();
            GenericObject.Name = Object.getName();

            return GenericObject;
        }

        public void Save(string Filename)
        {
            if (LevelData != null)
            {
                LevelData.LevelObjects = new List<LevelData.Generic3DObject>();
                foreach (Obj3d RealObject in LevelPieces)
                {
                    LevelData.LevelObjects.Add(ConvertToGeneric3DObject(RealObject));
                }
                LevelData.Save(Filename);
            }
        }

        public void Draw(String technique)
        {
            if (DrawingMode == DrawMode.DrawNotSelected)
            {
                DrawNotSelected(technique);
            }

        }

        public void DrawNotSelected(String technique)
        {
            if(!SelectedList.Contains(DrawTypes.LevelPieces))
            {
                foreach(Obj3d O in LevelPieces)
                {
                    O.DisplayModel(CameraClass.getLookAt(), technique, Vector3.Zero);
                }
            }
        }

        

    }
}
