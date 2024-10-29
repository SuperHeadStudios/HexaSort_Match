using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AdsControl;

public class BoosterPopup : MonoBehaviour
{
    public enum BoosterState
    {
        Hammer,
        Refresh,
        Swap
    }

   [ SerializeField ] private BoosterState _state;

    [Header("----- UI Settings -----"), Space(5)]
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite swapSprite;
    [SerializeField] private Sprite refreshSprite;
    [SerializeField] private TextMeshProUGUI tittleText;
    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("----- Boster Btns -----"), Space(5)]
    [SerializeField] private Button hammerBtn;
    [SerializeField] private Button swapBtn;
    [SerializeField] private Button refreshBtn;

    [SerializeField] private GameView gameView;


    [SerializeField] private Button boosterPurchaseBtn;
    [SerializeField] private Button boosterPurchaseAdsBtn;


    void Start()
    {
        hammerBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Hammer);
        });

        swapBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Swap);
        });

        refreshBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Refresh);
        });


        boosterPurchaseBtn.onClick.AddListener(PurchaseBoosterWithCoins);
        boosterPurchaseAdsBtn.onClick.AddListener(WatchAds);
    }

    public void ChangeBooster(BoosterState state)
    {
        if (GameManager.instance.coinValue < 50)
        {
            boosterPurchaseBtn.interactable = false;
        }
        else
        {
            boosterPurchaseBtn.interactable = true;
        }
        _state = state;

        switch (state)
        {
            case BoosterState.Hammer:
                SetCurrentBoosterData(hammerSprite, "HAMMER", "Tap any flower tile stack to clear it");
                break;

            case BoosterState.Swap:
                SetCurrentBoosterData(swapSprite, "SWAP", "Move And Replace A flower tile stack On The Board");
                break;

            case BoosterState.Refresh:
                SetCurrentBoosterData(refreshSprite, "REFRESH", "Refresh Tray To Get New Stack Options");
                break;
        }
    }

    public void SetCurrentBoosterData(Sprite currentSprite, string tittle, string dialogue)
    {
        iconImage.sprite = currentSprite;
        tittleText.text = tittle;
        DialogueText.text = dialogue;
    }

    private void PurchaseBoosterWithCoins()
    {
        
        switch (_state)
        {
            case BoosterState.Hammer:
                PurchaseHammerWithCoin();
                break;
            case BoosterState.Swap:
                PurchaseSwapWithCoin();
                break;
            case BoosterState.Refresh:
                PurchaseShuffleWithCoin();
                break;
        }
    }

    public void PurchaseHammerWithCoin()
    {
        if (GameManager.instance.coinValue >= 50)
        {
            GameManager.instance.SubCoin(50);
            GameManager.instance.AddHammerBooster(1);
            gameView.UpdateBoosterView();
        }

    }

    public void PurchaseShuffleWithCoin()
    {
        if (GameManager.instance.coinValue >= 50)
        {
            GameManager.instance.SubCoin(50);
            GameManager.instance.AddShuffleBooster(1);
            gameView.UpdateBoosterView();
        }
    }

   
    public void PurchaseSwapWithCoin()
    {
        if (GameManager.instance.coinValue >= 50)
        {
            GameManager.instance.SubCoin(50);
            GameManager.instance.AddMoveBooster(1);
            gameView.UpdateBoosterView();
        }
    }

    private void ShowAdBtnPressed()
    {
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
    }

    public void EarnReward(Reward reward)
    {
        switch (_state)
        {
            case BoosterState.Hammer:
                GameManager.instance.AddHammerBooster(1);
                gameView.UpdateBoosterView();
                break;
            case BoosterState.Swap:
                GameManager.instance.AddMoveBooster(1);
                gameView.UpdateBoosterView();
                break;
            case BoosterState.Refresh:

                GameManager.instance.AddShuffleBooster(1);
                gameView.UpdateBoosterView();
                break;
        }
    }
}
