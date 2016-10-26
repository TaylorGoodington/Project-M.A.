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
    public ProgressBar ProgressBar;

    public virtual void Start()
    {
        AudioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
        BlockUserControlPanel.SetActive(true);
    }

    public enum Clip
    {
        TutorialClip,
        DemoClip,
        RoundSuccess,
        RoundFailure
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
        AudioSource.clip = AudioClips[(int)Clip.TutorialClip];
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
        AudioSource.clip = AudioClips[(int)Clip.DemoClip];
        demoClip.Play();
        AudioSource.Play();
        Invoke("TellGameEngineDemoIsOver", demoClip.duration);
    }

    public void TellGameEngineDemoIsOver()
    {
        MoviePlayer.SetActive(false);
        GameEngine.DemoIsOver();
    }

    public void NotifyPlayer(bool answer)
    {
        BlockUserControlPanel.SetActive(true);
        ShowCorrectNumber();

        if (answer)
        {
            AudioSource.clip = AudioClips[(int)Clip.RoundSuccess];
            AudioSource.Play();
            Invoke("UpdateProgressBar", AudioSource.clip.length);
        }
        else
        {
            AudioSource.clip = AudioClips[(int)Clip.RoundFailure];
            AudioSource.Play();
            Invoke("TellGameEnginePlayerHasBeenNotified", AudioSource.clip.length);
        }
    }

    public void UpdateProgressBar ()
    {
        ProgressBar.UpdateProgressBar();
    }

    public virtual void ShowCorrectNumber ()
    {
        //Always Override.
    }

    //Called at the end of both the success and failure notification animations.
    public void TellGameEnginePlayerHasBeenNotified()
    {
        GameEngine.PlayerHasBeenNotified();
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
        TellGameEnginePlayerHasSelectedAnAnswer(true);
    }

    //Added as a listener to the button during start round.
    public void InCorrect()
    {
        TellGameEnginePlayerHasSelectedAnAnswer(false);
    }

    public void TellGameEnginePlayerHasSelectedAnAnswer (bool answer)
    {
        GameEngine.PlayerHasSelectedAnAnswer(answer);
    }

    //Used to prevent the user from doing anything while an animation is playing.
    private IEnumerator TurnOffBlockPanel (float delay)
    {
        yield return new WaitForSeconds(delay);
        BlockUserControlPanel.SetActive(false);
    }

    //Called by the options controller if there is a change in the sound effects volume.
    public void UpdateSoundEffectsVolume()
    {
        AudioSource.volume = PlayerPrefsManager.GetMasterEffectsVolume();
    }
}