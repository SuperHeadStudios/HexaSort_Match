using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FillBarManager : MonoBehaviour
{
    [Header("Fill Bar Components")]
    [SerializeField] private Image fillImage;
    [SerializeField] private int totalPoints;
    [SerializeField] private float fillDuration = 0.5f;
    [SerializeField] private DailyRewardManager dailyRewardManager;

    [Header("Point Objects")]
    [SerializeField] private GameObject[] dots;
    [SerializeField] private GameObject[] ticks;

    private int currentIndex = 0;
    int dotIndex = 0;

    private void Start()
    {
        currentIndex = dailyRewardManager.GetRewardIndex();
        InitializeBar();
    }

    private void InitializeBar()
    {
        fillImage.fillAmount = 0;

        for (int i = 0; i < totalPoints; i++)
        {
            if (i < currentIndex)
            {
                ticks[i].SetActive(true);
            }
            else
            {
                ticks[i].SetActive(false);
            }

            if(i== currentIndex)
            {
                dots[i].SetActive(true);
            }
            else
            {
                dots[i].SetActive(false);
            }
        }

        StartCoroutine(DoFill());
    }

    public void IncreaseProgress()
    {
        currentIndex = dailyRewardManager.GetRewardIndex();
        if (currentIndex >= totalPoints)
            return;

        StartCoroutine(DoFill());
    }

    private IEnumerator DoFill()
    { 
        float targetFill = (float)(currentIndex) / 3;
        float initialFill = fillImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Lerp(initialFill, targetFill, elapsedTime / fillDuration);
            yield return null;
        }

        fillImage.fillAmount = targetFill;

        if (currentIndex < totalPoints)
        {
            dots[currentIndex].SetActive(true);
        }

        if (currentIndex > 0)
        {
            ticks[currentIndex - 1].SetActive(true);
        }

      
    }

    public void ResetFillbar()
    {
        currentIndex = dailyRewardManager.GetRewardIndex();
        InitializeBar();
    }

    public void FourthTikc()
    {
        currentIndex = dailyRewardManager.GetRewardIndex();
        if (currentIndex == 3)
        {
            ticks[currentIndex].SetActive(true);
        }
    }
}
