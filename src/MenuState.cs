using System;
using System.Drawing;
using System.Collections.Generic;
using SwinGame;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;

namespace Sprung
{
    public class MenuState : GameState
    {
        private List<Button> buttons = new List<Button>();
        
        public MenuState()
        {
            MenuButton btnPlay = new MenuButton("Play");
            btnPlay.MoveTo((Core.ScreenWidth() - btnPlay.Width) / 2, 200);
            btnPlay.Clicked += new EventHandler(OnPlayButtonClicked);
            buttons.Add(btnPlay);
            
            MenuButton btnHelp = new MenuButton("Help");
            btnHelp.MoveTo((int)btnPlay.X, (int)btnPlay.Y + 60);
            btnHelp.Clicked += new EventHandler(OnHelpButtonClicked);
            buttons.Add(btnHelp);
            
            MenuButton btnScores = new MenuButton("Scores");
            btnScores.MoveTo((int)btnPlay.X, (int)btnHelp.Y + 60);
            btnScores.Clicked += new EventHandler(OnScoresButtonClicked);
            buttons.Add(btnScores);
            
            MenuButton btnCredits = new MenuButton("Credits");
            btnCredits.MoveTo((int)btnPlay.X, (int)btnScores.Y + 60);
            btnCredits.Clicked += new EventHandler(OnCreditsButtonClicked);
            buttons.Add(btnCredits);
            
            MenuButton btnQuit = new MenuButton("Quit");
            btnQuit.MoveTo((int)btnPlay.X, Core.ScreenHeight() - btnQuit.Height - 80);
            btnQuit.Clicked += new EventHandler(OnQuitButtonClicked);
            buttons.Add(btnQuit);
            
            if(!Audio.IsMusicPlaying())
                Resources.GameMusic("Menu").Play(-1);
        }
        
        public override void Update()
        {
        }
        
        public override void Render()
        {
            Graphics.DrawBitmapOnScreen(Resources.GameImage("MenuBackground"), 0, 0);
            
            GameObject.RenderAll();
        }
        
        public override void HandleInput()
        {
            Cursor.SetType(Cursor.Type.Normal);
            
            foreach(Button button in buttons)
            {
                button.HandleInput();
            }
            
            if(Input.WasKeyTyped(SwinGame.Keys.VK_ESCAPE))
            {
                Main.Quit();
            }
        }
        
        public void OnPlayButtonClicked(object button, EventArgs e)
        {
            Audio.StopMusic();
            Main.EnterState(new TransitionState(new InGameState()));
        }
        
        public void OnHelpButtonClicked(object button, EventArgs e)
        {
            Main.EnterState(new TransitionState(new HelpState()));
        }
        
        public void OnScoresButtonClicked(object button, EventArgs e)
        {
            Main.EnterState(new TransitionState(new ScoreboardState()));
        }
        
        public void OnCreditsButtonClicked(object button, EventArgs e)
        {
            Main.EnterState(new TransitionState(new CreditsState()));
        }
        
        public void OnQuitButtonClicked(object button, EventArgs e)
        {
            Main.Quit();
        }
    }
}
