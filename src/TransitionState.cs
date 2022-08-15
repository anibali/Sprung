using System;
using System.Drawing;
using System.Collections.Generic;
using SwinGame;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;

namespace Sprung
{
    public class TransitionState : GameState
    {
        private Bitmap background;
        private int framesPassed = 0;
        private int maxFrames = 15;
        private GameState oldState, newState;
        bool fadeIn = false;
        
        public TransitionState(GameState newState) : base(false)
        {
            this.oldState = Main.CurrentState();
            this.newState = newState;
            
            background = new Bitmap(Core.ScreenWidth(), Core.ScreenHeight());
            for(int x = 0; x < Core.ScreenWidth(); x++)
                for(int y = 0; y < Core.ScreenHeight(); y++)
                    Graphics.DrawPixel(background, Graphics.GetPixelFromScreen(x, y), x, y);
        }
        
        public override void Update()
        {
            framesPassed++;
            
            if(framesPassed >= maxFrames && !fadeIn)
            {
                fadeIn = true;
                newState.Render();
                background = new Bitmap(Core.ScreenWidth(), Core.ScreenHeight());
                for(int x = 0; x < Core.ScreenWidth(); x++)
                    for(int y = 0; y < Core.ScreenHeight(); y++)
                        Graphics.DrawPixel(background, Graphics.GetPixelFromScreen(x, y), x, y);
            }
            
            if(framesPassed >= maxFrames * 2)
                Main.EnterState(newState);
        }
        
        public override void Render()
        {
            Graphics.DrawBitmapOnScreen(background, 0, 0);
            int alpha = (int)Math.Round(255f * framesPassed / maxFrames);
            if(fadeIn)
            {
                alpha = 255 - (alpha - 255);
            }
            Graphics.FillRectangleOnScreen(Color.FromArgb(alpha, 0, 0, 0), 0, 0, Core.ScreenWidth(), Core.ScreenHeight());
        }
        
        public override void HandleInput()
        {
        }
    }
}
