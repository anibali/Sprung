using System;
using System.Drawing;
using System.Collections.Generic;
using SwinGame;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;
using Font = SwinGame.Font;
using FontStyle = SwinGame.FontStyle;

namespace Sprung
{
    public class CreditsState : GameState
    {
        private static Bitmap background = Resources.GameImage("CreditsBackground");
        private static List<Hyperlink> links = new List<Hyperlink>();
        
        static CreditsState()
        {
            Font font = Resources.GameFont("SansSerifSmall");
            Color color = Color.FromArgb(0xcccccc);
            const int gap = 18;
            int x = 64;
            int y = 64;
            
            font.SetStyle(FontStyle.BoldFont | FontStyle.ItalicFont);
            
            Text.DrawText(background, "Code and graphics:", color, font, x, y);
            
            x += gap;
            font.SetStyle(FontStyle.NormalFont);
            
            y += gap;
            Text.DrawText(background, "Aiden Nibali", color, font, x, y);
            links.Add(new Hyperlink("http://dismaldenizen.wordpress.com/", "http://dismaldenizen.wordpress.com/", 300, y));
            
            y += gap;
            x -= gap;
            font.SetStyle(FontStyle.BoldFont | FontStyle.ItalicFont);
            
            y += gap;
            Text.DrawText(background, "Music:", color, font, x, y);
            
            x += gap;
            font.SetStyle(FontStyle.NormalFont);
            
            y += gap;
            Text.DrawText(background, "Arthur Cravan", color, font, x, y);
            links.Add(new Hyperlink("http://www.jamendo.com/en/artist/arthur.cravan", "http://www.jamendo.com/en/artist/arthur.cravan", 300, y));
            
            y += gap;
            Text.DrawText(background, "Raskolnikov's Dream", color, font, x, y);
            links.Add(new Hyperlink("http://raskolnikovsdream.com/", "http://raskolnikovsdream.com/", 300, y));
            
            y += gap;
            x -= gap;
            font.SetStyle(FontStyle.BoldFont | FontStyle.ItalicFont);
            
            y += gap;
            Text.DrawText(background, "Built with:", color, font, x, y);
            
            x += gap;
            font.SetStyle(FontStyle.NormalFont);
            
            y += gap;
            Text.DrawText(background, "SDL", color, font, x, y);
            links.Add(new Hyperlink("http://www.libsdl.org/", "http://www.libsdl.org/", 300, y));
            
            y += gap;
            Text.DrawText(background, "SwinGame", color, font, x, y);
            links.Add(new Hyperlink("http://swingame.com/", "http://swingame.com/", 300, y));
            
            y += gap;
            Text.DrawText(background, "Mono", color, font, x, y);
            links.Add(new Hyperlink("http://www.mono-project.com/", "http://www.mono-project.com/", 300, y));
        }
        
        public override void Update()
        {
            foreach(Hyperlink link in links)
            {
                link.Update();
            }
        }
        
        public override void Render()
        {
            Graphics.DrawBitmapOnScreen(background, 0, 0);
            
            foreach(Hyperlink link in links)
            {
                link.Render();
            }
        }
        
        public override void HandleInput()
        {
            Cursor.SetType(Cursor.Type.Normal);
            
            foreach(Hyperlink link in links)
            {
                link.HandleInput();
            }
            
            if(Input.WasKeyTyped(SwinGame.Keys.VK_ESCAPE))
            {
                Main.EnterState(new TransitionState(new MenuState()));
            }
        }
    }
}
