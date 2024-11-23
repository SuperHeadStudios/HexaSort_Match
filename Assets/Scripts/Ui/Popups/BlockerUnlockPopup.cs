using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GameSystem;

public class BlockerUnlockPopup : MonoBehaviour
{
    [Header("----- Blocker Unlock Popup-----"), Space(5)]
    [SerializeField] private Image mainPopup;
    [SerializeField] private GameObject woodPopup;
    [SerializeField] private GameObject honeyPopup;
    [SerializeField] private GameObject grassPopup;
    [SerializeField] private GameObject icePopup;
    [SerializeField] private GameObject vinesPopup;

    public void ShowBlockerUnlockPopup()
    {
        switch (GameManager.instance.levelIndex)
        {
            case 4:
                if (!PlayerPrefsManager.GetWoodUnlocked())
                {
                   
                    mainPopup.enabled = true;
                    woodPopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
                    woodPopup.transform.localScale = Vector3.zero;
                    woodPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                    });

                    PlayerPrefsManager.SetWoodUnlocked(true);
                }
                break;
            case 7:
                if (!PlayerPrefsManager.GetIceUnlocked())
                {
                    mainPopup.enabled = true;
                    icePopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
                    icePopup.transform.localScale = Vector3.zero;
                    icePopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                    });

                    PlayerPrefsManager.SetIceUnlocked(true);
                }

                break;

            case 11:
                if (!PlayerPrefsManager.GetVinesUnlocked())
                {
                    mainPopup.enabled = true;
                    vinesPopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
                    vinesPopup.transform.localScale = Vector3.zero;
                    vinesPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                    });

                    PlayerPrefsManager.SetVinesUnlocked(true);
                }

                break;

            case 15:
                if (!PlayerPrefsManager.GetGrassUnlocked())
                {
                    mainPopup.enabled = true;
                    grassPopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
                    grassPopup.transform.localScale = Vector3.zero;
                    grassPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                    });

                    PlayerPrefsManager.SetGrassUnlocked(true);
                }
                break;

            case 18:
                if (!PlayerPrefsManager.GetHoneyUnlocked())
                {
                    mainPopup.enabled = true;
                    honeyPopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
                    honeyPopup.transform.localScale = Vector3.zero;
                    honeyPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                    });

                    PlayerPrefsManager.SetHoneyUnlocked(true);
                }
                break;

        }
    }


    public void HidePopup()
    {
        switch (GameManager.instance.levelIndex)
        {
            case 4:
                HidPopUpView(woodPopup.transform);
                break;
            case 7:
                HidPopUpView(icePopup.transform);
                break;
            case 11:
                HidPopUpView(vinesPopup.transform);
                break;
            case 15:
                HidPopUpView(grassPopup.transform);
                break;
            case 18:
                HidPopUpView(honeyPopup.transform);
                break;
        }
    }

    private void HidPopUpView(Transform popup)
    {
        popup.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainPopup.enabled = false;
            GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;
        });
    }
}
