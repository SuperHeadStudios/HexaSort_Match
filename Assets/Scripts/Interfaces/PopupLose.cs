using DG.Tweening;
using UnityEngine;
using static AdsControl;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PopupLose : BasePopup
{
    public Transform offerTrans;

    public Transform losePopup;

    public Transform areYouSurePopup;

    public CanvasGroup areYouSure;

    [SerializeField] private HomeView homeView;
    [SerializeField] private Button coinBtn;
    [SerializeField] private GameObject settingBtn;
    [SerializeField] GameObject adBtn;

    private void FixedUpdate()
    {
        
    }

    public override void InitView()
    {
       
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {
       
    }

    public override void ShowView()
    {
        losePopup.localScale = Vector3.zero;
        canvasGroup.alpha = 1;
        losePopup.DOScale(Vector3.one, 1f).SetEase(Ease.OutQuart).OnComplete(()=> AppLovinMaxAdManager.instance.ShowBottomMrecAd());
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShow = true;

        if(GameManager.instance.coinValue < 200)
        {
            coinBtn.interactable = false;
        }
        else
        {
            coinBtn.interactable = true;
        }
    }
    public void HideSure()
    {
        AppLovinMaxAdManager.instance.HideBottomMrecAd();
        areYouSurePopup.DOScale(Vector3.zero, 0.8f).SetEase(Ease.InBack).OnComplete(() =>
        {
            areYouSure.alpha = 0;
            ShowView();
            settingBtn.SetActive(true);
        });
    }

    public void AreYouSure()
    {
        losePopup.DOScale(Vector3.zero, 0.01f).SetEase(Ease.InBack).OnComplete(() =>
        {
            settingBtn.SetActive(false);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isShow = true;
            areYouSure.alpha = 1;
            areYouSure.blocksRaycasts = true;
            areYouSure.interactable = true;
            areYouSurePopup.DOScale(Vector3.one * 0.95f, 0.5f).SetEase(Ease.OutQuart);
            AppLovinMaxAdManager.instance.ShowBottomMrecAd();
        });
    }

    public void LifeLoseButton()
    {
        AppLovinMaxAdManager.instance.HideBottomMrecAd();
        GameManager.instance.livesManager.ConsumeLife();
      /*  GameManager.instance.BackToHome();
        homeView.PlayGame();*/
        AdsControl.Instance.directPlay = true;
        SceneManager.LoadScene(2);
    }

    public override void HideView()
    {
        
        base.HideView();
    }

    public void GoToHome()
    {
        AudioManager.instance.clickSound.Play();
        HideView();
        GameManager.instance.livesManager.ConsumeLife();
        GameManager.instance.BackToHome();
    }



    private IEnumerator Retrive()
    {
        AppLovinMaxAdManager.instance.HideBottomMrecAd();
        HideView();
        GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;

        yield return new WaitForSeconds(2f);

        GameManager.instance.boardController.DestroyThreeColums();
        GameManager.instance.cellHolder.ShuffleHolder();
    }

    public void RetriveByCoin()
    {
        AppLovinMaxAdManager.instance.HideBottomMrecAd();
        AudioManager.instance.clickSound.Play();
        if (GameManager.instance.coinValue >= 200)
        {
            StartCoroutine(Retrive());
        }
    }

    public void RetriveByAds()
    {
        AudioManager.instance.clickSound.Play();
        WatchAds();
    }

    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        AppLovinMaxAdManager.instance.ShowRewardedAd(() =>
        {
            StartCoroutine(Retrive());
        });
    }


    public void BuySpecialPack()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.shopPopup.BuyIAPPackage(Config.IAPPackageID.special_offer);
    }
}
