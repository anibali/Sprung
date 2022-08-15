using System;
using System.Drawing;

using SwinGame;
using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;
using Font = SwinGame.Font;
using FontStyle = SwinGame.FontStyle;

namespace Sprung
{
    public class Hyperlink : GameObject
    {
        private string text, url;
        private static Font font = Resources.GameFont("SansSerifSmall");
        private bool rollover = false;
        
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        
        public string URL
        {
            get { return url; }
            set { url = value; }
        }
        
        public Hyperlink(string text, string url) : this(text, url, 0, 0)
        {
        }
        
        public Hyperlink(string text, string url, float x, float y) : base(new Bitmap(1, 1))
        {
            this.text = text;
            this.url = url;
            X = x;
            Y = y;
        }
        
        private void OnClicked()
        {
            System.Diagnostics.Process.Start(url);
        }
        
        public void HandleInput()
        {
            int mx = (int)Input.GetMousePosition().X, my = (int)Input.GetMousePosition().Y;
            
            rollover = false;
            
            if(Shapes.CreateRectangle(X, Y, font.TextWidth(text), font.TextHeight(text)).Contains(mx, my))
            {
                rollover = true;
                
                if(Input.MouseWasClicked(MouseButton.LeftButton))
                {
                    OnClicked();
                }
            }
        }
        
        public override void Render()
        {
            if(rollover)
            {
                font.SetStyle(FontStyle.UnderlineFont);
                Cursor.SetType(Cursor.Type.Hand);
            }
            else
            {
                font.SetStyle(FontStyle.NormalFont);
            }
            
            SwinGame.Text.DrawText(text, Color.FromArgb(64, 96, 255), font, X, Y);
        }
    }
}
