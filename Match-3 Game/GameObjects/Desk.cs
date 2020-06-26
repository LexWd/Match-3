using System ;
using System.Diagnostics.CodeAnalysis ;
using System.Linq ;
using Match_3_Game.BaseLogic ;
using Match_3_Game.GameLogic ;
using SFML.Audio ;
using SFML.Graphics ;
using SFML.System ;
using SFML.Window ;

namespace Match_3_Game.GameObjects
{
    public class Desk: Transformable, Drawable
    {
        public enum WhatToDo
        {
            ChangeGem,
            Unfocus
        }

        private static uint   _countOfMakedMoves ;
        private static Sprite _backgroundForGems ;
        private static Sprite _menuSprite ;

        private static          Text      _totalTime ;
        private static          Text      _scoreText ;
        private static          Text      _lastMoves ;
        private static          Gem [ , ] DescGem { get ; set ; }
        internal static          int       Score   { get ; set ; }
        private static          bool      _gemWasDeleted ;
        public static           Gem []    FocusedGems = { null, null } ;
        public static         Clock     TimeOnDesc ;
        private static readonly Clock     PauseClock = new Clock () ;
        
        public Desk ( )
        {
            TimeOnDesc         = new Clock () ;
            _countOfMakedMoves = 30 ;
            _backgroundForGems =
            new Sprite ( Content.Content.BackgroundForGem ) { Position = new Vector2f ( 400, 120 ), Color = Color.White } ;
            _menuSprite = new Sprite ( Content.Content.ButtonMenuV2 ) { Position = new Vector2f ( 0, 500 ) } ;
            var rnd = new Random () ;
            DescGem = new Gem[ 8, 8 ] ;
            for ( var i = 0 ; i < 8 ; i++ )
            for ( var j = 0 ; j < 8 ; j++ )
                SetGem ( ( GemType ) rnd.Next ( 1, 5 ), i, j ) ;
        }

        private static void SetGem ( GemType type, int i, int j )
        {
            var gemUp    = GetGem ( i, j - 1 ) ;
            var gemDown  = GetGem ( i, j + 1 ) ;
            var gemLeft  = GetGem ( i    - 1, j ) ;
            var gemRight = GetGem ( i    + 1, j ) ;
            DescGem [ i, j ] = new Gem ( type, gemUp, gemDown, gemLeft, gemRight, i, j ) ;
        }

        private static Gem GetGem ( int i, int j )
        {
            if ( i >= 0 && j >= 0 && i < 8 && j < 8 ) return DescGem [ i, j ] ;
            return null ;
        }

        private static void TextUpdate ( )
        {
            var scoreString = Score.ToString () ;
            _scoreText = new Text ( $"Очки:\n  {scoreString}", Content.Content.GhoticFont, 50 )
                         {
                         Position = new Vector2f ( 0, 90 ), FillColor = new Color ( 255, 1, 1 )
                         } ;
            _totalTime =
            new Text ( $"Секунд осталось :\n{550 - TimeOnDesc.ElapsedTime.AsSeconds ():000}",
                       Content.   Content.GhoticFont,
                       50 ) { Position = new Vector2f ( 0, 280 ), FillColor = new Color ( 255, 1, 1 ) } ;
            _lastMoves =
            new Text ( $"Ходов осталось  :{_countOfMakedMoves}", Content.Content.GhoticFont, 50 )
            {
            Position = new Vector2f ( 400, 600 ), FillColor = new Color ( 255, 1, 1 )
            } ;
        }

        public static void Update ( )
        {
            FindMatches () ;
            DeleteMarked () ;
            while ( _gemWasDeleted )
            {
                foreach ( var i in DescGem ) i?.UpdateView () ;
                GameLoop.ChangeTimeScale(0.5f);
                GemFall () ;
            }
            GameLoop.UnchangeTimeScale();
            AddNewGem () ;
            foreach ( var i in DescGem ) i?.UpdateView () ;
            TextUpdate () ;
            if ( _countOfMakedMoves < 1 || TimeOnDesc.ElapsedTime.AsSeconds () >= 550 ) EndGame();
            if ( Game.GameScreenOn == false ) return ;
            GameLoop.Window.KeyPressed += ToMenu ;
            if ( Mouse.IsButtonPressed ( Mouse.Button.Left ) ) Click () ;
        }

        private static void Click ( )
        {
            if ( !Mouse.IsButtonPressed ( Mouse.Button.Left ) ||
                 PauseClock != null && PauseClock.ElapsedTime.AsMilliseconds () < 300 ) return ;

            GameLoop.StopUpdate = true ;
            var pos    = GameLoop.Window.MapPixelToCoords ( Mouse.GetPosition ( GameLoop.Window ) ) ;
            var vector = GetCoordinatesInMatrix ( DescGem, pos ) ;
            if ( vector.X == - 1 || vector.Y == - 1 )
            {
                GameLoop.StopUpdate = false ;
                if ( _menuSprite != null && _menuSprite.GetGlobalBounds ().Contains ( pos.X, pos.Y ) )
                    ToMenu ( GameLoop.Window, new KeyEventArgs ( new KeyEvent { Code = Keyboard.Key.Escape } ) ) ;
            }
            else
            {
                if ( FocusedGems [ 0 ] == null )
                {
                    GameLoop.StopUpdate                   = false ;
                    FocusedGems [ 0 ]                     = DescGem [ vector.X, vector.Y ] ;
                    FocusedGems [ 0 ].Rectangle.FillColor = Color.Magenta ;
                }
                else if ( FocusedGems [ 0 ] != null && FocusedGems [ 1 ] == null )
                {
                    GameLoop.StopUpdate                   = false ;
                    FocusedGems [ 1 ]                     = DescGem [ vector.X, vector.Y ] ;
                    FocusedGems [ 1 ].Rectangle.FillColor = Color.Magenta ;
                    if ( FocusedGems [ 0 ].Equals ( FocusedGems [ 1 ] ) )
                    {
                        GameLoop.StopUpdate = false ;
                        ChangeGem ( FocusedGems [ 0 ], FocusedGems [ 1 ], WhatToDo.Unfocus ) ;
                    }
                    else
                    {
                        if ( Equals ( FocusedGems [ 0 ], FocusedGems [ 1 ].GemDown ) ||
                             Equals ( FocusedGems [ 0 ], FocusedGems [ 1 ].GemUp )   ||
                             Equals ( FocusedGems [ 0 ], FocusedGems [ 1 ].GemLeft ) ||
                             Equals ( FocusedGems [ 0 ], FocusedGems [ 1 ].GemRight ) )
                        {
                            GameLoop.StopUpdate = false ;
                            ChangeGem ( FocusedGems [ 0 ], FocusedGems [ 1 ], WhatToDo.ChangeGem ) ;
                            _countOfMakedMoves-- ;
                            GameLoop.StopUpdate = false ;
                        }
                        else
                        {
                            GameLoop.StopUpdate = false ;
                            ChangeGem ( FocusedGems [ 0 ], FocusedGems [ 1 ], WhatToDo.Unfocus ) ;
                            GameLoop.StopUpdate = false ;
                        }
                    }
                }
            }

            PauseClock?.Restart () ;
        }


        private static Vector2i GetCoordinatesInMatrix ( Gem [ , ] matrix, Vector2f pos )
        {
            for ( var i = 0 ; i < 8 ; i++ )
            for ( var j = 0 ; j < 8 ; j++ )
                if ( matrix [ i, j ].Rectangle.GetGlobalBounds ().Contains ( pos.X, pos.Y ) ) return new Vector2i ( i, j ) ;

            return new Vector2i ( - 1, - 1 ) ;
        }

        public static void Draw ( RenderTarget target, RenderStates states )
        {
            Game.Background.Draw ( target, states ) ;
            _backgroundForGems.Draw ( target, states ) ;
            for ( var i = 0 ; i < 8 ; i++ )
            for ( var j = 0 ; j < 8 ; j++ )
                DescGem [ i, j ]?.Draw ( target, states ) ;
            TextUpdate () ;
            _scoreText?.Draw ( target, states ) ;
            _totalTime?.Draw ( target, states ) ;
            _lastMoves?.Draw ( target, states ) ;
            _menuSprite?.Draw ( target, states ) ;
        }

        void Drawable.Draw ( RenderTarget target, RenderStates states ) { Draw ( target, states ) ; }

        private static void ToMenu ( object sender, KeyEventArgs e )
        {
            if ( e.Code != Keyboard.Key.Escape ) return ;
            GameLoop.Window.KeyPressed -= ToMenu ;
            Content.Statistic.AddRecord(Score);
            Score = 0 ;

            Game.MenuScreenOn      = true ;
            Game.FaQScreenOn       = false ;
            Game.GameScreenOn      = false ;
            Game.StatisticScreenOn = false ;
            foreach ( var i in DescGem ) i.Dispose () ;
            _menuSprite = null ;
            Game.Desk.Destroy ( true ) ;
        }

        [SuppressMessage ( "ReSharper", "HeapView.ClosureAllocation" )]
        public static void ChangeGem ( Gem first, Gem second, WhatToDo something )
        {
            void ChooseAnimation ( Gem first1, Gem second1 )
            {
                if ( second1.X      == first1.X - 1 ) Animation.Up ( first1, second1 ) ;
                else if ( second1.X == first1.X + 1 ) Animation.Down ( first1, second1 ) ;
                else if ( second1.Y == first1.Y - 1 ) Animation.Left ( first1, second1 ) ;
                else Animation.Right ( first1, second1 ) ;
            }

            switch ( something )
            {
                case WhatToDo.ChangeGem :

                    GameLoop.StopUpdate = true ;
                    ChooseAnimation ( first, second ) ;
                    FindMatches();
                    if ( DescGem.Cast<Gem> ().Any ( gem => gem.MarkedForDelete ) == false ) ChooseAnimation ( first, second ) ;
                    ChangeGem ( first, second, WhatToDo.Unfocus ) ;
                    GameLoop.StopUpdate = false ;
                    break ;
                case WhatToDo.Unfocus :
                    if ( FocusedGems [ 0 ] != null ) FocusedGems [ 0 ].Rectangle.FillColor = Color.White ;
                    if ( FocusedGems [ 1 ] != null ) FocusedGems [ 1 ].Rectangle.FillColor = Color.White ;
                    FocusedGems [ 0 ] = null ;
                    FocusedGems [ 1 ] = null ;
                    break ;
                default :
                    throw new ArgumentOutOfRangeException ( nameof ( something ), something, null ) ;
            }
        }


        private static void GemFall ( )
        {
            GameLoop.StopUpdate = true ;


            for ( var x = 7 ; x > - 1 ; x-- )
            for ( var y = 7 ; y > - 1 ; y-- )
                if ( DescGem [ x, y ].CurrentGemType == 0 )
                {
                    var i = x ;
                    for ( ; i != 0 ; i-- )
                        if ( DescGem [ i, y ].CurrentGemType != GemType.None )
                            break ;

                    if ( DescGem [ i, y ].CurrentGemType == 0 ) continue ;
                    Animation.Down ( DescGem [ i, y ], DescGem [ x, y ] ) ;
                    DescGem [ i, y ].Rectangle.FillColor = Color.White ;
                    DescGem [ x, y ].Rectangle.FillColor = Color.White ;
                    FocusedGems                          = new Gem [] { null, null } ;
                    TextUpdate () ;
                    Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                    Draw ( GameLoop.Window, RenderStates.Default ) ;
                    GameLoop.Window.Display () ;
                }


            _gemWasDeleted      = false ;
            GameLoop.StopUpdate = false ;
        }

        private static void AddNewGem ( )
        {
            GameLoop.StopUpdate = true ;
            var    rnd    = new Random () ;
            Sprite newGem = null ;
            for ( var x = 7 ; x > - 1 ; x-- )
            for ( var y = 7 ; y > - 1 ; y-- )
                if ( DescGem [ x, y ].CurrentGemType == GemType.None )
                {
                    var newGemType = ( GemType ) rnd.Next ( 1, 5 ) ;

                    switch ( newGemType )
                    {
                        case GemType.Cross :
                            newGem = new Sprite ( Content.Content.GemCross ) ;
                            break ;
                        case GemType.Hand :
                            newGem = new Sprite ( Content.Content.GemHand ) ;
                            break ;
                        case GemType.Key :
                            newGem = new Sprite (Content. Content.GemKey ) ;
                            break ;
                        case GemType.Skull :
                            newGem = new Sprite (Content. Content.GemSkull ) ;
                            break ;
                        case GemType.None :
                            break ;
                        default :
                            throw new ArgumentOutOfRangeException () ;
                    }

                    if ( newGem != null )
                    {
                        newGem.Origin   = new Vector2f ( 30,                                    30 ) ;
                        newGem.Position = new Vector2f ( DescGem [ x, y ].Rectangle.Position.X, - 64 ) ;
                        newGem.Draw ( GameLoop.Window, RenderStates.Default ) ;
                        GameLoop.Window.Display () ;
                        while ( newGem.Position.Y <= DescGem [ x, y ].Rectangle.Position.Y )
                        {
                            newGem.Position = new Vector2f ( newGem.Position.X, newGem.Position.Y + 3.5f ) ;
                            Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                            TextUpdate () ;
                            Draw ( GameLoop.Window, RenderStates.Default ) ;
                            newGem.Draw ( GameLoop.Window, RenderStates.Default ) ;
                            GameLoop.Window.Display () ;
                        }
                    }

                    DescGem [ x, y ].CurrentGemType = newGemType ;
                    DescGem [ x, y ].UpdateView () ;
                    TextUpdate () ;
                    Draw ( GameLoop.Window, RenderStates.Default ) ;
                    GameLoop.Window.Display () ;
                }

            GameLoop.StopUpdate = false ;
        }

        private static void FindMatches ( )
        {
            for ( var x = 0 ; x < 8 ; x++ )
            for ( var y = 0 ; y < 8 ; y++ )

                if ( x - 1                           >= 0                                   &&
                     x + 1                           <= 7                                   &&
                     DescGem [ x, y ].CurrentGemType != GemType.None                        &&
                     DescGem [ x, y ].CurrentGemType == DescGem [ x + 1, y ].CurrentGemType &&
                     DescGem [ x, y ].CurrentGemType == DescGem [ x - 1, y ].CurrentGemType )
                {
                    DescGem [ x, y ].MarkedForDelete     = true ;
                    DescGem [ x + 1, y ].MarkedForDelete = true ;
                    DescGem [ x - 1, y ].MarkedForDelete = true ;
                }
                else if ( y - 1                           >= 0                                   &&
                          y + 1                           <= 7                                   &&
                          DescGem [ x, y ].CurrentGemType != GemType.None                        &&
                          DescGem [ x, y ].CurrentGemType == DescGem [ x, y - 1 ].CurrentGemType &&
                          DescGem [ x, y ].CurrentGemType == DescGem [ x, y + 1 ].CurrentGemType )
                {
                    DescGem [ x, y ].MarkedForDelete     = true ;
                    DescGem [ x, y + 1 ].MarkedForDelete = true ;
                    DescGem [ x, y - 1 ].MarkedForDelete = true ;
                }
        }


        private static void DeleteMarked ( )
        {
            var shouldDelete = DescGem.Cast<Gem> ().Any ( gem => gem.MarkedForDelete ) ;

            if ( !shouldDelete ) return ;
            {
                GameLoop.StopUpdate = true ;
                var counter    = 0 ;
                var clock      = new Clock () ;
                var breakSound = new Sound ( Content.Content.BreakBuffer ) { Volume = 10 } ;
                breakSound.Play () ;
                while ( clock.ElapsedTime.AsSeconds () <= 0.5f )
                {
                    GameLoop.Window.Clear () ;
                    foreach ( var gem in DescGem )
                        if ( gem.MarkedForDelete )
                        {
                            gem.Rectangle.FillColor = Color.Black ;
                            gem.UpdateView () ;
                        }

                    Draw ( GameLoop.Window, RenderStates.Default ) ;
                    GameLoop.Window.Display () ;
                    GameLoop.Window.Clear () ;
                    foreach ( var gem in DescGem )
                        if ( gem.MarkedForDelete )
                        {
                            gem.Rectangle.FillColor = Color.White ;
                            gem.UpdateView () ;
                        }

                    Draw ( GameLoop.Window, RenderStates.Default ) ;
                    GameLoop.Window.Display () ;
                }

                breakSound.Stop () ;

                foreach ( var gem in DescGem )
                    if ( gem.MarkedForDelete )
                    {
                        counter++ ;
                        gem.MarkedForDelete = false ;

                        gem.CurrentGemType = GemType.None ;
                        gem.UpdateView () ;
                        _gemWasDeleted = true ;
                    }


                Score += counter * 100 ;
                clock.Restart () ;
                while ( clock.ElapsedTime.AsSeconds () <= 0.5f )
                {
                    GameLoop.Window.Clear () ;
                    Draw ( GameLoop.Window, RenderStates.Default ) ;
                    GameLoop.Window.Display () ;
                }

                GameLoop.StopUpdate = false ;
            }
        }

        private static void EndGame ( )
        {
            GameLoop.StopUpdate = true ;
            var gameOverStr = $"\n Ваш счёёт: {Score} " ;
            var gameOver = new Text ( gameOverStr, Content.Content.GhoticFont, 64 )
                           {
                           Position = new Vector2f ( 320, 150 ), FillColor = new Color ( 255, 1, 1 )
                           } ;
            var backSprite = new Sprite ( Content.Content.DiedTexture ) { Position = new Vector2f ( 0, 0 ) } ;
            var gameClock  = new Clock () ;
            var death      = new Sound (Content. Content.DeathBuffer ) { Volume = 10 } ;
            Content. Content.BackMusic.Stop () ;
            death.Play () ;
            while ( gameClock.ElapsedTime.AsSeconds () < 7f )
            {
                GameLoop.Window.Clear ( Color.Black ) ;
                backSprite.Draw ( GameLoop.Window, RenderStates.Default ) ;
                gameOver.Draw ( GameLoop.Window, RenderStates.Default ) ;
                GameLoop.Window.Display () ;
            }

            death.Stop () ;
            Content.Content.BackMusic.Play () ;
            ToMenu ( GameLoop.Window, new KeyEventArgs ( new KeyEvent { Code = Keyboard.Key.Escape } ) ) ;
            GameLoop.StopUpdate = false ;
        }
    }
    
}