using UnityEngine;

public class ProgressBarSlots : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        //animator = GetComponent<Animator>();

        //animator.Play("Enable");
    }

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