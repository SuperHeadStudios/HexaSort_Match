using DG.Tweening;
using GameSystem;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AppLovinMaxAdManager : MonoBehaviour
{
    public static AppLovinMaxAdManager instance;

    #region Serialized Fields

    [Header("Ad Unit IDs")]
    [SerializeField] private string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
    [SerializeField] private string interstitialAdUnitId = "YOUR_INTERSTITIAL_AD_UNIT_ID";
    [SerializeField] private string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";
    [SerializeField] private string mrecAdBottmUnitId = "YOUR_MREC_AD_UNIT_ID";
    [SerializeField] private string mrecAdTopUnitId = "YOUR_MREC_AD_UNIT_ID";
    [SerializeField] private string mrecCustomAdUnitId = "YOUR_MREC_AD_UNIT_ID";


    [SerializeField] private GameObject textNotiObj;

    [Header(" ---- ADLOAD PANEL ----"), Space(5)]
    [SerializeField] private GameObject AdLoadPanel;
    [SerializeField] private GameObject Round;

    public bool isIntrsClosed = false;
    private bool canShowInterstitial = true;

    public bool NoAds = false;


    #endregion

    #region Initialization

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


    private void Start()
    {
        InitializeAppLovinSdk();

        PaidEventInititalizaation();
    }

    private void OnDestroy()
    {
        PaidEventDiscard();
    }

    private void InitializeAppLovinSdk()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            Debug.Log("AppLovin SDK initialized successfully.");
            LoadAllAds();
        };

        MaxSdk.InitializeSdk();
    }

    #endregion

    #region NotiAds

    public void SpwanNotiText(Transform adBtn)
    {
        GameObject notiObj = Instantiate(textNotiObj, adBtn.position + new Vector3(0, 1, 0), Camera.main.transform.rotation, adBtn);
        notiObj.transform.localScale = Vector3.one * 1.5f;
        notiObj.GetComponent<TextMeshProUGUI>().text = "No ads available, try again later";
        notiObj.transform.DOLocalMoveY(adBtn.position.y + 400, 2).SetEase(Ease.OutSine).OnComplete(() =>
        {
            Destroy(notiObj);
        });
    }

    #endregion

    #region Load and Show Ads

    private void LoadAllAds()
    {
        InitializeBannerAds();
        InitializeInterstitialAds();
        InitializeRewardedAds();
        InitializeBottomMrecAds();
        InitializeTopMrecAds();
        InitializeCustomMrecAds();
    }

    #endregion

    #region PaidEvent

    private void PaidEventInititalizaation()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEventToFirebase;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEventToFirebase;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEventToFirebase;
/*
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += FirebaseManager.instance.OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += FirebaseManager.instance.OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += FirebaseManager.instance.OnAdRevenuePaidEvent;*/
    }

    private void PaidEventDiscard()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventToFirebase;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventToFirebase;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventToFirebase;
/*
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= FirebaseManager.instance.OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= FirebaseManager.instance.OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= FirebaseManager.instance.OnAdRevenuePaidEvent;*/
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Claling on ad eveni int ");

        double revenue = adInfo.Revenue;
        double cmp = revenue * 1000;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode;
        string networkName = adInfo.NetworkName;
        string adUnitIdentifier = adInfo.AdUnitIdentifier;
        string placement = adInfo.Placement;
        string networkPlacement = adInfo.NetworkPlacement;

        // Debugging ad unit ID
        Debug.Log("AdUnitId: " + adUnitId);


        if (adUnitId == bannerAdUnitId)
        {
            Debug.Log("Tracking Banner Ad Impression.");
            //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.Banner, adInfo);
            //FirebaseManager.instance.TrackAdImpression(AdType.Banner, AdLocation.Game, networkName, 1, cmp, revenue, PlayerPrefs.GetInt("CurrentLevel", 1));
        }
        else if (adUnitId == rewardedAdUnitId)
        {
            Debug.Log("Tracking Rewarded Ad Impression.");
            //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.RewardedVideo, adInfo);
            //FirebaseManager.instance.TrackAdImpression(AdType.Reward, reward_AdLocation, networkName, 1, cmp, revenue, PlayerPrefs.GetInt("CurrentLevel", 1));
        }
        else if (adUnitId == interstitialAdUnitId)
        {
            Debug.Log("Tracking Interstitial Ad Impression.");
            //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.Interstitial, adInfo);
            //FirebaseManager.instance.TrackAdImpression(AdType.Interstitial, inters_AdLocation, networkName, 1, cmp, revenue, PlayerPrefs.GetInt("CurrentLevel", 1));
        }
        else
        {
            Debug.LogWarning("Unrecognized Ad Unit ID: " + adUnitId);
        }
    }


    #endregion

    #region Interstitial Ad

    public void InitializeInterstitialAds()
    {

        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            return;
        }

        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {

        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            return;
        }

        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }


    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        StartCoroutine(AppLovingAppOpenAdManager.instance.RewardAndIntrsComplete());
        ShowBannerAd();
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
        isIntrsClosed = true;
        if (interCompletAction != null)
        {
            interCompletAction.Invoke();
            interCompletAction = null;
        }
    }

    private IEnumerator LoadIntersDelay()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            yield break;
        }

        while (!MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            LoadInterstitial();
            Debug.Log("Loading Interstitial Ad...");
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(ShowInterstitialAdWithLoadingCoroutine());
    }

    private Action interCompletAction;

    public void ShowInterstitialAdWithDelay(Action interAction)
    {
        interCompletAction = interAction;


        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            if (interCompletAction != null)
            {
                // If interCompletAction is not null, invoke it immediately
                interCompletAction.Invoke();
                interCompletAction = null;
            }
            return;
        }


        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId) && canShowInterstitial)
        {
            StartCoroutine(ShowInterstitialAdWithLoadingCoroutine());
        }
        else
        {
            Debug.Log("Interstitial Ad is not ready.");
            if(interCompletAction != null)
            {
                // If interCompletAction is not null, invoke it immediately
                interCompletAction.Invoke();
                interCompletAction = null;
            }
        }
    }


    public void ShowInterstitialAd(Action interAction)
    {
        interCompletAction = interAction;
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            if (interCompletAction != null)
            {
                interCompletAction.Invoke();
                interCompletAction = null;
            }
            return;
        }

        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            StartCoroutine(ShowInterstitialAdWithLoadingCoroutine());
        }
        else
        {
            if (interCompletAction != null)
            {
                // If interCompletAction is not null, invoke it immediately
                interCompletAction.Invoke();
                interCompletAction = null;
            }
            Debug.Log("Interstitial Ad is not ready.");
            StartCoroutine(LoadIntersDelay());
        }
    }

  

    public IEnumerator ShowInterstitialAdWithLoadingCoroutine()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            if (interCompletAction != null)
            {
                interCompletAction.Invoke();
                interCompletAction = null;
            }
            yield break;
        }

        AdLoadPanel.SetActive(true);
        Round.transform.DORotate(new Vector3(0, 0, 360), 4f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);

        yield return new WaitForSeconds(1f);

        AdLoadPanel.SetActive(false);

        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitId);
            canShowInterstitial = false;
            StartCoroutine(IntTimerStart());
        }

        Debug.Log("Intrs Showing");
        AppLovingAppOpenAdManager.instance.isIntersOrRwrdShowing = true;
        HideBannerAd();
    }

    public IEnumerator IntTimerStart()
    {
        yield return new WaitForSeconds(30f);
        canShowInterstitial = true;
    }

    #endregion

    #region Rewarded Ad

    int retryAttempt;

    Action onRewardedAdAction;

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        StartCoroutine(AppLovingAppOpenAdManager.instance.RewardAndIntrsComplete());
        ShowBannerAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if (onRewardedAdAction != null)
        {
            onRewardedAdAction.Invoke();
            onRewardedAdAction = null;
        }
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }


    public void ShowRewardedAd(Action reward)
    {
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {
            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
            onRewardedAdAction = reward;
            AppLovingAppOpenAdManager.instance.isIntersOrRwrdShowing = true;
            HideBannerAd();
        }
        else
        {
            Debug.Log("Rewarded Ad is not ready.");
        }
    }

    public bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
    }


    #endregion

    #region Banner Ad

    public void InitializeBannerAds()
    {
        if(NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            Debug.Log("No Ads enabled, skipping banner ad initialization.");
            return;
        }

        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "true");

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, UnityEngine.Color.black);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        ShowBannerAd();
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void HideBannerAd() => MaxSdk.HideBanner(bannerAdUnitId);

    public void ShowBannerAd()
    {
        if (NoAds)
        {
            HideBannerAd();
            return;
        }
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    #endregion

    #region MREC Bottom Ad

    public void InitializeBottomMrecAds()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            return;
        }

        MaxSdk.CreateMRec(mrecAdBottmUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
        MaxSdk.SetMRecExtraParameter(mrecAdBottmUnitId, "adaptive_banner", "true");

        //MaxSdk.ShowMRec(mrecCustomAdUnitId);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnBottomMrecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnBottomMrecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnBottomMrecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnBottomMrecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnBottomMrecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnBottomMrecAdCollapsedEvent;

    }

    private void OnBottomMrecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBottomMrecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBottomMrecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBottomMrecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.Banner, adInfo);
    }

    private void OnBottomMrecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBottomMrecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowBottomMrecAd()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            HideBottomMrecAd();
            return;
        }

        HideBannerAd();
        MaxSdk.ShowMRec(mrecAdBottmUnitId);
        Debug.Log("Bottom MREC Ad is shown.");
    }

    public void HideBottomMrecAd()
    {
        MaxSdk.HideMRec(mrecAdBottmUnitId);
        MaxSdk.DestroyMRec(mrecAdBottmUnitId);
        Debug.Log("Bottom MREC Ad is destoryed.");
        ShowBannerAd();
        InitializeBottomMrecAds();
    }

    #endregion

    #region MREC Top Ad

    public void InitializeTopMrecAds()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            return;
        }

        MaxSdk.CreateMRec(mrecAdTopUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
        MaxSdk.SetMRecExtraParameter(mrecAdTopUnitId, "adaptive_banner", "true");

        //MaxSdk.ShowMRec(mrecCustomAdUnitId);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnTopMrecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnTopMrecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnTopMrecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnTopMrecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnTopMrecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnTopMrecAdCollapsedEvent;

    }

    private void OnTopMrecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnTopMrecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnTopMrecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnTopMrecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.Banner, adInfo);
    }

    private void OnTopMrecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnTopMrecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowTopMrecAd()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            HideTopMrecAd();
            return;
        }

        HideBannerAd();
        MaxSdk.ShowMRec(mrecAdTopUnitId);
        Debug.Log("Top MREC Ad is shown.");
    }

    public void HideTopMrecAd()
    {
        MaxSdk.HideMRec(mrecAdTopUnitId);
        MaxSdk.DestroyMRec(mrecAdTopUnitId);
        Debug.Log("Top MREC Ad is destoryed.");
        ShowBannerAd();
        InitializeTopMrecAds();
    }

    #endregion

    #region Custom MREC Ad

    public void InitializeCustomMrecAds()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            return;
        }

        // MaxSdk.CreateMRec(mrecAdBottmUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
        // MaxSdk.SetMRecExtraParameter(mrecAdBottmUnitId, "adaptive_banner", "true");

        int paddingBottom = 30; // Space from bottom

        int x = 20;
        int y = 450;

        MaxSdk.CreateMRec(mrecCustomAdUnitId, x, y);
        MaxSdk.SetMRecExtraParameter(mrecCustomAdUnitId, "adaptive_banner", "true");
        //MaxSdk.ShowMRec(mrecCustomAdUnitId);


        Debug.Log($"MREC positioned at X:{x}, Y:{y}");

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnCustomMrecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnCustomMrecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnCustomMrecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnCustomMrecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnCustomMrecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnCustomMrecAdCollapsedEvent;

    }

    private void OnCustomMrecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnCustomMrecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnCustomMrecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnCustomMrecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //GAManger.Instance.OnAdRevenuePaidEvent(adUnitId, GameAnalyticsSDK.GAAdType.Banner, adInfo);
    }

    private void OnCustomMrecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnCustomMrecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
   
    public void ShowCustomMrec()
    {
        if (NoAds || PlayerPrefsManager.Get_Noads_Done())
        {
            HideCustomMrecAd();
            return;
        }

        MaxSdk.ShowMRec(mrecCustomAdUnitId);
        Debug.Log("MREC Ad is shown.");
    }

    public void HideCustomMrecAd()
    {
        MaxSdk.HideMRec(mrecCustomAdUnitId);
        MaxSdk.DestroyMRec(mrecCustomAdUnitId);
        Debug.Log("MREC Ad is destoryed.");
        InitializeCustomMrecAds();
    }

    #endregion

    #region No Ads Purchase

    /* public void UpdateNoAds()
     {
         NoAds = PlayerPrefsManager.Get_Noads_Done();
         HideBannerAd();
         MaxSdk.DestroyBanner(bannerAdUnitId);
     }*/

    #endregion

    #region No Ads Purchase

    public void UpdateNoAds()
    {
        NoAds = PlayerPrefsManager.Get_Noads_Done();
        HideBannerAd();
        MaxSdk.DestroyBanner(bannerAdUnitId);
    }

    #endregion

    #region Applovin Max Track With Firebase
    private void OnAdRevenuePaidEventToFirebase(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        double revenue = impressionData.Revenue;
        var impressionParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
            new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
            new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
            new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    }
    #endregion
}
