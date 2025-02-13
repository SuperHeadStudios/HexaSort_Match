using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BoardController boardController;

    public BoardGenerator boardGenerator;

    public CellHolder cellHolder;

    public ColorConfig colorConfig;

    public RewardConfig rewardConfig;

    public PoolManager poolManager;

    public UIManager uiManager;

    public LivesManager livesManager;

    public ParticleSystem confetiVfx;

    public ParticleSystem hammerExplosionVfx;

    [Header("----- Collective Counts -----"), Space(5)]
    [SerializeField] public int woodCount;
    [SerializeField] public int honeyCount;
    [SerializeField] public int grassCount;

    [SerializeField] private TextMeshProUGUI woodCountText;
    [SerializeField] private TextMeshProUGUI honeyCountText;
    [SerializeField] private TextMeshProUGUI grassCountText;

    [SerializeField] private GameObject woodTick;
    [SerializeField] private GameObject woodGoal;
    [SerializeField] private GameObject honeyTick;
    [SerializeField] private GameObject honeyGoal;
    [SerializeField] private GameObject grassTick;
    [SerializeField] private GameObject grassGoal;

    public int currentLevel;

    [HideInInspector]
    public int levelIndex;

    public int levelTest;

    public bool isTestMode;

    public int coinValue;

    public int currentLuckyWheel;

    public int hammerBoosterValue;

    public int moveBoosterValue;

    public int shuffleBoosterValue;

    [HideInInspector] public int prefilledColorIndex;

     public bool isProgressFinished = false;

    public enum GAME_STATE
    {
        HOME,
        READY,
        PLAYING,
        SHOW_POPUP,
        GAME_WIN,
        GAME_OVER
    }

    public GAME_STATE currentGameState;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    #region Blocker IncreaseDecrese
    public void IncreaseWoodCount()
    {
        boardGenerator.currentWoodGoalNumber--;

        if (boardGenerator.currentWoodGoalNumber > 0)
        {
            uiManager.gameView.woodGoalText.text = boardGenerator.currentWoodGoalNumber.ToString();
        }

        if (boardGenerator.currentWoodGoalNumber <= 0)
        {
            woodTick.SetActive(true);
            woodGoal.SetActive(false);
            if (boardGenerator.currentGoalNumber <= 0 &&
                    boardGenerator.currentWoodGoalNumber <= 0 &&
                    boardGenerator.currentHoneyGoalNumber <= 0 &&
                    boardGenerator.currentGrassGoalNumber <= 0)
            {
                StartCoroutine(GameManager.instance.ShowGameWin());
            }
        }
    }
    public void IncreaseHoneyCount()
    {
        boardGenerator.currentHoneyGoalNumber--;
        if (boardGenerator.currentHoneyGoalNumber > 0)
        {
            uiManager.gameView.honeyGoalText.text = boardGenerator.currentHoneyGoalNumber.ToString();
        }

        if(boardGenerator.currentHoneyGoalNumber <= 0)
        {
            honeyTick.SetActive(true);
            honeyGoal.SetActive(false);
            if (boardGenerator.currentGoalNumber <= 0 &&
                    boardGenerator.currentWoodGoalNumber <= 0 &&
                    boardGenerator.currentHoneyGoalNumber <= 0 &&
                    boardGenerator.currentGrassGoalNumber <= 0)
            {
                StartCoroutine(GameManager.instance.ShowGameWin());
            }
        }
    }
    public void IncreaseGrassCount()
    {
        Debug.Log("callingGrass");
        boardGenerator.currentGrassGoalNumber --;
        if (boardGenerator.currentGrassGoalNumber > 0)
        {
            uiManager.gameView.grassGoalText.text = boardGenerator.currentGrassGoalNumber.ToString();
        }
        if (boardGenerator.currentGrassGoalNumber <= 0)
        {
            grassTick.SetActive(true);
            grassGoal.SetActive(false);
            if (boardGenerator.currentGoalNumber <= 0 &&
                    boardGenerator.currentWoodGoalNumber <= 0 &&
                    boardGenerator.currentHoneyGoalNumber <= 0 &&
                    boardGenerator.currentGrassGoalNumber <= 0)
            {
                StartCoroutine(GameManager.instance.ShowGameWin());
            }
        }
    }
    
    public void ResetBlockerValue()
    {
        boardGenerator.currentWoodGoalNumber = 0;
        boardGenerator.currentHoneyGoalNumber = 0;
        boardGenerator.grassGoalNumber = 0;

        woodTick.SetActive(false);
        woodGoal.SetActive(true);
        honeyTick.SetActive(false);
        honeyGoal.SetActive(true);
        grassTick.SetActive(false);
        grassGoal.SetActive(true);
    }

    #endregion

    public void LoadGame()
    {
        LoadGameData();

        if (AdsControl.Instance.directPlay)
        {
            uiManager.homeView.PlayGame();
            return;
        }
        else
        {
            ShowHome();
        }
    }
    private void LoadGameData()
    {
        if (PlayerPrefs.GetInt("FirstGame") == 0)
        {
            levelIndex = 1;
           // coinValue = 50;
            hammerBoosterValue = 1;
            moveBoosterValue = 1;
            shuffleBoosterValue = 1;
            currentLuckyWheel = 1;

            coinValue = PlayerPrefs.GetInt("Coin", 50);
            PlayerPrefs.SetInt("FirstGame", 1);
            PlayerPrefs.SetInt("Coin", coinValue);
            PlayerPrefs.SetInt("Hammer", hammerBoosterValue);
            PlayerPrefs.SetInt("Move", moveBoosterValue);
            PlayerPrefs.SetInt("Shuffle", shuffleBoosterValue);
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            PlayerPrefs.SetInt("CurrentLuckyWheel", currentLuckyWheel);
        }
        else
        {
            coinValue = PlayerPrefs.GetInt("Coin" , 50);
            levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
            hammerBoosterValue = PlayerPrefs.GetInt("Hammer");
            moveBoosterValue = PlayerPrefs.GetInt("Move");
            shuffleBoosterValue = PlayerPrefs.GetInt("Shuffle");
            currentLuckyWheel = PlayerPrefs.GetInt("CurrentLuckyWheel");
        }

        FirebaseManager.instance.LogStartEvent(levelIndex);

        if (isTestMode)
            levelIndex = levelTest;
    }


    public void LoadGameData(int level, bool isTestMode = false)
    {
        if (PlayerPrefs.GetInt("FirstGame") == 0)
        {
            levelIndex = 1;
            // coinValue = 50;
            hammerBoosterValue = 1;
            moveBoosterValue = 1;
            shuffleBoosterValue = 1;
            currentLuckyWheel = 1;

            coinValue = PlayerPrefs.GetInt("Coin", 50);
            PlayerPrefs.SetInt("FirstGame", 1);
            PlayerPrefs.SetInt("Coin", coinValue);
            PlayerPrefs.SetInt("Hammer", hammerBoosterValue);
            PlayerPrefs.SetInt("Move", moveBoosterValue);
            PlayerPrefs.SetInt("Shuffle", shuffleBoosterValue);
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            PlayerPrefs.SetInt("CurrentLuckyWheel", currentLuckyWheel);
        }
        else
        {
            coinValue = PlayerPrefs.GetInt("Coin", 50);
            levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
            hammerBoosterValue = PlayerPrefs.GetInt("Hammer");
            moveBoosterValue = PlayerPrefs.GetInt("Move");
            shuffleBoosterValue = PlayerPrefs.GetInt("Shuffle");
            currentLuckyWheel = PlayerPrefs.GetInt("CurrentLuckyWheel");
        }

        if (isTestMode)
            levelIndex = level;

        //PlayerPrefs.SetInt("CurrentLevel", level);
    }

    private void ShowHome()
    {
        currentGameState = GAME_STATE.HOME;
        poolManager.InitPool();
        uiManager.homeView.InitView();
        uiManager.homeView.ShowView();
        uiManager.coinView.InitView();
        uiManager.coinView.ShowView();
        uiManager.coinView.coinContent.gameObject.SetActive(true);
        uiManager.coinView.lifeContent.gameObject.SetActive(true);
    }

    private void InitGame()
    {
        currentGameState = GAME_STATE.READY;
        boardGenerator.InitBoardGenerator();
        boardController.InitBoardController();
        uiManager.homeView.HideView();
        uiManager.coinView.ShowView();
        uiManager.coinView.coinContent.gameObject.SetActive(true);
        uiManager.coinView.lifeContent.gameObject.SetActive(false);
        uiManager.gameView.InitView();
        uiManager.gameView.ShowView();
        uiManager.startGoalPanel.InitView();
        uiManager.startGoalPanel.ShowView();
    }

    public void PlayGame()
    {
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddCoin(1000);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowGameWin();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            uiManager.popupWin.HideView();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            uiManager.questGiftReward.SetModel(rewardConfig.rewardQuest3);
            uiManager.questGiftReward.InitView();
            uiManager.questGiftReward.ShowView();
        }
    }

    public IEnumerator ShowGameWin()
    {
        yield return new WaitUntil(() => isProgressFinished);

        yield return new WaitForSeconds(1f);

        currentGameState = GAME_STATE.GAME_WIN;
        //AudioManager.instance.winSound.Play();
        cellHolder.ClearCellHolder();
        boardController.ClearBoard();
        boardController.RotateAnim();
        uiManager.gameView.HideView();
        confetiVfx.Play();
        AudioManager.instance.confettiBlast.Play();

        StartCoroutine(ShowGameWinIE());
    }

    public void ShowGameLose()
    {
        currentGameState = GAME_STATE.GAME_OVER;
        //AudioManager.instance.winSound.Play();
        StartCoroutine(ShowGameLoseIE());
    }

    IEnumerator ShowGameWinIE()
    {
        FirebaseManager.instance.LogWinEvent(levelIndex);

        yield return new WaitForSeconds(.02f);
        AudioManager.instance.confettiBlast.Play();

        yield return new WaitForSeconds(2.0f);
        uiManager.popupWin.InitView();

        yield return new WaitForSeconds(0.1f);

        AdmobManager.instance.HideBannerAd();
        AdsControl.Instance.ShowInterstital(AdLocation.Win);

        if (levelIndex >= 1)
        {
            AdmobManager.instance.HideBannerAd();
            AdsControl.Instance.ShowInterstital(AdLocation.Win);
        }
        uiManager.popupWin.ShowView();

        if (!isTestMode)
        {
            levelIndex++;

            if (currentLuckyWheel < 5)
                currentLuckyWheel++;

            PlayerPrefs.SetInt("CurrentLuckyWheel", currentLuckyWheel);
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        }
        uiManager.questPopup.IncreaseProgressQuest(0, 1);
        uiManager.questPopup.IncreaseProgressQuest(4, 1);
    }


    IEnumerator ShowGameLoseIE()
    {
        FirebaseManager.instance.LogLoseEvent(levelIndex);
        yield return new WaitForSeconds(1.0f);

        if (levelIndex >= 1)
        {
            AdmobManager.instance.HideBannerAd();
            AdsControl.Instance.ShowInterstital(AdLocation.Win);
        }

        uiManager.popupLose.InitView();
        uiManager.popupLose.ShowView();
    }

    public void Replay()
    {
        uiManager.popupWin.HideView();
        InitGame();
    }

    public void ReplayNow()
    {
        InitGame();
    }

    public void NextLevel()
    {
        uiManager.popupWin.HideView();
        //InitGame();
    }

    public void BackToHome()
    {
        currentGameState = GAME_STATE.HOME;
        cellHolder.ClearCellHolder();
        boardController.ClearBoard();
        boardGenerator.ClearMap();
        uiManager.gameView.DisableArrow();
        uiManager.gameView.DisableArrow_2();
        uiManager.gameView.HideView();
        uiManager.homeView.InitView();
        uiManager.homeView.ShowView();
        uiManager.coinView.InitView();
        uiManager.coinView.ShowView();
        uiManager.coinView.coinContent.gameObject.SetActive(true);
        uiManager.coinView.lifeContent.gameObject.SetActive(true);
    }

    public void AddCoin(int moreCoin)
    {
        StartCoroutine(AddCoinIE(moreCoin));
    }

    public void SubCoin(int subCoin)
    {
        coinValue -= subCoin;
        PlayerPrefs.SetInt("Coin", coinValue);
        uiManager.coinView.UpdateCoinTxt();
    }

    IEnumerator AddCoinIE(int moreCoin)
    {
        //uiManager.coinView.SpawnCoin(Vector3.zero - new Vector3(0.0f, 10.0f, 0.0f));
        coinValue += moreCoin;
        uiManager.coinView.UpdateCoinTxt();
        PlayerPrefs.SetInt("Coin", coinValue);
        /*
        uiManager.questPopup.IncreaseProgressQuest(1, moreCoin);
        uiManager.questPopup.IncreaseProgressQuest(3, moreCoin);*/

        yield return null;
    }

    public void AddHammerBooster(int moreValue)
    {
        hammerBoosterValue += moreValue;
        PlayerPrefs.SetInt("Hammer", hammerBoosterValue);
    }

    public void AddMoveBooster(int moreValue)
    {
        moveBoosterValue += moreValue;
        PlayerPrefs.SetInt("Move", moveBoosterValue);
    }

    public void AddShuffleBooster(int moreValue)
    {
        shuffleBoosterValue += moreValue;
        PlayerPrefs.SetInt("Shuffle", shuffleBoosterValue);
    }
}
