using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class GameBase : MonoBehaviour
{
    public Animator GameAnimator;
    public GameObject BlockUserControlPanel;
    public AudioSource Source;
    public List<AudioClip> AudioClips;
    private bool Success;

    public virtual void Start()
    {
        TranisitionIn();
        Source.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }

    #region Animation Functions
    //Called by the Start function.
    public void TranisitionIn()
    {
        GameAnimator.Play("transitionIn");
    }

    //Called at the end of the GameOver animations.
    public void TranisitionOut()
    {
        GameAnimator.Play("transitionOut");
    }

    //Called at the end of the transitionIn animation.
    public void AskGameEngineToSetUpGame()
    {
        GameEngine.SetUpGame();
    }

    //Called by the GameEngine if the game has not been played this session.
    public void PlayTutorial()
    {
        GameAnimator.Play("tutorial");
    }

    //Called by the GameEngine if the player gets a question wrong which will make them unable to clear the stage.
    public void PlayDemo()
    {
        //includes the failure animation.
        GameAnimator.Play("demo");
    }

    //Called by the GameEngine after it is notified of the results from the round.
    public void NotifyPlayer()
    {
        if (Success)
        {
            //Also updates the progress bar.
            GameAnimator.Play("success");
        }
        else
        {
            GameAnimator.Play("failure");
        }
    }

    //Called at the end of the demo and the end of updating the progress bar and the end of the demo and after a failure notification.
    public void EndOfRound()
    {
        GameEngine.EndOfRound();
    }

    //Called by the GameEngine if the final round has been completed.
    public void GameOver(bool didPlayerWin)
    {
        if (didPlayerWin)
        {
            GameAnimator.Play("victory");
        }
        else
        {
            GameAnimator.Play("defeat");
        }
    }

    //Called by the animator when needed.
    public void PlayAudio(int clip)
    {
        Source.clip = AudioClips[clip];
        Source.Play();
    }
    #endregion
    
    //Added as a listener to the button during start round.
    public void Correct()
    {
        GameEngine.WasQuestionAnsweredCorrectly(true);
        Success = true;
    }

    //Added as a listener to the button during start round.
    public void InCorrect()
    {
        GameEngine.WasQuestionAnsweredCorrectly(false);
        Success = false;
    }

    //Used to prevent the user from doing anything while an animation is playing.
    private IEnumerator ToggleUserBlockPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        bool toggle = BlockUserControlPanel.activeSelf;
        BlockUserControlPanel.SetActive(!toggle);
    }

    //Called by the options controller if there is a change in the sound effects volume.
    public void UpdateSoundEffectsVolume()
    {
        Source.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }
}