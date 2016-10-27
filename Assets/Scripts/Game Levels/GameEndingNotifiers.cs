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
	}

    public void CallTransitionOut()
    {
        SendMessageUpwards("TranisitionOut");
    }
}