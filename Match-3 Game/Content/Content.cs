using System ;
using SFML.Audio ;
using SFML.Graphics ;

namespace Match_3_Game.Content
{
    public static class Content
    {
        private static readonly string DirTexture = $"{Environment.CurrentDirectory}\\Content\\Textures\\" ;
        private static readonly string DirFonts = $"{Environment.CurrentDirectory}\\Content\\Fonts\\" ;
        private static readonly string DirSounds = $"{Environment.CurrentDirectory}\\Content\\Sounds\\" ;
        public static readonly string DirStatistic = $"{Environment.CurrentDirectory}\\Content\\Statistic\\" ;

        public static Image Icon ;

        public static Texture GemSkull ;
        public static Texture GemHand ;
        public static Texture GemKey ;
        public static Texture GemCross ;
        public static Texture GemNone ;


        public static Texture ButtonExitFont ;
        public static Texture ButtonNewGameV2 ;
        public static Texture ButtonStatisticV2 ;
        public static Texture ButtonMenuV2 ;
        public static Texture ButtonFaQv2 ;


        public static Texture BackgroundAll ;
        public static Texture BackgroundForStats1 ;
        public static Texture BackgroundForStats2 ;
        public static Texture BackgroundForGem ;
        public static Texture BackFaq ;
        public static Texture CursorTexture ;
        public static Texture DiedTexture ;
        public static Font    GhoticFont ;

        public static SoundBuffer DeathBuffer ;
        public static SoundBuffer BreakBuffer ;

        public static Music BackMusic ;


        public static void LoadContent ( )
        {
            Icon = new Image ( $"{DirTexture}Gem3.jpg" ) ;
            
            GemCross       = new Texture ( $"{DirTexture}Gem1.jpg" ) ;         
            GemHand        = new Texture ( $"{DirTexture}Gem2.jpg" ) ;
            GemSkull       = new Texture ( $"{DirTexture}Gem3.jpg" ) ;
            GemKey         = new Texture ( $"{DirTexture}Gem4.jpg" ) ;
            GemNone        = new Texture ( $"{DirTexture}None.psd" ) ;
            
            ButtonExitFont = new Texture ( $"{DirTexture}Exit1.psd" ) ;
            
            ButtonMenuV2      = new Texture ( $"{DirTexture}Menu1.psd" ) ;
            ButtonNewGameV2   = new Texture ( $"{DirTexture}NewGameV2.psd" ) ;
            ButtonStatisticV2 = new Texture ( $"{DirTexture}Statistic1.psd" ) ;
            ButtonFaQv2       = new Texture ( $"{DirTexture}FaQV2.psd" ) ;

            BackgroundAll       = new Texture ( $"{DirTexture}BackgroundForAll.psd" ) ;
            BackgroundForStats1 = new Texture ( $"{DirTexture}TopofTop.psd" ) ;
            BackgroundForStats2 = new Texture ( $"{DirTexture}TopofTop2.psd" ) ;
            BackgroundForGem    = new Texture ( $"{DirTexture}BackgroundForGems.jpg" ) ;
            BackFaq             = new Texture ( $"{DirTexture}FaQBackground.psd" ) ;

            CursorTexture = new Texture ( $"{DirTexture}Cursor.psd" ) ;

            DiedTexture = new Texture ( $"{DirTexture}Died.jpg" ) ;

            GhoticFont = new Font ( $"{DirFonts}Ghotic2.ttf" ) ;

            DeathBuffer = new SoundBuffer ( $"{DirSounds}DS.wav" ) ;
            BreakBuffer = new SoundBuffer ( $"{DirSounds}Break.wav" ) ;

            BackMusic = new Music ( $"{DirSounds}Back.wav" ) { Volume = 5, Loop = true } ;
        }
    }
}