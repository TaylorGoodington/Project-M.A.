using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Game11 : MonoBehaviour
{
    #region Generic Game Properties
    public Animator gameAnimator;
    public GameObject blockUserControlPanel;
    public AudioSource audioSource;
    private bool success;
    #endregion

    #region Specific Game Properties
    public Transform numberSlotsParent;
    public List<AudioClip> audioClips;
    private List<GameObject> numberSlots;
    #endregion

	void Start ()
    {
        #region Specific Game Properties
        numberSlots = new List<GameObject>();
	    foreach (Transform child in numberSlotsParent)
        {
            numberSlots.Add(child.gameObject);
        }
        #endregion
        
        #region Generic Game Properties
        TranisitionIn();
        audioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
        #endregion
    }

    #region Animation Functions
    //Called by the Start function.
    public void TranisitionIn ()
    {
        gameAnimator.Play("transitionIn");
    }

    //Called at the end of the GameOver animations.
    public void TranisitionOut()
    {
        gameAnimator.Play("transitionOut");
    }

    //Called at the end of the transitionIn animation.
    public void AskGameEngineToSetUpGame ()
    {
        GameEngine.SetUpGame();
    }

    //Called by the GameEngine if the game has not been played this session.
    public void PlayTutorial()
    {
        gameAnimator.Play("tutorial");
    }

    //Called by the GameEngine if the player gets a question wrong which will make them unable to clear the stage.
    public void PlayDemo()
    {
        gameAnimator.Play("demo");
    }

    //Called by the GameEngine after it is notified of the results from the round.
    public void NotifyPlayer ()
    {
        if (success)
        {
            gameAnimator.Play("success");
        }
        else
        {
            gameAnimator.Play("failure");
        }
    }

    //Called at the end of the demo and the end of updating the progress bar.
    public void EndOfRound()
    {
        GameEngine.EndOfRound();
    }

    //Called by the GameEngine if the final round has been completed.
    public void GameOver(bool didPlayerWin)
    {
        if (didPlayerWin)
        {
            gameAnimator.Play("victory");
        }
        else
        {
            gameAnimator.Play("defeat");
        }
    }

    //Called by the animator when needed.
    public void PlayAudio (int clip)
    {
        audioSource.clip = audioClips[clip];
        audioSource.Play();
    }
    #endregion

    //Called by the GameEngine at the begining of each round.
    public void StartRound (int currentRound)
    {
        #region Specific to this game.
        int slot = Random.Range(0, 3);
        string resoucePath = "Game Icons/Numbers/Level" + GameControl.studentLevelProgress + "/Category" + int.Parse(GameEngine.currentGame.Substring(1, 1)) + "";

        #region Correct Button
        //the correct number will be stage - 1
        Button correctNumber = numberSlots[slot].GetComponent<Button>();
        correctNumber.image.sprite = Resources.Load<Sprite>(resoucePath + "");
        SpriteState sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "");
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "");
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "");
        correctNumber.spriteState = sprites;
        //Add Listener for correct button push.
        #endregion

        //Narrate which number should be selected according to the correct number.
        //coroutine delay should be length of audioclip.
        StartCoroutine("ToggleUserBlockPanel", 0.4);
        #endregion
    }

    //Added as a listener to the button during start round.
    public void Correct()
    {
        GameEngine.WasQuestionAnsweredCorrectly(true);
        success = true;
    }

    //Added as a listener to the button during start round.
    public void InCorrect()
    {
        GameEngine.WasQuestionAnsweredCorrectly(false);
        success = false;
    }

    private IEnumerator ToggleUserBlockPanel (float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void UpdateSoundEffectsVolume ()
    {
        audioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }
}