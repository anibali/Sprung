using System;
using System.Drawing;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class Fan : GameObject
    {
        private Platform platform;
        
        public Fan(Platform platform) : base(Resources.GameImage("Fan"), 1, 6, 32, 62)
        {
            this.platform = platform;
            
            X = platform.CenterPoint.X - Width / 2;
            Y = platform.Y - Height;
            
            EndingAction = SpriteEndingAction.ReverseLoop;
            
            Depth = 5;
        }
        
        public bool Blows(Sprite sprite)
        {
            return sprite.HasCollidedWithRect(new Rectangle(0, (int)Y, (int)X, (int)Height));
        }
        
        public override void Update()
        {
            UpdateAnimation();
            
            X = platform.CenterPoint.X - Width / 2;
            Y = platform.Y - Height;
            
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
