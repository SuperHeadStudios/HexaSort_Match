using UnityEngine;

public class AdsControl : MonoBehaviour
{

    private static AdsControl instance;


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



    public void ShowInterstital(AdLocation adLocation)
    {
        if (IsRemoveAds())
            return;
        AdmobManager.instance.ShowInterstitialAd();
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

