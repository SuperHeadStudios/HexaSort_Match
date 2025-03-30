using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    [Header("In Testing Mode")]
    public bool isTestMode;
    public bool isAdInspector = false; 
    public bool isNoAds = false;

    [Header("Android Ads Id's")]
    [SerializeField] private string bannerAndroidAdId;
    [SerializeField] private string interstitialAndroidAdId;
    [SerializeField] private string rewardedIntrsAndroidAdId;
    [SerializeField] private string rewardedAndroidAdId;

    [Header("iOS Ads Id's")]
    [SerializeField] private string bannerIosAdId;
    [SerializeField] private string interstitialIosAdId;
    [SerializeField] private string rewardedIosAdId;


    #region AllAds

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd _rewardedAd;
    private RewardedInterstitialAd _rewardedInterstitialAd;

    #endregion

    #region InitializeArea

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initializes the ads in the app
    private void InitializeAds()
    {
        if (isAdInspector)
        {
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.Add("a5964035-277a-49d8-828e-ed68cc4cac20");

            // Apply the request configuration.
            MobileAds.SetRequestConfiguration(requestConfiguration);


        }
        MobileAds.Initialize(initStatus =>
        {// Add your test device ID to avoid real ads during testing.
          
            // Initialize the Mobile Ads SDK.
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }

            if (isAdInspector)
            {

                // Show the Ad Inspector when the app starts
                MobileAds.OpenAdInspector((error) =>
                {
                    if (error != null)
                    {
                        Debug.LogError("Failed to open Ad Inspector: " + error.GetMessage());
                    }
                    else
                    {
                        Debug.Log("Ad Inspector opened successfully.");
                    }
                });
            }

            Debug.Log("Google Mobile Ads SDK initialized.");
            LoadBannerAd();
            RequestInterstitial();
            RequestRewardedAd();
            RequestInterstitialReward();
        });
    }

    #endregion

    #region BannerAd

    public void LoadBannerAd()
    {
        if (isNoAds) return;

        if (bannerView == null)
        {
            RequestBanner();
            var adRequest = new AdRequest();
            bannerView.LoadAd(adRequest);
        }
        else
        {
            Debug.Log("Banner ad already loaded");
        }

    }

    private void RequestBanner()
    {
        string adUnitId;
#if UNITY_ANDROID
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
        }
        else
        {
            //adUnitIdNativeAd = "ca-app-pub-3940256099942544/6300978111";
            adUnitId = bannerAndroidAdId;
        }
#elif UNITY_IPHONE
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/2934735716";
        }
        else
        {
            adUnitId = bannerIosAdId;
        }
#else
        adUnitId = "unused";
#endif

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyBannerAd();
        }

        AdSize adaptiveSize =
               AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

        // Register for ad events.
        bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
        bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null; // Ensure the reference is cleared
        }
    }

    public void ShowBannerAd()
    {
        if (isNoAds) return;

        // Destroy the previous banner if it exists
        if (bannerView != null)
        {
            bannerView.Show();
        }
        else
        {
            // Request a new banner and show it
            RequestBanner();
            bannerView?.Show();
        }
        FirebaseManager.instance.TrackTotalAds(AdType.Banner);
    }

    public void HideBannerAd()
    {
        if (bannerView == null)
        {
            Debug.Log("BannerView is null! Cannot hide a non-existent banner.");
            return; // Prevent further execution
        }

        bannerView.Hide();
        Debug.Log("Banner hidden successfully.");
    }
    #region Banner callback handlers

    private void OnBannerAdLoaded()
    {
        Debug.Log("Banner view loaded an ad with response : "
                 + bannerView.GetResponseInfo());
        Debug.Log("Ad Height: {0}, width: {1}" +
                bannerView.GetHeightInPixels() +
                bannerView.GetWidthInPixels());
    }

    private void OnBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : "
                + error);
    }

    #endregion

    public void HandleOnAdClosedBanner(object sender, EventArgs args)
    {
        Debug.Log("Banner ad closed. Loading a new banner ad.");
        DestroyBannerAd();
        LoadBannerAd(); // Reload the banner ad
    }

    #endregion

    #region InterstitialAds

    private void RequestInterstitial()
    {
        string adUnitId;
#if UNITY_ANDROID
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/1033173712";
        }
        else
        {
            //adUnitIdNativeAd = "ca-app-pub-3940256099942544/1033173712";
            adUnitId = interstitialAndroidAdId;
        }
#elif UNITY_IPHONE
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/4411468910";
        }
        else
        {
            adUnitId = interstitialIosAdId;
        }
#else
        adUnitId = "unexpected_platform";
#endif

        LoadInterstitialAd(adUnitId);
    }

    public void SendAgain_RequestInterstitial()
    {
        RequestInterstitial();
    }

    public InterstitialAd Interstital_Return()
    {
        return interstitial;
    }

    public bool IsInterstitialLoaded()
    {
        return interstitial.CanShowAd();
    }

    public void LoadInterstitialAd(string adUnitId)
    {
        // Clean up the old ad before loading a new one.
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // Create the ad request and load the ad.
        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load with error: " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded.");
                interstitial = ad;
                RegisterInterstitialHandler(interstitial);
            });

    }

    public void ShowInterstitialAd()
    {
        if (isNoAds) return;

        if (interstitial != null)
        {
            HideBannerAd();
            Debug.Log("Showing interstitial ad.");
            interstitial.Show();
            FirebaseManager.instance.TrackTotalAds(AdType.Interstitial);
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterInterstitialHandler(InterstitialAd interstitialAd)
    {
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad closed.");
            RequestInterstitial(); // Request a new interstitial ad
        };
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open with error: " + error);
            RequestInterstitial(); // Request a new interstitial ad
        };
    }

    #endregion

    #region RewardInterstitial

    private void RequestInterstitialReward()
    {
        string adUnitId;
#if UNITY_ANDROID
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/5354046379";
        }
        else
        {
            adUnitId = rewardedIntrsAndroidAdId;
        }
#elif UNITY_IPHONE
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/6978759866";
        }
        else
        {
            adUnitId = interstitialIosAdId;
        }
#else
        adUnitId = "unexpected_platform";
#endif

        LoadRewardedInterstitialAd(adUnitId);
    }

    /// <summary>
    /// Loads the rewarded interstitial ad.
    /// </summary>
    public void LoadRewardedInterstitialAd(string _adUnitId)
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedInterstitialAd.Load(_adUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedInterstitialAd = ad;
            });

        RegisterEventHandlers(_rewardedInterstitialAd);
    }


    public void ShowRewardedInterstitialAd(Action action)
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                RewardAction = action;
                RewardAction.Invoke();
                FirebaseManager.instance.TrackTotalAds(AdType.Reward_Interstitial);
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);
        };
    }


    #endregion

    #region RewardAd

    private void RequestRewardedAd()
    {
        string adUnitId;
#if UNITY_ANDROID
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }
        else
        {
            //adUnitId = "ca-app-pub-3940256099942544/5224354917";
            adUnitId = rewardedAndroidAdId;
        }
#elif UNITY_IPHONE
        if (isTestMode)
        {
            adUnitId = "ca-app-pub-3940256099942544/1712485313"; 
        }
        else 
        {
            adUnitId = rewardedIosAdId;
        }
#else
        adUnitId = "unexpected_platform";
#endif

        LoadRewardedAd(adUnitId);
    }

    public void LoadRewardedAd(string adUnitId)
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // Create the ad request and load the ad.
        var adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load with error: " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded.");
                _rewardedAd = ad;
                RegisterRewardedAdHandler(_rewardedAd);
            });
    }

    Action RewardAction;

    public void ShowRewardedAd(Action action)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                RewardAction = action;
                RewardAction.Invoke();
                FirebaseManager.instance.TrackTotalAds(AdType.Reward);
                Debug.Log($"Rewarded ad rewarded the user. Type: {reward.Type}, amount: {reward.Amount}");
            });
        }
        else
        {
            ShowRewardedInterstitialAd(action);
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private void RegisterRewardedAdHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad closed.");
            RequestRewardedAd(); // Request a new rewarded ad
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open with error: " + error);
            
            //ShowRewardedInterstitialAd();
            RequestRewardedAd(); // Request a new rewarded ad
        };
    }

    #endregion


    private void OnDestroy()
    {

        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
        }

        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}
