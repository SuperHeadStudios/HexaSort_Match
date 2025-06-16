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
        //AppLovinMaxAdManager.instance.ShowInterstitialAd(action);
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
    private Coroutine thirtySecondTimerCoroutine;

    private Coroutine sixtySecondTimerCoroutine;

    public void StartTimeSixtySeconds()
    {
        if (GameManager.instance.levelIndex < 2) return;

        if (sixtySecondTimerCoroutine != null)
        {
            StopCoroutine(sixtySecondTimerCoroutine);
            sixtySecondTimerCoroutine = null;
        }

        sixtySecondTimerCoroutine = StartCoroutine(SixtySecondTimers());
    }

    private IEnumerator SixtySecondTimers()
    {
        yield return new WaitForSeconds(60f);
        isSixtySecAdReady = true;

        if (GameManager.instance.currentGameState == GameManager.GAME_STATE.PLAYING)
        {
            AppLovinMaxAdManager.instance.ShowInterstitialAd(() =>
            {
                isSixtySecAdReady = false;

                // Restart the 60s ad loop properly
                StartTimeSixtySeconds();

                // Stop & restart 30s ad timer
                if (thirtySecondTimerCoroutine != null)
                {
                    StopCoroutine(thirtySecondTimerCoroutine);
                }
                isThirtySecAdReady = false;
                thirtySecondTimerCoroutine = StartCoroutine(ThirtySecondTimers());
            });
        }
        else
        {
            isSixtySecAdReady = false;
        }
    }

    #endregion

    #region Ads WithTime 30 Sec

    private bool isThirtySecAdReady = false;

    public void StartTimeThirtySeconds()
    {
        if (thirtySecondTimerCoroutine != null)
        {
            StopCoroutine(thirtySecondTimerCoroutine);
        }

        isThirtySecAdReady = false;
        thirtySecondTimerCoroutine = StartCoroutine(ThirtySecondTimers());
    }

    private IEnumerator ThirtySecondTimers()
    {
        yield return new WaitForSeconds(30);
        isThirtySecAdReady = true;
    }

    public void ShowThirtySecIntAd(Action action)
    {
        if (isThirtySecAdReady)
        {
            AppLovinMaxAdManager.instance.ShowInterstitialAd(action);
            isThirtySecAdReady = false;
        }
        else
        {
            Debug.Log("30 Sec Ad not ready");
            action?.Invoke();
        }

        StartTimeThirtySeconds(); // restart for next opportunity
    }

    #endregion


}
