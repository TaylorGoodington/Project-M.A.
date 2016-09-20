using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameEngine : MonoBehaviour
{
    public static List<string> gamesPlayedThisSession;
    public static int currentRound;
    public static int maxRounds;
    public static int roundsFailed;
    public static bool canStageBeCleared;
    public static string currentGame;
    public static int currentStage;
    private static GameObject game;

	void Start ()
    {
        gamesPlayedThisSession = new List<string>();
        canStageBeCleared = true;
	}

    public static void SetUpGame ()
    {
        game = GameObject.FindGameObjectWithTag("game");
        canStageBeCleared = true;
        currentRound = 0;
        roundsFailed = 0;
        currentGame = SceneManager.GetActiveScene().name;

        //TODO Adjust when the total number of categories is set.
        #region Current Stage and Max Rounds
        int category = int.Parse(currentGame.Substring(currentGame.Length - 1, 1));
        if (category == 1)
        {
            currentStage = GameControl.game1Progress;
            maxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[currentGame]].stage1Rounds;
        }
        else if (category == 2)
        {
            currentStage = GameControl.game2Progress;
            maxRounds = GamesDatabase.games[GamesDatabase.gamesIndex[currentGame]].stage2Rounds;
        }
        #endregion

        if (!gamesPlayedThisSession.Contains(currentGame))
        {
            gamesPlayedThisSession.Add(currentGame);
            CallTutorial();
        }
        else
        {
            AdvanceToNextRound();
        }
    }

    public static void CallTutorial ()
    {
        game.SendMessage("PlayTutorial");
    }

    public static void AdvanceToNextRound ()
    {
        currentRound += 1;
        game.SendMessage("StartRound", currentRound);
    }

    public static void WasQuestionAnsweredCorrectly (bool response)
    {
        if (response)
        {
            //tell game to notify player and update the progress bar.
        }
        else
        {
            //see if demo is needed otherwise tell game to notify player.
        }
    }

    public static void EndOfRound ()
    {
        if (currentRound != maxRounds)
        {
            AdvanceToNextRound();
        }
        else
        {
            //check if the player won and then pass the response to the game.
            game.SendMessage("GameOver", 1);
        }
    }
}