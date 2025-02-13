using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    [Header("----- 1 Hours Reward -----"), Space(5)]
    [SerializeField] private Button hourRewardBtn;
    [SerializeField] private TextMeshProUGUI hourlyBtnText;
    [SerializeField] private Sprite btnEnableSprite;
    [SerializeField] private Sprite btnDisbaleSprite;
    [SerializeField] private FillBarManager FillBarManager;
    [SerializeField] private CanvasGroup canvasGroup;
 
    private DateTime hourlyReward;
    private bool hourlyTimerActive = false;

    [Header("----- Lives Reward -----"), Space(5)]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI heartTextInf;
    [SerializeField] private Transform heartImg;
    [SerializeField] private Transform heartInPanel;
    [SerializeField] private ParticleSystem heartPs;
    [SerializeField] private ParticleSystem heartCollectPs;

    private DateTime livesRewardTim;
    private bool livesTimerActive = false;

    [Header("----- Daily Reward -----"), Space(5)]
    [SerializeField] private TextMeshProUGUI timerText; 
    private DateTime nextRewardTime;
    private bool timerActive = false;

    [Header("----- 15X Reward -----"), Space(5)]
    [SerializeField] private Button fifteenXBtn;
    [SerializeField] private GameObject fifteenTextObj;
    [SerializeField] private GameObject fifteenLockIcon;
    [SerializeField] private GameObject fifteenAdIcon;

    [Header("----- 25X Reward -----"), Space(5)]
    [SerializeField] private Button twentyFiveXBtn;
    [SerializeField] private GameObject twentyFiveTextObj;
    [SerializeField] private GameObject twentyFiveLockIcon;
    [SerializeField] private GameObject twentyFiveAdIcon;

    [Header("----- 35X Reward -----"), Space(5)]
    [SerializeField] private Button thirtyFiveXBtn;
    [SerializeField] private GameObject thiryFiveTextObj;
    [SerializeField] private GameObject thirtyFiveLockIcon;
    [SerializeField] private GameObject thirtyFiveAdIcon;

    [Header("----- Lives Reward -----"), Space(5)]
    [SerializeField] private Button livesBtn;
    [SerializeField] private GameObject livesTextObj;
    [SerializeField] private GameObject livesLockIcon;
    [SerializeField] private GameObject livesAdIcon;

    [Header("----- Coin Reward Animation -----"), Space(5)]
    [SerializeField] private CanvasGroup coinPanelCG;
    [SerializeField] private Transform demoCoin;
    [SerializeField] private GameObject coinPrfb;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private ParticleSystem sparkelsPs;

    [Space(5)]
    [SerializeField] private Transform coinParent;
    [SerializeField] private ParticleSystem coinCollectPs;

    private const string RewardTimeKey = "NextRewardTime";
    private const string ONEHOURKEY = "ONEHOURREWARDKEY";
    private const string LIVETIMEKEY = "LIVESTIME";
    private const string REWARDINDEX = "REWARDINDEXKEY";

    #region Initialization

    void Start()
    {
        coinPanelCG.alpha = 0;

        CheckLastRewardTime();
        CheckLastHourlyRewardTime();
        CheckLivesRewardTime();
        ButtonsInitialization();
        UpdateCoinRewaardOnStart();
        UpdateLivesTimerDisplay();
    }

    void Update()
    {
        if (timerActive)
        {
            UpdateTimerDisplay();
        }

        if (hourlyTimerActive)
        {
            UpdateHourlyTimerDisplay();
        }

        if (livesTimerActive)
        {
            UpdateLivesTimerDisplay();
        }
    }

    public void ButtonsInitialization()
    {
        hourRewardBtn.onClick.AddListener(ClaimHourlyReward);
        fifteenXBtn.onClick.AddListener(ClaimAdsRewards);
        twentyFiveXBtn.onClick.AddListener(ClaimAdsRewards);
        livesBtn.onClick.AddListener(ClaimAdsRewards);
        thirtyFiveXBtn.onClick.AddListener(ClaimAdsRewards);
    }

    #endregion

    #region Coin Ads Rewards

    private void UpdateCoinRewaardOnStart()
    {
        int rewardIndex = GetRewardIndex();

        if (rewardIndex == 0)
        {
            fifteenXBtn.gameObject.SetActive(true);
            fifteenXBtn.interactable = true;
            fifteenXBtn.image.sprite = btnEnableSprite;
            fifteenTextObj.SetActive(false);
            fifteenLockIcon.SetActive(false);
            fifteenAdIcon.SetActive(true);
        }
        else
        {
            fifteenXBtn.interactable = false;
            fifteenXBtn.gameObject.SetActive(false);
            fifteenLockIcon.SetActive(false);
            fifteenAdIcon.SetActive(false);
            fifteenTextObj.SetActive(true);
        }

        if (rewardIndex == 1)
        {
            twentyFiveXBtn.gameObject.SetActive(true);
            twentyFiveXBtn.interactable = true;
            twentyFiveXBtn.image.sprite = btnEnableSprite;
            twentyFiveTextObj.SetActive(false);
            twentyFiveLockIcon.SetActive(false);
            twentyFiveAdIcon.SetActive(true);
        }
        else if (rewardIndex < 1)
        {
            twentyFiveXBtn.gameObject.SetActive(true);
            twentyFiveXBtn.interactable = false;
            twentyFiveXBtn.image.sprite = btnDisbaleSprite;
            twentyFiveTextObj.SetActive(false);
            twentyFiveLockIcon.SetActive(true);
            twentyFiveAdIcon.SetActive(false);
        }
        else
        {
            twentyFiveXBtn.interactable = false;
            twentyFiveXBtn.gameObject.SetActive(false);
            twentyFiveLockIcon.SetActive(false);
            twentyFiveAdIcon.SetActive(false);
            twentyFiveTextObj.SetActive(true);
        } 
        
        
        
        if (rewardIndex == 2)
        {
            livesBtn.gameObject.SetActive(true);
            livesBtn.interactable = true;
            livesBtn.image.sprite = btnEnableSprite;
            livesTextObj.SetActive(false);
            livesLockIcon.SetActive(false);
            livesAdIcon.SetActive(true);
        }
        else if (rewardIndex < 2)
        {
            livesBtn.gameObject.SetActive(true);
            livesBtn.interactable = false;
            livesBtn.image.sprite = btnDisbaleSprite;
            livesTextObj.SetActive(false);
            livesLockIcon.SetActive(true);
            livesAdIcon.SetActive(false);
        }
        else
        {
            livesBtn.interactable = false;
            livesBtn.gameObject.SetActive(false);
            livesTextObj.SetActive(true);
            livesLockIcon.SetActive(false);
            livesAdIcon.SetActive(false);
        }


        if (rewardIndex == 3)
        {
            thirtyFiveXBtn.gameObject.SetActive(true);
            thirtyFiveXBtn.interactable = true;
            thirtyFiveXBtn.image.sprite = btnEnableSprite;
            thiryFiveTextObj.SetActive(false);
            thirtyFiveLockIcon.SetActive(false);
            thirtyFiveAdIcon.SetActive(true);
        }
        else if(rewardIndex < 3)
        {
            thirtyFiveXBtn.gameObject.SetActive(true);
            thirtyFiveXBtn.interactable = false;
            thirtyFiveXBtn.image.sprite = btnDisbaleSprite;
            thiryFiveTextObj.SetActive(false);
            thirtyFiveLockIcon.SetActive(true);
            thirtyFiveAdIcon.SetActive(false);
        }
        else
        {
            thirtyFiveXBtn.interactable = false;
            thirtyFiveXBtn.gameObject.SetActive(false);
            thiryFiveTextObj.SetActive(true);
            thirtyFiveLockIcon.SetActive(false);
            thirtyFiveAdIcon.SetActive(false);

        }
    }

    public void ClaimAdsRewards()
    {
        Transform adBtn = null;
        int index = GetRewardIndex();
        switch (index)
        {
            case 0:
                adBtn = thirtyFiveXBtn.transform;
                break;
            case 1:
                adBtn = twentyFiveXBtn.transform;
                break;
            case 2:
                adBtn = livesBtn.transform;
                break;
            case 3:
                adBtn = twentyFiveXBtn.transform;
                break;

        }

        AdmobManager.instance.ShowRewardedAd(() =>
        {
            StartCoroutine(MakeReward());
            canvasGroup.blocksRaycasts = false;
        });
    }

    private IEnumerator MakeReward()
    {
        yield return new WaitForSeconds(1);
        int index = GetRewardIndex();

        switch (index)
        {
            case 0:
                MakinCoinReward(15);
                ClaimReward();
                break;
            case 1:
                MakinCoinReward(25);
                break;
            case 2:
                LiveHeartAnimation();
                break;
            case 3:
                MakinCoinReward(35);
                break;
        }

        FillBarManager.FourthTikc();
        index++;
        SetRewardIndex(index);
        UpdateCoinRewaardOnStart();
        FillBarManager.IncreaseProgress();
    }


    private void LiveHeartAnimation()
    {
        coinPanelCG.alpha = 1;
        heartImg.localScale = Vector3.zero;
        heartImg.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            heartPs.Play();

            heartImg.DOScale(Vector3.one / 2, 0.5f).SetEase(Ease.OutSine);

            heartImg.DOMove(heartInPanel.position, .5f).SetDelay(0.2f).OnComplete(() =>
            {
                heartCollectPs.Play();
                heartImg.position = Vector3.zero;
                heartImg.localScale = Vector3.zero;

                LivesManager.instance.GiveInifinite(15);
                ClaimLivesReward();
                coinPanelCG.alpha = 0;
                canvasGroup.blocksRaycasts = true;
            });
        });
    }

    #endregion

    #region Lives Timer

    private void CheckLivesRewardTime()
    {
        if (PlayerPrefs.HasKey(LIVETIMEKEY))
        {
            string savedTimeString = PlayerPrefs.GetString(LIVETIMEKEY);

            if (DateTime.TryParse(savedTimeString, out DateTime parsedTime))
            {
                livesRewardTim = parsedTime;

                if (DateTime.Now >= livesRewardTim)
                {
                    SetLivesRewardUnavailable();
                }
                else
                {
                    SetRewardLivesAvailable();
                }
            }
            else
            {
                Debug.Log("Invalid saved DateTime format. Resetting timer.");
                ResetHourlyRewardTimer();
            }
        }
        else
        {
            SetLivesRewardUnavailable();
        }
    }

    private void SetRewardLivesAvailable()
    {
        livesTimerActive = false;
    }

    private void SetLivesRewardUnavailable()
    {
        livesTimerActive = true;
    }

    private void ResetLivesRewardTimer()
    {
        livesRewardTim = DateTime.Now.AddMinutes(15);

        PlayerPrefs.SetString(LIVETIMEKEY, livesRewardTim.ToString("o"));
        PlayerPrefs.Save();

        livesTimerActive = true;
    }

    private void UpdateLivesTimerDisplay()
    {
        TimeSpan remainingTime = livesRewardTim - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            heartTextInf.gameObject.SetActive(true);
            livesTimerActive = true;
            livesText.text = remainingTime.ToString(@"mm\:ss");
            heartTextInf.text = "∞";
        }
        else
        {
            livesText.text = LivesManager.instance.lives.ToString();
            livesTimerActive = false;
            heartTextInf.text = "";
        }
    }

    private void ClaimLivesReward()
    {
        ResetLivesRewardTimer();
    }

    #endregion

    #region 1 Hours Reward

    private void CheckLastHourlyRewardTime()
    {
        if (PlayerPrefs.HasKey(ONEHOURKEY))
        {
            string savedTimeString = PlayerPrefs.GetString(ONEHOURKEY);

            if (DateTime.TryParse(savedTimeString, out DateTime parsedTime))
            {
                hourlyReward = parsedTime;

                if (DateTime.Now >= hourlyReward)
                {
                    SetRewardAvailable();
                }
                else
                {
                    SetRewardUnavailable();
                }
            }
            else
            {
                Debug.Log("Invalid saved DateTime format. Resetting timer.");
                ResetHourlyRewardTimer();
            }
        }
        else
        {
            SetRewardAvailable();
        }
    }
        
    private void SetRewardAvailable()
    {
        hourRewardBtn.interactable = true;
        hourRewardBtn.image.sprite = btnEnableSprite;
        hourlyBtnText.text = "COLLECT";
        hourlyTimerActive = false;
    }

    private void SetRewardUnavailable()
    {
        hourRewardBtn.interactable = false;
        hourRewardBtn.image.sprite = btnDisbaleSprite;
        hourlyTimerActive = true;
    }

    private void ResetHourlyRewardTimer()
    {
        hourlyReward = DateTime.Now.AddHours(1);

        PlayerPrefs.SetString(ONEHOURKEY, hourlyReward.ToString("o")); 
        PlayerPrefs.Save();

        hourlyTimerActive = true;
    }

    private void UpdateHourlyTimerDisplay()
    {
        TimeSpan remainingTime = hourlyReward - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            hourRewardBtn.interactable = false;
            hourRewardBtn.image.sprite = btnDisbaleSprite;

            hourlyBtnText.text = remainingTime.ToString(@"mm\:ss");
        }
        else
        {
            hourRewardBtn.interactable = true;
            hourRewardBtn.image.sprite = btnEnableSprite;
            hourlyBtnText.text = "COLLECT";

            hourlyTimerActive = false;
        }
    }

    private void ClaimHourlyReward()
    {
        hourlyReward.AddHours(1);
        ResetHourlyRewardTimer();
        MakinCoinReward(5);
        canvasGroup.blocksRaycasts = false;
    }


    #endregion

    #region Daily Timer Setup

    public void ClaimReward()
    {
        if (IsRewardAvailable())
        {
            Debug.Log("Reward Claimed!");
            // Set the timer to start from this moment
            nextRewardTime = DateTime.Now.AddHours(24);
            PlayerPrefs.SetString(RewardTimeKey, nextRewardTime.ToString());
            PlayerPrefs.Save();
            timerActive = true; // Ensure the timer is active
        }
        else
        {
            Debug.Log("Reward not yet available.");
        }
    }

    private void CheckLastRewardTime()
    {
        if (PlayerPrefs.HasKey(RewardTimeKey))
        {
            string savedTimeString = PlayerPrefs.GetString(RewardTimeKey);
            nextRewardTime = DateTime.Parse(savedTimeString);

            if (DateTime.Now >= nextRewardTime)
            {
                timerActive = false;
                timerText.text = "Reward Available!";
            }
            else
            {
                timerActive = true;
            }
        }
        else
        {
            timerActive = false;
            timerText.text = "Reward Available!";
        }
    }

    private bool IsRewardAvailable()
    {
        return DateTime.Now >= nextRewardTime;
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan remainingTime = nextRewardTime - DateTime.Now;

        if (remainingTime.TotalSeconds > 0)
        {
            timerText.text = $"Reset In: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
        else
        {
            timerText.text = "Reward Available!";
            timerActive = false;
            SetRewardIndex(0);
            FillBarManager.ResetFillbar();
            CheckLastRewardTime();
            CheckLastHourlyRewardTime();
            CheckLivesRewardTime();
            UpdateCoinRewaardOnStart();
        }
    }

    #endregion

    #region Coin Reward & Text

    private void MakinCoinReward(int coinCount)
    {
        coinPanelCG.alpha = 1;
        demoCoin.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            sparkelsPs.Play();
            StartCoroutine(SpwanCoins(coinCount));

            Spwantext(coinCount);
        });
    }

    private void Spwantext(int coinCount)
    {
        GameObject textPr = Instantiate(textPrefab, demoCoin.transform.position + new Vector3(0f, 2f,0f), Camera.main.transform.rotation, transform);
        textPr.GetComponent<TextMeshProUGUI>().text = "X" + coinCount;
        textPr.transform.localScale = Vector3.zero;
        textPr.transform.DOScale(Vector3.one * 3, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            textPr.transform.DOLocalMoveY(transform.position.y + 250f, 1f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Destroy(textPr, 0.5f);
            });
        });
    }

    private IEnumerator SpwanCoins(int coinCount)
    {
        Vector3 startPosition = demoCoin.transform.position;
        Vector3 targetPosition = coinParent.transform.position;

        Vector3 midPoint = new Vector3(
            (startPosition.x + targetPosition.x) / 2,
            startPosition.y - 2f,                    
            (startPosition.z + targetPosition.z) / 2 
        );

        Vector3[] path = new Vector3[]
        {    
            startPosition, midPoint, targetPosition  
        };
        
        while (coinCount > 0)
        {
            AudioManager.instance.coinCollectSound.Play();
            GameObject coinP = Instantiate(coinPrfb, demoCoin.position, Camera.main.transform.rotation, coinParent);

            coinP.transform.DOPath(path, 0.8f, PathType.CatmullRom).SetEase(Ease.OutSine).OnComplete(() =>
            {
                GameManager.instance.AddCoin(1);

                coinP.SetActive(false);
                Destroy(coinP, 1f);
                coinCollectPs.Play();
            });

            coinCount--;
            yield return new WaitForSeconds(0.025f);
        }

        

        if(coinCount == 0)
        {
            demoCoin.localScale = Vector3.zero;
            yield return new WaitForSeconds(2f);
            coinCollectPs.Stop();
            coinPanelCG.alpha = 0;
            canvasGroup.blocksRaycasts = true;
        }
    }


    #endregion

    #region Data Get Set

    public void SetRewardIndex(int index)
    {
        PlayerPrefs.SetInt(REWARDINDEX, index);
    }

    public int GetRewardIndex()
    {
        return PlayerPrefs.GetInt(REWARDINDEX);
    }

    #endregion
}

