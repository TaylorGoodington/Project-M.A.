using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Game11 : GameBase
{
    public Transform NumberSlotsParent;
    private List<GameObject> _numberSlots;
    private int correctSlot;
    private int correctNumber;
    private string resourcePath;

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
        foreach (var slot in _numberSlots)
        {
            slot.SetActive(true);
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        correctSlot = Random.Range(0, 3);
        correctNumber = GameEngine.CurrentStage - 1;
        resourcePath = "Numbers/Level " + GameControl.StudentLevelProgress +
                             "/Category " + int.Parse(GameEngine.CurrentGame.Substring(GameEngine.CurrentGame.Length - 1, 1)) + "/";
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
        
        #region Correct Button
        var correctButton = _numberSlots[correctSlot].GetComponent<Button>();
        var buttonSprite1 = _numberSlots[correctSlot].GetComponent<Image>();
        buttonSprite1.sprite = Resources.Load<Sprite>(resourcePath + "default" + correctNumber);
        var sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resourcePath + "highlighted" + correctNumber),
            pressedSprite = Resources.Load<Sprite>(resourcePath + "pressed" + correctNumber),
            disabledSprite = Resources.Load<Sprite>(resourcePath + "disabled" + correctNumber)
        };
        correctButton.spriteState = sprites;
        correctButton.onClick.AddListener(Correct);
        #endregion

        #region Incorrect Buttons
        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, correctNumber);
        var wrongNumber = possibleNumbers[0];
        var incorrectNumber1 = _numberSlots[wrongSlot1].GetComponent<Button>();
        var buttonSprite2 = _numberSlots[wrongSlot1].GetComponent<Image>();
        buttonSprite2.sprite = Resources.Load<Sprite>(resourcePath + "default" + wrongNumber);
        sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resourcePath + "highlighted" + wrongNumber),
            pressedSprite = Resources.Load<Sprite>(resourcePath + "pressed" + wrongNumber),
            disabledSprite = Resources.Load<Sprite>(resourcePath + "disabled" + wrongNumber)
        };
        incorrectNumber1.spriteState = sprites;
        incorrectNumber1.onClick.AddListener(InCorrect);

        possibleNumbers = ReDefineAndShufflePossibleNumbers(possibleNumbers, wrongNumber);
        wrongNumber = possibleNumbers[0];
        var incorrectNumber2 = _numberSlots[wrongSlot2].GetComponent<Button>();
        var buttonSprite3 = _numberSlots[wrongSlot2].GetComponent<Image>();
        buttonSprite3.sprite = Resources.Load<Sprite>(resourcePath + "default" + wrongNumber);
        sprites = new SpriteState
        {
            highlightedSprite = Resources.Load<Sprite>(resourcePath + "highlighted" + wrongNumber),
            pressedSprite = Resources.Load<Sprite>(resourcePath + "pressed" + wrongNumber),
            disabledSprite = Resources.Load<Sprite>(resourcePath + "disabled" + wrongNumber)
        };
        incorrectNumber2.spriteState = sprites;
        incorrectNumber2.onClick.AddListener(InCorrect);
        #endregion

        //number 2 comes from the tutorial and demo audio clips.
        var audioClip = correctNumber + 2;
        PlayAudio(audioClip);
        StartCoroutine("TurnOffBlockPanel", AudioClips[audioClip].length);
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

    public override void ShowCorrectNumber()
    {
        TurnOffButtons();

        _numberSlots[correctSlot].SetActive(true);
        var correctImage = _numberSlots[correctSlot].GetComponent<Image>();
        correctImage.sprite = Resources.Load<Sprite>(resourcePath + "highlighted" + correctNumber);
    }

    public override void TurnOffButtons()
    {
        foreach (var slot in _numberSlots)
        {
            slot.SetActive(false);
        }
    }
}