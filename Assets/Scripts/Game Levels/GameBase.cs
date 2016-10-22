using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class GameBase : MonoBehaviour
{
    public Animator GameAnimator;
    public GameObject BlockUserControlPanel;
    public AudioSource AudioSource;
    public List<AudioClip> AudioClips;
    public GameObject MoviePlayer;
    public Material Tutorial;
    public Material Demo;
    private bool _success;

    public virtual void Start()
    {
        AudioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
        BlockUserControlPanel.SetActive(true);
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
        MoviePlayer.SetActive(true);
        MoviePlayer.GetComponent<Image>().material = Tutorial;
        var tutorialClip = (MovieTexture)MoviePlayer.GetComponent<Image>().mainTexture;
        AudioSource.clip = AudioClips[0];
        tutorialClip.Play();
        AudioSource.Play();
        Invoke("TellGameEngineTutorialIsOver", tutorialClip.duration);
    }

    public void TellGameEngineTutorialIsOver()
    {
        MoviePlayer.SetActive(false);
        GameEngine.EndOfTutorial();
    }

    //Called by the GameEngine if the player gets a question wrong which will make them unable to clear the stage.
    public void PlayDemo()
    {
        MoviePlayer.SetActive(true);
        MoviePlayer.GetComponent<Image>().material = Demo;
        var demoClip = (MovieTexture)MoviePlayer.GetComponent<Image>().mainTexture;
        AudioSource.clip = AudioClips[1];
        demoClip.Play();
        AudioSource.Play();
        Invoke("TellGameEngineDemoIsOver", demoClip.duration);
    }

    public void TellGameEngineDemoIsOver()
    {
        MoviePlayer.SetActive(false);
        GameEngine.DemoIsOver();
    }

    public void NotifyPlayer()
    {
        BlockUserControlPanel.SetActive(true);
        if (_success)
        {
            //Also updates the progress bar.
            GameAnimator.Play("success");
        }
        else
        {
            GameAnimator.Play("failure");
        }
    }

    //Called at the end of both the success and failure notification animations.
    public void TellGameEnginePlayerHasBeenNotified()
    {
        GameEngine.WasQuestionAnsweredCorrectly(_success);
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
        AudioSource.clip = AudioClips[clip];
        AudioSource.Play();
    }
    #endregion
    
    //Added as a listener to the button during start round.
    public void Correct()
    {
        _success = true;
        NotifyPlayer();
    }

    //Added as a listener to the button during start round.
    public void InCorrect()
    {
        _success = false;
        NotifyPlayer();
    }

    //Used to prevent the user from doing anything while an animation is playing.
    private IEnumerator ToggleUserBlockPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        var toggle = BlockUserControlPanel.activeSelf;
        BlockUserControlPanel.SetActive(!toggle);
    }

    //Called by the options controller if there is a change in the sound effects volume.
    public void UpdateSoundEffectsVolume()
    {
        AudioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }
}