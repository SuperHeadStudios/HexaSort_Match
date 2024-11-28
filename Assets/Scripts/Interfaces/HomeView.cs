using DG.Tweening;
using JetBrains.Annotations;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    public TextMeshProUGUI currentLevelTxt;

    public TextMeshProUGUI wheelDialgueText;

    public TextMeshProUGUI spinProgressTxt;

    public Image spinProgressBar;
    public GameObject openPopup;
    public RectTransform settingPopup;

    [SerializeField] private GameObject lifeBar;
    [SerializeField] private CoinView coinView;

    [SerializeField] private Transform cameraT;
    [SerializeField] private Transform gamePlayerPosition;
    [SerializeField] private GameObject homeScreen_D;

    [SerializeField] private GameObject spinWheelBtn;
    [SerializeField] public Pointer wheelPopup;
    [SerializeField] private Transform wheelIcon;
    [SerializeField] private float wheelSpeed;

    [SerializeField] private CanvasGroup dailyReward;
    [SerializeField] private Transform dailyRewardPopup;
    public void ShowSettingPopup()
    {
        openPopup.SetActive(true);
    }    

    public override void InitView()
    {
        //currentLevelTxt.text = "Level " + GameManager.instance.levelIndex.ToString();
        //spinProgressBar.fillAmount = (float)(GameManager.instance.currentLuckyWheel) / 5.0f;
        //spinProgressTxt.text = GameManager.instance.currentLuckyWheel.ToString() + "/5";
        coinView.UpdateCoinTxt();
        lifeBar.SetActive(true);
        coinView.isGameSettings = false;
        ShowSpinWheelBtn();
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //cameraT.parent = gamePlayerPosition;
            cameraT.position = gamePlayerPosition.position;
            cameraT.rotation = gamePlayerPosition.rotation;
        }

        wheelIcon.Rotate(0, 0, wheelSpeed);
    }

    public void DebugFunction(int level)
    {
        GameManager.instance.LoadGameData(level,true);
        PlayGame();
    }


    public void PlayGame()
    {
        if (GameManager.instance.livesManager.lives > 0)
        {
            //AudioManager.instance.clickSound.Play();
            GameManager.instance.PlayGame();
            cameraT.position = gamePlayerPosition.position;
            cameraT.rotation = gamePlayerPosition.rotation;
            homeScreen_D.SetActive(false);
        }
        else
        {
            Debug.Log("No life ");
            //AudioManager.instance.clickSound.Play();
            GameManager.instance.uiManager.fillLivesPopup.InitView();
            GameManager.instance.uiManager.fillLivesPopup.ShowView();

        }
        coinView.UpdateCoinTxt();
    }

    //.OnComplete(() =>
    /*{
        music.DOLocalMoveX(-50f, 0.1f).OnComplete(() =>
        {
            vibrate.DOLocalMoveX(-50f, 0.1f).OnComplete(() =>
            {
                restart.DOLocalMoveX(-50f, 0.1f).OnComplete(() =>
                {
                    home.DOLocalMoveX(-50f, 0.1f);
                });
            });
        });
    });*/

    /*GameManager.instance.uiManager.settingPopup.InitView();
        GameManager.instance.uiManager.settingPopup.ShowView();*/
    public void ShowShop()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.shopPopup.InitView();
        GameManager.instance.uiManager.shopPopup.ShowView();
    }


    public void ShowPiggyBank()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.piggyBankPopup.InitView();
        GameManager.instance.uiManager.piggyBankPopup.ShowView();
    }

    public void ShowQuest()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.questPopup.InitView();
        GameManager.instance.uiManager.questPopup.ShowView();
    }

    public void ShowDaily()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.dailyPopup.InitView();
        GameManager.instance.uiManager.dailyPopup.ShowView();

    }

    public void ShowDailyReward()
    {
        AudioManager.instance.clickSound.Play();
        dailyReward.alpha = 1;
        dailyReward.blocksRaycasts = true;
        dailyReward.interactable = true;
        dailyRewardPopup.localScale = Vector3.one * 0.55f;
        dailyRewardPopup.DOScale(Vector3.one* 0.85f, 0.25f).SetEase(Ease.OutBounce);
    }


    public void CloseDailyReward()
    {
        AudioManager.instance.clickSound.Play();
        dailyRewardPopup.DOScale(Vector3.one * 1f, 0.05f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            dailyReward.alpha = 0;
            dailyReward.blocksRaycasts = false;
            dailyReward.interactable = false;
        });
    }

    public void ShowSpinWheelBtn()
    {
        if (GameManager.instance.levelIndex > 0)
        {
            spinWheelBtn.SetActive(true);
            spinWheelBtn.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                spinWheelBtn.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
            });
        }
        else
        {
            spinWheelBtn.SetActive(false);
        }
    }

    public void ShowLuckyWheel()
    {
        wheelPopup.UpdateSpinWheel();
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.luckyWheelView.InitView();
        GameManager.instance.uiManager.luckyWheelView.ShowView();

    }
}
