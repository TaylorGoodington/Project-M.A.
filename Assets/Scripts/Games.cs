[System.Serializable]
public class Games
{
    public int gameName;
    public int gameID;
    public string gameDescription;
    public int stage1Rounds;
    public int stage1NumberOfAcceptableRoundsToFail;
    public int stage2Rounds;
    public int stage2NumberOfAcceptableRoundsToFail;
    public int stage3Rounds;
    public int stage3NumberOfAcceptableRoundsToFail;
    public int stage4Rounds;
    public int stage4NumberOfAcceptableRoundsToFail;
    public int stage5Rounds;
    public int stage5NumberOfAcceptableRoundsToFail;
    public int stage6Rounds;
    public int stage6NumberOfAcceptableRoundsToFail;

    public Games (int name, int id, string description, int stage1Max, int stage1Needed, int stage2Max, 
                  int stage2Needed, int stage3Max, int stage3Needed, int stage4Max, int stage4Needed, 
                  int stage5Max, int stage5Needed, int stage6Max, int stage6Needed)
    {
        gameName = name;
        gameID = id;
        gameDescription = description;
        stage1Rounds = stage1Max;
        stage1NumberOfAcceptableRoundsToFail = stage1Needed;
        stage2Rounds = stage2Max;
        stage2NumberOfAcceptableRoundsToFail = stage2Needed;
        stage3Rounds = stage3Max;
        stage3NumberOfAcceptableRoundsToFail = stage3Needed;
        stage4Rounds = stage4Max;
        stage4NumberOfAcceptableRoundsToFail = stage4Needed;
        stage5Rounds = stage5Max;
        stage5NumberOfAcceptableRoundsToFail = stage5Needed;
        stage6Rounds = stage6Max;
        stage6NumberOfAcceptableRoundsToFail = stage6Needed;
    }
}