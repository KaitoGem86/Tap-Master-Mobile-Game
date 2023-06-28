using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;
    [SerializeField] int amount;
    [SerializeField] string rewardTag;
    bool isCollected;

    public void InitializeReward(DailyRewardData data)
    {
        icon.sprite = data.icon;
        amount = data.amount;
        rewardTag = data.rewardTag;
    }

    public void CollectReward()
    {
        if (rewardTag.Contains("Coin"))
            CollectCoin();
    }

    public void CollectCoin()
    {
        isCollected = true;
        button.interactable = false;
        int coin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", coin + this.amount);
    }
}
