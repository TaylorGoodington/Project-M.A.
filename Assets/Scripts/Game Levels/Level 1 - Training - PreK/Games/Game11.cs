using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Game11 : GameBase
{
    public Transform NumberSlotsParent;
    private List<GameObject> _numberSlots;

	public override void Start ()
    {
        base.Start();

        _numberSlots = new List<GameObject>();
	    foreach (Transform child in NumberSlotsParent)
        {
            _numberSlots.Add(child.gameObject);
        }
    }

    //Called by the GameEngine at the begining of each round.
    public void StartRound (int currentRound)
    {
        var correctSlot = Random.Range(0, 3);
        Debug.Log(correctSlot);
        int wrongSlot1;
        int wrongSlot2;

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

        var possibleNumbers = new List<int> { 0, 1, 2, 3, 4, 5 };
        var resoucePath = "Numbers/Level " + GameControl.StudentLevelProgress + 
                             "/Category " + int.Parse(GameEngine.CurrentGame.Substring(GameEngine.CurrentGame.Length - 1, 1)) + "/";
        #region Correct Button
        var number = GameEngine.CurrentStage - 1;
        var correctNumber = _numberSlots[correctSlot].GetComponent<Button>();
        var buttonSprite1 = _numberSlots[correctSlot].GetComponent<Image>();
        buttonSprite1.sprite = Resources.Load<Sprite>(resoucePath + "default" + number);
        var sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + number),
            pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + number),
            disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + number)
        };
        correctNumber.spriteState = sprites;
        correctNumber.onClick.AddListener(Correct);
        #endregion

        #region Incorrect Buttons
        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, number);
        var wrongNumber = possibleNumbers[0];
        var incorrectNumber1 = _numberSlots[wrongSlot1].GetComponent<Button>();
        var buttonSprite2 = _numberSlots[wrongSlot1].GetComponent<Image>();
        buttonSprite2.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber),
            pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber),
            disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber)
        };
        incorrectNumber1.spriteState = sprites;
        incorrectNumber1.onClick.AddListener(InCorrect);

        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, wrongNumber);
        wrongNumber = possibleNumbers[0];
        var incorrectNumber2 = _numberSlots[wrongSlot2].GetComponent<Button>();
        var buttonSprite3 = _numberSlots[wrongSlot2].GetComponent<Image>();
        buttonSprite3.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber),
            pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber),
            disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber)
        };
        incorrectNumber2.spriteState = sprites;
        incorrectNumber2.onClick.AddListener(InCorrect);
        #endregion

        CallSwitchNumbersBackOn();

        //number 2 comes from the tutorial and demo audio clips.
        var audioClip = number + 2;
        PlayAudio(audioClip);
        StartCoroutine("ToggleUserBlockPanel", AudioClips[audioClip].length);
    }

    //Called by StartRound() to fix the possibleNumbers going forward.
    private static List<int> ReDefineAndShufflePossibleNumbers(List<int> possibleNumbers, int number)
    {
        var temp = possibleNumbers.Where(i => i != number).ToList();

        possibleNumbers.Clear();
        possibleNumbers = temp;

        for (var i = 0; i < possibleNumbers.Count; i++)
        {
            var tempp = possibleNumbers[i];
            var randomIndex = Random.Range(i, possibleNumbers.Count);
            possibleNumbers[i] = possibleNumbers[randomIndex];
            possibleNumbers[randomIndex] = tempp;
        }

        return possibleNumbers;
    }

    private void CallSwitchNumbersBackOn ()
    {
        GameAnimator.Play("activateNumbers");
    }
}