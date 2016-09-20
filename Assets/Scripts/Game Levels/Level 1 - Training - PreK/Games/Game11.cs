using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

//TODO test and refactor most of this into a base class.
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
        int correctSlot = Random.Range(0, 3);
        int wrongSlot1;
        int wrongSlot2;
        List<int> possibleNumbers = new List<int> { 0, 1, 2, 3, 4, 5 };
        string resoucePath = "Game Icons/Numbers/Level" + GameControl.studentLevelProgress + 
                             "/Category" + int.Parse(GameEngine.currentGame.Substring(GameEngine.currentGame.Length - 1, 1)) + "/";

        #region Correct Button
        int number = GameEngine.currentStage - 1;
        Button correctNumber = numberSlots[correctSlot].GetComponent<Button>();
        correctNumber.image.sprite = Resources.Load<Sprite>(resoucePath + "default" + number);
        SpriteState sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + number);
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + number);
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + number);
        correctNumber.spriteState = sprites;
        correctNumber.onClick.AddListener(() => Correct());
        #endregion

        #region Incorrect Buttons
        if (correctSlot == 0)
        {
            wrongSlot1 = 1;
            wrongSlot2 = 2;
        }
        else if (correctSlot == 1)
        {
            wrongSlot1 = 0;
            wrongSlot2 = 2;
        }
        else
        {
            wrongSlot1 = 0;
            wrongSlot2 = 1;
        }

        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, number);
        int wrongNumber = possibleNumbers[0];
        Button incorrectNumber1 = numberSlots[wrongSlot1].GetComponent<Button>();
        incorrectNumber1.image.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber);
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber);
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber);
        incorrectNumber1.spriteState = sprites;
        incorrectNumber1.onClick.AddListener(() => InCorrect());

        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, wrongNumber);
        wrongNumber = possibleNumbers[0];
        Button incorrectNumber2 = numberSlots[wrongSlot2].GetComponent<Button>();
        incorrectNumber2.image.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber);
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber);
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber);
        incorrectNumber2.spriteState = sprites;
        incorrectNumber2.onClick.AddListener(() => InCorrect());
        #endregion

        PlayAudio(number);
        StartCoroutine("ToggleUserBlockPanel", audioClips[number].length);
        #endregion
    }

    //Called by StartRound() to fix the possibleNumbers going forward.
    private static List<int> ReDefineAndShufflePossibleNumbers(List<int> possibleNumbers, int number)
    {
        List<int> temp = new List<int>();
        foreach (int i in possibleNumbers)
        {
            if (i != number)
            {
                temp.Add(i);
            }
        }

        possibleNumbers.Clear();
        possibleNumbers = temp;

        for (int i = 0; i < possibleNumbers.Count; i++)
        {
            int tempp = possibleNumbers[i];
            int randomIndex = Random.Range(i, possibleNumbers.Count);
            possibleNumbers[i] = possibleNumbers[randomIndex];
            possibleNumbers[randomIndex] = tempp;
        }

        return possibleNumbers;
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

    //Used to prevent the user from doing anything while an animation is playing.
    private IEnumerator ToggleUserBlockPanel (float delay)
    {
        yield return new WaitForSeconds(delay);
        bool toggle = blockUserControlPanel.activeSelf;
        blockUserControlPanel.SetActive(!toggle);
    }

    //Called by the options controller if there is a change in the sound effects volume.
    public void UpdateSoundEffectsVolume ()
    {
        audioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }
}