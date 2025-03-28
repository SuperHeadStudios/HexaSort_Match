using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public CanvasGroup contentGroup;

    public RectTransform rootTrans;

    [HideInInspector]
    public bool isShow;

    private GameManager.GAME_STATE lastState;

    // Start is called before the first frame update
    public abstract void Start();


    // Update is called once per frame
    public abstract void Update();

    public abstract void InitView();

    public virtual void ShowView()
    {
        CustomBannerAdManager.instance.ShowBottomBanner();
        //GameManager.instance.uiManager.coinView.HideView();
        lastState = GameManager.instance.currentGameState;
        GameManager.instance.currentGameState = GameManager.GAME_STATE.SHOW_POPUP;
        canvasGroup.DOFade(1, 0.25f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isShow = true; 
            rootTrans.gameObject.SetActive(true);
            rootTrans.localScale = Vector3.one * 0.45f;

            rootTrans.DOScale(Vector3.one * 0.9f, 0.35f).SetEase(Ease.OutQuart).OnComplete(() =>
            {

            });

        });
    }

    public virtual void ShowView(string content)
    {
        ShowView();
    }

    public virtual void HideView()
    {
        GameManager.instance.currentGameState = lastState;
        AudioManager.instance.clickSound.Play();
        rootTrans.DOScale(Vector3.one * 1f, 0.25f).SetEase(Ease.InQuart).OnComplete(() => 
        {
            canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                isShow = false;
                GameManager.instance.uiManager.coinView.ShowView();
                CustomBannerAdManager.instance.HideBottomBanner();
                rootTrans.localScale = Vector3.one * 0.9f;
                rootTrans.gameObject.SetActive(false);
            });
        });
    }
}
