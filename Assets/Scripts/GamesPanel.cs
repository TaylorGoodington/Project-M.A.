using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GamesPanel : MonoBehaviour
{
    public Button game1;
    public Button game2;
    public Button game3;

    public List<string> gamesNames;

	void Start ()
    {
        //assign the correct games icons and update level progress.
        game1.image.sprite = Resources.Load<Sprite>("Game Icons/" + GameControl.studentLevelProgress + "1");
        game1.transform.GetChild(0).GetComponent<Text>().text = gamesNames[GameControl.studentLevelProgress * 10 + 1];
        game1.transform.GetChild(1).GetComponent<Text>().text = GameControl.game1Progress.ToString();
        game2.image.sprite = Resources.Load<Sprite>("Game Icons/" + GameControl.studentLevelProgress + "2");
        game2.transform.GetChild(0).GetComponent<Text>().text = gamesNames[GameControl.studentLevelProgress * 10 + 2];
        game2.transform.GetChild(1).GetComponent<Text>().text = GameControl.game2Progress.ToString();
        game3.image.sprite = Resources.Load<Sprite>("Game Icons/" + GameControl.studentLevelProgress + "3");
        game3.transform.GetChild(0).GetComponent<Text>().text = gamesNames[GameControl.studentLevelProgress * 10 + 3];
        game3.transform.GetChild(1).GetComponent<Text>().text = GameControl.game3Progress.ToString();

        //TODO Still need to add the listeners to the buttons so they can load the proper level.
    }
	
	void Update ()
    {
	
	}
}