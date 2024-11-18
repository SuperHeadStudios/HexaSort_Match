using DG.Tweening;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUnlock : MonoBehaviour
{
    [Header("----- Booster Unlock Popup-----"), Space(5)]
    [SerializeField] private RectTransform rays;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Image mainPopup;


    [SerializeField] private GameObject hammerPopup;
    [SerializeField] private GameObject swapPopup;
    [SerializeField] private GameObject shufflePopup;

    [Header("----- Hamer Lock -----"), Space(5)]
    [SerializeField] private Button hammerBtn;
    [SerializeField] private Sprite lockedHammerSprit;
    [SerializeField] private Sprite unlockHammerSprit;
    [Space(5)]  
    [SerializeField] private GameObject hammerCountIcon;
    [SerializeField] private GameObject swapCountIcon;
    [SerializeField] private GameObject shuffleCountIcon;
    [SerializeField] private GameObject hammerLockIcon;

    [Header("----- Swap Lock -----"), Space(5)]
    [SerializeField] private Button swapBtn;
    [SerializeField] private Sprite lockedSwapSprit;
    [SerializeField] private Sprite unlockSwapSprit;
    [SerializeField] private GameObject swapLockIcon;

    [Header("----- Shuffle Lock -----"), Space(5)]
    [SerializeField] private Button shuffleBtn;
    [SerializeField] private Sprite lockedshuffleSprit;
    [SerializeField] private Sprite unlockshuffleSprit;
    [SerializeField] private GameObject shuffleLockIcon;

    [Header("----- Particles -----"), Space(5)]
    [SerializeField] private ParticleSystem[] allPs;

    [SerializeField]  private bool isNotOn = false;

    private void Start()
    {
       // CheckBoosters();
    }

    public void CheckBoosters()
    {
        mainPopup.enabled = false;
        //transform.localScale = Vector3.zero;
        LockUnlockHammer();
        LockUnlockShuffle();
        LockUnlockSwap();
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            GameManager.instance.levelIndex++;
        }
    }

    private void LockUnlockHammer()
    {
        if (GameManager.instance.levelIndex >= 3)
        {
            hammerBtn.enabled = true;
            hammerBtn.image.sprite = unlockHammerSprit;
            hammerLockIcon.SetActive(false);

            if(GameManager.instance.hammerBoosterValue > 0)
            {
                hammerCountIcon.SetActive(true);
            }
            else
            {
                hammerCountIcon.SetActive(false);
            }
        }
        else
        {
            hammerCountIcon.SetActive(false);
            hammerBtn.enabled = false;
            hammerBtn.image.sprite = lockedHammerSprit;
            hammerLockIcon.SetActive(true);
        }
    }

    private void LockUnlockSwap()
    {
        if (GameManager.instance.levelIndex >= 5)
        {
            swapBtn.enabled = true;
            swapBtn.image.sprite = unlockSwapSprit;

            if (GameManager.instance.moveBoosterValue > 0)
            {
                swapCountIcon.SetActive(true);
            }
            else
            {
                swapCountIcon.SetActive(false);
            }

            swapLockIcon.SetActive(false);
        }
        else
        {
            swapCountIcon.SetActive(false);
            swapBtn.enabled = false;
            swapBtn.image.sprite = lockedSwapSprit;
            swapLockIcon.SetActive(true);
        }
    }

    private void LockUnlockShuffle()
    {
        if (GameManager.instance.levelIndex >= 9)
        {
            shuffleBtn.enabled = true;
            shuffleBtn.image.sprite = unlockshuffleSprit;

            if (GameManager.instance.shuffleBoosterValue > 0)
            {
                shuffleCountIcon.SetActive(true);
            }
            else
            {
                shuffleCountIcon.SetActive(false);
            }
            shuffleLockIcon.SetActive(false);
        }
        else
        {
            shuffleCountIcon.SetActive(false);
            shuffleBtn.enabled = false;
            shuffleBtn.image.sprite = lockedshuffleSprit;
            shuffleLockIcon.SetActive(true);
        }
    }


    private void Update()
    {
        rays.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }


    public void ShowBoosterUnlockPopup()
    {
        if (!isNotOn)
        {
            switch (GameManager.instance.levelIndex)
            {
                case 3: 
                    if (!PlayerPrefsManager.GetHammerUnlocked())
                    {
                        mainPopup.enabled = true;
                        hammerPopup.SetActive(true);

                        AudioManager.instance.boosterUnlockSound.Play();
                        AudioManager.instance.confettiBlast.Play();
                        AudioManager.instance.confettiBlast.Play();

                        hammerPopup.transform.localScale = Vector3.zero;
                        hammerPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => 
                        {
                            GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                            PlayAllPs(); }
                        );

                        PlayerPrefsManager.SetHammerUnlocked(true);
                    }
                    break;

                case 5:
                    if (!PlayerPrefsManager.GetSwapUnlocked())
                    {
                        mainPopup.enabled = true;
                        swapPopup.SetActive(true);

                        AudioManager.instance.boosterUnlockSound.Play();
                        AudioManager.instance.confettiBlast.Play();
                        AudioManager.instance.confettiBlast.Play();

                        swapPopup.transform.localScale = Vector3.zero;
                        swapPopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => 
                        {
                            GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                            PlayAllPs();
                        });

                        PlayerPrefsManager.SetSwapUnlocked(true);
                    }
                    break;

                case 9:
                    if (!PlayerPrefsManager.GetShuffleUnlocked())
                    {
                        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                        mainPopup.enabled = true;
                        shufflePopup.SetActive(true);

                        AudioManager.instance.boosterUnlockSound.Play();
                        AudioManager.instance.confettiBlast.Play();
                        AudioManager.instance.confettiBlast.Play();

                        shufflePopup.transform.localScale = Vector3.zero;
                        shufflePopup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => 
                        {
                            GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
                            PlayAllPs();
                        });

                        PlayerPrefsManager.SetShuffleUnlocked(true);
                    }
                    break;
            }
            isNotOn = true;
        }
    }

    private void PlayAllPs()
    {
        foreach (ParticleSystem ps in allPs)
        {
            ps.Play();
        }
    }


    public void HidePopup()
    {
        isNotOn = false;
        switch (GameManager.instance.levelIndex)
        {
            case 3:
                hammerPopup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    mainPopup.enabled = false;
                });
                break;

            case 5:
                swapPopup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    mainPopup.enabled = false;
                });
                break;

            case 9:
                shufflePopup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    mainPopup.enabled = false;
                });
                break;
        }

        LockUnlockHammer();
        LockUnlockShuffle();
        LockUnlockSwap();
        GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;
    }
}
