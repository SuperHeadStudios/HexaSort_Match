using GameSystem;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource winSound;

    public AudioSource trailAudio;

    public AudioSource columnJumpSfx;

    public AudioSource columnSellSfx;

    public AudioSource columnSpawnSfx;

    public AudioSource columnPlaceSfx;

    public AudioSource clickSound;

    public AudioSource hammerSound;

    public AudioSource coinCollectSound;

    public AudioSource coinFlySound;

    public AudioSource unlockCell;

    public AudioSource rewardDone;

    public AudioSource backgroundMusic;

    public AudioSource confettiBlast;

    public AudioSource boosterUnlockSound;

    public AudioSource flowerCollectedSound;

    public AudioSource spinWheel;

    public AudioSource[] soundList;

    public static AudioManager instance;

    [HideInInspector]
    public bool musicState, soundState, hapticState;

    private void Awake()
    {
        musicState = PlayerPrefsManager.GetMusicState();
        soundState = PlayerPrefsManager.GetSoundState();
        hapticState = PlayerPrefsManager.GetVibrateState();

        ToogleMusic(musicState);
        ToogleSound(soundState);
        ToogleHaptic(hapticState);

        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

       
        DontDestroyOnLoad(gameObject);
    }


    public void ToogleMusic(bool toogle)
    {
        if(toogle)
        {
            backgroundMusic.volume = 1.0f;
        }         
        else
        {
            backgroundMusic.volume = 0.0f;
        }
        PlayerPrefsManager.SetMusicState(toogle);
    }

    public void ToogleSound(bool toogle)
    {
        if (toogle)
        {

            for (int i = 0; i < soundList.Length; i++)
                soundList[i].volume = 1.0f;
            columnPlaceSfx.volume = .3f;
            trailAudio.volume = 0.2f;
            columnSellSfx.volume = .5f;
            columnSpawnSfx.volume = 0.5f;
            Debug.Log("Hit");
        }
        else
        {
            for (int i = 0; i < soundList.Length; i++)
                soundList[i].volume = 0.0f;
        }
        PlayerPrefsManager.SetSoundState(toogle);
    }
    public void ToogleHaptic(bool toogle)
    {
        PlayerPrefsManager.SetVibrateState(toogle);
    }

  
}
