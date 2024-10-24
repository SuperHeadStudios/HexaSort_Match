using System;
using UnityEngine;

public class PopupDaily : BasePopup
{
    private long mLastUnitTime;

    private int mLastSecondInDay;

    public DailyItemView[] dailyViewArr;

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
        long dailyKey = (long)(DateTime.Today.Subtract(new DateTime(2019, 1, 1))).TotalSeconds;

        for (int i = 0; i < dailyViewArr.Length; i++)
        {
            dailyViewArr[i].itemIndex = i;
            dailyViewArr[i].InitItem();
            if (PlayerPrefs.GetInt("Daily" + dailyKey.ToString() + i.ToString()) == 0)
            {
                dailyViewArr[i].EnableItem();
            }
            else
            {
                dailyViewArr[i].CheckCurrentReward();
            }
        }
    }
}
