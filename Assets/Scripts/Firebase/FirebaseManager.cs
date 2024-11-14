using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using GameSystem;
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
                // Log a start event to test
                // LogStartEvent(PlayerPrefs.GetInt("Level", 1).ToString());
            }
            else
            {
                Debug.LogError(String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });

        //FirebaseStartTrackLevel();
    }

    public void FirebaseStartTrackLevel()
    {
       /* if (PlayerPrefsManager.Get_LevelNumber() == PlayerPrefs.GetInt("WINLEVELKEY", 1))
        {
            LogStartEvent(PlayerPrefsManager.Get_LevelNumber());
            Debug.Log("Trackng");

        }
        else
        {
            Debug.Log("Already Played Start Tracking");
        }*/
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

    #region FindTheObject Game Event

    public void LogStartEvent_FindTheObject(string levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Start_.ToString() + "FindTheObjct_" + (levelName));
            Debug.Log("Logged start event for FindTheObject level: " + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    public void LogWinEvent_FindTheObject(string levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Win_.ToString() + "FindTheObjct_" + levelName);
            Debug.Log("Logged win event for FindTheObject level: " + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    public void LogLoseEvent_FindTheObject(string levelName)
    {
        if (app != null)
        {
            FirebaseAnalytics.LogEvent(LevelTrack.Lose_.ToString() + "FindTheObjct_" + levelName);
            Debug.Log("Logged lose event for FindTheObject level: " + levelName);
        }
        else
        {
            Debug.LogWarning("Firebase is not initialized. Event not logged.");
        }
    }

    #endregion

    #region Hint Used
    public void FirbaseTrackHintUsed(int levelName)
    {
        hintCount++;
        LogHintCountEvent(levelName);
    }

    private void LogHintCountEvent(int levelName)
    {
        FirebaseAnalytics.LogEvent("hint_used",
             new Parameter("Level_", levelName.ToString()),
             new Parameter("hint_count", hintCount)
         );

        Debug.Log("Hint used event logged: Level - " + levelName + ", Hint Count - " + hintCount);
    }

    public void ResetHintCount()
    {
        hintCount = 0;
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

    #region Inactive_Timer

    public void FirebaseTrackInactiveTime(string levelName, float inactivityTime)
    {
        FirebaseAnalytics.LogEvent("level_inactivity_time",
         new Parameter("Level_", levelName),
         new Parameter("inactivity_time", inactivityTime)
         );
        Debug.Log($"Level {levelName} , {inactivityTime} seconds of inactivity.");
    }

    #endregion

    #region Track_Tutorial_Complete

    public void FirebaseTrackTutorialCompletion(int currentLevel, string tutorialName)
    {
        FirebaseAnalytics.LogEvent("tutorial_completed",
            new Parameter("Level_", currentLevel.ToString()),  // Adjust accordingly
            new Parameter("tutorial_name", tutorialName),  // Adjust accordingly
            new Parameter("completion_time", Time.time) // Optionally log time
        );

        Debug.Log($"{tutorialName} Tutorial completed for Level_{currentLevel}.");
    }
    #endregion

    #region Track_Difference

    public void FirebaseTrackDifferences(int currentLevel, int currentMistakeNum, int totalMistakes)
    {
        if(currentLevel > 4) return;

        // Log the tutorial completion event
        FirebaseAnalytics.LogEvent("Found_Mistake",
            new Parameter("Level_", currentLevel.ToString()),  // Adjust accordingly
            new Parameter("Found_Mistake_", currentMistakeNum)  // Adjust accordingly
        );
        Debug.Log($"{currentMistakeNum} Mistake Found From {totalMistakes} for Level_{currentLevel}.");
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
