using System;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class Player : GameObject
    {
        private const float HSPEED = 5.5f;
        private const int FRAME_LEFT = 0;
        private const int FRAME_RIGHT = 1;
        
        private bool showExhaust = false;
        private float fuel = 100;
        private static Bitmap exhaust = Resources.GameImage("PlayerExhaust");
        
        /// <summary>
        /// Gets the amount of remaining fuel (0-100).
        /// </summary>
        public float Fuel
        {
            get { return fuel; }
        }
        
        public Player() : base(Resources.GameImage("PlayerLeft"))
        {
            AddBitmap(Resources.GameImage("PlayerRight"));
            Depth = 10;
        }
        
        public void MoveLeft()
        {
            if(X > 200)
            {
                Movement.X = -HSPEED;
                CurrentFrame = FRAME_LEFT;
            }
            else
            {
                Movement.X = 0;
            }
        }
        
        public void MoveRight()
        {
            if(X < 600 - Width)
            {
                Movement.X = HSPEED;
                CurrentFrame = FRAME_RIGHT;
            }
            else
            {
                Movement.X = 0;
            }
        }
        
        public void BurnFuel()
        {
            if(fuel >= 1)
            {
                if(Movement.Y > -12) Movement.Y -= 1.5f;
                fuel -= 1;
                showExhaust = true;
            }
        }
        
        public override void Update()
        {
            if(!showExhaust)
            {
                fuel += 0.05f;
                if(fuel > 100) fuel = 100;
            }
        }
        
        public override void Render()
        {
            Graphics.DrawSprite(this);
            
            if(showExhaust)
            {
                // Draw exhaust on player to indicate burning fuel.
                Graphics.DrawBitmap(exhaust, (int)Math.Round(X), (int)Math.Round(Y));
                
                showExhaust = false;
            }
        }
    }
}
