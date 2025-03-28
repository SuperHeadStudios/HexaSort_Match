
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

    [SerializeField] private LoadingScreenManager loadingScreenManager;
    [SerializeField] private GameObject termsPopop;

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

    private void Start()
    {
        loadingScreenManager.gameObject.SetActive(true);
    }

    public void ShowTermsPopup()
    {
        if (PlayerPrefs.GetInt("TermsAccepted", 0) == 0)
        {
            termsPopop.gameObject.SetActive(true);
        }
        else
        {
            termsPopop.gameObject.SetActive(false);
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
            string level = "Level_" + LevelTrack.Start_.ToString() + levelName;

            string level_ = "Level_" + levelName;

            FirebaseAnalytics.LogEvent(level_,
              new Parameter(LevelTrack.Start_.ToString(), levelName));

            FirebaseAnalytics.LogEvent(level);

            Debug.Log("Logged start event for level: " + LevelTrack.Start_ + levelName);
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
            string level = "Level_" + LevelTrack.Win_.ToString() + levelName;

            string level_ = "Level_" + levelName;

            FirebaseAnalytics.LogEvent(level_,
            new Parameter(LevelTrack.Win_.ToString(), levelName));

            FirebaseAnalytics.LogEvent(level);

            Debug.Log("Logged win event for level: " + LevelTrack.Win_ + levelName);
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
            string level = "Level_" + LevelTrack.Win_.ToString() + levelName;

            string level_ = "Level_" + levelName;
            FirebaseAnalytics.LogEvent(level_,
            new Parameter(LevelTrack.Lose_.ToString(), levelName));
            FirebaseAnalytics.LogEvent(level);
            Debug.Log("Logged Lose event for level: " + LevelTrack.Lose_ + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    #endregion

    #region Track_Ad_Impressions

    public void TrackAdImpression(AdType adType, AdLocation adLocation, string adNetwork, int adCount, double ecpm, double revenue, int levelNum)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent("ad_impression_custom",
                new Parameter("ad_type", adType.ToString()),
                new Parameter("ad_location", adLocation.ToString()),
                new Parameter("ad_network", adNetwork),
                new Parameter("ad_count", adCount),
                new Parameter("ad_cpm", ecpm),
                new Parameter("ad_revnue", revenue)
            );
            Debug.Log($"Ad impression tracked: Type - {adType}, Location - {adLocation}, Network - {adNetwork}, Count - {adCount}");

            //TrackTotalAds(adType, adNetwork, ecpm, revenue);
            LevelAdTrack(levelNum.ToString(), adType, revenue);

        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Ad impression not tracked.");
        }
    }

    public void TrackTotalAds(AdType adType)
    {
        switch (adType)
        {
            case AdType.Banner:
                FirebaseAnalytics.LogEvent("Banner_Ads");
                break;
            case AdType.Interstitial:
                FirebaseAnalytics.LogEvent("Interstitial_Ads");
                break;
            case AdType.Reward:
                FirebaseAnalytics.LogEvent("Reward_Ads");
                break;
            case AdType.AppOpen:
                FirebaseAnalytics.LogEvent("AppOpen_Ads");
                break;
        }
    }


    public void LevelAdTrack(string levelName, AdType adType, double revenue)
    {
        string level = "Level_" + levelName;

        FirebaseAnalytics.LogEvent(level,
            new Parameter(adType.ToString(), revenue)
        );
    }


    #endregion

    #region Booster Used InLevel 

    public void TrackBoostersUsed(int levelNum, BoosterTypeUsed boosterTypeUsed)
    {
        string BoosterUsed = boosterTypeUsed.ToString();

        string level = "Level_" + levelNum.ToString() + "_" + boosterTypeUsed;

        FirebaseAnalytics.LogEvent(level);

        Debug.Log(level);
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
    Game,
    Win,
    Lose,
    Home,
    Hammer_Booster,
    Swap_Booster,
    Shuffule_Booster,
    AddCoins,
    LuckyWheel,
    RefillLives,
    None,
    Adblocker,
    WinReward,
    dailyReward,
}

public enum BoosterTypeUsed
{
    Hammer,
    Move,
    Refresh
}