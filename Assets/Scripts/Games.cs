[System.Serializable]
public class Games
{
    public int gameName;
    public int gameID;
    public string gameDescription;
    public int stage1Rounds;
    public int stage1RoundsNeededToSucceed;
    public int stage2Rounds;
    public int stage2RoundsNeededToSucceed;
    public int stage3Rounds;
    public int stage3RoundsNeededToSucceed;
    public int stage4Rounds;
    public int stage4RoundsNeededToSucceed;
    public int stage5Rounds;
    public int stage5RoundsNeededToSucceed;
    public int stage6Rounds;
    public int stage6RoundsNeededToSucceed;

    public Games (int name, int id, string description, int stage1Max, int stage1Needed, int stage2Max, 
                  int stage2Needed, int stage3Max, int stage3Needed, int stage4Max, int stage4Needed, 
                  int stage5Max, int stage5Needed, int stage6Max, int stage6Needed)
    {
        gameName = name;
        gameID = id;
        gameDescription = description;
        stage1Rounds = stage1Max;
        stage1RoundsNeededToSucceed = stage1Needed;
        stage2Rounds = stage2Max;
        stage2RoundsNeededToSucceed = stage2Needed;
        stage3Rounds = stage3Max;
        stage3RoundsNeededToSucceed = stage3Needed;
        stage4Rounds = stage4Max;
        stage4RoundsNeededToSucceed = stage4Needed;
        stage5Rounds = stage5Max;
        stage5RoundsNeededToSucceed = stage5Needed;
        stage6Rounds = stage6Max;
        stage6RoundsNeededToSucceed = stage6Needed;
    }
}