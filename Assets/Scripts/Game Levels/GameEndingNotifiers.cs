using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class GameEndingNotifiers : MonoBehaviour
{
    private Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();

        animator.Play("enable");
        GameEngine.CallUpdloadInformation();
	}

    public void CallTransitionOut()
    {
        SendMessageUpwards("TranisitionOut");
    }
}