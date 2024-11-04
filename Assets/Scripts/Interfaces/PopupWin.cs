using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Api;
using static AdsControl;
using UnityEngine.Advertisements;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupWin : BasePopup
{
    public TextMeshProUGUI goalTxt;

    /*public TextMeshProUGUI currentLevelTxt;

    public TextMeshProUGUI rewardNonAdsTxt;

    public TextMeshProUGUI rewardWithAdsTxt;*/

    public Button nextBtn, x2ClaimBtn;

    [SerializeField] private TextMeshProUGUI honeyTxt;

    [SerializeField] private RectTransform size;
    [SerializeField] private Transform targetPos;

    [SerializeField] private RectTransform popUp;


    private int rwValue;

    public override void InitView()
    {
        rwValue = 5 * GameManager.instance.levelIndex;
        goalTxt.text = GameManager.instance.boardGenerator.levelConfig.Goals[0].Target.ToString();
        /*currentLevelTxt.text = "LEVEL " + GameManager.instance.levelIndex.ToString();
        rewardNonAdsTxt.text = rwValue.ToString();
        rewardWithAdsTxt.text = (2 * rwValue).ToString();*/
    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void ShowView()
    {
        size.DOScale(Vector3.one, .3f).OnComplete(() =>
        {
            StartCoroutine(MoveCrown());
            StartCoroutine(ResizeMenu());
        });
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isShow = true;
            contentGroup.alpha = 0.0f;
            rootTrans.localScale = Vector3.one * 0.35f;
            rootTrans.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOTween.To(() => contentGroup.alpha, x => contentGroup.alpha = x, 1.0f, 0.5f).SetDelay(0.35f).SetEase(Ease.Linear);
            });
    }

    private IEnumerator ResizeMenu()
    {
        yield return new WaitForSeconds(.5f);
        {
            popUp.DOScaleY(1f, .4f);
        }
    }

    private IEnumerator MoveCrown()
    {
        yield return new WaitForSeconds(.5f);
        size.DOMove(targetPos.position, .3f);
    }


    public override void HideView()
    {
        AudioManager.instance.clickSound.Play();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0.0f, 0.1f).SetEase(Ease.Linear)
           .OnComplete(() =>
           {
               canvasGroup.alpha = 0.0f;
               canvasGroup.interactable = false;
               canvasGroup.blocksRaycasts = false;
               isShow = false;
           });
    }

    public void NextLevel()
    {
        GameManager.instance.ResetBlockerValue();
        nextBtn.interactable = false;
        x2ClaimBtn.interactable = false;
        GameManager.instance.AddCoin(rwValue);
        StartCoroutine(NextGameIE());
        AdsControl.Instance.directPlay = true;
    }

    public void ClaimX2()
    {
        WatchAds();
    }

    IEnumerator NextGameIE()
    {
        yield return new WaitForSeconds(1.5f);


        if (GameManager.instance.levelIndex >= 3)
            AdsControl.Instance.ShowInterstital();
        nextBtn.interactable = true;
        x2ClaimBtn.interactable = true;
        GameManager.instance.NextLevel();
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
        nextBtn.interactable = false;
        x2ClaimBtn.interactable = false;
        GameManager.instance.AddCoin(2 * rwValue);
        StartCoroutine(NextGameIE());
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                nextBtn.interactable = false;
                x2ClaimBtn.interactable = false;
                GameManager.instance.AddCoin(2 * rwValue);
                StartCoroutine(NextGameIE());
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }
}
