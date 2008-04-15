using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    class Text
    {
        public string TextString;
        public Vector2 Coordinates;
        public Color TextColor;
        public float Rotation;
        public float Scale;
        public SpriteEffects SpriteEffect;

        public Text(string str, Vector2 p)
        {
            TextString = str;
            Coordinates = p;
            TextColor = Color.White;
            Rotation = 0f;
            Scale = 1f;
            SpriteEffect = SpriteEffects.None;
        }

        public Text(string str, Vector2 p, Color col)
        {
            TextString = str;
            Coordinates = p;
            TextColor = col;
            Rotation = 0f;
            Scale = 1f;
            SpriteEffect = SpriteEffects.None;
        }

        public Text(string str, Vector2 p, Color col, float rot, float scl, SpriteEffects fx)
        {
            TextString = str;
            Coordinates = p;
            TextColor = col;
            Rotation = rot;
            Scale = scl;
            SpriteEffect = fx;
        }

        public override string ToString()
        {
            StringBuilder returnstring = new StringBuilder(TextString);
            returnstring.Append(" [");
            returnstring.Append(Coordinates.X);
            returnstring.Append(" : ");
            returnstring.Append(Coordinates.Y);
            returnstring.Append("]");
            returnstring.Append(" {");
            returnstring.Append(Rotation);
            returnstring.Append("};");

            return returnstring.ToString();
        }
    }
}
