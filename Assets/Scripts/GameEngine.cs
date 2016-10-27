using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameEngine : MonoBehaviour
{
    public static List<string> GamesPlayedThisSession { get; set; }
    public static int CurrentRound { get; set; }
    public static int MaxRounds { get; set; }
    public static int RoundsFailed { get; set; }
    public static int RoundsSucceeded { get; set; }
    public static int NumberOfAcceptableRoundsToFail { get; set; }
    public static string CurrentGame { get; set; }
    public static int CurrentStage { get; set; }
    public static bool DidPlayerPassCurrentRound { get; set; }
    public static bool IsGameCleared { get; set; }
    public static double TimePlayed { get; set; }
    public static GameObject Game { get; private set; }

    private static DateTime timeStart;

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
        IsGameCleared = false;
        timeStart = DateTime.Now;

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

    public static void PlayerHasBeenNotified ()
    {
        if (DidPlayerPassCurrentRound == false && RoundsFailed > NumberOfAcceptableRoundsToFail)
        {
            Game.SendMessage("PlayDemo");
        }
        else
        {
            EndOfRound();
        }
    }

    public static void PlayerHasSelectedAnAnswer(bool answer)
    {
        if (answer)
        {
            RoundsSucceeded++;
        }
        else
        {
            RoundsFailed++;
        }

        DidPlayerPassCurrentRound = answer;

        Game.SendMessage("NotifyPlayer", answer);
    }

    public static void DemoIsOver ()
    {
        EndOfRound();
    }

    public static void EndOfRound ()
    {
        Debug.Log("Current Round: " + CurrentRound + "  Max Rounds: " + MaxRounds);
        if (RoundsSucceeded == MaxRounds - NumberOfAcceptableRoundsToFail)
        {
            CallUpdloadInformation();
            IsGameCleared = true;
            TimePlayed = (timeStart - DateTime.Now).TotalSeconds;
            Game.SendMessage("GameOver", IsGameCleared);
        }
        else if (CurrentRound != MaxRounds)
        {
            AdvanceToNextRound();
        }
        else
        {
            CallUpdloadInformation();

            if (RoundsFailed <= NumberOfAcceptableRoundsToFail)
            {
                IsGameCleared = true;
                Game.SendMessage("GameOver", IsGameCleared);
            }
            else
            {
                IsGameCleared = false;
                Game.SendMessage("GameOver", IsGameCleared);
            }
        }
    }

    public static void CallUpdloadInformation ()
    {
        if (IsGameCleared)
        {
            GameControl.UploadStudentInformation(TimePlayed, CurrentStage++);

            if (CurrentGame == "1")
            {
                GameControl.Game1Progress++;
            }
            else if (CurrentGame == "2")
            {
                GameControl.Game2Progress++;
            }
            else if (CurrentGame == "3")
            {
                GameControl.Game3Progress++;
            }

        }
        else
        {
            GameControl.UploadStudentInformation(TimePlayed);
        }
    }
}