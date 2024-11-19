using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheelController : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private Transform wheel;
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private float initialSpeed = 1000f;
    [SerializeField] private Transform roots;

    [Header("UI and Assets")]
    [SerializeField] private GameObject resultObject;
    [SerializeField] private TextMeshProUGUI rewardValueTxt;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Sprite coinSpr, hammerSpr, moveSpr, shuffleSpr;
    [SerializeField] private GameObject[] selectObjList;
    [SerializeField] private GameObject freeBtn, adsBtn, closeBtn, closeTextBtn;
/*
    [Header("Audio")]
    [SerializeField] private AudioSource spinSound;
    [SerializeField] private AudioSource rewardSound;*/

    private enum RewardType { CoinX5, ShuffleX2, SwapX2, CoinX10, HammerX1, MoveX3 }
    private int spinCount = 3;
    private bool isSpinning;

    private void Start()
    {
        ResetRewardDisplay();
    }

    public void SpinWheel()
    {
        if (isSpinning) return;

        isSpinning = true;
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        float anglePerReward = 360f / System.Enum.GetValues(typeof(RewardType)).Length;
        float targetAngle = Random.Range(0, 360f);
        int rewardIndex = Mathf.FloorToInt((360f - targetAngle) / anglePerReward) % System.Enum.GetValues(typeof(RewardType)).Length;

        float finalAngle = 360f * 3 + targetAngle; // Add extra spins
        float currentAngle = 0f;
        float spinSpeed = initialSpeed;

        /*if (spinSound) spinSound.Play();*/

        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / spinDuration;
            spinSpeed = Mathf.Lerp(initialSpeed, 0, EaseOutQuad(t)); // Smoothly reduce speed

            currentAngle += spinSpeed * Time.deltaTime;
            wheel.localRotation = Quaternion.Euler(0, 0, -currentAngle);

            yield return null;
        }

        wheel.localRotation = Quaternion.Euler(0, 0, -finalAngle);

        /*if (spinSound) spinSound.Stop();
        if (rewardSound) rewardSound.Play();*/

        DisplayReward(rewardIndex);
        isSpinning = false;
    }

    private float EaseOutQuad(float t)
    {
        return t * (2 - t);
    }

    private void DisplayReward(int rewardIndex)
    {
        resultObject.SetActive(true);
        selectObjList[rewardIndex].SetActive(true);

        RewardType reward = (RewardType)rewardIndex;
        UpdateRewardUI(reward);
        UpdateButtons();

    }

    private void UpdateRewardUI(RewardType reward)
    {
        switch (reward)
        {
            case RewardType.CoinX5:
                SetRewardUI(coinSpr, "X5", () => GameManager.instance.AddCoin(5));
                break;
            case RewardType.ShuffleX2:
                SetRewardUI(shuffleSpr, "X2", () => GameManager.instance.AddShuffleBooster(2));
                break;
            case RewardType.SwapX2:
                SetRewardUI(shuffleSpr, "X2", () => GameManager.instance.AddShuffleBooster(2));
                break;
            case RewardType.CoinX10:
                SetRewardUI(coinSpr, "X10", () => GameManager.instance.AddCoin(10));
                break;
            case RewardType.HammerX1:
                SetRewardUI(hammerSpr, "X1", () => GameManager.instance.AddHammerBooster(1));
                break;
            case RewardType.MoveX3:
                SetRewardUI(moveSpr, "X3", () => GameManager.instance.AddMoveBooster(3));
                break;
        }
    }

    private void SetRewardUI(Sprite icon, string value, System.Action rewardAction)
    {
        rewardIcon.sprite = icon;
        rewardValueTxt.text = value;
        rewardAction.Invoke();
        roots.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear);
    }

    private void UpdateButtons()
    {
        bool spinsLeft = --spinCount > 0;

        freeBtn.SetActive(spinsLeft);
        adsBtn.SetActive(!spinsLeft);
        closeBtn.SetActive(!spinsLeft);
        closeTextBtn.SetActive(spinsLeft);
    }

    private void ResetRewardDisplay()
    {
        rewardValueTxt.text = "Spin the Wheel!";
        resultObject.SetActive(false);
        foreach (var obj in selectObjList)
        {
            obj.SetActive(false);
        }
    }
}
