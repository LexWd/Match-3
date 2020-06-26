namespace Match_3_Game.BaseLogic
{
    public class GameTime
    {
        public float TimeScale { get ; set ; } = 1f ;

        // ReSharper disable once UnusedMember.Global
        public float DeltaTime
        {
            get => DeltaTimeUnscaled * TimeScale ;
            set => DeltaTimeUnscaled = value ;
        }

        private float DeltaTimeUnscaled { get ; set ; }
        // ReSharper disable once NotAccessedField.Local
        private static float _totalTimeElapsed ;

        public void Update ( float deltaTime, float totalTimeElapsed )
        {
            DeltaTimeUnscaled = deltaTime ;
            _totalTimeElapsed = totalTimeElapsed ;
        }
    }
}