using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static AdsControl;
using System.Collections;

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


    private int spinCount;


    public bool isRewardedDone = false;

    private void ReinitializeRewardList()
    {
        rewardIndexList.Clear();
        List<int> tempIndexList = new List<int>();

        for (int i = 0; i < 8; i++) // Assuming 8 rewards
        {
            tempIndexList.Add(i);
        }

        while (tempIndexList.Count > 0)
        {
            int randomIndex = Random.Range(0, tempIndexList.Count);
            rewardIndexList.Add(tempIndexList[randomIndex]);
            tempIndexList.RemoveAt(randomIndex);
        }

        Debug.Log("Reward list reinitialized.");
    }

    public void StartSpin()
    {
        if (rewardIndexList.Count == 0)
        {
            Debug.LogWarning("Reward list is empty. Reinitializing...");
            ReinitializeRewardList();
        }

        AudioManager.instance.clickSound.Play();
        GameManager.instance.currentLuckyWheel = 1;
        PlayerPrefs.SetInt("CurrentLuckyWheel", 1);
        GameManager.instance.uiManager.questPopup.IncreaseProgressQuest(2, 1);
        GameManager.instance.uiManager.homeView.InitView();
        spinCount--;

        randomRound = rewardIndexList[0];
        rewardIndexList.RemoveAt(0);

        //rootItemTrans.DORotate(new Vector3(0f, 0f, 6 * 360 + 60 * randomRound), 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo);
    }

    public IEnumerator SpinWithAd()
    {
        if (rewardIndexList.Count == 0)
        {
            Debug.LogWarning("Reward list is empty. Reinitializing...");
            ReinitializeRewardList();
        }

        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(3f);
        AudioManager.instance.clickSound.Play();
        GameManager.instance.currentLuckyWheel = 1;
        PlayerPrefs.SetInt("CurrentLuckyWheel", 1);
        GameManager.instance.uiManager.questPopup.IncreaseProgressQuest(2, 1);
        GameManager.instance.uiManager.homeView.InitView();
        spinCount--;

        randomRound = rewardIndexList[0];
        rewardIndexList.RemoveAt(0);

        //rootItemTrans.DORotate(new Vector3(0f, 0f, 6 * 360 + 60 * randomRound), 5f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo);
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
        /*randomRound = 0;
        spinCount = 8;
        resultObject.SetActive(false);
        rewardIndexList.Clear();

        freeBtn.SetActive(true);
        adsBtn.SetActive(false);
        closeBtn.SetActive(false);
        closeTextBtn.SetActive(true);
*/
        /*List<int> tempIndexList = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            tempIndexList.Add(i);
            selectObjList[i].SetActive(false);
        }


        for (int i = 0; i < 8; i++)
        {
            int randomIndex = Random.Range(0, tempIndexList.Count);
            rewardIndexList.Add(tempIndexList[randomIndex]);
            tempIndexList.RemoveAt(randomIndex);
        }*/
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
                rewardValueTxt.text = "X1";
                GameManager.instance.AddHammerBooster(1);
                break;

            case 1:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "X5";
                GameManager.instance.AddCoin(20);
                break;

            case 2:
                rewardIcon.sprite = moveSpr;
                rewardValueTxt.text = "X3";
                GameManager.instance.AddMoveBooster(1);
                break;

            case 3:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "X10";
                GameManager.instance.AddCoin(50);
                break;

            case 4:
                rewardIcon.sprite = shuffleSpr;
                rewardValueTxt.text = "X2";
                GameManager.instance.AddShuffleBooster(1);
                break;

            case 5:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "X100";
                GameManager.instance.AddShuffleBooster(1);
                break;

            case 6:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "X10";
                GameManager.instance.AddShuffleBooster(1);
                break;

            case 7:
                rewardIcon.sprite = coinSpr;
                rewardValueTxt.text = "X5";
                GameManager.instance.AddShuffleBooster(1);
                break;
        }

        if (spinCount > 0)
        {
        }

        else
        {
        }
    }

    public void Close()
    {
        /*AudioManager.instance.clickSound.Play();*/
        HideView();
    }

    public void FreeSpin()
    {
        /*AudioManager.instance.clickSound.Play();
        StartSpin();*/
    }
    
    public void WatchAdsSpin()
    {
        Debug.Log("LuckyWheel");
    }
}
