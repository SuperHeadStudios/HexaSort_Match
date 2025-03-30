using DG.Tweening;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    [Header("----- Sprite Swap -----"), Space(5)]
    [SerializeField] private Image soundImg;
    [SerializeField] private Image musicImg;
    [SerializeField] private Image vibrationImg;
    [Space(5)]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
  
    [Header("----- Btn Animation -----"), Space(5)]
    [SerializeField] private RectTransform soundOnIcon;
    [SerializeField] private RectTransform musicOnIcon;    
    [SerializeField] private RectTransform vibrateOnIcon;
    [SerializeField] private Transform popup;

    [Header("----- Buttons -----"), Space(5)]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button moreAppsBtn;
    [SerializeField] private Button supportBtn;

    [SerializeField] private bool isMusicToggle;
    [SerializeField] private bool isSoundToggle;
    [SerializeField] private bool isVibrationToggle;

    private void Start()
    {
        closeBtn.onClick.AddListener(CloseSettingsPopup);
        moreAppsBtn.onClick.AddListener(MoreAppsBtnPressed);
        supportBtn.onClick.AddListener(SupportBtnPressed);

    }

    private void OnEnable()
    {
        isMusicToggle = PlayerPrefsManager.GetMusicState();
        isSoundToggle = PlayerPrefsManager.GetSoundState();
        isVibrationToggle = PlayerPrefsManager.GetVibrateState();

       

        popup.localScale = Vector3.zero;
        popup.DOScale(Vector3.one * 0.85f, 1f).SetEase(Ease.OutBounce);
        UpdateUiOnStart();
    }
    private void UpdateUiOnStart()
    {
        if (isSoundToggle == true)
        {
            soundOnIcon.DOLocalMoveX(85f, 0.1f);
            soundImg.sprite = onSprite;
        }
        else
        {
            soundOnIcon.DOLocalMoveX(-85f, 0.1f);
            soundImg.sprite = offSprite;
        }

        if (isMusicToggle == true)
        {
            musicOnIcon.DOLocalMoveX(85f, 0.1f);
            musicImg.sprite = onSprite;
        }
        else
        {
            musicOnIcon.DOLocalMoveX(-85f, 0.1f);
            musicImg.sprite = offSprite;   
        }

        if (isVibrationToggle == true)
        {  
            vibrateOnIcon.DOLocalMoveX(85f, 0.1f);
            vibrationImg.sprite = onSprite;
        }
        else
        {
            vibrateOnIcon.DOLocalMoveX(-85f, 0.1f);
            vibrationImg.sprite = offSprite;
        }
    }

    private void CloseSettingsPopup()
    {
        CustomBannerAdManager.instance.HideBottomBanner();
        popup.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void SoundBottonClick()
    {
        isSoundToggle = !isSoundToggle;
        if (isSoundToggle == true)
        {
            AudioManager.instance.ToogleSound(true);
            soundOnIcon.DOLocalMoveX(85f, 0.1f);
            soundImg.sprite = onSprite;
        }
        else
        {
            AudioManager.instance.ToogleSound(false);
            soundOnIcon.DOLocalMoveX(-85f, 0.1f);
            soundImg.sprite = offSprite;
        }
    }

    public void MusicBottonClick()
    {
        isMusicToggle = !isMusicToggle;
        if (isMusicToggle)
        {
            AudioManager.instance.ToogleMusic(isMusicToggle);
            musicOnIcon.DOLocalMoveX(85f, 0.1f);
            musicImg.sprite = onSprite;
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicOnIcon.DOLocalMoveX(-85f, 0.1f);
            musicImg.sprite = offSprite;
        }
    }
    public void VibrateBottonClick()
    {
        isVibrationToggle = !isVibrationToggle;
        if (isVibrationToggle == true)
        {
            AudioManager.instance.ToogleHaptic(true);
            vibrateOnIcon.DOLocalMoveX(85f, 0.1f);
            vibrationImg.sprite = onSprite;

        }
        else
        {
            AudioManager.instance.ToogleHaptic(false);
            vibrateOnIcon.DOLocalMoveX(-85f, 0.1f);
            vibrationImg.sprite = offSprite;
        }
    }


    public void MoreAppsBtnPressed()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=7754076325081229652");
    }

    public void SupportBtnPressed()
    {
        Application.OpenURL("https://www.superheadstudio.com/");
    }
}
