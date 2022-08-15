using System;
using System.Drawing;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class MenuButton : Button
    {
        public MenuButton(string text) : base(Resources.GameImage("MenuButton"), text)
        {
        }
        
        public override void Render()
        {
            Graphics.DrawSprite(this);
            
            if(rollover)
            {
                Helper.DrawCenteredText(this.Text, Color.Green, Resources.GameFont("MenuButton"), this.CenterPoint);
                Cursor.SetType(Cursor.Type.Hand);
            }
            else
            {
                Helper.DrawCenteredText(this.Text, Color.Black, Resources.GameFont("MenuButton"), this.CenterPoint);
            }
        }
    }
}
