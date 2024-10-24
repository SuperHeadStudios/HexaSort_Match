using System;
using GameSystem;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using static AdsControl;

public class DailyItemView : MonoBehaviour
{
    public enum REWARD_TYPE
    {
        GOLD,
        LIVES,
        HAMMER,
        MOVE,
        SHUFFLE
    }

    [Header("----- Reward Settings -----"), Space(5)]
    [SerializeField] private REWARD_TYPE currentRewardType;
    [SerializeField] private int rewardValue;
    [HideInInspector] public int itemIndex;

    [Header("----- Btn BG Settings -----"), Space(5)]

    [SerializeField] private Image btnBgImage;
    [SerializeField] private Sprite activeReward;
    [SerializeField] private Sprite doneReward;
    [SerializeField] private Sprite upcomingReward;

    [Header("----- Elements -----"), Space(5)]
    
    [SerializeField] private Button collectBtn;
    [SerializeField] private GameObject doneImage;
    [SerializeField] private TextMeshProUGUI rewardvalueTxt;
    [SerializeField] private TextMeshProUGUI tittleText;

    public bool isUpComing = false;

    public void Collect()
    {
        long dailyKey = (long)(DateTime.Today.Subtract(new DateTime(2019, 1, 1))).TotalSeconds;
        PlayerPrefs.SetInt("Daily" + dailyKey.ToString() + itemIndex.ToString(), 1);
        PlayerPrefsManager.SetUpcomingReward(true);
        GetReward();
        RewardClaimed();
        CheckCurrentReward();
    }

    public void CollectAds()
    {
        WatchAds();
    }

    public void InitItem()
    {
        if(currentRewardType == REWARD_TYPE.LIVES)
            rewardvalueTxt.text = "+ " + rewardValue.ToString() + " s";
        else
            rewardvalueTxt.text = rewardValue.ToString();
    }

    public void EnableItem()
    {
        tittleText.text = "CLAIM";
        btnBgImage.sprite = activeReward;
        doneImage.SetActive(false);
    }

    public void CheckCurrentReward()
    {
        isUpComing = PlayerPrefsManager.GetUpcomingReward();

        if(isUpComing)
        {
            UpcomingReward();
        }
        else
        {
            RewardClaimed();
        }
    }

    public void RewardClaimed()
    {
        btnBgImage.sprite = doneReward;
        tittleText.text = "CLAIMED";
        doneImage.SetActive(true);
    }

    public void UpcomingReward()
    {
        btnBgImage.sprite = doneReward;
        tittleText.text = "DAY " + itemIndex;
        doneImage.SetActive(false);
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

        if (currentRewardType == REWARD_TYPE.LIVES)
        {
            GameManager.instance.livesManager.GiveInifinite(10);
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
        RewardClaimed();
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
                RewardClaimed();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}
