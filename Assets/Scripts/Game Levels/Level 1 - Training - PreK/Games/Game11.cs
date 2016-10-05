using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        int correctSlot = Random.Range(0, 3);
        int wrongSlot1;
        int wrongSlot2;
        List<int> possibleNumbers = new List<int> { 0, 1, 2, 3, 4, 5 };
        string resoucePath = "Game Icons/Numbers/Level" + GameControl.StudentLevelProgress + 
                             "/Category" + int.Parse(GameEngine.CurrentGame.Substring(GameEngine.CurrentGame.Length - 1, 1)) + "/";

        #region Correct Button
        int number = GameEngine.CurrentStage - 1;
        Button correctNumber = _numberSlots[correctSlot].GetComponent<Button>();
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
        Button incorrectNumber1 = _numberSlots[wrongSlot1].GetComponent<Button>();
        incorrectNumber1.image.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber);
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber);
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber);
        incorrectNumber1.spriteState = sprites;
        incorrectNumber1.onClick.AddListener(() => InCorrect());

        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, wrongNumber);
        wrongNumber = possibleNumbers[0];
        Button incorrectNumber2 = _numberSlots[wrongSlot2].GetComponent<Button>();
        incorrectNumber2.image.sprite = Resources.Load<Sprite>(resoucePath + "default" + wrongNumber);
        sprites = new SpriteState();
        sprites.highlightedSprite = Resources.Load<Sprite>(resoucePath + "highlighted" + wrongNumber);
        sprites.pressedSprite = Resources.Load<Sprite>(resoucePath + "pressed" + wrongNumber);
        sprites.disabledSprite = Resources.Load<Sprite>(resoucePath + "disabled" + wrongNumber);
        incorrectNumber2.spriteState = sprites;
        incorrectNumber2.onClick.AddListener(() => InCorrect());
        #endregion

        PlayAudio(number);
        StartCoroutine("ToggleUserBlockPanel", AudioClips[number].length);
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
}