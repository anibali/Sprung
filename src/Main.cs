using System;
using System.IO;
using System.Drawing;

using SwinGame;
using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;
using Font = SwinGame.Font;
using FontStyle = SwinGame.FontStyle;
using Event = SwinGame.Event;
using CollisionSide = SwinGame.CollisionSide;
using Sprite = SwinGame.Sprite;

namespace Sprung
{
    public static class Main
    {
        public const bool DEBUG = true;
        
		private static GameState state;
		private static bool running = true, paused = false;
		
        public static void RunGame()
        {
			// Initialise the game
			Init();
            
            // Main game Loop
            do
            {
                if(paused)
                {
                    Core.Sleep(10);
                }
                else
                {
    				// Update game logic
    				state.Update();
    				
    				// Render the display
    				state.Render();
                    if(DEBUG)
                    {
                        Graphics.FillRectangleOnScreen(Color.FromArgb(128, 0, 0, 0), 0, 0, 180, 10);
                        Text.DrawFramerate(0, 0);
                    }
                    Cursor.Render();
    				Core.RefreshScreen(50);
                }
                
                // Handle input events
                Core.ProcessEvents();
                state.HandleInput();
                if(Core.WindowCloseRequested()) Main.Quit();
            } while(running);

			// Finish and clean up
            Finish();
        }
		
		public static void Init()
		{
            string configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "sprung");
            if(!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);
            
            Core.SetIcon(Resources.GetPathToResource("icon.png", ResourceKind.ImageResource));
            
			// Open a window
    		Core.OpenGraphicsWindow("Sprung", 800, 600);
            
            // Open audio device
            Audio.OpenAudio();
			
            // Load resources (images, sounds, etc)
            Resources.LoadResources();
			
			// Enter first game state
			EnterState(new MenuState());
		}
		
		public static void Quit()
		{
            Graphics.ClearScreen();
            Helper.DrawCenteredTextOnScreen("Goodbye!", Color.White, Resources.GameFont("SansSerifLarge"), Core.ScreenWidth() / 2, Core.ScreenHeight() / 2);
            Core.RefreshScreen();
			running = false;
		}
		
		private static void Finish()
		{
            Resources.FreeResources();
            Audio.CloseAudio();
		}
        
        /// <summary>
        /// Determines whether the game is paused.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public static bool IsPaused()
        {
            return paused;
        }
        
        /// <summary>
        /// Pause the game. The current GameState will not
        /// be updated while the game is paused.
        /// </summary>
        public static void Pause()
        {
            paused = true;
        }
        
        /// <summary>
        /// Resume paused game.
        /// </summary>
        public static void Resume()
        {
            paused = false;
        }
		
        /// <summary>
        /// Set the active game state.
        /// </summary>
        /// <param name="state">
        /// A <see cref="GameState"/>
        /// </param>
		public static void EnterState(GameState state)
		{
			Main.state = state;
		}
        
        public static GameState CurrentState()
        {
            return Main.state;
        }
    }
}
