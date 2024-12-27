namespace Scripts.Game
{
    /// <summary>
    /// Static Class Containing All available difficulty settings available in game.
    /// </summary>
    public static class GameDifficultyOptions
    {

        public static DifficultySettings EASY = new DifficultySettings("Easy", 600f, 10, 20);
        public static DifficultySettings MEDIUM = new DifficultySettings("Medium", 300f, 5, 10);
        public static DifficultySettings HARD = new DifficultySettings("Hard", 180f, 0, 5);


        /// <summary>
        ///Returns Difficulty Settings for the passed difficulty mode
        /// </summary>
        public static DifficultySettings GetDifficultyModeSettings(DifficultyModes difficulty = DifficultyModes.Medium)
        {
            if(difficulty == DifficultyModes.Easy) return EASY;
            else if(difficulty == DifficultyModes.Medium) return MEDIUM;
            else return HARD;
        }
    }

    /// <summary>
    /// Enum for difficulty modes.
    /// </summary>
    public enum DifficultyModes
    {
        Easy,
        Medium,
        Hard,
    }

    /// <summary>
    /// Struct Used to Represent Difficulty Settings for each Difficulty Mode
    /// </summary>
    public struct DifficultySettings
    {
        public string Difficulty { get; private set; }
        public float GameTime { get; private set; }
        public int InitialMicrochips { get; private set; }
        public int RechargePercentage {get; private set;}

        /// Constructor for configuring each settings 
        public DifficultySettings(string difficulty, float gameTime, int initialMicrochips, int rechargePercentage)
        {
            Difficulty = difficulty;
            GameTime = gameTime;
            InitialMicrochips = initialMicrochips;
            RechargePercentage = rechargePercentage;
        }
    }
}
