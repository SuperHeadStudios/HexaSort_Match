using DG.Tweening;
using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    [Header("----- Sprite Swap -----"), Space(5)]
    [SerializeField] private Image soundImg;
    [SerializeField] private Image musicImg;
    [SerializeField] private Image vibrationImg;

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    [SerializeField] private RectTransform soundOnIcon;
    
    [SerializeField] private RectTransform musicOnIcon;
    
    [SerializeField] private RectTransform vibrateOnIcon;

    [SerializeField] private Transform popup;

    [Header("----- Buttons -----"), Space(5)]
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button moreAppsBtn;
    [SerializeField] private Button supportBtn;

    [SerializeField] private bool isMusicToggle = false;
    [SerializeField] private bool isSoundToggle = false;
    [SerializeField] private bool isVibrationToggle = false;

    private void Start()
    {
        isMusicToggle = PlayerPrefsManager.GetMusicState();
        isSoundToggle = PlayerPrefsManager.GetSoundState();
        isVibrationToggle = PlayerPrefsManager.GetVibrateState();

        closeBtn.onClick.AddListener(CloseSettingsPopup);
        moreAppsBtn.onClick.AddListener(MoreAppsBtnPressed);
        supportBtn.onClick.AddListener(SupportBtnPressed);

        UpdateUiOnStart();
    }

    private void UpdateUiOnStart()
    {
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

        if (isMusicToggle == true)
        {
            AudioManager.instance.ToogleMusic(true);
            musicOnIcon.DOLocalMoveX(85f, 0.1f);
            musicImg.sprite = onSprite;
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicOnIcon.DOLocalMoveX(-85f, 0.1f);
            musicImg.sprite = offSprite;
        }

        if (isMusicToggle == true)
        {
            AudioManager.instance.ToogleMusic(true);
            musicOnIcon.DOLocalMoveX(85f, 0.1f);
            musicImg.sprite = onSprite;
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
            musicOnIcon.DOLocalMoveX(-85f, 0.1f);
            musicImg.sprite = offSprite;
        }

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

    private void OnEnable()
    {
        popup.localScale = Vector3.zero;
        popup.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
    }

    private void CloseSettingsPopup()
    {
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
        if (isMusicToggle == true)
        {
            AudioManager.instance.ToogleMusic(true);
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
