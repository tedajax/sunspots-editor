using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SunspotsEditor
{
    public class KeyboardMouseManager
    {
        KeyboardState NewKeyboardState;
        KeyboardState OldKeyboardState;

        MouseState NewMouseState;
        MouseState OldMouseState;

        KeyboardTyping KeyboardTyping;

        String CurrentKeyboardString;

        public KeyboardMouseManager(KeyboardState KeyboardState, MouseState MouseState)
        {
         

            NewKeyboardState = KeyboardState;
            NewMouseState = MouseState;
            OldKeyboardState = KeyboardState;
            OldMouseState = MouseState;
            KeyboardTyping = new KeyboardTyping(OldKeyboardState);
        }

        public KeyInputType getKeyData(Keys Key)
        {
            if (NewKeyboardState.IsKeyDown(Key))
            {
                if (OldKeyboardState.IsKeyDown(Key))
                {
                    return KeyInputType.Held;
                }
                else
                {
                    return KeyInputType.Pressed;
                }
            }
            return KeyInputType.Released;
        }

        public void Update(KeyboardState KeyboardState, MouseState MouseState, GameTime gameTime)
        {
            
            OldKeyboardState = NewKeyboardState;
            OldMouseState = NewMouseState;
            NewKeyboardState = KeyboardState;
            NewMouseState = MouseState;

            CurrentKeyboardString = KeyboardTyping.update(gameTime, NewKeyboardState);

        }

        public String GetKeyboardTyping()
        {
            return CurrentKeyboardString;
        }

        public void ClearKeyboardString() { KeyboardTyping.clearCurrentString(); }

        public void SetKeyboardString(String newString) { KeyboardTyping.setCurrentString(newString); }
    }
}
