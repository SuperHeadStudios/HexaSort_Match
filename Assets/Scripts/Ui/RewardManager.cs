using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public List<RewardData> hourlyRewards; // 1-hour reset rewards
    public List<RewardData> dailyRewards; // 4-24 hour reset rewards

    public TextMeshProUGUI hourlyCountdownText; // UI Text for hourly countdown
    public TextMeshProUGUI dailyCountdownText; // UI Text for daily countdown
    public Button claimHourlyButton; // Button to claim hourly reward
    public Button claimDailyButton; // Button to claim daily rewards

    private void Start()
    {
        LoadRewardData();
        UpdateUI();
        InvokeRepeating(nameof(UpdateTimers), 0, 1f); // Update timers every second
    }

    private void UpdateTimers()
    {
        // Update hourly reward timer
        foreach (var reward in hourlyRewards)
        {
            if (reward.isClaimed)
            {
                reward.resetTime -= 1f;
                if (reward.resetTime <= 0)
                {
                    reward.isClaimed = false; // Reset reward
                    reward.resetTime = 3600f; // Reset to 1 hour
                }
            }
        }

        // Update daily rewards timer
        foreach (var reward in dailyRewards)
        {
            if (reward.isClaimed)
            {
                reward.resetTime -= 1f;
                if (reward.resetTime <= 0)
                {
                    reward.isClaimed = false; // Reset reward
                    reward.resetTime = 86400f; // Reset to 24 hours
                }
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update hourly countdown text
        foreach (var reward in hourlyRewards)
        {
            if (reward.isClaimed)
            {
                hourlyCountdownText.text = TimeSpan.FromSeconds(reward.resetTime).ToString(@"hh\:mm\:ss");
                claimHourlyButton.interactable = false; // Disable button
            }
            else
            {
                hourlyCountdownText.text = "Ready!";
                claimHourlyButton.interactable = true; // Enable button
            }
        }

        // Update daily countdown text
        foreach (var reward in dailyRewards)
        {
            if (reward.isClaimed)
            {
                dailyCountdownText.text = TimeSpan.FromSeconds(reward.resetTime).ToString(@"hh\:mm\:ss");
                claimDailyButton.interactable = false; // Disable button
            }
            else
            {
                dailyCountdownText.text = "Ready!";
                claimDailyButton.interactable = true; // Enable button
            }
        }
    }

    public void ClaimHourlyReward()
    {
        foreach (var reward in hourlyRewards)
        {
            if (!reward.isClaimed)
            {
                reward.isClaimed = true; // Claim the reward
                reward.resetTime = 3600f; // Reset to 1 hour
                // Add logic to give the reward to the player
                break;
            }
        }
    }

    public void ClaimDailyReward()
    {
        foreach (var reward in dailyRewards)
        {
            if (!reward.isClaimed)
            {
                reward.isClaimed = true; // Claim the reward
                reward.resetTime = 86400f; // Reset to 24 hours
                // Add logic to give the reward to the player
                break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveRewardData();
    }

    private void SaveRewardData()
    {
        string jsonData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("RewardData", jsonData);
        PlayerPrefs.Save();
    }

    private void LoadRewardData()
    {
        if (PlayerPrefs.HasKey("RewardData"))
        {
            string jsonData = PlayerPrefs.GetString("RewardData");
            JsonUtility.FromJsonOverwrite(jsonData, this);
        }
        else
        {
            // Initialize default values if no data exists
            foreach (var reward in hourlyRewards)
            {
                reward.isClaimed = false;
                reward.resetTime = 0f; // Start with unclaimed rewards
            }

            foreach (var reward in dailyRewards)
            {
                reward.isClaimed = false;
                reward.resetTime = 0f; // Start with unclaimed rewards
            }
        }
    }
}

[System.Serializable]
public class RewardData
{
    public string rewardName;
    public float rewardValue; // Adjust according to your reward type
    public float resetTime; // Time when the reward resets (in seconds)
    public bool isClaimed; // Flag to check if the reward is claimed
}

