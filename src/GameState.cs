using SwinGame;
using System;

namespace Sprung
{
	public abstract class GameState
	{
        public GameState(bool init)
        {
            if(init) Init();
        }
        
        public GameState() : this(true)
        {
        }
        
        public void Init()
        {
            GameObject.DestroyAll();
            Camera.SetScreenOffset(0, 0);
        }
        
		public abstract void Update();
		public abstract void Render();
		public abstract void HandleInput();
	}
}
