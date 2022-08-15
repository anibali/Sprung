using System;
using System.Collections.Generic;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public abstract class GameObject : Sprite
    {
        protected Random rand = new Random();
        
        private static List<GameObject> register = new List<GameObject>();
        private int depth = 0;
        
        public event EventHandler Destroyed;
        
        public GameObject(Bitmap image) : base(image)
        {
            Init();
        }
        
        public GameObject(Bitmap image, int a, int b, int c, int d) : base(image, a, b, c, d)
        {
            Init();
        }
        
        private void Init()
        {
            register.Add(this);
            register.Sort(new DepthComparer());
        }
        
        public static void UpdateAll()
        {
            foreach(GameObject obj in register.ToArray())
            {
                obj.Update();
            }
        }
        
        public static void RenderAll()
        {
            foreach(GameObject obj in register.ToArray())
            {
                obj.Render();
            }
        }
        
        public static void DestroyAll()
        {
            foreach(GameObject obj in register.ToArray())
            {
                obj.Destroy();
            }
        }
        
        public static GameObject[] All()
        {
            return register.ToArray();
        }
        
        public abstract void Render();
        new public virtual void Update() {}
        
        protected virtual void OnDestroyed(EventArgs e)
        {
            if(Destroyed != null)
            {
                Destroyed(this, e);
            }
        }
        
        public void Destroy()
        {
            register.Remove(this);
            OnDestroyed(EventArgs.Empty);
            //Graphics.FreeSprite(this);
        }
        
        // Render depth (z-order). Objects with a higher Depth appear
        // on top of those with a lesser value for this property.
        public int Depth
        {
            get
            {
                return depth;
            }
            
            set
            {
                depth = value;
                register.Sort(new DepthComparer());
            }
        }
    }
    
    class DepthComparer : IComparer<GameObject>
    {
        public int Compare(GameObject x, GameObject y)
        {
            return(x.Depth.CompareTo(y.Depth));
        }
    }
}
