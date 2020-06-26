using System ;
using Match_3_Game.BaseLogic ;
using SFML.Graphics ;
using SFML.System ;

namespace Match_3_Game.GameObjects
{
    public enum GemType
    {
        None,
        Cross,
        Hand,
        Key,
        Skull
    }

    public class Gem : Transformable, Drawable
    {
        public readonly RectangleShape Rectangle ;

        private readonly Vector2f _gemSize = new Vector2f ( 60f, 60f ) ;

        public GemType CurrentGemType ;

        public readonly int X ;
        public readonly int Y ;

        public Gem GemUp    { private set ; get ; }
        public Gem GemDown  { private set ; get ; }
        public Gem GemLeft  { private set ; get ; }
        public Gem GemRight { private set ; get ; }


        public bool MarkedForDelete ;

        public Gem ( GemType type, Gem gemU, Gem gemD, Gem gemL, Gem gemR, int x, int y )
        {
            X = x ;
            Y = y ;
            if ( gemU != null )
            {
                GemUp         = gemU ;
                GemUp.GemDown = this ;
            }

            if ( gemD != null )
            {
                GemDown       = gemD ;
                GemDown.GemUp = this ;
            }

            if ( gemL != null )
            {
                GemLeft          = gemL ;
                GemLeft.GemRight = this ;
            }

            if ( gemR != null )
            {
                GemRight         = gemR ;
                GemRight.GemLeft = this ;
            }

            switch ( type )
            {
                case GemType.Cross :
                    Rectangle      = new RectangleShape ( _gemSize ) { Texture = Content.Content.GemCross } ;
                    CurrentGemType = GemType.Cross ;
                    break ;
                case GemType.Hand :
                    Rectangle      = new RectangleShape ( _gemSize ) { Texture = Content.Content.GemHand } ;
                    CurrentGemType = GemType.Hand ;
                    break ;
                case GemType.Key :
                    Rectangle      = new RectangleShape ( _gemSize ) { Texture = Content.Content.GemKey } ;
                    CurrentGemType = GemType.Key ;
                    break ;
                case GemType.Skull :
                    Rectangle      = new RectangleShape ( _gemSize ) { Texture = Content.Content.GemSkull } ;
                    CurrentGemType = GemType.Skull ;
                    break ;
                case GemType.None :
                    Rectangle      = new RectangleShape ( _gemSize ) { Texture = Content.Content.GemNone } ;
                    CurrentGemType = GemType.None ;
                    break ;
                default :
                    throw new ArgumentOutOfRangeException ( nameof ( type ), type, null ) ;
            }

            Rectangle.Origin   = new Vector2f ( 30,                30 ) ;
            Rectangle.Position = new Vector2f ( 430 + 60.005f * y, 150 + 60.005f * x ) ;
        }

        public void UpdateView ( )
        {
            switch ( CurrentGemType )
            {
                case GemType.None :
                    Rectangle.Texture = Content.Content.GemNone ;
                    break ;
                case GemType.Cross :
                    Rectangle.Texture = Content.Content.GemCross ;
                    break ;
                case GemType.Hand :
                    Rectangle.Texture = Content.Content.GemHand ;
                    break ;
                case GemType.Key :
                    Rectangle.Texture = Content.Content.GemKey ;
                    break ;
                case GemType.Skull :
                    Rectangle.Texture = Content.Content.GemSkull ;
                    break ;
                default : throw new ArgumentOutOfRangeException () ;
            }

            GameLoop.StopUpdate = false ;
            if ( Desk.FocusedGems [ 0 ] != null && !Equals ( Desk.FocusedGems [ 0 ] ) )
                Rectangle.FillColor = Color.White ;
        }


        public void Draw ( RenderTarget target, RenderStates states )
        {
            Rectangle.Draw ( target, RenderStates.Default ) ;
        }

        public override bool Equals ( object o )
        {
            if ( ReferenceEquals ( null, o ) ) return false ;
            if ( ReferenceEquals ( this, o ) ) return true ;
            return o.GetType () == GetType () && Equals ( ( Gem ) o ) ;
        }

        private bool Equals ( Gem other )
        {
            return CurrentGemType == other.CurrentGemType && X == other.X && Y == other.Y ;
        }

        public override int GetHashCode ( )
        {
            unchecked
            {
                // ReSharper disable once NonReadonlyMemberInGetHashCode
                var hashCode = ( int ) CurrentGemType ;
                hashCode = (hashCode * 397) ^ X ;
                hashCode = (hashCode * 397) ^ Y ;
                return hashCode ;
            }
        }
    }
}