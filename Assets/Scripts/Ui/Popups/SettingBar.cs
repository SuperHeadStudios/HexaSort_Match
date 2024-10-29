using DG.Tweening;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingBar : MonoBehaviour
{
    [Header("----- Sprite Swap -----"), Space(5)]
    [SerializeField] private Image musicImg;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [Space(5)]
    [SerializeField] private Image soundImg;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    [Space(5)]
    [SerializeField] private Image vibrationImg;
    [SerializeField] private Sprite vibrateOnSprite;
    [SerializeField] private Sprite vibrateOffSprite;

    [Header("----- Btn Animation -----"), Space(5)]
    [SerializeField] private RectTransform soundOnIcon;
    [SerializeField] private RectTransform musicOnIcon;
    [SerializeField] private RectTransform vibrateOnIcon;

    [Header("----- Buttons -----"), Space(5)]
    
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button vibrateBtn;
    [SerializeField] private Button restartbtn;
    [SerializeField] private Button homeBtn;
    


    [SerializeField] private bool isMusicToggle = false;
    [SerializeField] private bool isSoundToggle = false;
    [SerializeField] private bool isVibrationToggle = false;

    [SerializeField] private HomeView homeView;
    [SerializeField] private GameObject lifeBar;

    private void Start()
    {
        settingBtn.onClick.AddListener(UpdateUiOnStart);
        soundBtn.onClick.AddListener(SoundBottonClick);
        musicBtn.onClick.AddListener(MusicBottonClick);
        restartbtn.onClick.AddListener(UpdateUiOnStart);
        homeBtn.onClick.AddListener(UpdateUiOnStart);

        isMusicToggle = PlayerPrefsManager.GetMusicState();
        isSoundToggle = PlayerPrefsManager.GetSoundState();
        isVibrationToggle = PlayerPrefsManager.GetVibrateState();
    }

    public void ReplayBottonClick()
    {
        GameManager.instance.BackToHome();
        homeView.PlayGame();
    }
    public void HomeBottonClick()
    {
        GameManager.instance.BackToHome();
        AudioManager.instance.clickSound.Play();
        lifeBar.SetActive(true);
    }

    private void UpdateUiOnStart()
    {
        isMusicToggle = PlayerPrefsManager.GetMusicState();
        isSoundToggle = PlayerPrefsManager.GetSoundState();
        isVibrationToggle = PlayerPrefsManager.GetVibrateState();

        if (isSoundToggle == true)
        {
            soundImg.sprite = soundOnSprite;
        }
        else
        {
            soundImg.sprite = soundOffSprite;
        }

        if (isMusicToggle == true)
        {
            musicImg.sprite = musicOnSprite;
        }
        else
        {
            musicImg.sprite = musicOffSprite;
        }

        if (isVibrationToggle == true)
        {
            vibrationImg.sprite = vibrateOnSprite;
        }
        else
        {
            vibrationImg.sprite = vibrateOffSprite;
        }
    }

  
    public void SoundBottonClick()
    {
        isSoundToggle = !isSoundToggle;
        if (isSoundToggle == true)
        {
            AudioManager.instance.ToogleSound(true);
            soundImg.sprite = soundOnSprite;
        }
        else
        {
            AudioManager.instance.ToogleSound(false);
            soundImg.sprite = soundOffSprite;
        }
    }

    public void MusicBottonClick()
    {
        isMusicToggle = !isMusicToggle;
        if (isMusicToggle == true)
        {
            AudioManager.instance.ToogleMusic(true);
            musicImg.sprite = musicOnSprite;
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicImg.sprite = musicOffSprite;
        }
    }
    public void VibrateBottonClick()
    {
        isVibrationToggle = !isVibrationToggle;
        if (isVibrationToggle == true)
        {
            AudioManager.instance.ToogleHaptic(true);
            vibrationImg.sprite = vibrateOnSprite;
        }
        else
        {
            AudioManager.instance.ToogleHaptic(false);
            vibrationImg.sprite = vibrateOffSprite;
        }
    }
}
