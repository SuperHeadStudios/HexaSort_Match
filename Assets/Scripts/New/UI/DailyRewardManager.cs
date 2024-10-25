using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static RewardButton;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager instance;

    private const string LastClaimDateKey = "LastClaimDate";
    private const string CurrentDayKey = "CurrentDay";

    [Header("----- Reward Buttons -----"), Space(5)]
    [SerializeField] private RewardButton[] rewardButtons;

    [Header("----- Reward Cycle Settings -----"), Space(5)]
    [SerializeField] private int totalDays = 7;

    [Header("----- Global Claim Button -----"), Space(5)]
    [SerializeField] private Button globalClaimButton;

    private int currentDay;
    private DateTime lastClaimDate;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        LoadProgress();
        UpdateUI();
        globalClaimButton.onClick.AddListener(ClaimCurrentDayRewards);
    }

    private void LoadProgress()
    {
        currentDay = PlayerPrefs.GetInt(CurrentDayKey, 1);
        string lastClaimDateStr = PlayerPrefs.GetString(LastClaimDateKey, DateTime.MinValue.ToString());
        lastClaimDate = DateTime.Parse(lastClaimDateStr);
        CheckDay(); // Ensure we check if a new day has started
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(CurrentDayKey, currentDay);
        PlayerPrefs.SetString(LastClaimDateKey, DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void CheckDay()
    {
        // Check if the day has changed
        if (DateTime.Now.Date > lastClaimDate.Date)
        {
            // A new day has started, increment the current day
            currentDay = (currentDay % totalDays) + 1;
            // Update the last claim date
            lastClaimDate = DateTime.Now;
            SaveProgress();
        }
    }

    public void ClaimCurrentDayRewards()
    {
        // Only claim if the current day matches the day's reward
        if (currentDay <= totalDays)
        {
            // Check if the reward for today has already been claimed
            if (!IsRewardClaimed(currentDay))
            {
                ClaimReward(currentDay);
            }
            else
            {
                Debug.Log("Reward for today has already been claimed.");
            }
        }
    }

    public void ClaimReward(int day)
    {
        rewardButtons[day - 1].MarkAsCollected(); // Mark as done

        // Grant rewards based on the current day
        if (day == totalDays)
        {
            // Grant all rewards on the last day
            GrantReward(day, REWARD_TYPE.GOLD, 100); // Example
            GrantReward(day, REWARD_TYPE.HAMMER, 1);
            GrantReward(day, REWARD_TYPE.MOVE, 1);
            GrantReward(day, REWARD_TYPE.SHUFFLE, 1);
        }
        else
        {
            // Grant single reward based on the current reward type
            GrantReward(day, rewardButtons[day - 1].currentRewardType, rewardButtons[day - 1].rewardValue);
        }

        // Update UI after claiming
        UpdateUI();
    }

    private void GrantReward(int day, REWARD_TYPE rewardType, int value)
    {
        switch (rewardType)
        {
            case REWARD_TYPE.GOLD:
                GameManager.instance.AddCoin(value);
                break;
            case REWARD_TYPE.HAMMER:
                GameManager.instance.AddHammerBooster(value);
                break;
            case REWARD_TYPE.MOVE:
                GameManager.instance.AddMoveBooster(value);
                break;
            case REWARD_TYPE.SHUFFLE:
                GameManager.instance.AddShuffleBooster(value);
                break;
            case REWARD_TYPE.ALL:
                GameManager.instance.AddCoin(150);
                GameManager.instance.AddHammerBooster(1);
                GameManager.instance.AddMoveBooster(1);
                GameManager.instance.AddShuffleBooster(1);
                break;
        }
        // Mark the reward as claimed
        MarkRewardAsClaimed(day);
    }

    private bool IsRewardClaimed(int day)
    {
        return PlayerPrefs.GetInt($"Day{day}Claimed", 0) == 1; // 1 means claimed
    }

    private void MarkRewardAsClaimed(int day)
    {
        PlayerPrefs.SetInt($"Day{day}Claimed", 1); // Mark as claimed
        PlayerPrefs.Save();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < totalDays; i++)
        {
            int dayIndex = i + 1;
            if (dayIndex < currentDay)
                rewardButtons[i].SetDoneState();
            else if (dayIndex == currentDay)
                rewardButtons[i].SetActiveState();
            else
                rewardButtons[i].SetUpcomingState();
        }
    }
}
