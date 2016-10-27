using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProgressBarSlots : MonoBehaviour
{
    private Animator animator;

    public void TellGameEnginePlayerHasBeenNotified()
    {
        GameEngine.PlayerHasBeenNotified();
    }

    void OnEnable()
    {
        animator = GetComponent<Animator>();

        animator.Play("Enable");
    }
}