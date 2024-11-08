using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GameSystem;

public class BlockerUnlockPopup : MonoBehaviour
{
    [Header("----- Blocker Unlock Popup-----"), Space(5)]
    [SerializeField] private RectTransform bg;
    [SerializeField] private Image mainPopup;
    [SerializeField] private GameObject woodPopup;
    [SerializeField] private GameObject honeyPopup;
    [SerializeField] private GameObject grassPopup;

    public int indexLevel;


    private void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutExpo);
            mainPopup.enabled = true;
            woodPopup.SetActive(true);
            woodPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        }
        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutExpo);
            mainPopup.enabled = true;
            honeyPopup.SetActive(true);
            honeyPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        }
        if (Input.GetKeyUp(KeyCode.Keypad3))
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutExpo);
            mainPopup.enabled = true;
            grassPopup.SetActive(true);
            grassPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlayerPrefs.SetInt("CurrentLevel", indexLevel);
        }
    }

    public void ShowBlockerUnlockPopup()
    {
        switch (GameManager.instance.levelIndex)
        {
            case 8:
                if (!PlayerPrefsManager.GetWoodUnlocked())
                {
                    mainPopup.enabled = true;
                    woodPopup.SetActive(true);
                    AudioManager.instance.boosterUnlockSound.Play();
                    woodPopup.transform.localScale = Vector3.zero;
                    woodPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);

                    PlayerPrefsManager.SetWoodUnlocked(true);
                }
                break;

            case 24:
                if (!PlayerPrefsManager.GetHoneyUnlocked())
                {
                    mainPopup.enabled = true;
                    honeyPopup.SetActive(true);
                    AudioManager.instance.boosterUnlockSound.Play();
                    honeyPopup.transform.localScale = Vector3.zero;
                    honeyPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);

                    PlayerPrefsManager.SetHoneyUnlocked(true);
                }
                break;

            case 15:
                if (!PlayerPrefsManager.GetGrassUnlocked())
                {
                    mainPopup.enabled = true;
                    grassPopup.SetActive(true);
                    AudioManager.instance.boosterUnlockSound.Play();
                    grassPopup.transform.localScale = Vector3.zero;
                    grassPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);

                    PlayerPrefsManager.SetGrassUnlocked(true);
                }
                break;
        }
    }


    public void HidePopup()
    {
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            mainPopup.enabled = false;
            grassPopup.transform.DOScale(Vector3.zero, 0.5f);
            honeyPopup.transform.DOScale(Vector3.zero, 0.5f);
            woodPopup.transform.DOScale(Vector3.zero, 0.5f);
        });
    }
}
