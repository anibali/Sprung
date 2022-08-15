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
	public class InGameState : GameState
	{
		private Player player;
        private static Bitmap background = Resources.GameImage("Background");
		private Queue<Platform> platforms = new Queue<Platform>(10);
		private float risingSpeed;
		private int topFloor, bestFloor, score;
		private Random rand = new Random();
        private long startTime = DateTime.Now.ToFileTime();
        private long pauseStartTime;
        private long timeOfDeath = 0;
        private bool leftKeyFirst;
		
		public InGameState()
		{
            Cursor.SetType(Cursor.Type.None);
            
			player = new Player();
			
			player.X = (Core.ScreenWidth() - player.Width) / 2;
			player.Y = Core.ScreenHeight() - (player.Height + 32);
			
			for(int i = 0; i < 10; i++)
			{
				NextPlatform();
			}
			
			risingSpeed = 0;

            if(!Main.DEBUG)
            if(!Audio.IsMusicPlaying())
                Resources.GameMusic("InGame").Play(-1);
		}
		
		public override void Update()
		{
            GameObject.UpdateAll();
            
            const int FIRST_SNOW_FLOOR = 300;
            if(topFloor > FIRST_SNOW_FLOOR && rand.NextDouble() / Math.Min((topFloor - FIRST_SNOW_FLOOR) * 20, 10000) < 0.0001)
            {
                new Snowflake();
            }
            
            if(timeOfDeath > 0)
            {
                if(player.Y > Camera.GameY(Core.ScreenHeight()))
                {
                    Main.EnterState(new TransitionState(new GameOverState(bestFloor, score, DateTime.Now.ToFileTime() - startTime)));
                }
                else
                {
                    if(player.Movement.Y < 15)
                        player.Movement.Y += 0.675f;
                    player.Move();
                }
                return;
            }
            
            foreach(GameObject obj in GameObject.All())
            {
                if(obj is Fan)
                {
                    const float FAN_POWER = 4;
                    Fan fan = (Fan)obj;
                    if(fan.Blows(player))
                    {
                        if(player.X > 200 + FAN_POWER) player.X -= FAN_POWER;
                    }
                    foreach(GameObject obj2 in GameObject.All())
                    {
                        if(obj2 is Snowflake && fan.Blows(obj2))
                            if(obj2.X > 200 + FAN_POWER) obj2.X -= FAN_POWER;
                    }
                }
                
                if(obj is Star && player.HasCollidedWith(obj))
                {
                    Star star = (Star)obj;
                    star.Explode();
                    score += 200;
                }
                
                if(obj is Platform)
                {
                    Platform platform = (Platform)obj;
                    Rectangle platformRect = Shapes.CreateRectangle(platform);
        
                    if (player.Movement.Y > 0 && 
                        (platformRect.Contains((int)Math.Round(player.X), (int)Math.Round(player.Y + player.Height)) ||
                         platformRect.Contains((int)Math.Round(player.X + player.Width), (int)Math.Round(player.Y + player.Height))))
                    {
                        player.Movement.Y = -16.5f;
                        
                        if(platform.Floor() > bestFloor)
                        {
                            int dfloor = platform.Floor() - bestFloor;
                            score += 20 * dfloor + (int)Math.Pow(dfloor, 2);
                            bestFloor = platform.Floor();
                        }
                        
                        if(platform.PlatformType() == Platform.Type.Hyper && platform.CurrentFrame == 0)
                        {
                            Resources.GameSound("Boing").Play();
                            platform.CurrentFrame = 1;
                            platform.Y -= 7;
                            player.Movement.Y = -30;
                        }
                        else if(platform.PlatformType() == Platform.Type.Spiky)
                        {
                            Die();
                        }
                    }
                }
            }

			while(!(platforms.Count == 0) && platforms.Peek().Y > Camera.GameY(Core.ScreenHeight()))
			{
				platforms.Dequeue().Destroy();

                NextPlatform();
			}
			
            if(player.Movement.Y < 15)
			    player.Movement.Y += 0.675f;
			player.Move();
			
			if(DateTime.Now.ToFileTime() - startTime > 20000000)
			{
                risingSpeed = -3 - ((float)topFloor / 330);
                if(risingSpeed < -6) risingSpeed = -6;
			}
			
			Camera.MoveVisualArea(0, risingSpeed);
			
			if(player.Y < Camera.GameY(100))
			{
				Camera.SetScreenOffset(0, player.Y - 100);
			}
			
			if(player.Y + player.Height > Camera.GameY(Core.ScreenHeight()))
			{
                Die();
			}
		}
        
        private void Die()
        {
            timeOfDeath = DateTime.Now.ToFileTime();
            player.Movement.Y = -5;
            player.Movement.X = 0;
        }
        
        private Platform NextPlatform()
        {
            Platform platform;

            if(topFloor % 100 == 0)
            {
                platform = new Platform(Platform.Type.Floor, topFloor);
                platform.X = 200;
            }
            else
            {
                if(rand.Next() % 20 == 0)
                {
                    platform = new Platform(Platform.Type.Hyper, topFloor);
                }
                else if(topFloor % 3 == 0 && rand.Next() % 10 == 0)
                {
                    platform = new Platform(Platform.Type.Spiky, topFloor);
                }
                else
                {
                    platform = new Platform(Platform.Type.Normal, topFloor);
                }
                
                platform.X = rand.Next() % (400 - platform.Width) + 200;
            }
            
            platform.Y = Core.ScreenHeight() - platform.Height - 64 * topFloor;
                    
            topFloor += 1;
            
            platforms.Enqueue(platform);
            
            if(rand.Next() % 5 == 0)
            {
                new Star(platform);
            }
            else if(topFloor > 500 && platform.PlatformType() == Platform.Type.Normal && platform.X > 300 && rand.Next() % 10 == 0)
            {
                new Fan(platform);
            }
            
            return(platform);
        }
		
		public override void Render()
		{
			Graphics.DrawBitmapOnScreen(background, 0, 0);
			
			GameObject.RenderAll();
            
            float percent = player.Fuel / 100;
            Color color = Color.FromArgb((int)((1 - percent) * 200), (int)(percent * 200), 32);
            Graphics.FillRectangleOnScreen(color, 20, 327, (int)Math.Round(percent * 159), 9);
            
            Font font = Resources.GameFont("SansSerifLarge");
            
            string txtTime = new DateTime(DateTime.Now.ToFileTime() - startTime).ToString("mm:ss");
            Helper.DrawCenteredTextOnScreen(txtTime, Color.Black, font, 700, 212);
            Helper.DrawCenteredTextOnScreen(bestFloor.ToString(), Color.Black, font, 89, 122);
            Helper.DrawCenteredTextOnScreen(score.ToString(), Color.Black, font, 700, 94);
		}
		
		public override void HandleInput()
		{
            if(Main.IsPaused())
            {
                // Resume game with 'P'.
                if(Input.WasKeyTyped(SwinGame.Keys.VK_P))
                {
                    Main.Resume();
                    startTime += DateTime.Now.ToFileTime() - pauseStartTime;
                }
            }
            else
            {
                // Disable controls if player has died.
                if(timeOfDeath > 0) return;
                
                // Horizontal movement with arrow keys
                if(Input.IsKeyPressed(SwinGame.Keys.VK_LEFT) && !Input.IsKeyPressed(SwinGame.Keys.VK_RIGHT))
                    leftKeyFirst = true;
                else if(Input.IsKeyPressed(SwinGame.Keys.VK_RIGHT) && !Input.IsKeyPressed(SwinGame.Keys.VK_LEFT))
                    leftKeyFirst = false;
                
                bool left = Input.IsKeyPressed(SwinGame.Keys.VK_LEFT);
                bool right = Input.IsKeyPressed(SwinGame.Keys.VK_RIGHT);
                
                if(left && right)
                {
                    left = !leftKeyFirst;
                    right = leftKeyFirst;
                }
                
                if(left)
                    player.MoveLeft();
                else if(right)
                    player.MoveRight();
                else
                    player.Movement.X = 0;
                
                // Burn fuel with spacebar.
                if(Input.IsKeyPressed(SwinGame.Keys.VK_SPACE))
                {
                    player.BurnFuel();
                }
                
                // Pause game with 'P'.
                if(Input.WasKeyTyped(SwinGame.Keys.VK_P))
                {
                    Helper.DrawCenteredTextOnScreen("Paused", Color.Gray, Resources.GameFont("SansSerifLarge"), Core.ScreenWidth() / 2, Core.ScreenHeight() / 2);
                    Core.RefreshScreen();
                    Main.Pause();
                    pauseStartTime = DateTime.Now.ToFileTime();
                }
                
                
                // Quit to menu with escape.
                if(Input.WasKeyTyped(SwinGame.Keys.VK_ESCAPE))
                {
                    Audio.StopMusic();
                    Main.EnterState(new TransitionState(new MenuState()));
                }
            }
		}
	}
}
