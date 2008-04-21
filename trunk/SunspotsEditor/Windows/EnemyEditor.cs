using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SunspotsEditor.Windows
{
    class EnemyEditor : TestWindow
    {
        public override void Initialize()
        {
            base.Initialize();

            this.Name = "Enemy";
        }
    }
}
