using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GamesPanel : MonoBehaviour
{
    public Button game1Button;
    public Button game2Button;
    public Button game3Button;

    public List<string> gamesNames;

	void Start ()
    {
        RefreshGamesPanel();
    }

    public void RefreshGamesPanel()
    {
        var gameLevel = GameControl.StudentLevelProgress;

        var game1Id = gameLevel * 10 + 1;
        game1Button.image.sprite = Resources.Load<Sprite>("Game Icons/" + game1Id);
        game1Button.transform.GetChild(0).GetComponent<Text>().text = gamesNames[game1Id];
        game1Button.transform.GetChild(1).GetComponent<Text>().text = GameControl.Game1Progress.ToString();
        game1Button.onClick.RemoveAllListeners();
        game1Button.onClick.AddListener(() => LoadLevel(game1Id.ToString()));

        var game2Id = game1Id + 1;
        game2Button.image.sprite = Resources.Load<Sprite>("Game Icons/" + game2Id);
        game2Button.transform.GetChild(0).GetComponent<Text>().text = gamesNames[game2Id];
        game2Button.transform.GetChild(1).GetComponent<Text>().text = GameControl.Game2Progress.ToString();
        game2Button.onClick.RemoveAllListeners();
        game2Button.onClick.AddListener(() => LoadLevel(game2Id.ToString()));

        var game3Id = game2Id + 1;
        game3Button.image.sprite = Resources.Load<Sprite>("Game Icons/" + game3Id);
        game3Button.transform.GetChild(0).GetComponent<Text>().text = gamesNames[game3Id];
        game3Button.transform.GetChild(1).GetComponent<Text>().text = GameControl.Game3Progress.ToString();
        game3Button.onClick.RemoveAllListeners();
        game3Button.onClick.AddListener(() => LoadLevel(game3Id.ToString()));
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}