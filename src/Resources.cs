using System;
using System.Drawing;
using System.IO;
using SwinGame;
using System.Collections.Generic;

using Graphics = SwinGame.Graphics;
using Bitmap = SwinGame.Bitmap;
using Font = SwinGame.Font;
using FontStyle = SwinGame.FontStyle;

namespace Sprung
{
    /// <summary>
    /// The Resources Class stores all of the Games Media Resources, such as Images, Fonts
    /// Sounds, Music, and Maps.
    /// </summary>
    public static class Resources
    {
        private static Dictionary<string, Bitmap> _Images = new Dictionary<string, Bitmap>();
        private static Dictionary<string, Font> _Fonts = new Dictionary<string, Font>();
        private static Dictionary<string, SoundEffect> _Sounds = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Music> _Music = new Dictionary<string, Music>();
        private static Dictionary<string, Map> _Maps = new Dictionary<string, Map>();
        
        private static Bitmap _Background;
        private static Bitmap _LoaderFull;
        private static Bitmap _LoaderEmpty;
        private static Font _LoadingFont;
        
        private static void LoadFonts()
        {
            NewFont("SansSerifSmall", "DejaVuSans.ttf", 16);
            NewFont("SansSerifMedium", "DejaVuSans.ttf", 28);
            NewFont("SansSerifLarge", "DejaVuSans.ttf", 44);
            NewFont("MenuButton", "DejaVuSans.ttf", 32);
        }

        private static void LoadImages()
        {
            NewImage("CursorNormal", "cursor_normal.png");
            NewImage("CursorHand", "cursor_hand.png");
			NewImage("PlayerLeft", "player_left.png");
			NewImage("PlayerRight", "player_right.png");
            NewImage("PlayerExhaust", "exhaust.png");
			NewImage("Platform", "platform.png");
            NewImage("PlatformFrosty", "platform_frosty.png");
            NewImage("PlatformFrozen", "platform_frozen.png");
            NewImage("PlatformFull", "platform_full.png");
            NewImage("PlatformFullFrosty", "platform_full_frosty.png");
            NewImage("PlatformFullFrozen", "platform_full_frozen.png");
            NewImage("PlatformHyper", "platform_hyper.png");
            NewImage("PlatformHyperRaised", "platform_hyper_raised.png");
            NewImage("PlatformSpiky", "platform_spiky.png");
			NewImage("Background", "background.png");
            NewImage("GameOverBackground", "game_over_background.png");
            NewImage("MenuBackground", "menu_background.png");
            NewImage("MenuButton", "menu_button.png");
            NewImage("HelpBackground", "help_background.png");
            NewImage("ScoreboardBackground", "scoreboard_background.png");
            NewImage("CreditsBackground", "credits_background.png");
            NewImage("Star", "star.png");
            NewImage("Fan", "fan.png");
            NewImage("StarParticle", "star_particle.png");
            NewImage("Snowflake", "snowflake.png");
        }

        private static void LoadSounds()
        {
            NewSound("GetStar", "star_chime.ogg");
            NewSound("Boing", "boing.ogg");
        }

        private static void LoadMusic()
        {
            NewMusic("InGame", "watership_down.ogg");
            NewMusic("Menu", "colorando_noise_sound_nation.ogg");
        }

        private static void LoadMaps()
        {
        }

        /// <summary>
        /// Loads Resources
        /// </summary>
        public static void LoadResources()
        {
            uint minSplashTime = 1500;
            long startTime = DateTime.Now.ToFileTime();
            
            int width = Core.ScreenWidth();
            int height = Core.ScreenHeight();

            Core.ChangeScreenSize(800, 600);

            ShowLoadingScreen();

            ShowMessage("Loading fonts...", 0);
            LoadFonts();

            ShowMessage("Loading images...", 1);
            LoadImages();

            ShowMessage("Loading sounds...", 2);
            LoadSounds();

            ShowMessage("Loading music...", 3);
            LoadMusic();

            ShowMessage("Loading maps...", 4);
            LoadMaps();
            
            ShowMessage("Game loaded.", 5);
            
            uint deltaTime = (uint)(DateTime.Now.ToFileTime() - startTime) / 10000;

            if(deltaTime < minSplashTime)
            {
                Core.Sleep(minSplashTime - deltaTime);
            }

            EndLoadingScreen(width, height);
        }

        private static void ShowLoadingScreen()
        {
            // Hide default cursor
            Input.HideMouse();
            
            _Background = Graphics.LoadBitmap(GetPathToResource("SplashBack.png", ResourceKind.ImageResource));
            Graphics.DrawBitmap(_Background, 0, 0);
            Core.RefreshScreen();
            Core.ProcessEvents();

            _LoadingFont = Text.LoadFont(GetPathToResource("DejaVuSans.ttf", ResourceKind.FontResource), 12);
			_LoaderFull = Graphics.LoadBitmap(GetPathToResource("loader_full.png", ResourceKind.ImageResource));
			_LoaderEmpty = Graphics.LoadBitmap(GetPathToResource("loader_empty.png", ResourceKind.ImageResource));
        }

        private static void ShowMessage(String message, int number)
        {
			const int STEPS = 5;
            int BG_X = (Core.ScreenWidth() - _LoaderEmpty.Width) / 2, BG_Y = 450;

			int fullW = 260 * number / STEPS;
			Graphics.DrawBitmap(_LoaderEmpty, BG_X, BG_Y);
			Graphics.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y);

            Helper.DrawCenteredTextOnScreen(message, Color.White, _LoadingFont, Core.ScreenWidth() / 2, 500);
            Core.RefreshScreen();
            Core.ProcessEvents();
        }

        private static void EndLoadingScreen(int width, int height)
        {
			Core.ProcessEvents();
            Graphics.ClearScreen();
            Core.RefreshScreen();
            Text.FreeFont(_LoadingFont);
            Graphics.FreeBitmap(_Background);
	        Graphics.FreeBitmap(_LoaderEmpty);
	        Graphics.FreeBitmap(_LoaderFull);

            Core.ChangeScreenSize(width, height);
        }
        
        public static string GetPathToResource(String filename, ResourceKind kind)
        {
            string path;
            string asmLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            if(asmLoc == "")
            {
                path = Environment.CurrentDirectory;
            }
            else
            {
                path = Path.GetDirectoryName(asmLoc);
            }
            
            switch(kind)
            {
            case ResourceKind.FontResource:
                path = Path.Combine(path, "fonts");
                break;
            case ResourceKind.ImageResource:
                path = Path.Combine(path, "images");
                break;
            case ResourceKind.MapResource:
                path = Path.Combine(path, "maps");
                break;
            case ResourceKind.SoundResource:
                path = Path.Combine(path, "sounds");
                break;
            }
            
            path = Path.Combine(path, filename);
            
            return(path);
        }

        private static void NewMap(String mapName)
        {
			_Maps.Add(mapName, MappyLoader.LoadMap(mapName));
        }

        private static void NewFont(String fontName, String filename, int size)
        {
			_Fonts.Add(fontName, Text.LoadFont(GetPathToResource(filename, ResourceKind.FontResource), size));
        }

        private static void NewImage(String imageName, String filename)
        {
			_Images.Add(imageName, Graphics.LoadBitmap(GetPathToResource(filename, ResourceKind.ImageResource)));
        }

		private static void NewTransparentColorImage(String imageName, String fileName, Color transColor)
        {
            _Images.Add(imageName, Graphics.LoadBitmap(GetPathToResource(fileName, ResourceKind.ImageResource), true, transColor));
        }

        private static void NewSound(String soundName, String filename)
        {
			_Sounds.Add(soundName, Audio.LoadSoundEffect(GetPathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void NewMusic(String musicName, String filename)
        {
			_Music.Add(musicName, Audio.LoadMusic(GetPathToResource(filename, ResourceKind.SoundResource)));
        }

        private static void FreeFonts()
        {
            foreach(Font f in _Fonts.Values)
            {
                Text.FreeFont(f);
            }
			_Fonts.Clear();
        }

        private static void FreeImages()
        {
            foreach(Bitmap b in _Images.Values)
            {
                Graphics.FreeBitmap(b);
                //_ImagesStr[i] = String.Empty;
            }
			_Images.Clear();
        }

        private static void FreeSounds()
        {
            foreach(SoundEffect ef in _Sounds.Values)
            {
                Audio.FreeSoundEffect(ef);
            }
			_Sounds.Clear();
        }

        private static void FreeMusic()
        {
            foreach (Music m in _Music.Values)
            {
                Audio.FreeMusic(m);
            }
			_Music.Clear();
        }

        private static void FreeMaps()
        {
            foreach (Map m in _Maps.Values)
            {
                MappyLoader.FreeMap(m);
            }
			_Maps.Clear();
        }

        /// <summary>
        /// Frees All Resources
        /// </summary>
        public static void FreeResources()
        {
            FreeFonts();
            FreeImages();
            FreeMusic();
            FreeSounds();
            FreeMaps();
            Core.ProcessEvents();
        }

        /// <summary>
        /// Gets the Desired Font from the Loaded Resources
        /// </summary>
        /// <param name="font">Font to Get</param>
        /// <returns>The Font</returns>
        public static Font GameFont(String font)
        {
            return _Fonts[font];
        }

        /// <summary>
        /// Gets the Desired Image from the Loaded Resources
        /// </summary>
        /// <param name="image">Image to Get</param>
        /// <returns>The Image</returns>
        public static Bitmap GameImage(String image)
        {
			return _Images[image];
        }

        /// <summary>
        /// Gets the Desired Sound from the Loaded Resources
        /// </summary>
        /// <param name="sound">Sound to Get</param>
        /// <returns>The Sound</returns>
        public static SoundEffect GameSound(String sound)
        {
            return _Sounds[sound];
        }

        /// <summary>
        /// Gets the Desired Music from the Loaded Resources
        /// </summary>
        /// <param name="music">Music to get</param>
        /// <returns>The Music</returns>
        public static Music GameMusic(String music)
        {
            return _Music[music];
        }

        /// <summary>
        /// Gets the Desired Map from the Loaded Resources
        /// </summary>
        /// <param name="map">Map to get</param>
        /// <returns>The map</returns>
        public static Map GameMap(String map)
        {
            return _Maps[map];
        }
    }
}