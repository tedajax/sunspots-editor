using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SunspotsEditor
{
    public class Trigger
    {
        public Vector3 Position;
        public Vector3 Scale;

        public Trigger(Vector3 pos, Vector3 scl)
        {
            Position = pos;
            Scale = scl;
        }
    }
}
