using GameSystem;
using System.Collections;
using UnityEngine;

public class AppLovingAppOpenAdManager : MonoBehaviour
{
    public static AppLovingAppOpenAdManager instance;

    [SerializeField] private string appOpenAdUnitId = "YOUR_APP_OPEN_AD_UNIT_ID";
    [SerializeField] public bool NoAds = false;

    public bool isIntersOrRwrdShowing = false;
    public bool isConsentGiven = false;
    public bool isInitialized = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        InitializeAppLovinSdk();
/*
#if UNITY_ANDROID || UNITY_IOS
        CheckForConsent();
#else
  CheckForConsent();   
//InitializeAppLovinSdk();
#endif*/
    }

    #region GDPR Consent

    private void CheckForConsent()
    {
        if (PlayerPrefs.GetInt("GDPR_Consent_Given", 0) == 1)
        {
            isConsentGiven = true;
            MaxSdk.SetHasUserConsent(true);
            InitializeAppLovinSdk();
        }
        else
        {
            ShowGDPRConsentPopup();
        }
    }

    private void ShowGDPRConsentPopup()
    {
        Debug.Log("Showing GDPR Consent Popup...");

        // Simulate user consent (replace with your own UI if needed)
        isConsentGiven = true;
        PlayerPrefs.SetInt("GDPR_Consent_Given", 1);
        //MaxSdk.SetHasUserConsent(true);

        InitializeAppLovinSdk();
    }

    #endregion

    #region Initialization

    private void InitializeAppLovinSdk()
    {
        if (NoAds) return;

        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;

            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += (adUnitId, adInfo) =>
            {
                Debug.Log("AppOpen Ad Loaded");
            };

            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += (adUnitId, errorInfo) =>
            {
                Debug.LogWarning("AppOpen Ad Failed to Load: " + errorInfo.Message);
            };

            MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        };

        MaxSdk.InitializeSdk();
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    #endregion

    #region App Lifecycle

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && !isIntersOrRwrdShowing)
        {
            ShowAdIfReady();
        }
    }

    private void OnDestroy()
    {
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
    }

    #endregion

    #region Ad Logic

    public void ShowAdIfReady()
    {
        if (NoAds) return;
        if (MaxSdk.IsAppOpenAdReady(appOpenAdUnitId))
        {
            MaxSdk.ShowAppOpenAd(appOpenAdUnitId);
        }
        else
        {
            MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        }
    }

    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        if (NoAds) return;
        isInitialized = true;
        MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
    }

    public IEnumerator RewardAndIntrsComplete()
    {
        yield return new WaitForSeconds(1f);
        isIntersOrRwrdShowing = false;
    }

    public bool IsAppOpenAdReady()
    {
        return MaxSdk.IsAppOpenAdReady(appOpenAdUnitId);
    }

    public void LoadAppOpenAd()
    {
        if (NoAds) return;
        if (!MaxSdk.IsAppOpenAdReady(appOpenAdUnitId))
        {
            MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        }
        else
        {
            Debug.Log("App Open Ad is already loaded.");
        }
    }

    #endregion

    #region Tracking Ads

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Calling OnAdRevenuePaidEvent...");
        double revenue = adInfo.Revenue;
        double cmp = revenue * 1000;

        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        string networkName = adInfo.NetworkName;
        string adUnitIdentifier = adInfo.AdUnitIdentifier;
        string placement = adInfo.Placement;
        string networkPlacement = adInfo.NetworkPlacement;

        Debug.Log("AdUnitId: " + adUnitId);

        if (adUnitId == appOpenAdUnitId)
        {
            Debug.Log("Tracking App Open Ad Impression.");

            FirebaseManager.instance.TrackAdImpression(
                AdType.AppOpen,
                networkName,
                1,
                cmp,
                revenue,
                PlayerPrefsManager.GetLevel());
        }
        else
        {
            Debug.LogWarning("Unrecognized Ad Unit ID: " + adUnitId);
        }
    }

    #endregion
}
