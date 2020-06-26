using Match_3_Game.BaseLogic ;
using Match_3_Game.GameObjects ;
using SFML.Graphics ;
using SFML.System ;
using SFML.Window ;
using static Match_3_Game.Content.Content ;

namespace Match_3_Game.GameLogic
{
    internal class Game : GameLoop
    {
        private const  uint     DefaultWindowWidth1 = 1280 ;
        private const  uint     DefaultWindowHight1 = 720 ;
        private static uint     DefaultWindowWidth2 => DefaultWindowWidth1 ;
        private static uint     DefaultWindowHight2 => DefaultWindowHight1 ;
        private static Sprite   _cursor ;

        public static RectangleShape Background ;

        public static Desk Desk ;
        private       Menu _menu ;
        public static bool MenuScreenOn ;
        public static bool FaQScreenOn ;
        public static bool GameScreenOn ;
        public static bool StatisticScreenOn ;

        public Game ( ) : base ( DefaultWindowWidth1, DefaultWindowHight1, "Match-3", Color.Black )
        {
            Window.SetMouseCursorVisible ( false ) ;
            Window.SetKeyRepeatEnabled ( false ) ;
        }


        protected override void LoadContent ( )
        {
            Content.Content.LoadContent () ;
            _cursor = new Sprite ( CursorTexture )
                      {
                      Position = new Vector2f ( Mouse.GetPosition ( Window ).X, Mouse.GetPosition ( Window ).Y )
                      } ;
            Background = new RectangleShape ( new Vector2f ( DefaultWindowWidth2, DefaultWindowHight2 ) )
                         {
                         Position = new Vector2f ( 0, 0 ), Texture = BackgroundAll
                         } ;
        }

        protected override void Initialize ( )
        {
            Window.SetIcon ( 60, 60, Icon.Pixels ) ;
            Content.Statistic.StatisticData = Content.Statistic.Read ();
            Content.Statistic.StatisticData.AllowEdit = true ;
            Content.Statistic.StatisticData.AllowNew = true ;
            Content.Statistic.StatisticData.AllowRemove = true ;
            MenuScreenOn = true ;
            Desk         = new Desk () ;
            _menu        = new Menu () ;
            BackMusic.Play () ;
        }

        protected override void Update ( )
        {
            if ( !Window.HasFocus () ) return ;
            if ( MenuScreenOn ) _menu.MenuScreenUpdate () ;
            else if ( FaQScreenOn ) _menu.FaQScreenUpdate () ;
            else if ( StatisticScreenOn ) _menu.StatisticScreenUpdate () ;
            else if ( GameScreenOn ) Desk.Update () ;
            _cursor.Position = GetMousePosition () ;
        }

        protected override void Draw ( )
        {
            Background.Draw ( Window, RenderStates.Default ) ;

            if ( MenuScreenOn ) _menu.Draw ( Window, RenderStates.Default ) ;
            else if ( FaQScreenOn ) _menu.FaQScreenDraw () ;
            else if ( StatisticScreenOn ) _menu.StatisticScreenDraw () ;
            else if ( GameScreenOn ) Desk.Draw ( Window, RenderStates.Default ) ;
            _cursor.Draw ( Window, RenderStates.Default ) ;
        }

        private static Vector2f GetMousePosition ( )
        {
            var posF = Window.MapPixelToCoords ( Mouse.GetPosition ( Window ) ) ;
            return posF ;
        }

        public static void RestartDesk ( )
        {
            Desk       = new Desk () ;
            Desk.Score = 0 ;
            Desk.TimeOnDesc.Restart () ;
        }
    }
}