using System;
using GameSystem;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using static AdsControl;
public class RewardButton : MonoBehaviour
{
    public enum REWARD_TYPE
    {
        GOLD,
        ALL,
        HAMMER,
        MOVE,
        SHUFFLE
    }

    [Header("----- Reward Settings -----"), Space(5)]
    public REWARD_TYPE currentRewardType;
    public int rewardValue;
    public int itemIndex;

    [Header("----- Btn BG Settings -----"), Space(5)]

    [SerializeField] private Image btnBgImage;
    [SerializeField] private Sprite activeReward;
    [SerializeField] private Sprite doneReward;
    [SerializeField] private Sprite upcomingReward;

    [Header("----- Elements -----"), Space(5)]
    
    [SerializeField] public Button collectBtn;
    [SerializeField] private GameObject doneImage;
    [SerializeField] private TextMeshProUGUI rewardvalueTxt;
    [SerializeField] private TextMeshProUGUI titleText;

    private void Start()
    {
        UpdateUI();
        collectBtn.onClick.AddListener(() => DailyRewardManager.instance.ClaimReward(itemIndex));
    }

    public void SetActiveState()
    {
        btnBgImage.sprite = activeReward;
        collectBtn.interactable = true;
        doneImage.SetActive(false);
        rewardvalueTxt.text = rewardValue.ToString();
    }

    public void SetDoneState()
    {
        btnBgImage.sprite = doneReward;
        collectBtn.interactable = false;
        doneImage.SetActive(true);
        rewardvalueTxt.text = rewardValue.ToString();
    }

    public void SetUpcomingState()
    {
        btnBgImage.sprite = upcomingReward;
        collectBtn.interactable = false;
        doneImage.SetActive(false);
        rewardvalueTxt.text = rewardValue.ToString();
    }

    public void MarkAsCollected()
    {
        btnBgImage.sprite = doneReward;
        doneImage.SetActive(true);
        collectBtn.interactable = false;
    }

    private void UpdateUI()
    {
        titleText.text = $"Day {itemIndex + 1}";
        rewardvalueTxt.text = rewardValue.ToString();
    }

    private void GetReward()
    {
        if (currentRewardType == REWARD_TYPE.GOLD)
        {
            GameManager.instance.AddCoin(rewardValue);
        }

        if (currentRewardType == REWARD_TYPE.HAMMER)
        {
            GameManager.instance.AddHammerBooster(rewardValue);
        }

        if (currentRewardType == REWARD_TYPE.MOVE)
        {
            GameManager.instance.AddMoveBooster(rewardValue);
        }

        if (currentRewardType == REWARD_TYPE.SHUFFLE)
        {
            GameManager.instance.AddShuffleBooster(rewardValue);
        }

        if (currentRewardType == REWARD_TYPE.ALL)
        {
            GameManager.instance.AddCoin(150); 
            GameManager.instance.AddHammerBooster(1);
            GameManager.instance.AddMoveBooster(1);
            GameManager.instance.AddShuffleBooster(1);
        }
    }

    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnReward);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnReward);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnReward(Reward reward)
    {
        AudioManager.instance.rewardDone.Play();
        long dailyKey = (long)(DateTime.Today.Subtract(new DateTime(2019, 1, 1))).TotalSeconds;
        PlayerPrefs.SetInt("Daily" + dailyKey.ToString() + itemIndex.ToString(), 1);
        GetReward();
       // RewardClaimed();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AudioManager.instance.rewardDone.Play();
                long dailyKey = (long)(DateTime.Today.Subtract(new DateTime(2019, 1, 1))).TotalSeconds;
                PlayerPrefs.SetInt("Daily" + dailyKey.ToString() + itemIndex.ToString(), 1);
                GetReward();
                //RewardClaimed();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}
