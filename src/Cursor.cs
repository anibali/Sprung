using System;
using System.Drawing;
using System.Collections.Generic;
using SwinGame;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;

namespace Sprung
{
    public class Cursor
    {
        public enum Type
        {
            Normal,
            Hand,
            None
        }
        
        private static Type type;
        
        public static void Render()
        {
            if(type == Type.None)
            {
            }
            else if(type == Type.Hand)
            {
                Graphics.DrawBitmapOnScreen(Resources.GameImage("CursorHand"), Input.GetMousePosition());
            }
            else
            {
                Graphics.DrawBitmapOnScreen(Resources.GameImage("CursorNormal"), Input.GetMousePosition());
            }
        }
        
        public static void SetType(Type type)
        {
            Cursor.type = type;
        }
    }
}
