using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppOpenAdController : MonoBehaviour
{
    public static AppOpenAdController instance;

    [SerializeField] private bool isTestMode = false;
    [SerializeField] private string appOpenAndroidId;
    [SerializeField] private bool NoAds = false; // New variable to control ads

    public bool isLoadHome = false;

    private AppOpenAd appOpenAd;

    private bool isSeconPlay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Check if ads are disabled
            if (NoAds) 
            {
                isLoadHome = true;
                return; 
            }

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                RequestAppOpen();
            });
        }
        else
        {
            Destroy(gameObject);
        }

        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    public bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null;
        }
    }

    private void OnAppStateChanged(AppState state)
    {
        if (NoAds) return; // Do not show ads if NoAds is true

        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            Debug.Log("App State changed to : " + state);
            StartCoroutine(ShowAppOpenAdWithDelay());
        }
    }

    private IEnumerator ShowAppOpenAdWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        if (SceneManager.GetActiveScene().name == "Game")
        {
            ShowAppOpenAd();
        }
    }

    private void RequestAppOpen()
    {
        if (NoAds) return; // Do not request ads if NoAds is true

        string adUnitId;
#if UNITY_ANDROID
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/9257395921";
        }
        else
        {
            adUnitId = appOpenAndroidId;
        }
#elif UNITY_IPHONE
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/5575463023";
        }
        else
        {
            adUnitId = appOpenAndroidId;
        }
#else
        adUnitId = "unused";
#endif

        LoadAppOpenAd(adUnitId);
    }

    public void LoadAppOpenAd(string _adUnitId)
    {
        if (NoAds) return; // Do not load ads if NoAds is true

        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(_adUnitId, adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                appOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    public void ShowAppOpenAd()
    {
        if (NoAds) return; // Do not show ads if NoAds is true

        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
            FirebaseManager.instance.TrackTotalAds(AdType.AppOpen);
        }
        else
        {
            isLoadHome = true;
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        if (NoAds) return; // Do not register event handlers if NoAds is true

        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            RequestAppOpen();
            isLoadHome = true;
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            isLoadHome = true;
            RequestAppOpen();
        };
    }
}
