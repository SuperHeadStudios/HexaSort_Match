using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;
using System.Collections;

public class DailyLoginRewardManager : MonoBehaviour
{
    public static DailyLoginRewardManager instance;

    private const string LastClaimDateKey = "LastClaimDate";
    private const string CurrentDayKey = "CurrentDay";

    [Header("----- Reward Buttons -----"), Space(5)]
    [SerializeField] private RewardButton[] rewardButtons;

    [Header("----- Reward Cycle Settings -----"), Space(5)]
    [SerializeField] private int totalDays = 7;

    [Header("----- Global Claim Button -----"), Space(5)]
    [SerializeField] private Button globalClaimButton;

    [Header("----- Coin Reward Animation -----"), Space(5)]
    [SerializeField] private CanvasGroup coinPanelCG;
    [SerializeField] private Transform demoCoin;
    [SerializeField] private GameObject coinPrfb;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private ParticleSystem sparkelsPs;

    [Space(5)]
    [SerializeField] private Transform coinParent;
    [SerializeField] private ParticleSystem coinCollectPs;

    [Header("----- Lives Reward -----"), Space(5)]
    [SerializeField] private Transform hammerIcon;
    [SerializeField] private Transform swapIcon;
    [SerializeField] private Transform shuffleIcon;

    [SerializeField] private ParticleSystem hammerPs;
    [SerializeField] private ParticleSystem swapPs;
    [SerializeField] private ParticleSystem shufflePs;

    [SerializeField] private Transform playBtn;
    [SerializeField] private ParticleSystem boosterCollectPs;

    private int currentDay;
    private DateTime lastClaimDate;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadProgress();
        UpdateUI();
        globalClaimButton.onClick.AddListener(ClaimCurrentDayRewards);
    }

    private void LoadProgress()
    {
        currentDay = PlayerPrefs.GetInt(CurrentDayKey);

        if (long.TryParse(PlayerPrefs.GetString(LastClaimDateKey, DateTime.MinValue.Ticks.ToString()), out long lastClaimTicks))
        {
            lastClaimDate = new DateTime(lastClaimTicks);
        }
        else
        {
            lastClaimDate = DateTime.MinValue;
        }

        CheckDay();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(CurrentDayKey, currentDay);
        PlayerPrefs.SetString(LastClaimDateKey, DateTime.Now.Ticks.ToString());
        PlayerPrefs.Save();
    }

    private void CheckDay()
    {
        if (DateTime.Now.Date > lastClaimDate.Date)
        {
            globalClaimButton.interactable = true;
        }
        else
        {
            globalClaimButton.interactable = false;
        }
    }

    public void ClaimCurrentDayRewards()
    {
        if (currentDay <= totalDays)
        {
            ClaimReward(currentDay);
        }
    }

    public void ClaimReward(int day)
    {
        rewardButtons[day].MarkAsCollected();

        if (day == totalDays)
        {
            GrantReward(day, RewardButton.REWARD_TYPE.ALL, 100);
        }
        else
        {
            GrantReward(day, rewardButtons[day].currentRewardType, rewardButtons[day].rewardValue);
        }

        globalClaimButton.interactable = false;
        currentDay++;
        SaveProgress();
        UpdateUI();

    }

    private void GrantReward(int day, RewardButton.REWARD_TYPE rewardType, int value)
    {
        demoCoin.gameObject.SetActive(false);
        hammerIcon.gameObject.SetActive(false);
        swapIcon.gameObject.SetActive(false);
        shuffleIcon.gameObject.SetActive(false);
        
        switch (rewardType)
        {
            case RewardButton.REWARD_TYPE.GOLD:
                demoCoin.gameObject.SetActive(true);
                
                MakinCoinReward(value);
                break;
            case RewardButton.REWARD_TYPE.HAMMER:
                hammerIcon.gameObject.SetActive(true);
                BoosterAnimation(hammerIcon);
                GameManager.instance.AddHammerBooster(value);
                hammerPs.Play(); AudioManager.instance.boosterUnlockSound.Play();
                break;

            case RewardButton.REWARD_TYPE.MOVE:
                swapIcon.gameObject.SetActive(true);
                BoosterAnimation(swapIcon);
                GameManager.instance.AddMoveBooster(value);
                swapPs.Play(); AudioManager.instance.boosterUnlockSound.Play();
                break;

            case RewardButton.REWARD_TYPE.SHUFFLE:
                shuffleIcon.gameObject.SetActive(true);
                BoosterAnimation(shuffleIcon);
                GameManager.instance.AddShuffleBooster(value);
                shufflePs.Play(); AudioManager.instance.boosterUnlockSound.Play();
                break;

            case RewardButton.REWARD_TYPE.ALL:
                AudioManager.instance.boosterUnlockSound.Play();
                demoCoin.gameObject.SetActive(true);
                hammerIcon.gameObject.SetActive(true);
                swapIcon.gameObject.SetActive(true);
                shuffleIcon.gameObject.SetActive(true);

                MakinCoinReward(value);
                BoosterAnimation(hammerIcon);
                BoosterAnimation(swapIcon);
                BoosterAnimation(shuffleIcon);

                GameManager.instance.AddHammerBooster(1);
                GameManager.instance.AddMoveBooster(1);
                GameManager.instance.AddShuffleBooster(1);
                break;
        }
        MarkRewardAsClaimed(day);
    }

    private void MarkRewardAsClaimed(int day)
    {
        PlayerPrefs.SetInt($"Day{day}Claimed", 1);
        PlayerPrefs.Save();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < totalDays; i++)
        {
            if (i < currentDay)
            {
                rewardButtons[i].SetDoneState();
            }
            else if (i == currentDay)
            {
                rewardButtons[i].SetActiveState();
            }
            else
            {
                rewardButtons[i].SetUpcomingState();
            }
        }
    }


    private void BoosterAnimation(Transform item)
    {
        GetComponent<PopupDaily>().HideView();
        coinPanelCG.alpha = 1;
        item.localScale = Vector3.zero;
        item.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            item.DOScale(Vector3.one / 2, 0.5f).SetEase(Ease.OutSine);

            item.DOMove(playBtn.position, .5f).SetDelay(0.2f).OnComplete(() =>
            {
                boosterCollectPs.Play();
                item.position = Vector3.zero;
                item.localScale = Vector3.zero;
                coinPanelCG.alpha = 0;
            });
        });
    }


    #region Coin Reward & Text

    private void MakinCoinReward(int coinCount)
    {
        demoCoin.gameObject.SetActive(true);
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
        GameObject textPr = Instantiate(textPrefab, demoCoin.transform.position + new Vector3(0f, 2f, 0f), Camera.main.transform.rotation, demoCoin.parent.parent);
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

    private IEnumerator SpwanCoins(int currentCoint)
    {
        int coinCount = UnityEngine.Random.Range(10, 20);

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


        coinCollectPs.Play();

        while (coinCount > 0)
        {
            AudioManager.instance.coinCollectSound.Play();
            GameObject coinP = Instantiate(coinPrfb, demoCoin.position, Camera.main.transform.rotation, coinParent);

            coinP.transform.DOPath(path, 0.8f, PathType.CatmullRom).SetEase(Ease.OutSine).OnComplete(() =>
            {
                coinP.SetActive(false);
                Destroy(coinP, 1f);

                if (coinCount == 0)
                {
                    coinCollectPs.Stop();
                }
            });

          
            coinCount--;
            yield return new WaitForSeconds(0.025f);
        }


        if (coinCount == 0)
        {
            demoCoin.localScale = Vector3.zero;
            yield return new WaitForSeconds(1f);
            coinPanelCG.alpha = 0;
            GameManager.instance.uiManager.dailyPopup.HideView();
        }

        while (currentCoint > 0)
        {
            GameManager.instance.AddCoin(1);
            currentCoint--;
            yield return new WaitForSeconds(0.001f);
        }
    }


    #endregion
}
