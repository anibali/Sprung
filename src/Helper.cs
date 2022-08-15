using System;
using System.Drawing;

using SwinGame;
using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;
using Font = SwinGame.Font;
using FontStyle = SwinGame.FontStyle;
using Event = SwinGame.Event;
using CollisionSide = SwinGame.CollisionSide;
using Sprite = SwinGame.Sprite;

namespace Sprung
{
    public class Helper
    {
        public static void DrawCenteredTextOnScreen(string text, Color color, Font font, int x, int y)
        {
            Text.DrawTextOnScreen(text, color, font, x - Text.TextWidth(text, font) / 2, y - Text.TextHeight(text, font) / 2);
        }
        
        public static void DrawCenteredText(string text, Color color, Font font, Point2D point)
        {
            Text.DrawText(text, color, font, point.X - Text.TextWidth(text, font) / 2, point.Y - Text.TextHeight(text, font) / 2);
        }
        
        public static string Ordinal(int cardinal)
        {
            string suffix;
            
            switch(cardinal % 10)
            {
                case 1:
                    suffix = "st";
                    break;
            
                case 2:
                    suffix = "nd";
                    break;
            
                case 3:
                    suffix = "rd";
                    break;
            
                default:
                    suffix = "th";
                    break;
            }
            
            int lastTwoDigits = cardinal % 100;
            if(11 <= lastTwoDigits && lastTwoDigits <= 13)
            {
                suffix = "th";
            }
            
            return cardinal.ToString() + suffix;
        }
    }
}
