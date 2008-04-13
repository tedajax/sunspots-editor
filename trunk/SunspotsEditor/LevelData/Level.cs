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

        List<Real3DObject> LevelPieces;

        public Level(string Filename, ContentManager Content)
        {
            LevelData = LevelData.Load(Filename);
            ContentManager = Content;
        }

        public void Initialize()
        {
            if (LevelData != null)
            {
                LevelPieces = new List<Real3DObject>();
                foreach (LevelData.Generic3DObject GenericObject in LevelData.LevelObjects)
                {
                    Real3DObject RealObject = new Real3DObject(ContentManager, GenericObject);
                    if (RealObject.Initialize(ContentManager))
                    {
                        LevelPieces.Add(RealObject);
                    }
                }
            }

        }

        public void AddSampleData()
        {
            Real3DObject TestObject = new Real3DObject(ContentManager, "unwrappedmodel", Vector3.Zero, Vector3.Zero, new Vector3(1, 1, 1));
            LevelPieces.Add(TestObject);
        }

        public void Save(string Filename)
        {
            if (LevelData != null)
            {
                LevelData.LevelObjects = new List<LevelData.Generic3DObject>();
                foreach (Real3DObject RealObject in LevelPieces)
                {
                    LevelData.LevelObjects.Add(RealObject.GenericObject);
                }
                LevelData.Save(Filename);
            }
        }

    }
}
