using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using static AdsControl;

public class PopupMoreCoin : BasePopup
{
    public Button openShopBtn, moreCoinBtn;

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void InitView()
    {
        
    }

    public void OpenShop()
    {
        AudioManager.instance.clickSound.Play();
        HideView();
        GameManager.instance.uiManager.shopPopup.ShowView();
    }

    public void MoreCoins()
    {
        AudioManager.instance.clickSound.Play();
        WatchAds();
    }


    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        AdmobManager.instance.ShowRewardedAd(() =>
        {
            HideView();
            GameManager.instance.uiManager.coinView.InitView();
            GameManager.instance.uiManager.coinView.ShowView();
            GameManager.instance.AddCoin(50);
        });
    }
}
