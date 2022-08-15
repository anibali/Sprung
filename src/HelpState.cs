using System;
using System.Drawing;
using System.Collections.Generic;
using SwinGame;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;

namespace Sprung
{
    public class HelpState : GameState
    {
        private static Bitmap background = Resources.GameImage("HelpBackground");
        
        public override void Update()
        {
        }
        
        public override void Render()
        {
            Graphics.DrawBitmapOnScreen(background, 0, 0);
        }
        
        public override void HandleInput()
        {
            if(Input.WasKeyTyped(SwinGame.Keys.VK_ESCAPE))
            {
                Main.EnterState(new TransitionState(new MenuState()));
            }
        }
    }
}
