using System ;
using SFML.Graphics ;
using SFML.System ;
using SFML.Window ;
// ReSharper disable All

namespace Match_3_Game.BaseLogic
{
    public abstract class GameLoop
    {
        public static bool StopUpdate = false ;
        private const int  TargetFps  = 60 ;

        private const float TimeUntillUpdate = 1f / TargetFps ;

        public static RenderWindow Window { get ; private set ; }

        private static GameTime GameTime { get ; set ; }

        private Color WindowClearColour { get ; }

        protected GameLoop ( uint windowWidth, uint windowHight, string windowTitle, Color windowClearColour )
        {
            WindowClearColour  =  windowClearColour ;
            Window             =  new RenderWindow ( new VideoMode ( windowWidth, windowHight ), windowTitle ) ;
            GameTime           =  new GameTime () ;
            Window.Closed      += WindowClosed ;
            Window.LostFocus   += Window_LostFocus ;
            Window.GainedFocus += Window_GainedFocus ;
        }

        private static void Window_GainedFocus ( object sender, EventArgs e ) { UnchangeTimeScale(); }

        private static void Window_LostFocus ( object sender, EventArgs e )
        {
            Window.RequestFocus () ;
            StopGame();
        }

        public void Run ( )
        {
            LoadContent () ;
            Initialize () ;
            var totalTimeBeforeUpdate = 0f ;
            var previousTimeElapsed   = 0f ;
            var clock                 = new Clock () ;
            while ( Window.IsOpen )
            {
                Window.DispatchEvents () ;
                var totalTimeElapsed = clock.ElapsedTime.AsSeconds () ;
                var deltaTime        = totalTimeElapsed - previousTimeElapsed ;
                previousTimeElapsed   =  totalTimeElapsed ;
                totalTimeBeforeUpdate += deltaTime ;
                if ( StopUpdate || !(totalTimeBeforeUpdate >= TimeUntillUpdate) ) continue ;
                GameTime.Update ( totalTimeBeforeUpdate, clock.ElapsedTime.AsSeconds () ) ;
                totalTimeBeforeUpdate = 0f ;
                Update () ;
                Window.Clear ( WindowClearColour ) ;
                Draw () ;
                Window.Display () ;
            }
        }


        protected abstract void LoadContent ( ) ;
        protected abstract void Initialize ( ) ;
        protected abstract void Update ( ) ;
        protected abstract void Draw ( ) ;

        private static void WindowClosed ( object sender, EventArgs e )
        {
            Content.Content.BackMusic.Stop () ;
            Window.Close () ;
            Environment.Exit ( 0 ) ;
        }

        public static void ChangeTimeScale ( float value)
        {
            GameTime.TimeScale += value ;
        }

        public static void UnchangeTimeScale ( )
        {
            GameTime.TimeScale = 1f ;
        }

        private static void StopGame ( )
        {
            GameTime.TimeScale = 0f ;
        }
    }
}