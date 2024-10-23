using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoosterPopup : MonoBehaviour
{
    public enum BoosterState
    {
         Hammer,
         Refresh,
         Swap
    }

    [Header("----- UI Settings -----"), Space(5)]
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite swapSprite;
    [SerializeField] private Sprite refreshSprite;
    [SerializeField] private TextMeshProUGUI tittleText;
    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("----- Boster Btns -----"), Space(5)]
    [SerializeField] private Button hammerBtn;
    [SerializeField] private Button swapBtn;
    [SerializeField] private Button refreshBtn;

    void Start()
    {
        hammerBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Hammer);
        } );

        swapBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Swap);
        });

        refreshBtn.onClick.AddListener(() =>
        {
            ChangeBooster(BoosterState.Refresh);
        });
    }

    public void ChangeBooster(BoosterState state) 
    {
        switch (state)
        {
            case BoosterState.Hammer:
                SetCurrentBoosterData(hammerSprite, "HAMMER", "Tap any flower tile stack to clear it");
                break;

            case BoosterState.Swap:
                SetCurrentBoosterData(swapSprite, "SWAP", "Move And Replace A flower tile stack On The Board");
                break;

            case BoosterState.Refresh:
                SetCurrentBoosterData(refreshSprite, "REFRESH", "Refresh Tray To Get New Stack Options");
                break;
        }
    }

    public void SetCurrentBoosterData(Sprite currentSprite, string tittle,string dialogue)
    {
        iconImage.sprite = currentSprite;
        tittleText.text = tittle;
        DialogueText.text = dialogue;
    }
}
