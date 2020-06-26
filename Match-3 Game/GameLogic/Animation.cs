using Match_3_Game.BaseLogic ;
using Match_3_Game.GameObjects ;
using SFML.Graphics ;
using SFML.System ;

namespace Match_3_Game.GameLogic
{
    internal static class Animation
    {
        public static void Left ( Gem first, Gem second )
        {
            GameLoop.StopUpdate = true ;

            var temporalPosition1 = first.Rectangle.Position ;
            var temporalPosition2 = second.Rectangle.Position ;

            first.Rectangle.FillColor  = Color.White ;
            second.Rectangle.FillColor = Color.White ;

            while ( first.Rectangle.Position.X  >= temporalPosition2.X ||
                    second.Rectangle.Position.X <= temporalPosition1.X )
            {
                first.UpdateView () ;
                second.UpdateView () ;
                GameLoop.Window.Clear ( Color.Black ) ;
                first.Rectangle.Position =
                new Vector2f ( first.Rectangle.Position.X - 0.6f, first.Rectangle.Position.Y ) ;
                second.Rectangle.Position =
                new Vector2f ( second.Rectangle.Position.X + 0.6f, second.Rectangle.Position.Y ) ;
                Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
                GameLoop.Window.Display () ;
            }

            var temporalType = first.CurrentGemType ;
            first.CurrentGemType  = second.CurrentGemType ;
            second.CurrentGemType = temporalType ;


            first.Rectangle.Position  = temporalPosition1 ;
            second.Rectangle.Position = temporalPosition2 ;

            first.UpdateView () ;
            second.UpdateView () ;

            Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
            Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
            GameLoop.Window.Display () ;

            Desk.ChangeGem ( first, second, Desk.WhatToDo.Unfocus ) ;
            GameLoop.StopUpdate = false ;
        }

        public static void Right ( Gem first, Gem second )
        {
            GameLoop.StopUpdate = true ;


            var temporalPosition1 = first.Rectangle.Position ;
            var temporalPosition2 = second.Rectangle.Position ;

            first.Rectangle.FillColor  = Color.White ;
            second.Rectangle.FillColor = Color.White ;

            while ( first.Rectangle.Position.X  <= temporalPosition2.X ||
                    second.Rectangle.Position.X >= temporalPosition1.X )
            {
                GameLoop.Window.Clear ( Color.Black ) ;
                first.Rectangle.Position =
                new Vector2f ( first.Rectangle.Position.X + 0.6f, first.Rectangle.Position.Y ) ;
                second.Rectangle.Position =
                new Vector2f ( second.Rectangle.Position.X - 0.6f, second.Rectangle.Position.Y ) ;
                Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
                GameLoop.Window.Display () ;
            }

            var temporalType = first.CurrentGemType ;
            first.CurrentGemType  = second.CurrentGemType ;
            second.CurrentGemType = temporalType ;

            first.Rectangle.Position  = temporalPosition1 ;
            second.Rectangle.Position = temporalPosition2 ;

            first.UpdateView () ;
            second.UpdateView () ;

            Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
            Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
            GameLoop.Window.Display () ;

            Desk.ChangeGem ( first, second, Desk.WhatToDo.Unfocus ) ;
            GameLoop.StopUpdate = false ;
        }

        public static void Up ( Gem first, Gem second )
        {
            GameLoop.StopUpdate = true ;

            first.Rectangle.FillColor  = Color.White ;
            second.Rectangle.FillColor = Color.White ;
            var temporalPosition1 = first.Rectangle.Position ;
            var temporalPosition2 = second.Rectangle.Position ;

            while ( first.Rectangle.Position.Y  >= temporalPosition2.Y ||
                    second.Rectangle.Position.Y <= temporalPosition1.Y )
            {
                first.Rectangle.Position =
                new Vector2f ( first.Rectangle.Position.X, first.Rectangle.Position.Y - 0.6f ) ;
                second.Rectangle.Position =
                new Vector2f ( second.Rectangle.Position.X, second.Rectangle.Position.Y + 0.6f ) ;
                Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
                GameLoop.Window.Display () ;
            }

            var temporalType = first.CurrentGemType ;
            first.CurrentGemType      = second.CurrentGemType ;
            second.CurrentGemType     = temporalType ;
            first.Rectangle.Position  = temporalPosition1 ;
            second.Rectangle.Position = temporalPosition2 ;

            first.UpdateView () ;
            second.UpdateView () ;

            Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
            Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
            GameLoop.Window.Display () ;

            Desk.ChangeGem ( first, second, Desk.WhatToDo.Unfocus ) ;
            GameLoop.StopUpdate = false ;
        }


        public static void Down ( Gem first, Gem second )
        {
            GameLoop.StopUpdate = true ;


            var temporalPosition1 = first.Rectangle.Position ;
            var temporalPosition2 = second.Rectangle.Position ;
            first.Rectangle.FillColor  = Color.White ;
            second.Rectangle.FillColor = Color.White ;

            while ( first.Rectangle.Position.Y  <= temporalPosition2.Y ||
                    second.Rectangle.Position.Y >= temporalPosition1.Y )
            {
                GameLoop.Window.Clear ( Color.Black ) ;

                first.Rectangle.Position =
                new Vector2f ( first.Rectangle.Position.X, first.Rectangle.Position.Y + 2.5f ) ;
                second.Rectangle.Position =
                new Vector2f ( second.Rectangle.Position.X, second.Rectangle.Position.Y - 2.5f ) ;
                Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
                Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
                GameLoop.Window.Display () ;
            }

            var temporalType = first.CurrentGemType ;
            first.CurrentGemType  = second.CurrentGemType ;
            second.CurrentGemType = temporalType ;

            first.Rectangle.Position  = temporalPosition1 ;
            second.Rectangle.Position = temporalPosition2 ;

            first.UpdateView () ;
            second.UpdateView () ;

            Game.Background.Draw ( GameLoop.Window, RenderStates.Default ) ;
            Desk.Draw ( GameLoop.Window, RenderStates.Default ) ;
            GameLoop.Window.Display () ;

            Desk.ChangeGem ( first, second, Desk.WhatToDo.Unfocus ) ;
            GameLoop.StopUpdate = false ;
        }
    }
}