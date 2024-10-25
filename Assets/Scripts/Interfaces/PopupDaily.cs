using System;
using UnityEngine;

public class PopupDaily : BasePopup
{
    private long mLastUnitTime;

    private int mLastSecondInDay;

    public RewardButton[] dailyViewArr;

    private const int SECONDS_PER_DAY = 24 * 60 * 60;

    public void CheckNewDay()
    {
        long timeStamp = (long)(DateTime.UtcNow.Subtract(new DateTime(2019, 1, 1))).TotalSeconds;
        int currentSecondInDay = (int)(DateTime.Now - DateTime.Today).TotalSeconds;

        if (currentSecondInDay == (mLastUnitTime + SECONDS_PER_DAY))
        {
            mLastUnitTime = timeStamp;
            mLastSecondInDay = currentSecondInDay;
        }
    }


    public override void Start()
    {
       
    }

    public override void Update()
    {
        
    }

    public override void InitView()
    {
        /*long dailyKey = (long)(DateTime.Today.Subtract(new DateTime(2019, 1, 1))).TotalDays;

        for (int i = 0; i < dailyViewArr.Length; i++)
        {
            dailyViewArr[i].itemIndex = i;
            dailyViewArr[i].InitItem();

            // Check if the reward for the day hasn't been claimed yet (value is 0)
            if (PlayerPrefs.GetInt("Daily_" + dailyKey.ToString() + "_" + i.ToString()) == 0)
            {
                dailyViewArr[i].EnableItem(); // Make the reward claimable
            }
            else
            {
                dailyViewArr[i].CheckCurrentReward(); // Show claimed state or relevant logic
            }
        }*/
    }
}
