using UnityEngine;

public class AdsControl : MonoBehaviour
{

    private static AdsControl instance;

    //for Admob
    #region ADMOB_KEY
    public string Android_AppID, IOS_AppID;

    public string Android_Banner_Key, IOS_Banner_Key;

    public string Android_Interestital_Key, IOS_Interestital_Key;

    public string Android_RW_Key, IOS_RW_Key;

    #endregion

    #region UNITY_ADS_KEY
    public string androidUnityGameId;
    public string iOSUnityGameId;
    public string androidUnityAdUnitId;
    public string iOSUnityAdUnitId;

    [HideInInspector]
    public string adUnityUnitId;

    public string androidUnityRWAdUnitId;
    public string iOSUnityRWAdUnitId;

    [HideInInspector]
    public string adUnityRWUnitId = null; // This will remain null for unsupported platforms

    public bool testMode;
    private string unityGameId;
    #endregion

    public enum ADS_TYPE
    {
        ADMOB,
        UNITY,
        MEDIATION
    }

    public ADS_TYPE currentAdsType;

    public static AdsControl Instance { get { return instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }


        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool directPlay = false;



    public void ShowInterstital()
    {
        if (IsRemoveAds())
            return;

        AppLovinMaxAdManager.instance.ShowInterstitialAd();
        
    }

    public void RemoveAds()
    {

        PlayerPrefs.SetInt("removeAds", 1);
        //if banner is active and user bought remove ads the banner will automatically hide
    
    }

    public bool IsRemoveAds()
    {
        if (!PlayerPrefs.HasKey("removeAds"))
        {
            return false;
        }
        else
        {
            if (PlayerPrefs.GetInt("removeAds") == 1)
            {
                return true;
            }
        }
        return false;
    }
}

