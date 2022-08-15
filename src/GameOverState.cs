using System;
using System.Drawing;
using System.Collections.Generic;

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
	public class GameOverState : GameState
	{
        private int bestFloor, score, placing;
        private long time;
		
		public GameOverState(int bestFloor, int score, long time)
		{
            this.bestFloor = bestFloor;
            this.score = score;
            this.time = time;
            placing = Hiscores.ProcessScore(score, bestFloor);
		}
		
		public override void Update()
		{
		}
 
		public override void Render()
		{
			Graphics.DrawBitmapOnScreen(Resources.GameImage("GameOverBackground"), 0, 0);

            Font font = Resources.GameFont("SansSerifLarge");
            Font mediumFont = Resources.GameFont("SansSerifMedium");
            
            if(score < 1000)
                Helper.DrawCenteredTextOnScreen("Bad luck!", Color.Red, font, Core.ScreenWidth() / 2, 220);
            else if(score < 10000)
                Helper.DrawCenteredTextOnScreen("Good!", Color.Orange, font, Core.ScreenWidth() / 2, 220);
            else if(score < 100000)
                Helper.DrawCenteredTextOnScreen("Great!", Color.Yellow, font, Core.ScreenWidth() / 2, 220);
            else
                Helper.DrawCenteredTextOnScreen("Amazing!", Color.Green, font, Core.ScreenWidth() / 2, 220);
            
            Helper.DrawCenteredTextOnScreen("You reached the " + Helper.Ordinal(bestFloor) + " floor", Color.White, mediumFont, Core.ScreenWidth() / 2, 260);
            Helper.DrawCenteredTextOnScreen("with a total score of", Color.White, mediumFont, Core.ScreenWidth() / 2, 290);
            Helper.DrawCenteredTextOnScreen(score.ToString(), Color.Cyan, font, Core.ScreenWidth() / 2, 340);
            
            if(placing == 1)
                Helper.DrawCenteredTextOnScreen("New top score!", Color.Magenta, mediumFont, Core.ScreenWidth() / 2, 400);
            else if(placing > 1)
                Helper.DrawCenteredTextOnScreen("New " + Helper.Ordinal(placing) + " best score!", Color.Magenta, mediumFont, Core.ScreenWidth() / 2, 400);
		}
		
		public override void HandleInput()
		{
            Cursor.SetType(Cursor.Type.Normal);
            
            if(Input.WasKeyTyped(SwinGame.Keys.VK_SPACE))
            {
                Main.EnterState(new TransitionState(new InGameState()));
            }
            
            if(Input.WasKeyTyped(SwinGame.Keys.VK_ESCAPE))
            {
                Audio.StopMusic();
                Main.EnterState(new TransitionState(new MenuState()));
            }
		}
	}
}
