using UnityEngine;
using System.Collections.Generic;

public class ProgressBar : MonoBehaviour
{
    private List<GameObject> barSlots;

	void Start ()
    {
        barSlots = new List<GameObject>();

	    foreach (Transform child in transform)
        {
            barSlots.Add(child.gameObject);
        }
	}

    public void UpdateProgressBar ()
    {
        barSlots[GameEngine.RoundsSucceeded - 1].SetActive(true);
    }
}