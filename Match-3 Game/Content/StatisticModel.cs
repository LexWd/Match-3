using System ;

namespace Match_3_Game.Content
{
    public class StatisticModel
    {
        public string Date { get ; }

        public string Score { get ; }

        public StatisticModel (int score )
        {
            Score = score.ToString() ;
            Date = DateTime.Now.ToShortDateString () ;
        }
    }
}