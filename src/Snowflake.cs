using System;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class Snowflake : GameObject
    {
        public Snowflake() : base(Resources.GameImage("Snowflake"))
        {
            Depth = 70;
            
            X = 200 + (float)(400 * rand.NextDouble());
            Y = Camera.GameY(0) - (float)(1000 * rand.NextDouble());
            Movement.Y = (float)rand.NextDouble() + 1.5f;
        }
        
        public override void Update()
        {
            if(Y > Camera.GameY(Core.ScreenHeight()))
            {
                Destroy();
            }
            else
            {
                Move();
            }
        }
        
        public override void Render()
        {
            Graphics.DrawSprite(this);
        }
    }
}
