using UnityEngine;
using UnityEngine.UI;
using System;
using static RewardButton;

public class DailyLoginRewardManager : MonoBehaviour
{
    public static DailyLoginRewardManager instance;

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
        else
            Destroy(gameObject); // Ensure only one instance exists
    }

    private void Start()
    {
        LoadProgress();
        Debug.Log($"Loaded progress - Current Day: {currentDay}, Last Claim Date: {lastClaimDate}");
        UpdateUI();
        globalClaimButton.onClick.AddListener(ClaimCurrentDayRewards);
        UpdateClaimButtonState();
    }

    private void LoadProgress()
    {
        Debug.Log(PlayerPrefs.GetString(LastClaimDateKey, DateTime.MinValue.Ticks.ToString()));

        currentDay = PlayerPrefs.GetInt(CurrentDayKey, 1);

        // Parse last claim date, default to DateTime.MinValue if parsing fails
        if (long.TryParse(PlayerPrefs.GetString(LastClaimDateKey, DateTime.MinValue.Ticks.ToString()), out long lastClaimTicks))
        {
            lastClaimDate = new DateTime(lastClaimTicks);
        }
        else
        {
            lastClaimDate = DateTime.MinValue;
            Debug.LogWarning("Failed to parse last claim date. Defaulting to DateTime.MinValue.");
        }

        CheckDay();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(CurrentDayKey, currentDay);
        PlayerPrefs.SetString(LastClaimDateKey, DateTime.Now.Ticks.ToString());
        PlayerPrefs.Save();
    }

    private void CheckDay()
    {
        if (DateTime.Now.Date > lastClaimDate.Date)
        {
            currentDay = (currentDay % totalDays) + 1;
            lastClaimDate = DateTime.Now;
            SaveProgress();
            Debug.Log($"New day detected. Current day updated to {currentDay}");
        }
    }

    public void ClaimCurrentDayRewards()
    {
        Debug.Log($"Attempting to claim reward for day {currentDay}");
        if (currentDay <= totalDays)
        {
            if (!IsRewardClaimed(currentDay))
            {
                ClaimReward(currentDay);
                Debug.Log($"Reward successfully claimed for day {currentDay}");
                UpdateClaimButtonState();
            }
            else
            {
                Debug.Log("Reward for today has already been claimed.");
            }
        }
    }

    public void ClaimReward(int day)
    {
        rewardButtons[day - 1].MarkAsCollected();

        if (day == totalDays)
        {
            GrantReward(day, REWARD_TYPE.GOLD, 100);
            GrantReward(day, REWARD_TYPE.HAMMER, 1);
            GrantReward(day, REWARD_TYPE.MOVE, 1);
            GrantReward(day, REWARD_TYPE.SHUFFLE, 1);
        }
        else
        {
            GrantReward(day, rewardButtons[day - 1].currentRewardType, rewardButtons[day - 1].rewardValue);
        }

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
        MarkRewardAsClaimed(day);
        Debug.Log($"Granted {rewardType} reward of {value} for day {day}");
    }

    private bool IsRewardClaimed(int day)
    {
        return PlayerPrefs.GetInt($"Day{day}Claimed", 0) == 1;
    }

    private void MarkRewardAsClaimed(int day)
    {
        PlayerPrefs.SetInt($"Day{day}Claimed", 1);
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

    private void UpdateClaimButtonState()
    {
        globalClaimButton.interactable = !IsRewardClaimed(currentDay);
    }
}
