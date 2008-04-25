using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    public class Enemy
    {
        public Obj3d EnemyObject;
        public String EnemyType;
        public Trigger EnemyTrigger;

        public Enemy()
        {
            EnemyTrigger = new Trigger(Vector3.Zero, Vector3.One);
        }
    }
}
