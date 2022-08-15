using System;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class Star : GameObject
    {
        private Platform platform;
        
        public Star(Platform platform) : base(Resources.GameImage("Star"))
        {
            this.platform = platform;
            X = platform.CenterPoint.X - Width / 2;
            Y = platform.Y - 30;
            
            Depth = 5;
        }
        
        public void Explode()
        {
            Resources.GameSound("GetStar").Play();
            
            int n = rand.Next() % 20 + 20;
            for(int i = 0; i < n; i++)
            {
                StarParticle particle = new StarParticle();
                particle.Movement.X = (float)(rand.NextDouble() * 16 - 8);
                particle.Movement.Y = (float)-(rand.NextDouble() * 6 + 3);
                particle.X = X;
                particle.Y = Y;
            }
            
            Destroy();
        }
        
        public override void Update()
        {
            X = platform.CenterPoint.X - Width / 2;
            Y = platform.Y - 30;
            
            if(Y > Camera.GameY(Core.ScreenHeight()))
            {
                Destroy();
            }
        }
        
        public override void Render()
        {
            Graphics.DrawSprite(this);
        }
    }
}
