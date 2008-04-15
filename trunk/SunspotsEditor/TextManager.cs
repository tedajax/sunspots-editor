using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor
{
    public class TextManager
    {
        List<Text> TextToDraw;
        SpriteBatch TextBatch;

        public TextManager()
        {
            TextToDraw = new List<Text>();
            TextBatch = WindowManager.SpriteBatch;
        }

        #region Draw Calls
        public void DrawText(Vector2 coord, string str)
        {
            Text newtext = new Text(str, coord);
            TryAddText(newtext);
        }

        public void DrawText(Vector2 coord, string str, Color col)
        {
            Text newtext = new Text(str, coord, col);
            TryAddText(newtext);
        }

        public void DrawText(Vector2 coord, string str, Color col, float rot, float scl, SpriteEffects fx)
        {
            Text newtext = new Text(str, coord, col, rot, scl, fx);
            TryAddText(newtext);
        }
        #endregion

        private void TryAddText(Text t)
        {
            if (!CheckDuplicate(t))
                TextToDraw.Add(t);
        }

        private bool CheckDuplicate(Text t)
        {
            //Standard for loop for being nice to 360s
            for (int i = 0; i < TextToDraw.Count; i++)
            {
                if (t.ToString().Equals(TextToDraw[i].ToString()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Draws all the text in text to draw.
        /// Should be located in Draw function.
        /// Clears the TextToDraw list so you have to repopulate it
        /// after every call to DrawAllText!
        /// This allows all the text drawing to be done
        /// in call, which solves the problem of text
        /// causing depth buffer issues.
        /// </summary>
        public void DrawAllText()
        {
            TextBatch.Begin();

            for (int i = 0; i < TextToDraw.Count; i++)
            {
                Text temptext = TextToDraw[i];
                Vector2 Origin = WindowManager.EditorFont.MeasureString(temptext.TextString);
                Origin /= 2;
                TextBatch.DrawString(WindowManager.EditorFont,
                                     temptext.TextString,
                                     temptext.Coordinates + Origin,
                                     temptext.TextColor,
                                     temptext.Rotation,
                                     Origin,
                                     temptext.Scale,
                                     temptext.SpriteEffect,
                                     1f);
            }

            TextToDraw.Clear();

            TextBatch.End();
        }
    }
}
