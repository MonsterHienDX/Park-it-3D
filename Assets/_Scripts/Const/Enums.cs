public enum LetterState
{
    Correct = 0,
    Incorrect = 1,
    WrongLocation = 2,
    Unknown = 4
}
public enum PuzzleState
{
    InProgress,
    Failed,
    Complete
}
public enum WordsState
{
    Easy12,
    Easy3,
    Medium4,
    Medium5,
    Hard
}

public enum RewardType
{
    Coins,
    SkipItem,
    TargetItem,
    SearchItem,
    RemoveAds,
}

public enum ItemType
{
    Skip,
    Search,
    Target,
}

public enum GameScreen
{
    PlayScreen,
    Shop,
    MainScreen,
    UserProfile,
}

[System.Serializable]
public enum Difficult
{
    EASYI = 0,
    EASYII,
    EASYIII,
    MEDIUMIV,
    MEDIUMV,
    MEDIUMVI,
    HARDVII,
    HARDVIII,
}
