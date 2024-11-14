using DG.Tweening;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    [SerializeField] private CanvasGroup areYouSure;
    [SerializeField] private Transform areYouSurePopup;
    [SerializeField] private GameObject setBtn;


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
        vibrateBtn.onClick.AddListener(VibrateBottonClick);

        LoadData();
    }

    public void LoadData()
    {
        isMusicToggle = PlayerPrefsManager.GetMusicState();
        isSoundToggle = PlayerPrefsManager.GetSoundState();
        isVibrationToggle = PlayerPrefsManager.GetVibrateState();

        UpdateUiOnStart();
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

    public void AreYouSure()
    {
        setBtn.SetActive(false);
        areYouSure.alpha = 1;
        areYouSure.blocksRaycasts = true;
        areYouSure.interactable = true;
        areYouSurePopup.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.OutBounce);
    }

    public void HideSure()
    {
        areYouSurePopup.DOScale(Vector3.zero, 0.8f).SetEase(Ease.InBack).OnComplete(() =>
        {
            setBtn.SetActive(true);
            areYouSure.alpha = 0;
            areYouSure.blocksRaycasts = false;
            areYouSure.interactable = false;
        });
    }

    public void LifeLoseButton()
    {
        AdsControl.Instance.directPlay = false;
        GameManager.instance.livesManager.ConsumeLife();
        GameManager.instance.BackToHome();
        SceneManager.LoadScene(0);
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
        Debug.Log("SoundBtn clicking_ " + isSoundToggle);

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
        Debug.Log("MusicBtn clicking_ " + isMusicToggle);
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
        Debug.Log("V clicking_ " + isVibrationToggle);
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
