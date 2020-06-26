using System ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using Match_3_Game.BaseLogic ;
using Newtonsoft.Json ;
using SFML.Graphics ;
using SFML.System ;

namespace Match_3_Game.Content
{
    public static class Statistic
    {
        private static readonly string                      Path = $"{Content.DirStatistic}Statistic.json" ;
        public static           BindingList<StatisticModel> StatisticData ;
        private static readonly Text []                     Top = new Text[ 10 ] ;

        public static void AddRecord ( int score )
        {
            var abc = new StatisticModel ( score ) ;
            StatisticData.Add ( abc ) ;
            Save ( StatisticData ) ;
        }


        public static void Delete ( )
        {
            if ( StatisticData.Count <= 0 ) return ;
            StatisticData.Clear () ;
            Save ( StatisticData ) ;
        }

        public static void MakeTop ( )
        {
            Read () ;
            Sort ( out var sortedListInstance ) ;
            if ( StatisticData != null )
                CreateTop ( sortedListInstance ) ;
        }

        public static void Draw ( )
        {
            foreach ( var s in Top ) s?.Draw ( GameLoop.Window, RenderStates.Default ) ;
        }

        private static void Sort ( out BindingList<StatisticModel> sortedListInstance )
        {
            sortedListInstance =
            new BindingList<StatisticModel> ( StatisticData.OrderByDescending ( x => x.Score ).ToList () ) ;
        }

        private static void CreateTop ( BindingList<StatisticModel> data )
        {
            if ( data == null ) throw new ArgumentNullException ( nameof ( data ) ) ;
            for ( var i = 0 ; i < 10 ; i++ )
            {
                if ( data.Count <= i ) continue ;
                var score = data [ i ].Score ;
                if ( score == null ) continue ;
                if ( Convert.ToInt32 ( score ) <= 0 ) continue ;
                var temp = $"{i + 1}:   {score}   {data [ i ].Date}" ;
                Top [ i ] = new Text ( temp, Content.GhoticFont, 20 )
                            {
                            Position = new Vector2f ( 244, 52 * (i + 1) ), FillColor = Color.Black
                            } ;
            }
        }

        public static BindingList<StatisticModel> Read ( )
        {
            var fileExist = File.Exists ( Path ) ;
            if ( fileExist )
                using (var reader = File.OpenText ( Path ))
                {
                    var input = reader.ReadToEnd () ;
                    return JsonConvert.DeserializeObject<BindingList<StatisticModel>> ( input ) ;
                }

            File.CreateText ( Path ).Dispose () ;
            return new BindingList<StatisticModel> { new StatisticModel ( - 1 ) } ;
        }

        private static void Save ( BindingList<StatisticModel> statistic )
        {
            using (var writer = File.CreateText ( Path ))
            {
                var output = JsonConvert.SerializeObject ( statistic ) ;
                writer.Write ( output ) ;
            }
        }
    }
}