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

    public void ShowBlockerUnlockPopup()
    {
        switch (GameManager.instance.levelIndex)
        {
            case 8:
                if (!PlayerPrefsManager.GetWoodUnlocked())
                {
                    mainPopup.enabled = true;
                    woodPopup.SetActive(true);
                    AudioManager.instance.trailAudio.Play();
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
                    AudioManager.instance.trailAudio.Play();
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
                    AudioManager.instance.trailAudio.Play();
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
