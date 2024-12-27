namespace Scripts.Game
{
    public static class GameDifficultyOptions
    {

        public static DifficultySettings EASY = new DifficultySettings("Easy", 600f, 10, 20);
        public static DifficultySettings MEDIUM = new DifficultySettings("Medium", 300f, 5, 10);
        public static DifficultySettings HARD = new DifficultySettings("Hard", 180f, 0, 5);
        
        public static DifficultySettings GetDifficultyModeSettings(DifficultyModes difficulty = DifficultyModes.Medium)
        {
            if(difficulty == DifficultyModes.Easy) return EASY;
            else if(difficulty == DifficultyModes.Medium) return MEDIUM;
            else return HARD;
        }
    }

    public enum DifficultyModes
    {
        Easy,
        Medium,
        Hard,
    }
    public struct DifficultySettings
    {
        public string Difficulty { get; private set; }
        public float GameTime { get; private set; }
        public int InitialMicrochips { get; private set; }
        public int RechargePercentage {get; private set;}

        public DifficultySettings(string difficulty, float gameTime, int initialMicrochips, int rechargePercentage)
        {
            Difficulty = difficulty;
            GameTime = gameTime;
            InitialMicrochips = initialMicrochips;
            RechargePercentage = rechargePercentage;
        }
    }
}
