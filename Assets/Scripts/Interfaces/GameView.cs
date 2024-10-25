using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

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

    public Transform switchGuidePanel;

    public Transform purchaseBooster;

    public Text hammerCountTxt, moveCountTxt, shuffleCountTxt;

    public GameObject hammerPriceTag, movePriceTag, shufflePriceTag;

    public GameObject lv1Arrow;

    private bool finishTut;

    public bool isBlockers;

    public GameObject blockers;

    public GameObject targetFill;

    

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

    public override void InitView()
    {        
        currentGoalValue = 0.0f;

        if (GameManager.instance.isBlockers)
        {
            blockers.SetActive(true);
            targetFill.SetActive(false);
            goalText.text = GameManager.instance.boardGenerator.currentGoalNumber.ToString(); /*+"/" + GameManager.instance.boardGenerator.goalNumber.ToString();*/
        }
        else
        {
            targetFill.SetActive(true);
            blockers.SetActive(false);
            goal_Text.text = GameManager.instance.boardGenerator.currentGoalNumber + "/" + GameManager.instance.boardGenerator.goalNumber.ToString();
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
        if (GameManager.instance.hammerBoosterValue > 0)
        {
            ShowHammerBoosterView();
        }
        else
        {
            if (GameManager.instance.coinValue >= 50)
            {
                GameManager.instance.SubCoin(50);
                GameManager.instance.AddHammerBooster(1);
                UpdateBoosterView();
            }
            else
            {
                purchaseBooster.DOScale(Vector3.one, 0.1f);
            }
                GameManager.instance.uiManager.moreCoinPopup.ShowView();
        }
       
    }



    private void ShowHammerBoosterView()
    {
        currentState = BOOSTER_STATE.HAMMER;
        HideView();
        GameManager.instance.uiManager.coinView.HideView();
        hammerGuidePanel.DOScaleY(1f, 0.1f);
    }

    public void UseMove()
    {

        if (GameManager.instance.moveBoosterValue > 0)
        {
            ShowMoveBoosterView();
        }
        else
        {
            if (GameManager.instance.coinValue >= 50)
            {
                GameManager.instance.SubCoin(50);
                GameManager.instance.AddMoveBooster(1);
                UpdateBoosterView();
            }
            else
            {
                purchaseBooster.DOScale(Vector3.one, 0.1f);
            }
                //GameManager.instance.uiManager.moreCoinPopup.ShowView();
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

    public void CloseHammer()
    {
        currentState = BOOSTER_STATE.NONE;
        ShowView();
        GameManager.instance.uiManager.coinView.ShowView();

        hammerGuidePanel.DOScaleY(0.0001f, 0.01f);
    }

    public void CloseMove()
    {
        currentState = BOOSTER_STATE.NONE;
        ShowView();
        GameManager.instance.uiManager.coinView.ShowView();

        moveGuidePanel.DOScaleY(0.001f, 0.01f);

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
            if (GameManager.instance.coinValue >= 50)
            {
                GameManager.instance.SubCoin(50);
                GameManager.instance.AddShuffleBooster(1);
                UpdateBoosterView();
            }
            else
            {
                purchaseBooster.DOScale(Vector3.one, 0.1f);
            }
                //GameManager.instance.uiManager.moreCoinPopup.ShowView();

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
    }

    public void UpdateBoosterView()
    {
        if(GameManager.instance.hammerBoosterValue > 0)
        {
            hammerCountTxt.transform.parent.gameObject.SetActive(true);
            hammerCountTxt.text = GameManager.instance.hammerBoosterValue.ToString();
            hammerPriceTag.SetActive(false);
        }
        else
        {
            hammerCountTxt.transform.parent.gameObject.SetActive(false);
            hammerPriceTag.SetActive(true);
        }
        

        if (GameManager.instance.moveBoosterValue > 0)
        {
            moveCountTxt.transform.parent.gameObject.SetActive(true);
            moveCountTxt.text = GameManager.instance.moveBoosterValue.ToString();
            movePriceTag.SetActive(false);
        }
        else
        {
            moveCountTxt.transform.parent.gameObject.SetActive(false);
            movePriceTag.SetActive(true);
        }


        if (GameManager.instance.shuffleBoosterValue > 0)
        {
            shuffleCountTxt.transform.parent.gameObject.SetActive(true);
            shuffleCountTxt.text = GameManager.instance.shuffleBoosterValue.ToString();
            shufflePriceTag.SetActive(false);
        }
        else
        {
            shuffleCountTxt.transform.parent.gameObject.SetActive(false);
            shufflePriceTag.SetActive(true);
        }
    }
}
