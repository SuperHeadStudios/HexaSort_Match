using UnityEngine;
using GoogleMobileAds.Api;
using System.Security.Cryptography;
using System;

public class CustomBannerAdManager : MonoBehaviour
{
    public static CustomBannerAdManager instance;

    public bool isNoAds;

    private string androidAdUnitId = "ca-app-pub-6312582432828070/7459018732"; // Test ID
    private string iosAdUnitId = "ca-app-pub-3940256099942544/2934735716"; // Test ID

    private BannerView topBannerView;
    private BannerView bottomBannerView;
    private string adUnitId;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        LoadBanner();
        ReqeustTopBanner();
        RequestBottomBanner();
    }


    #region Initialization
    public void LoadBanner()
    {
        if(isNoAds)return;

#if UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_IOS
        adUnitId = iosAdUnitId;
#else
        adUnitId = null;
#endif

    }


    #endregion

    #region Top Banner Ad Methods

    public void ReqeustTopBanner(int width = 300, int height = 250, AdPosition position = AdPosition.Top)
    {
        if (topBannerView != null)
        {
            topBannerView.Destroy();
        }

        AdSize customAdSize = new AdSize(width, height);
        topBannerView = new BannerView(adUnitId, customAdSize, position);
        AdRequest request = new AdRequest();
        topBannerView.LoadAd(request);
        topBannerView.Hide();
    }

    public void ShowTopBanner()
    {
        if (isNoAds) return;
        if (topBannerView != null)
        {
            topBannerView.Show();
        }
    }

    public void HideTopBanner()
    {
        if (topBannerView != null)
        {
            topBannerView.Hide();
        }
    }

    public void DestroyTopBanner()
    {
        if (topBannerView != null)
        {
            topBannerView.Destroy();
        }
    }

    #endregion

    #region Bottom Banner Ad Methods

    public void RequestBottomBanner(int width = 300, int height = 250, AdPosition position = AdPosition.Bottom)
    {
        if (bottomBannerView != null)
        {
            bottomBannerView.Destroy();
        }

        AdSize customAdSize = new AdSize(width, height);
        bottomBannerView = new BannerView(adUnitId, customAdSize, position);
        AdRequest request = new AdRequest();
        bottomBannerView.LoadAd(request);
        bottomBannerView.Hide();
    }

    public void ShowBottomBanner()
    {
        if(isNoAds) return;
        if (bottomBannerView != null)
        {
            AdmobManager.instance.HideBannerAd();
            bottomBannerView.Show();
        }
    }

    public void HideBottomBanner()
    {
        if (bottomBannerView != null)
        {
            bottomBannerView.Hide();
            AdmobManager.instance.ShowBannerAd();
        }
    }

    public void DestroyBottomBanner()
    {
        if (bottomBannerView != null)
        {
            bottomBannerView.Destroy();
        }
    }
    #endregion


}
