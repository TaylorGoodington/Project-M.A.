using UnityEngine;
using System.Collections.Generic;

public class GamesDatabase : MonoBehaviour
{
    public static List<Games> games;
    //The dictionary serves as a reference for accessing the index of the list.
    public static Dictionary<string, int> gamesIndex;
	void Start ()
    {
        games.Add(new Games(11, 0, "", 8, 1, 8, 1, 8, 1, 8, 1, 8, 1, 8, 1));
        gamesIndex.Add("11", 0);
	}
}