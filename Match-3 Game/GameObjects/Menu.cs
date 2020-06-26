using System ;
using Match_3_Game.BaseLogic ;
using Match_3_Game.Content ;
using Match_3_Game.GameLogic ;
using SFML.Graphics ;
using SFML.System ;
using SFML.Window ;

namespace Match_3_Game.GameObjects
{
    public class Menu : Transformable, Drawable
    {
        
        private Sprite ButtonExit    { get ; }
        private Sprite ButtonFaq     { get ; }
        private Sprite ButtonNewGame { get ; }
        private Sprite ButtonStatistic { get ; }

        private readonly Sprite _backSprite1 = new Sprite ( Content.Content.BackgroundForStats1 ) { Position = new Vector2f ( 0, 0 ) } ;

        private readonly Sprite _backSprite2 = new Sprite ( Content.Content.BackgroundForStats2 ) { Position = new Vector2f ( 0, 0 ) } ;

        private readonly Sprite _backSprite3 = new Sprite ( Content.Content.BackFaq ) { Position = new Vector2f ( 0, 0 ) } ;

        private bool _texture = true ;

        private const string Str =
        "Данная компьютерная игра\r\nбыла разработанна в ходе\r\nвыполнения моей курсовой работы\r\n\r\nОптимизация отсутствует\r\nЗато количество багов минимально\r\n\r\nВ меню можно выйти с помощью\r\nклавиши ESC или воспользовавшись\r\nодной из кнопок на экране\r\n\r\nСнять выделение с камня можно\r\nнажав на правую кнопку мыши в\r\nлюбом месте или левую на\r\nвыбранном камне" ;

        private readonly Text _faqText = new Text ( Str, Content.Content.GhoticFont, 25 )
                                         {
                                         FillColor = Color.Black, Position = new Vector2f ( 244, 52 )
                                         } ;


        public Menu ( )
        {
            ButtonExit =new Sprite ( Content.Content.ButtonExitFont ) { Position = new Vector2f ( 800, 500 ), Color = Color.Red } ;
            ButtonFaq = new Sprite ( Content.Content.ButtonFaQv2 )
                        {
                        Position = new Vector2f ( 800, 450 ), Color = Color.Red
                        } ;
            ButtonNewGame = new Sprite ( Content.Content.ButtonNewGameV2 ) { Position = new Vector2f ( 800, 350 ), Color = Color.Red } ;
            ButtonStatistic = new Sprite ( Content.Content.ButtonStatisticV2 )
                              {
                              Position = new Vector2f ( 795, 400 ), Color = Color.Red
                              } ;
        }

        public void Draw ( RenderTarget target, RenderStates states )
        {
            ButtonExit.Draw ( target, states ) ;
            ButtonFaq.Draw ( target, states ) ;
            ButtonNewGame.Draw ( target, states ) ;
            ButtonStatistic.Draw ( target, states ) ;
        }

        public void MenuScreenUpdate ( )
        {
            if ( Game.MenuScreenOn )
                GameLoop.Window.MouseButtonReleased += MenuScreenControl ;
        }

        private void MenuScreenControl ( object sender, MouseButtonEventArgs e )
        {
            if ( e.Button != Mouse.Button.Left || !Game.MenuScreenOn ) return ;
            if ( ButtonExit.GetGlobalBounds ().Contains ( e.X, e.Y ) )
            {
                var clock = new Clock () ;

                while ( clock.ElapsedTime.AsSeconds () <= 0.2f )
                {
                }

                Content.Content.BackMusic.Stop () ;
                Environment.Exit ( 0 ) ;
            }
            else if ( ButtonNewGame.GetGlobalBounds ().Contains ( e.X, e.Y ) )
            {
                var clock = new Clock () ;

                while ( clock.ElapsedTime.AsSeconds () <= 0.2f )
                {
                }

                GameLoop.StopUpdate = false ;
                Game.MenuScreenOn   = false ;
                Game.GameScreenOn   = true ;
                
                Game.RestartDesk();
                
                GameLoop.Window.Clear () ;
            }
            else if ( ButtonStatistic.GetGlobalBounds ().Contains ( e.X, e.Y ) )
            {
                var clock = new Clock () ;

                while ( clock.ElapsedTime.AsSeconds () <= 0.2f )
                {
                }

                Game.MenuScreenOn      = false ;
                Game.StatisticScreenOn = true ;
            }
            else if ( ButtonFaq.GetGlobalBounds ().Contains ( e.X, e.Y ) )
            {
                var clock = new Clock () ;

                while ( clock.ElapsedTime.AsSeconds () <= 0.2f )
                {
                }

                Game.MenuScreenOn = false ;
                Game.FaQScreenOn  = true ;
            }
        }

        public  void FaQScreenUpdate ( )
        {
            if ( !Game.FaQScreenOn ) return ;
            GameLoop.Window.KeyReleased         += ReturnToMenu ;
            GameLoop.Window.MouseButtonReleased += FaQScreenControl ;
        }

        public void FaQScreenDraw ( )
        {
            _backSprite3.Draw ( GameLoop.Window, RenderStates.Default ) ;
            _faqText.Draw ( GameLoop.Window, RenderStates.Default ) ;
        }

        private  void FaQScreenControl ( object o,MouseButtonEventArgs e )
        {
            if ( e.Button != Mouse.Button.Left ) return ;
            var rect = new IntRect ( 676, 57, 1054, 552 ) ;
            if ( rect.Contains ( e.X, e.Y ) )
            {
                ReturnToMenu ( this,new KeyEventArgs ( new KeyEvent () ) { Code = Keyboard.Key.Escape } ) ;
            }
        }

        private  void ReturnToMenu ( object sender, KeyEventArgs keyEventArgs )
        {
            if ( keyEventArgs.Code != Keyboard.Key.Escape ) return ;
            GameLoop.Window.MouseButtonReleased -= MenuScreenControl;
            GameLoop.Window.MouseButtonReleased -= FaQScreenControl ;
            GameLoop.Window.MouseButtonReleased -= StatisticScreenControl ;
            GameLoop.Window.KeyReleased -= ReturnToMenu ;
            Game.MenuScreenOn      = true ;
            Game.FaQScreenOn       = false ;
            Game.GameScreenOn      = false ;
            Game.StatisticScreenOn = false ;
        }

        public void StatisticScreenUpdate ( )
        {
            Statistic.MakeTop () ;
            if ( !Game.StatisticScreenOn ) return ;
            GameLoop.Window.KeyReleased         += ReturnToMenu ;
            GameLoop.Window.MouseButtonReleased += StatisticScreenControl ;
        }

        private void StatisticScreenControl ( object o, MouseButtonEventArgs e )
        {
            if ( !Game.StatisticScreenOn || e.Button != Mouse.Button.Left ) return ;
            if ( e.X                                 < 700 || e.X > 1016 ) return ;
            if ( e.Y >= 83 && e.Y <= 252 )
            {
                Statistic.Delete () ;
            }
            else if ( e.Y >= 260 && e.Y <= 414 )
            {
                ReturnToMenu ( this ,new KeyEventArgs ( new KeyEvent () ) { Code = Keyboard.Key.Escape } ) ;
            }
            else if ( e.Y >= 415 && e.Y <= 541 && e.X >= 810 && e.X <= 930 )
            {
                switch ( _texture )
                {
                    case true :
                        _texture = false ;
                        break ;
                    default :
                        _texture = true ;
                        break ;
                }
            }
        }

        public void StatisticScreenDraw ( )
        {
            if ( _texture )
                _backSprite1.Draw ( GameLoop.Window, RenderStates.Default ) ;
            else
                _backSprite2.Draw ( GameLoop.Window, RenderStates.Default ) ;
            Statistic.Draw () ;
        }
    }
}