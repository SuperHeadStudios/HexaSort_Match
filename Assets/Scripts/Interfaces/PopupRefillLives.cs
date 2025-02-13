
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsControl;

public class PopupRefillLives : BasePopup
{
    public TextMeshProUGUI textTime;
    [SerializeField] private Button coinBtn;
    [SerializeField] private GameObject[] hearts;

    [SerializeField] private GameObject adsBtn;

    private const string LIVES_SAVEKEY = "Lives";
    private int totalLives = 0;

    public override void InitView()
    {
        GameManager.instance.uiManager.coinView.InitView();
        GameManager.instance.uiManager.coinView.ShowView();
        totalLives = PlayerPrefs.GetInt(LIVES_SAVEKEY);
        FillHearts();

        if (GameManager.instance.coinValue >= 150)
        {
            coinBtn.interactable = true;
        }
        else
        {
            coinBtn.interactable = false;
        }
    }

    private void FillHearts()
    {
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }

        for (int i = 0; i < totalLives; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    public void MoreLivesByCoin()
    {
        HideView();
        AudioManager.instance.clickSound.Play();
        if(GameManager.instance.coinValue >= 150)
        {
            GameManager.instance.SubCoin(150);
            GameManager.instance.livesManager.GiveOneLife();
            FillHearts();
        }
        else
        {
            GameManager.instance.uiManager.moreCoinPopup.InitView();
            GameManager.instance.uiManager.moreCoinPopup.ShowView();
        }
    }

    public void MoreLivesByAds()
    {
        HideView();
        AudioManager.instance.clickSound.Play();
        WatchAds();
    }

    public override void HideView()
    {
        base.HideView();
        GameManager.instance.uiManager.coinView.ShowView();
    }

    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        AdmobManager.instance.ShowRewardedAd(() =>
        {
            GameManager.instance.livesManager.GiveOneLife();
            FillHearts();
        });
    }
}
