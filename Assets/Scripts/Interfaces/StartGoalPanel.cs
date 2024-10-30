using DG.Tweening;
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
        rootTrans.localScale = Vector3.one * 0.15f;
        rootTrans.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            HideView();
        });
    }

    public override void HideView()
    {
        rootTrans.DOScale(Vector3.one * 0.5f, 0.25f).SetDelay(1.0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            boosterPannel.ShopBoosterUnlockPopup();
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isShow = false;
            GameManager.instance.currentGameState = GameManager.GAME_STATE.PLAYING;
        });
        
            
       
    }
}
