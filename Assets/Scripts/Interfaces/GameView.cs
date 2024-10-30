using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class GameView : BaseView
{
    public Text levelTxt;

    //public TextMeshProUGUI levelText;
    
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI goal_Text;

    public TextMeshProUGUI woodGoalText;

    public TextMeshProUGUI honeyGoalText;

    public TextMeshProUGUI grassGoalText;

    public Image goalValueBar;

    public float currentGoalValue;

    public Transform hammerGuidePanel;

    public Transform moveGuidePanel;

    //public Transform switchGuidePanel;

    public Transform purchaseBooster;

    public Text hammerCountTxt, moveCountTxt, shuffleCountTxt;

    public GameObject hammerPriceTag, movePriceTag, shufflePriceTag;

    public GameObject lv1Arrow;

    private bool finishTut;

    public bool isBlockers;

    public GameObject blockers;

    public GameObject targetFill;

    public Transform settingPanel;

    public Transform settingBar;
    public Transform[] transforms;

    [SerializeField] private GameObject goalbar;
    [SerializeField] private GameObject blockergoalbar;

    //public Transform setting;

    public bool isTogle = false;

    

    public enum BOOSTER_STATE
    {
        NONE,
        HAMMER,
        MOVE,
        SHUFFLE
    };

    public BOOSTER_STATE currentState;

    private void Awake()
    {
        finishTut = false;
        isBlockers = true;
    }

    public bool IsBlocker()
    {
        Debug.Log(GameManager.instance.woodCount + "WoodCount");
        Debug.Log(GameManager.instance.honeyCount + "honey");
        Debug.Log(GameManager.instance.grassCount + "");
        return GameManager.instance.woodCount > 0 ||
            GameManager.instance.honeyCount > 0 ||
            GameManager.instance.grassCount > 0;
    }

    public void GoallbarShow()
    {
        if(IsBlocker())
        {
            blockergoalbar.SetActive(true);
            goalbar.SetActive(false);
        }
        else
        {
            blockergoalbar.SetActive(false);
            goalbar.SetActive(true);
        }
    }



    public void ShowSetting()
    {
        isTogle = !isTogle;
        if (isTogle == true)
        {
            ShowSettingBar();
            Debug.Log("Istogle");
        }
        else
        {
            Debug.Log("IstogleNot");
            HideSetting();
        }
        AudioManager.instance.clickSound.Play();

    }

    private void ShowSettingBar()
    {
        settingBar.DOScale(Vector3.one, 0.1f).OnComplete(() => StartCoroutine(ShowSettingItems()));
    }

    private IEnumerator ShowSettingItems()
    {
        for (int i = 0; i < 5; i++)
        {
            transforms[i].DOLocalMoveX(-50, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void HideSetting()
    {
        AudioManager.instance.clickSound.Play();
        StartCoroutine(HideSettingItems());

    }

    private IEnumerator HideSettingItems()
    {
        for (int i = 4; i > 0; i--)
        {
            transforms[i].DOLocalMoveX(120, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        settingBar.DOScale(Vector3.zero, 0.1f);
    }




    public override void InitView()
    {        
        currentGoalValue = 0.0f;

       /* if (GameManager.instance.isBlockers)
        {
            blockers.SetActive(true);
            targetFill.SetActive(false);
        }
        else*/
        {
            goalText.text = GameManager.instance.boardGenerator.currentGoalNumber.ToString(); /*+"/" + GameManager.instance.boardGenerator.goalNumber.ToString();*/
            //targetFill.SetActive(true);
            //blockers.SetActive(false);
            //goal_Text.text = GameManager.instance.boardGenerator.currentGoalNumber + "/" + GameManager.instance.boardGenerator.goalNumber.ToString();
            woodGoalText.text = GameManager.instance.boardGenerator.woodGoalNumber.ToString();
            honeyGoalText.text = GameManager.instance.boardGenerator.honeyGoalNumber.ToString();
            grassGoalText.text = GameManager.instance.boardGenerator.grassGoalNumber.ToString();
            currentGoalValue = (float)(GameManager.instance.boardGenerator.currentGoalNumber) / (float)(GameManager.instance.boardGenerator.goalNumber);
            goalValueBar.fillAmount = currentGoalValue;

            levelTxt.text = "Level " + GameManager.instance.levelIndex.ToString();
            currentState = BOOSTER_STATE.NONE;
            UpdateBoosterView();

            if (GameManager.instance.levelIndex == 1)
                ShowArrow();
            else
                DisableArrow();
        }
    }

    public void DisableArrow()
    {
        if(!finishTut)
        {
            lv1Arrow.SetActive(false);
            finishTut = true;
        }
        
    }

    public void ShowArrow()
    {
        if (!finishTut)
        {
            lv1Arrow.SetActive(true);
        }

    }

    public override void Start()
    {
        purchaseBooster.parent.GetComponent<Image>().enabled = false;
    }

    public override void Update()
    {

    }

    public void UpdateGoalBar()
    {
        StartCoroutine(UpdateGoalBarIE());
    }

    IEnumerator UpdateGoalBarIE()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameManager.instance.boardGenerator.currentGoalNumber <= GameManager.instance.boardGenerator.goalNumber)
        {
            goalText.text = GameManager.instance.boardGenerator.currentGoalNumber.ToString(); /*+ "/" + GameManager.instance.boardGenerator.goalNumber.ToString();*/
            currentGoalValue = (float)(GameManager.instance.boardGenerator.currentGoalNumber) / (float)(GameManager.instance.boardGenerator.goalNumber);
            goalValueBar.fillAmount = currentGoalValue;
        }
        else if (GameManager.instance.boardGenerator.currentWoodGoalNumber <= GameManager.instance.boardGenerator.woodGoalNumber)
        {
            woodGoalText.text = GameManager.instance.boardGenerator.currentWoodGoalNumber + "/" + GameManager.instance.boardGenerator.woodGoalNumber;
        }
        else if (GameManager.instance.boardGenerator.currentHoneyGoalNumber <= GameManager.instance.boardGenerator.honeyGoalNumber)
        {
            honeyGoalText.text = GameManager.instance.boardGenerator.currentHoneyGoalNumber + "/" + GameManager.instance.boardGenerator.honeyGoalNumber;
        }
        else if (GameManager.instance.boardGenerator.currentGrassGoalNumber <= GameManager.instance.boardGenerator.grassGoalNumber)
        {
            grassGoalText.text = GameManager.instance.boardGenerator.currentGrassGoalNumber + "/" + GameManager.instance.boardGenerator.grassGoalNumber;
        }

        else
        {
            goalText.text = GameManager.instance.boardGenerator.currentGoalNumber.ToString();/* + "/" + GameManager.instance.boardGenerator.goalNumber.ToString();*/
            /*woodGoalText.text = GameManager.instance.boardGenerator.woodGoalNumber + "/" + GameManager.instance.boardGenerator.woodGoalNumber;
            honeyGoalText.text = GameManager.instance.boardGenerator.honeyGoalNumber + "/" + GameManager.instance.boardGenerator.honeyGoalNumber;
            grassGoalText.text = GameManager.instance.boardGenerator.grassGoalNumber + "/" + GameManager.instance.boardGenerator.grassGoalNumber;*/
            goalValueBar.fillAmount = 1.0f;
        }
    }

    public void PauseGame()
    {
        AudioManager.instance.clickSound.Play();
        GameManager.instance.uiManager.settingPopup.InitView();
        GameManager.instance.uiManager.settingPopup.ShowView();
    }

    public void UseHammer()
    {
        if (GameManager.instance.hammerBoosterValue > 0 )
        {
            ShowHammerBoosterView();
        }
        else
        {
            purchaseBooster.DOScale(Vector3.one * 0.9f, 0.1f);
            purchaseBooster.parent.GetComponent<Image>().enabled = true;
        }

    }

    public void ShowSettingPanel()
    {
        settingPanel.DOScale(Vector3.one, 0.1f);
    }

    private void ShowHammerBoosterView()
    {
        currentState = BOOSTER_STATE.HAMMER;
        HideView();
        GameManager.instance.uiManager.coinView.HideView();
        hammerGuidePanel.DOScaleY(1f, 0.1f);
    }

    public void CloseHammer()
    {
        currentState = BOOSTER_STATE.NONE;
        ShowView();
        hammerGuidePanel.DOScaleY(0.0001f, 0.01f);
        GameManager.instance.uiManager.coinView.ShowView();
    }

    public void UseMove()
    {

        if (GameManager.instance.moveBoosterValue > 0)
        {
            ShowMoveBoosterView();
        }
        else
        {
            purchaseBooster.DOScale(Vector3.one * 0.9f, 0.1f);
            purchaseBooster.parent.GetComponent<Image>().enabled = true;
        }
    }

    public void AdCoins()
    {
        GameManager.instance.AddCoin(50);
    }

    private void ShowMoveBoosterView()
    {
        currentState = BOOSTER_STATE.MOVE;
        HideView();
        GameManager.instance.uiManager.coinView.HideView();
        moveGuidePanel.DOScaleY(1f, 0.1f);
    }

    public void SubHammerValue()
    {
        GameManager.instance.AddHammerBooster(-1);
        UpdateBoosterView();
    }

    public void SubMoveValue()
    {
        GameManager.instance.AddMoveBooster(-1);
        UpdateBoosterView();
    }

    public void SubShuffleValue()
    {
        GameManager.instance.AddShuffleBooster(-1);
        UpdateBoosterView();
    }

  

    public void CloseMove()
    {
        currentState = BOOSTER_STATE.NONE;
        ShowView();
        GameManager.instance.uiManager.gameView.ShowView();
        moveGuidePanel.DOScaleY(0.001f, 0.01f);
        GameManager.instance.uiManager.coinView.ShowView();
    }


    public void UseShuffle()
    {
        if (GameManager.instance.shuffleBoosterValue > 0)
        {
            //StartCoroutine(shuffleBoosterTut());
            GameManager.instance.cellHolder.ShuffleHolder();

            SubShuffleValue();
            UpdateBoosterView();
        }
        else
        {
            purchaseBooster.DOScale(Vector3.one * 0.9f, 0.1f);
            purchaseBooster.parent.GetComponent<Image>().enabled = true;
        }
    }

    /*private IEnumerator shuffleBoosterTut()
    {
        switchGuidePanel.DOScaleY(1f, 0.01f);
        yield return new WaitForSeconds(3);
        switchGuidePanel.DOScaleY(0.0001f, 0.01f);
    }*/

    public void closePurchse()
    {
        purchaseBooster.DOScale(Vector3.zero, 0.1f);
        purchaseBooster.parent.GetComponent<Image>().enabled = false;
        GameManager.instance.uiManager.gameView.ShowView(); 
    }

    public void UpdateBoosterView()
    {
        if(GameManager.instance.hammerBoosterValue > 0)
        {
            hammerCountTxt.text = GameManager.instance.hammerBoosterValue.ToString();
            hammerPriceTag.SetActive(false);
        }
        else
        {
            hammerPriceTag.SetActive(true);
        }
        

        if (GameManager.instance.moveBoosterValue > 0)
        {
            moveCountTxt.text = GameManager.instance.moveBoosterValue.ToString();
            movePriceTag.SetActive(false);
        }
        else
        {
            movePriceTag.SetActive(true);
        }


        if (GameManager.instance.shuffleBoosterValue > 0)
        {
            shuffleCountTxt.text = GameManager.instance.shuffleBoosterValue.ToString();
            shufflePriceTag.SetActive(false);
        }
        else
        {
            shufflePriceTag.SetActive(true);
        }
    }
}
