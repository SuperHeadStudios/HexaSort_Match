using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    public TextMeshProUGUI currentLevelTxt;


    public TextMeshProUGUI spinProgressTxt;

    public Image spinProgressBar;
    public GameObject openPopup;
    public RectTransform settingPopup;
    //public Ease EaseType;

    public void ShowSettingPopup()
    {
        openPopup.SetActive(true);
        settingPopup.DOScale(Vector3.one,1f).SetEase(Ease.OutBounce);
    }
    public void HideSettingPopup()
    {
        settingPopup.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            openPopup.SetActive(false);
        });
    }


    

    public override void InitView()
    {
        //
        //currentLevelTxt.text = "Level " + GameManager.instance.levelIndex.ToString();
        //spinProgressBar.fillAmount = (float)(GameManager.instance.currentLuckyWheel) / 5.0f;
        //spinProgressTxt.text = GameManager.instance.currentLuckyWheel.ToString() + "/5";
    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public void PlayGame()
    {
        if (GameManager.instance.livesManager.lives > 0)
        {
            AudioManager.instance.clickSound.Play();
            GameManager.instance.PlayGame();
        }
        else
        {
            AudioManager.instance.clickSound.Play();
            GameManager.instance.uiManager.fillLivesPopup.InitView();
            GameManager.instance.uiManager.fillLivesPopup.ShowView();
        }
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

    public void ShowLuckyWheel()
    {
        if (GameManager.instance.currentLuckyWheel == 5)
        {
            AudioManager.instance.clickSound.Play();
            GameManager.instance.uiManager.luckyWheelView.InitView();
            GameManager.instance.uiManager.luckyWheelView.ShowView();
        }

    }
}
