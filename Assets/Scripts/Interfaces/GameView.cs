using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameView : BaseView
{
    public Text levelTxt;

    //public TextMeshProUGUI levelText;
    
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI fillCounText;

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
    [Space(10)]

    [SerializeField] private GameObject honeyGoal;
    [SerializeField] private GameObject woodGoal;
    [SerializeField] private GameObject grassGoal;

    [Space(10)]
    [SerializeField] private ParticleSystem flowerPs_1;
    [SerializeField] private ParticleSystem flowerPps_2;
    [SerializeField] private Transform flowerIcon_1;
    [SerializeField] private Transform flowerIcon_2;

    [SerializeField] private BoosterUnlock boosterUnlock;


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
        return BoardController.instance.boardGenerator.woodGoalNumber > 0 ||
            BoardController.instance.boardGenerator.honeyGoalNumber > 0 ||
            BoardController.instance.boardGenerator.grassGoalNumber > 0;
    }

    public void GoallbarShow()
    {
        if(IsBlocker())
        {
            blockergoalbar.SetActive(true);
            goalbar.SetActive(false);

            if(BoardController.instance.boardGenerator.woodGoalNumber > 0)
            {
                woodGoal.SetActive(true);
            }
            if (BoardController.instance.boardGenerator.grassGoalNumber > 0)
            {
                grassGoal.SetActive(true);
            }
            if (BoardController.instance.boardGenerator.honeyGoalNumber > 0)
            {
                honeyGoal.SetActive(true);
            }
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


    public void PlayCollectEffect()
    {
        flowerPs_1.Play();
        flowerPps_2.Play();

        AudioManager.instance.flowerCollectedSound.Play();

        flowerIcon_1.DOScale(Vector3.one * 1.2f, 0.3f / 2).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                flowerIcon_1.DOScale(Vector3.one, 0.3f / 2).SetEase(Ease.InOutSine);
            });

        flowerIcon_2.DOScale(Vector3.one * 1.2f, 0.3f / 2).SetEase(Ease.OutBack)
           .OnComplete(() =>
           {
               flowerIcon_2.DOScale(Vector3.one, 0.3f / 2).SetEase(Ease.InOutSine);
           });
    }

    public override void InitView()
    {        
        currentGoalValue = 0.0f;

        int fillGoalTarget = BoardController.instance.boardGenerator.goalNumber - GameManager.instance.boardGenerator.currentGoalNumber;
        if(fillGoalTarget < 0)
        {
            fillGoalTarget = 0;
        }

        fillCounText.text = fillGoalTarget + "/" + BoardController.instance.boardGenerator.goalNumber;
        currentGoalValue = (float)fillGoalTarget / (float)(GameManager.instance.boardGenerator.goalNumber);
        goalValueBar.fillAmount = currentGoalValue;

        goalText.text = GameManager.instance.boardGenerator.goalNumber.ToString() ;
        woodGoalText.text = BoardController.instance.boardGenerator.woodGoalNumber.ToString();
        honeyGoalText.text = BoardController.instance.boardGenerator.honeyGoalNumber.ToString();
        grassGoalText.text = BoardController.instance.boardGenerator.grassGoalNumber.ToString();

        levelTxt.text = "Level " + GameManager.instance.levelIndex.ToString();
        currentState = BOOSTER_STATE.NONE;
        UpdateBoosterView();

        if (GameManager.instance.levelIndex == 1)
            ShowArrow();
        else
            DisableArrow();

        GameManager.instance.uiManager.coinView.isGameSettings = true;

        boosterUnlock.CheckBoosters();
        settingBar.GetComponent<SettingBar>().LoadData();
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
        yield return new WaitForSeconds(0.01f);

        if (GameManager.instance.boardGenerator.currentGoalNumber > 0)
        {
            UpdateText(GameManager.instance.boardGenerator.currentGoalNumber, 0.2f);
            int fillGoalTarget = BoardController.instance.boardGenerator.goalNumber - GameManager.instance.boardGenerator.currentGoalNumber;
            IncrementText((fillGoalTarget-1), 0.2f);

            currentGoalValue = (float)fillGoalTarget / (float)(GameManager.instance.boardGenerator.goalNumber);
            goalValueBar.DOFillAmount(currentGoalValue, 0.5f);
        }

        //woodGoalText.text = GameManager.instance.boardGenerator.woodGoalNumber.ToString();
        //honeyGoalText.text = GameManager.instance.boardGenerator.honeyGoalNumber.ToString();
        //grassGoalText.text = GameManager.instance.boardGenerator.grassGoalNumber.ToString();

        PlayCollectEffect();
    }

    int displayValue = 0;
    public void IncrementText(int increment, float duration)
    {
        int startValue = displayValue;
        int targetValue = increment;

        DOTween.To(() => startValue, x => startValue = x, targetValue, duration)
            .OnUpdate(() => {
                displayValue++;
                fillCounText.text = displayValue + "/" + BoardController.instance.boardGenerator.goalNumber;

                if(displayValue > BoardController.instance.boardGenerator.goalNumber)
                {
                    fillCounText.text = BoardController.instance.boardGenerator.goalNumber + "/" + BoardController.instance.boardGenerator.goalNumber;
                }

            })
            .SetEase(Ease.Linear)
            .OnComplete(() => displayValue = targetValue); // Ensure it ends at the exact target value
    }


    int displayValue_1 = 0;
    public void UpdateText(int endValue, float duration)
    {
        // Use the current goal number as the target value for decreasing
        DOTween.To(() => displayValue_1, x => displayValue_1 = x, endValue, duration)
            .OnUpdate(() => {
                goalText.text = displayValue_1.ToString();
            })
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                displayValue_1 = endValue; // Ensure it ends exactly at the target value
            });
    }


    public void PauseGame()
    {
        AudioManager.instance.clickSound.Play();
        //GameManager.instance.uiManager.settingPopup.InitView();
        //GameManager.instance.uiManager.settingPopup.ShowView();
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
