using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour
{
    public static List<string> GamesPlayedThisSession { get; set; }
    public static int CurrentRound { get; set; }
    public static int MaxRounds { get; set; }
    public static int RoundsFailed { get; set; }
    public static int NumberOfAcceptableRoundsToFail { get; set; }
    public static string CurrentGame { get; set; }
    public static int CurrentStage { get; set; }
    public static GameObject Game { get; private set; }

    void Start ()
    {
        GamesPlayedThisSession = new List<string>();
	}

    public static void LogOut()
    {
        GamesPlayedThisSession = new List<string>();
    }

    public static void SetUpGame ()
    {
        Game = GameObject.FindGameObjectWithTag("game");
        CurrentRound = 0;
        RoundsFailed = 0;
        CurrentGame = SceneManager.GetActiveScene().name;

        //TODO Adjust when the total number of categories is set.
        #region Current Stage and Max Rounds
        var category = int.Parse(CurrentGame.Substring(CurrentGame.Length - 1, 1));
        if (category == 1)
        {
            CurrentStage = GameControl.Game1Progress;
            SetRoundInformation();
        }
        else if (category == 2)
        {
            CurrentStage = GameControl.Game2Progress;
            SetRoundInformation();
        }
        #endregion

        if (!GamesPlayedThisSession.Contains(CurrentGame))
        {
            GamesPlayedThisSession.Add(CurrentGame);
            CallTutorial();
        }
        else
        {
            AdvanceToNextRound();
        }
    }

    private static void SetRoundInformation()
    {
        if (CurrentStage == 1)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage1Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage1NumberOfAcceptableRoundsToFail;
        }
        else if (CurrentStage == 2)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage2Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage2NumberOfAcceptableRoundsToFail;
        }
        else if (CurrentStage == 3)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage3Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage3NumberOfAcceptableRoundsToFail;
        }
        else if (CurrentStage == 4)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage4Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage4NumberOfAcceptableRoundsToFail;
        }
        else if (CurrentStage == 5)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage5Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage5NumberOfAcceptableRoundsToFail;
        }
        else if (CurrentStage == 6)
        {
            MaxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage6Rounds;
            NumberOfAcceptableRoundsToFail = GamesDatabase.games[GamesDatabase.gamesIndex[CurrentGame]].stage6NumberOfAcceptableRoundsToFail;
        }
    }

    public static void CallTutorial ()
    {
        Game.SendMessage("PlayTutorial");
    }

    public static void EndOfTutorial()
    {
        AdvanceToNextRound();
    }

    public static void AdvanceToNextRound ()
    {
        CurrentRound += 1;
        Game.SendMessage("StartRound", CurrentRound);
    }

    public static void WasQuestionAnsweredCorrectly (bool response)
    {
        if (response == false && RoundsFailed >= NumberOfAcceptableRoundsToFail)
        {
            Game.SendMessage("PlayDemo");
        }
        else
        {
            EndOfRound();
        }
    }

    public static void DemoIsOver ()
    {
        EndOfRound();
    }

    public static void EndOfRound ()
    {
        if (CurrentRound != MaxRounds)
        {
            AdvanceToNextRound();
        }
        else
        {
            if (RoundsFailed <= NumberOfAcceptableRoundsToFail)
            {
                Game.SendMessage("GameOver", true);
            }
            else
            {
                Game.SendMessage("GameOver", false);
            }
        }
    }
}