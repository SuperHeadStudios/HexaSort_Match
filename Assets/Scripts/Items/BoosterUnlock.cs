using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUnlock : MonoBehaviour
{

    [Header("----- Booster Unlock Popup-----"), Space(5)]
    [SerializeField] private RectTransform rays;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject hammeerPopup;
    [SerializeField] private GameObject swapPopup;
    [SerializeField] private GameObject shufflePopup;

    [Header("----- Hamer Lock -----"), Space(5)]
    [SerializeField] private Button hammerBtn;
    [SerializeField] private Sprite lockedHammerSprit;
    [SerializeField] private Sprite unlockHammerSprit;
    [Space(5)]  
    [SerializeField] private GameObject hammerCountIcon;
    [SerializeField] private GameObject swapCountIcon;
    [SerializeField] private GameObject shuffleCountIcon;
    [SerializeField] private GameObject hammerLockIcon;

    [Header("----- Swap Lock -----"), Space(5)]
    [SerializeField] private Button swapBtn;
    [SerializeField] private Sprite lockedSwapSprit;
    [SerializeField] private Sprite unlockSwapSprit;
    [SerializeField] private GameObject swapLockIcon;

    [Header("----- Shuffle Lock -----"), Space(5)]
    [SerializeField] private Button shuffleBtn;
    [SerializeField] private Sprite lockedshuffleSprit;
    [SerializeField] private Sprite unlockshuffleSprit;
    [SerializeField] private GameObject shuffleLockIcon;

    private int levelIndex;

    private void OnEnable()
    {
        levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        transform.localScale = Vector3.zero;
        LockUnlockHammer();
        LockUnlockShuffle();
        LockUnlockSwap();
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            levelIndex++;
        }
    }

    private void LockUnlockHammer()
    {
        if(levelIndex >= 3)
        {
            hammerBtn.enabled = true;
            hammerBtn.image.sprite = unlockHammerSprit;

            if(GameManager.instance.hammerBoosterValue > 0)
            {
                hammerCountIcon.SetActive(true);
            }
            else
            {
                hammerCountIcon.SetActive(false);
            }
            hammerLockIcon.SetActive(false);
        }
        else
        {
            hammerCountIcon.SetActive(false);
            hammerBtn.enabled = false;
            hammerBtn.image.sprite = lockedHammerSprit;
            hammerLockIcon.SetActive(true);
        }
    }

    private void LockUnlockSwap()
    {
        if (levelIndex >= 5)
        {
            swapBtn.enabled = true;
            swapBtn.image.sprite = unlockSwapSprit;

            if (GameManager.instance.moveBoosterValue > 0)
            {
                swapCountIcon.SetActive(true);
            }
            else
            {
                swapCountIcon.SetActive(false);
            }

            swapLockIcon.SetActive(false);
        }
        else
        {
            swapCountIcon.SetActive(false);
            swapBtn.enabled = false;
            swapBtn.image.sprite = lockedSwapSprit;
            swapLockIcon.SetActive(true);
        }
    }

    private void LockUnlockShuffle()
    {
        if (levelIndex >= 8)
        {
            shuffleBtn.enabled = true;
            shuffleBtn.image.sprite = unlockshuffleSprit;

            if (GameManager.instance.shuffleBoosterValue > 0)
            {
                shuffleCountIcon.SetActive(true);
            }
            else
            {
                shuffleCountIcon.SetActive(false);
            }
            shuffleLockIcon.SetActive(false);
        }
        else
        {
            shuffleCountIcon.SetActive(false);
            shuffleBtn.enabled = false;
            shuffleBtn.image.sprite = lockedshuffleSprit;
            shuffleLockIcon.SetActive(true);
        }
    }


    private void Update()
    {
        rays.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }


    public void ShopBoosterUnlockPopup()
    {
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);

        hammeerPopup.SetActive(false);
        swapPopup.SetActive(false);
        shufflePopup.SetActive(false);

        switch (levelIndex)
        {
            case 3:
                hammeerPopup.SetActive(true);
                break; 
            case 5:
                swapPopup.SetActive(true);
                break; 
            case 8:
                shufflePopup.SetActive(true);
                break;
        }
    }

    public void HidePopup()
    {
        transform.DOScale(Vector3.zero, 0.01f);
        LockUnlockHammer();
        LockUnlockShuffle();
        LockUnlockSwap();
    }
}
