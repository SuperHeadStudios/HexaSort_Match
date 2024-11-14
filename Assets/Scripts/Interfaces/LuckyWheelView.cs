using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static AdsControl;

public class LuckyWheelView : BaseView
{
    public RectTransform rootItemTrans;

    public GameObject resultObject;

    public Text rewardValueTxt;

    public Image rewardIcon;

    public Sprite coinSpr;

    public Sprite hammerSpr;

    public Sprite moveSpr;

    public Sprite shuffleSpr;

    int randomRound;

    private List<int> rewardIndexList = new List<int>();

    public GameObject[] selectObjList;

    public GameObject freeBtn;

    public GameObject adsBtn;

    public GameObject closeBtn;

    public GameObject closeTextBtn;

    private int spinCount;

    private void StartSpin()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.currentLuckyWheel = 1;
        PlayerPrefs.SetInt("CurrentLuckyWheel", 1);
        GameManager.instance.uiManager.questPopup.IncreaseProgressQuest(2, 1);
        GameManager.instance.uiManager.homeView.InitView();
        freeBtn.SetActive(false);
        adsBtn.SetActive(false);
        closeBtn.SetActive(false);
        closeTextBtn.SetActive(false);
        spinCount--;

        randomRound = rewardIndexList[0];
        rewardIndexList.RemoveAt(0);

        rootItemTrans.DORotate(new Vector3(0f, 0f, 6 * 360 + 60 * randomRound), 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            ShowReward();
        });


    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void InitView()
    {
        //AdsControl.Instance.HideBannerAd();
        randomRound = 0;
        spinCount = 6;
        resultObject.SetActive(false);
        rewardIndexList.Clear();

        freeBtn.SetActive(true);
        adsBtn.SetActive(false);
        closeBtn.SetActive(false);
        closeTextBtn.SetActive(true);

        List<int> tempIndexList = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            tempIndexList.Add(i);
            selectObjList[i].SetActive(false);
        }


        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, tempIndexList.Count);
            rewardIndexList.Add(tempIndexList[randomIndex]);
            tempIndexList.RemoveAt(randomIndex);
        }
    }

    private void ShowReward()
    {
        AudioManager.instance.rewardDone.Play();
        resultObject.SetActive(true);
        selectObjList[randomRound].SetActive(true);

        switch (randomRound)
        {
            case 0:
                rewardIcon.sprite = hammerSpr;
                rewardValueTxt.text = "+1";
                GameManager.instance.AddHammerBooster(1);
                break;

            case 1:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "+20";
                GameManager.instance.AddCoin(20);
                break;

            case 2:
                rewardIcon.sprite = moveSpr;
                rewardValueTxt.text = "+1";
                GameManager.instance.AddMoveBooster(1);
                break;

            case 3:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "+10";
                GameManager.instance.AddCoin(50);
                break;

            case 4:
                rewardIcon.sprite = shuffleSpr;
                rewardValueTxt.text = "+1";
                GameManager.instance.AddShuffleBooster(1);
                break;
        }

        if (spinCount > 0)
        {
            freeBtn.SetActive(false);
            adsBtn.SetActive(true);
            closeBtn.SetActive(false);
            closeTextBtn.SetActive(true);
        }

        else
        {
            freeBtn.SetActive(false);
            adsBtn.SetActive(false);
            closeBtn.SetActive(true);
            closeTextBtn.SetActive(false);
        }
    }

    public void Close()
    {
        AudioManager.instance.clickSound.Play();
        HideView();
    }

    public void FreeSpin()
    {
        AudioManager.instance.clickSound.Play();
        StartSpin();

    }
    
    public void WatchAdsSpin()
    {
        AudioManager.instance.clickSound.Play();

        AppLovinMaxAdManager.instance.ShowRewardedAd();
        StartSpin();
    }
}
