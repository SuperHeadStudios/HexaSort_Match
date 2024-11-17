using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppLovingAppOpenAdManager : MonoBehaviour
{
    public static AppLovingAppOpenAdManager instance;

    [SerializeField] private string appOpenAdUnitId = "YOUR_APP_OPEN_AD_UNIT_ID";

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
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            ShowAdIfReady();
        };

        MaxSdk.InitializeSdk();

        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    private void OnDestroy()
    {
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
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

        if (adUnitId == appOpenAdUnitId)
        {
            Debug.Log("Tracking Banner Ad Impression.");
            FirebaseManager.instance.TrackAdImpression(AdType.AppOpen, AdLocation.Game, networkName, 1, cmp, revenue);
        }
        else
        {
            Debug.LogWarning("Unrecognized Ad Unit ID: " + adUnitId);
        }
    }

    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            ShowAdIfReady();
        }
    }

    public void ShowAdIfReady()
    {
        if (MaxSdk.IsAppOpenAdReady(appOpenAdUnitId))
        {
            MaxSdk.ShowAppOpenAd(appOpenAdUnitId);
        }
        else
        {
            MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
        }
    }
}
