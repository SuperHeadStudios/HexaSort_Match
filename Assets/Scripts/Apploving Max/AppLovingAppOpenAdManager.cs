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
