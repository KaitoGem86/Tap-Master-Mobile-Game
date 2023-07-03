using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardSystem : MonoBehaviour
{
    public static bool isDailyCollected;

    public static bool CheckCanCollectTime()
    {
        DateTime now = DateTime.Now;
        DateTime oldDate;
        if (DateTime.TryParse(PlayerPrefs.GetString("The last time logged in"), out oldDate))
        {
            if (oldDate != now && now.Minute != oldDate.Minute)
            {
                isDailyCollected = false;
                PlayerPrefs.SetInt("Collected Daily Reward", 0);
                PlayerPrefs.SetString("The last time logged in", now.ToString());
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("No logged in");
            PlayerPrefs.SetString("The last time logged in", now.ToString());
            return true;
        }
    }

    public static bool CheckCanCollect()
    {
        return CheckCanCollectTime() || !isDailyCollected;
    }
}
