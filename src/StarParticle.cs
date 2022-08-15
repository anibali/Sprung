using System;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class StarParticle : GameObject
    {
        public StarParticle() : base(Resources.GameImage("StarParticle"))
        {
            Depth = 50;
        }
        
        public override void Update()
        {
            Movement.Y += 0.6f;
            Move();
            
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
