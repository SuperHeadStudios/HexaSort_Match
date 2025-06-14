using GameSystem;
using System;
using System.Collections;
using UnityEngine;

public class AdsControl : MonoBehaviour
{

    private static AdsControl instance;


    public enum ADS_TYPE
    {
        ADMOB,
        UNITY,
        MEDIATION
    }

    public ADS_TYPE currentAdsType;

    public static AdsControl Instance { get { return instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool directPlay = false;

    private void Start()
    {
        StartTimeThirtySeconds();
    }

    public void ShowInterstital(Action action)
    {
        if (IsRemoveAds())
            return;
        AppLovinMaxAdManager.instance.ShowInterstitialAd(action);
    }

    public void RemoveAds()
    {
        PlayerPrefsManager.Set_NoAds_Buy_Done(true);
        //if banner is active and user bought remove ads the banner will automatically hide

    }

    public bool IsRemoveAds()
    {
        return PlayerPrefsManager.Get_Noads_Done();
    }


    #region Ads WithTime 60 Sec

    private bool isSixtySecAdReady = false;

    public void StartTimeSixtySeconds()
    {
        if (GameManager.instance.levelIndex < 2) return;

        if(GameManager.instance.currentGameState == GameManager.GAME_STATE.PLAYING)
        {   
            StartCoroutine(SixtySecondTimers());
        }
        else
        {
            isSixtySecAdReady = false;
            StopCoroutine(SixtySecondTimers());
        }
    }

    private IEnumerator SixtySecondTimers()
    {
        yield return new WaitForSeconds(60f);
        isSixtySecAdReady = true;
        AppLovinMaxAdManager.instance.ShowInterstitialAd(() =>
        {
            isSixtySecAdReady = false;
        });
    }

    #endregion

    #region Ads WithTime 30 Sec

    private bool isThirtySecAdReady = false;

    public void StartTimeThirtySeconds()
    {
        StartCoroutine(ThirtySecondTimers());
    }

    private IEnumerator ThirtySecondTimers()
    {
        yield return new WaitForSeconds(30);
        isSixtySecAdReady = true;
    }


    public void ShowThirtySecIntAd(Action action)
    {
        if (isThirtySecAdReady)
        {
            AppLovinMaxAdManager.instance.ShowInterstitialAd(action);
            isSixtySecAdReady = false; // Reset the ad ready state after showing the ad
        }
        else
        {
            Debug.Log("30 Sec Ad not ready");
            action?.Invoke(); // Invoke the action even if the ad is not ready
        }
    }
    #endregion
}
