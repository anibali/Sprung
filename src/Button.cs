using System;
using System.Drawing;
using SwinGame;
using Graphics = SwinGame.Graphics;

namespace Sprung
{
    public abstract class Button : GameObject
    {
        private string text;
        protected bool rollover = false;
        public event EventHandler Clicked;
        
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        
        public Button(SwinGame.Bitmap image, string text) : base(image)
        {
            this.text = text;
        }
        
        protected virtual void OnClicked(EventArgs e)
        {
            if(Clicked != null)
            {
                Clicked(this, e);
            }
        }
        
        public void HandleInput()
        {
            int mx = (int)Input.GetMousePosition().X, my = (int)Input.GetMousePosition().Y;
            
            rollover = false;
            
            if(Shapes.CreateRectangle(this).Contains(mx, my))
            {
                rollover = true;
                
                if(Input.MouseWasClicked(MouseButton.LeftButton))
                {
                    OnClicked(EventArgs.Empty);
                }
            }
        }
    }
}
