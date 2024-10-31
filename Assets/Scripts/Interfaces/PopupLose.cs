using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GoogleMobileAds.Api;
using UnityEngine;
using static AdsControl;
using UnityEngine.Advertisements;

public class PopupLose : BasePopup
{
    public Transform offerTrans;

    public Transform losePopup;

    public Transform areYouSurePopup;

    public CanvasGroup areYouSure;

    [SerializeField] private HomeView homeView;

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
        canvasGroup.alpha = 1;
        losePopup.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShow = true;
    }
    public void HideSure()
    {
        areYouSurePopup.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            areYouSure.alpha = 0;
        });
    }

    public void AreYouSure()
    {
        losePopup.DOScale(Vector3.zero, 0.01f).SetEase(Ease.InBack).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isShow = true;
            areYouSure.alpha = 1;
            areYouSure.blocksRaycasts = true;
            areYouSure.interactable = true;
            areYouSurePopup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        });
    }

    public void LifeLoseButton()
    {
        GameManager.instance.livesManager.ConsumeLife();
        GameManager.instance.BackToHome();
        homeView.PlayGame();
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

    private void Retrive()
    {
        HideView();
        GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;
        GameManager.instance.boardController.DestroyThreeColums();
        GameManager.instance.cellHolder.ShuffleHolder();
    }

    public void RetriveByCoin()
    {
        AudioManager.instance.clickSound.Play();
        if (GameManager.instance.coinValue >=0)
        {
            Retrive();
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
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnReward);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnReward);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnReward(Reward reward)
    {
        Retrive();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Retrive();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }
        });
    }

    public void BuySpecialPack()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.shopPopup.BuyIAPPackage(Config.IAPPackageID.special_offer);
    }
}
