using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RewardType
{
    Coin,
    Puzzle
}


[Serializable]
public class DailyRewardData
{
    public Sprite icon;
    public int amount;
    public RewardType rewardType;
}
