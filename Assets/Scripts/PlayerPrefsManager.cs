using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string MASTER_MUSIC_VOLUME_KEY = "master_volume";
    const string MASTER_EFFECTS_VOLUME_KEY = "master_effects";
    const string TEACHER_ID = "teacher_id";

    public static void SetMasterMusicVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_MUSIC_VOLUME_KEY, volume);
        }
    }

    public static float GetMasterMusicVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_MUSIC_VOLUME_KEY);
    }

    public static void SetMasterEffectsVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_EFFECTS_VOLUME_KEY, volume);
        }
    }

    public static float GetMasterEffectsVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_EFFECTS_VOLUME_KEY);
    }

    public static void SetTeacherID(int id)
    {
        PlayerPrefs.SetInt(TEACHER_ID, id);
    }

    public static int GetTeacherID()
    {
        return PlayerPrefs.GetInt(TEACHER_ID);
    }
}