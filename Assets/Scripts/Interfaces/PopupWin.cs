using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static AdsControl;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PopupWin : BasePopup
{
    public TextMeshProUGUI goalTxt;

    /*public TextMeshProUGUI currentLevelTxt;

    public TextMeshProUGUI rewardNonAdsTxt;

    public TextMeshProUGUI rewardWithAdsTxt;*/

    public Button nextBtn, x2ClaimBtn;

    [SerializeField] private TextMeshProUGUI honeyTxt;



    [SerializeField] private TextMeshProUGUI levelCoinText;
    [SerializeField] private TextMeshProUGUI goalCollectedText;

    [Header("------ Coins Move Settings ------"), Space(5)]
    [SerializeField] private float randPosi;
    [SerializeField] private GameObject cointPrefab;
    [SerializeField] private Transform cointTarget;
    [SerializeField] private Transform coinParent;

    [SerializeField] private List<GameObject> cointList;


    [Header("------ Popup Animation Settings ------"), Space(5)]
    [SerializeField] private RectTransform crown;
    [SerializeField] private Transform crownTarget;
    [SerializeField] private RectTransform popUp;

    [Space(5)]
    [SerializeField] private Transform rightLeaf;
    [SerializeField] private Transform leftLeaf;

    [Space(5)]
    [SerializeField] private Transform rightHorn;
    [SerializeField] private Transform leftHorn;

    [Space(5)]
    [SerializeField] private ParticleSystem rightConfeti;
    [SerializeField] private ParticleSystem rightConfetiFall;
    [SerializeField] private ParticleSystem leftConfeti;
    [SerializeField] private ParticleSystem leftConfetiFall;

    [Space(5)]
    [SerializeField] private Transform[] inPopupContent;

    private int currentLevel = 0;

    public int rwValue;

    #region Start Functions

    public override void Start()
    {

    }

    public override void Update()
    {
        
    }

    private void OnEnable()
    {

    }

    #endregion

    #region Initialization
    public override void InitView()
    {
        rwValue = 0;
        currentLevel = GameManager.instance.levelIndex;

        if(currentLevel <= 10)
        {
            rwValue = 10;
        }
        else if(currentLevel > 10  && currentLevel < 20)
        {
            rwValue = 15;
        }
        else
        {
            rwValue = 20;
        }

        //goalTxt.text = GameManager.instance.boardGenerator.levelConfig.Goals[currentLevel].Target.ToString();
        //levelCoinText.text = rwValue.ToString();

        /*currentLevelTxt.text = "LEVEL " + GameManager.instance.levelIndex.ToString();
        rewardNonAdsTxt.text = rwValue.ToString();
        rewardWithAdsTxt.text = (2 * rwValue).ToString();*/
    }

    public override void ShowView()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShow = true;
        ResetAnimation();      
        PopUpAnimation();

       
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

               SceneManager.LoadScene(2);
               //SceneLoader.LoadSceneWithLoading(2);
           });
    }

    #endregion

    #region Popup Animation

    public void PopUpAnimation()
    {
        crown.DOScale(Vector3.one, .3f).OnComplete(() =>
        {
            crown.DOMove(crownTarget.position, .8f).SetEase(Ease.OutBounce);

            AudioManager.instance.winSound.Play();
            popUp.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                rightHorn.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    AudioManager.instance.confettiBlast.Play();
                    rightConfeti.Play();
                    rightConfetiFall.Play();
                });
                leftHorn.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    AudioManager.instance.confettiBlast.Play();
                    leftConfeti.Play();
                    leftConfetiFall.Play();
                });

                rightLeaf.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
                leftLeaf.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
                
                StartCoroutine(UpdateTextInPopupElements());

                CustomBannerAdManager.instance.ShowBottomBanner();
            });
        });
    }

    private IEnumerator UpdateTextInPopupElements()
    {
        for (int i = 0; i < inPopupContent.Length; i++)
        {
            inPopupContent[i].DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.1f);

            if (i == 0)
            {
                UpdateTextToTarget(GameManager.instance.boardGenerator.goalNumber, goalTxt);
            }
            else if (i == 2)
            {
                UpdateTextToTarget(rwValue, levelCoinText);
            }
        }
    }

    private void ResetAnimation()
    {
        popUp.localScale = Vector3.zero;

        rightLeaf.localScale = Vector3.zero;
        leftLeaf.localScale = Vector3.zero;

        rightConfeti.Stop();
        leftConfeti.Stop();

        rightConfetiFall.Stop();
        leftConfetiFall.Stop();

        rightHorn.localScale = Vector3.zero;
        leftHorn.localScale = Vector3.zero;


        for (int i = 0; i < inPopupContent.Length; i++)
        {
            inPopupContent[i].localScale = Vector3.zero;
        }
    }


    public void UpdateTextToTarget(int targetValue, TextMeshProUGUI targetText)
    {
        int startValue = 0;

        // Use DOTween to animate the value from 0 to targetValue over the specified duration
        DOTween.To(() => startValue, x => startValue = x, targetValue, 0.5f)
            .OnUpdate(() =>
            {
                // Update the text with the current value
                targetText.text = startValue.ToString();
            })
            .SetEase(Ease.OutQuad) // Use easing for a smooth effect
            .OnComplete(() =>
            {
                targetText.text = targetValue.ToString(); // Ensure it ends at the exact target value
            });
    }

    #endregion


    #region Coins Spawn & Move

    public IEnumerator SpawnCoins(int multiplier)
    {
        int currentCoin = rwValue;

        for (int i = 0; i < rwValue; i++)
        {
            GameObject spwaCoin = Instantiate(cointPrefab, coinParent.position, Camera.main.transform.rotation, coinParent);
            cointList.Add(spwaCoin);
            
            if (currentCoin == rwValue)
            {
                GameManager.instance.uiManager.coinView.CoinBarAnimationPlay();
            }
            currentCoin--;


            levelCoinText.text = currentCoin.ToString();            
            AudioManager.instance.coinCollectSound.Play();
            spwaCoin.transform.DOMove(cointTarget.position, 0.8f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                Destroy(spwaCoin, 0.01f);

                if(currentLevel > 0)
                {
                    GameManager.instance.AddCoin(multiplier);
                }

                if(currentCoin == 0)
                {
                    GameManager.instance.uiManager.coinView.CoinAnimationStop();
                }
            });

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(NextGameIE());
        AdsControl.Instance.directPlay = true;

    }

    #endregion

    #region Button Setup

    public void NextLevel()
    {
       // GameManager.instance.ResetBlockerValue();
        StartCoroutine(SpawnCoins(1));
        nextBtn.interactable = false;
        x2ClaimBtn.interactable = false;
        CustomBannerAdManager.instance.HideBottomBanner();
    }


    IEnumerator NextGameIE()
    {
        nextBtn.interactable = true;
        x2ClaimBtn.interactable = true;
        HideView();
        yield return null;
    }

    public void ClaimX2()
    {
        CustomBannerAdManager.instance.HideBottomBanner();
        WatchAds();
    }

    #endregion

    #region Show Ads


    public void WatchAds()
    {
        AudioManager.instance.clickSound.Play();
        AdmobManager.instance.ShowRewardedAd(() =>
        {
            StartCoroutine(Reward());
        });
    }

    private IEnumerator Reward()
    {
        nextBtn.interactable = false;
        x2ClaimBtn.interactable = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnCoins(2));
    }

    #endregion
}
