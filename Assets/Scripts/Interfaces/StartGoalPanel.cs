using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGoalPanel : BasePopup
{
    public TextMeshProUGUI levelTxt;

    public TextMeshProUGUI goalTxt;

    public TextMeshProUGUI woodTargetText;

    public TextMeshProUGUI honeyTargetText;
    
    public TextMeshProUGUI grassTargetText;

    public BoosterUnlock boosterPannel;
    public BlockerUnlockPopup blockerUnlockPopup;

    [SerializeField] private Transform[] allGoals;

    [SerializeField] private GameObject honeyGoal;
    [SerializeField] private GameObject woodGoal;
    [SerializeField] private GameObject grassGoal;

    public override void InitView()
    {
        woodTargetText.text = BoardController.instance.boardGenerator.woodGoalNumber.ToString();
        honeyTargetText.text = BoardController.instance.boardGenerator.honeyGoalNumber.ToString();
        grassTargetText.text = BoardController.instance.boardGenerator.grassGoalNumber.ToString();
        levelTxt.text = "Level " + GameManager.instance.levelIndex.ToString();
        goalTxt.text = BoardController.instance.boardGenerator.goalNumber.ToString();
    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void ShowView()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShow = true;
        StartCoroutine(StarGoalPanlePopup());
    }

    private IEnumerator StarGoalPanlePopup()
    {
        if (BoardController.instance.boardGenerator.woodGoalNumber > 0)
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

        goalTxt.transform.localScale = Vector3.zero;
        for (int i = 0; i < allGoals.Length; i++)
        {
            allGoals[i].transform.localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(0.3f);

        rootTrans.DOLocalMoveY(rootTrans.localPosition.y + 1400, .8f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            goalTxt.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            StartCoroutine(GoalsAnimation());
        });

    }

    private IEnumerator GoalsAnimation()
    {
        for (int i = 0; i < allGoals.Length; i++)
        {
            allGoals[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                if (i == 4)
                {
                    HideView();
                }
            });

            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void HideView()
    {
        rootTrans.DOLocalMoveY(rootTrans.position.y + 1400, 0.6f).SetDelay(0.8f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isShow = false;
            boosterPannel.ShowBoosterUnlockPopup();
            blockerUnlockPopup.ShowBlockerUnlockPopup();
            rootTrans.DOLocalMoveY(rootTrans.position.y - 1400, 0f);
        });
    }
}
