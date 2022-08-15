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
    public class ScoreboardState : GameState
    {
        private static Bitmap background = Resources.GameImage("ScoreboardBackground");
        private List<Hiscores.Score> scores;
        
        public ScoreboardState()
        {
            scores = Hiscores.Load();
        }
        
        public override void Update()
        {
        }
        
        public override void Render()
        {
            Graphics.DrawBitmapOnScreen(background, 0, 0);
            
            int y = 164;
            Font font = Resources.GameFont("SansSerifSmall");
            
            foreach(Hiscores.Score score in scores)
            {
                Color color = Color.White;
                font.SetStyle(FontStyle.BoldFont);
                Helper.DrawCenteredTextOnScreen(score.Number.ToString(), color, font, 300, y);
                font.SetStyle(FontStyle.NormalFont);
                Helper.DrawCenteredTextOnScreen(Helper.Ordinal(score.Floor) + " floor on " + score.Time.ToShortDateString(), color, font, 470, y);
                y += 32;
            }
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
