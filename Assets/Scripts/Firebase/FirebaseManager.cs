using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    private FirebaseApp app;

    private int hintCount = 0;
    private int retryCount = 0;

    private float levelStartTime;
    private float levelEndTime;


    public enum AdType
    {
        Banner,
        Interstitial,
        Reward_Interstitial,
        Reward,
        AppOpen
    }

    public enum AdLocation
    {
        Win,
        Lose,
        Home,
        Hammer_Booster
    }

    enum LevelTrack
    {
        Start_,
        Win_,
        Lose_
    }

    #region Firebase_Initialization

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
               // RemoteConfigManager.instance.InitializeRemoteConfig();
                Debug.Log("Firebase initialized successfully");
                TrackDailyEngagement();
            }
            else
            {
                Debug.LogError(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    #endregion

    #region Main Game Event

    public void LogStartEvent(int levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Start_.ToString(), new Parameter("Level_", levelName));
            Debug.Log("Logged start event for level: " + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    public void LogWinEvent(int levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Win_.ToString(), new Parameter("Level_", levelName));
            Debug.Log("Logged win event for level: " + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    public void LogLoseEvent(int levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Lose_.ToString(), new Parameter("Level_", levelName));
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    #endregion

    #region Track_Ad_Impressions

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        string networkPlacement = adInfo.NetworkPlacement; // The placement ID from the network that showed the ad
    }

    private AdType DetermineAdType(string adUnitIdentifier)
    {
        if (adUnitIdentifier.Contains("rewarded"))
            return AdType.Reward;
        else if (adUnitIdentifier.Contains("interstitial"))
            return AdType.Interstitial;
        else if (adUnitIdentifier.Contains("appopen"))
            return AdType.AppOpen;
        else
            return AdType.Banner;
    }

    public void TrackAdImpression(AdType adType, AdLocation adLocation, string adNetwork, int adCount)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent("ad_impression",
                new Parameter("ad_type", adType.ToString()),
                new Parameter("ad_location", adLocation.ToString()),
                new Parameter("ad_network", adNetwork),
                new Parameter("ad_count", adCount)
            );
            Debug.Log($"Ad impression tracked: Type - {adType}, Location - {adLocation}, Network - {adNetwork}, Count - {adCount}");
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Ad impression not tracked.");
        }
    }

    #endregion


    #region Track_Retry

    public void FirebaseTrackOnPlayerRetry(int currentLevel)
    {
        retryCount++;
        LogRetryEvent(currentLevel.ToString());
    }

    private void LogRetryEvent(string currentLevel)
    {
        FirebaseAnalytics.LogEvent("level_retry",
            new Parameter("Level_", currentLevel),
            new Parameter("retry_count", retryCount)
        );
        Debug.Log("Retry event logged: Level - " + currentLevel + ", Retry Count - " + retryCount);
    }

    public void ResetRetryCount()
    {
        retryCount = 0;
    }

    #endregion

    #region Track_IAP
/*
    public void FirbaseTrackOnPurchaseComplete(string productId, string productName, string productCategory, double price *//*,string currency*//*)
    {
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventPurchase,
            new Parameter(FirebaseAnalytics.ParameterItemId, productId),
            new Parameter(FirebaseAnalytics.ParameterItemName, productName),
            new Parameter(FirebaseAnalytics.ParameterItemCategory, productCategory),
            new Parameter(FirebaseAnalytics.ParameterPrice, price)
        //new Parameter(FirebaseAnalytics.ParameterCurrency, currency)
        );

        Debug.Log($"Purchase tracked: {productName} (${price} )");
    }*/

    #endregion

    #region Track_Level_Win_Time

    public void FirbaseTrackStartLevelTime()
    {
        levelStartTime = Time.time;
        Debug.Log("Level started at: " + levelStartTime);
    }

    public void FirbaseTrackCompleteLevelTime(string levelName)
    {
        levelEndTime = Time.time;
        float timeTaken = levelEndTime - levelStartTime;

        Debug.Log($"Level {levelName} completed in {timeTaken} seconds.");

        FirebaseAnalytics.LogEvent("level_complete_time",
           new Parameter("Level_", levelName),
           new Parameter("time_taken", timeTaken)
       );
    }

    #endregion

    #region Track_Terms_And_Condiotion

    int count = 0;

    public void FirbaseTrackTermsAgree()
    {
        count++;
        FirebaseAnalytics.LogEvent($"Terms Accepted At_{count}");
        Debug.Log($"Terms Accepted At_{count}");
    }

    public void FirbaseTrackTermsDisagree()
    {
        count++;
        FirebaseAnalytics.LogEvent($"Terms Rejected At_{count}");
        Debug.Log($"Terms Rejected At_{count}");
    }


    #endregion

    #region Day_Wise_Track

    // This method should be called when the game starts for the day
    public static void LogUserDay(int dayNumber)
    {
        FirebaseAnalytics.LogEvent(
            "Days_Play",
            new Parameter("Day_", dayNumber) // Logs the day number as a parameter
        );
    }

    // Example method to determine the current play day for the user (could be based on last login)
    public static void TrackDailyEngagement()
    {
        int dayNumber = GetUserPlayDay(); // Custom logic to track which day (D1, D2, ...) user is on
        LogUserDay(dayNumber);
        IncrementPlayDay();
    }

    private static int GetUserPlayDay()
    {
        // Logic to calculate how many days the user has played (this can be done using PlayerPrefs or a server-side mechanism)
        int daysPlayed = PlayerPrefs.GetInt("daysPlayed", 1);
        return daysPlayed;
    }

    public static void IncrementPlayDay()
    {
        int daysPlayed = GetUserPlayDay();
        PlayerPrefs.SetInt("daysPlayed", daysPlayed + 1);
    }

    #endregion

}
