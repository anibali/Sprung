using System;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public class Platform : GameObject
    {
        public enum Type 
        {
            Normal,
            Floor,
            Hyper,
            Spiky,
        }
        
        private Type type;
        private int floor;
        
        public Platform(Type type, int floor) : base(ImageForType(type, floor))
        {
            this.type = type;
            this.floor = floor;
            
            if(type == Type.Hyper)
                this.AddBitmap(Resources.GameImage("PlatformHyperRaised"));
            
            if(floor > 900 && rand.Next() % Math.Max((1200 - floor) / 40, 2) == 0)
                Movement.X = ((float)rand.NextDouble() * 1.5f + 0.8f) * (rand.Next() % 2 == 0 ? -1 : 1);
        }
        
        public Type PlatformType()
        {
            return type;
        }
        
        public int Floor()
        {
            return floor;
        }
        
        public override void Update()
        {
            if(Movement.X != 0)
            {
                Move();
                if(X < 200 || X + Width > 600)
                {
                    Movement.X = -Movement.X;
                    Move();
                }
            }
        }
        
        public override void Render()
        {
            Graphics.DrawSprite(this);
        }
        
        private static Bitmap ImageForType(Type type, int floor)
        {
            switch(type)
            {
            case Type.Floor:
                if(floor >= 600)
                    return Resources.GameImage("PlatformFullFrozen");
                else if(floor >= 300)
                    return Resources.GameImage("PlatformFullFrosty");
                else
                    return Resources.GameImage("PlatformFull");
            case Type.Hyper:
                return Resources.GameImage("PlatformHyper");
            case Type.Spiky:
                return Resources.GameImage("PlatformSpiky");
            default:
                if(floor >= 600)
                    return Resources.GameImage("PlatformFrozen");
                else if(floor >= 300)
                    return Resources.GameImage("PlatformFrosty");
                else
                    return Resources.GameImage("Platform");
            }
        }
    }
}
