using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DailyRewardListController : MonoBehaviour
{
    [Header("Daily Reward Database")]
    [SerializeField] DailyRewardList data;


    [SerializeField] GameObject specialReward;
    [SerializeField] GameObject dailyRewardItemPrefab;

    [Space]
    [Header("Daily Reward List")]
    [SerializeField] List<DailyRewardItem> dailyReward = new List<DailyRewardItem>();
    int collectIndex;
    int count;
    DateTime now;
    bool isDailyCollected = false;

    public bool IsDailyCollected
    {
        get { return isDailyCollected; }
        set { isDailyCollected = value; }
    }

    private void Start()
    {
        count = 0;
        if (this.transform.childCount == 0)
        {
            InitializeList();
        }
        if (DailyRewardSystem.CheckCanCollect())
        {
            GetIndex();
            UpdateDailyRewardList();
        }
    }

    private void OnEnable()
    {
        if (count == 0)
            return;
        if (DailyRewardSystem.CheckCanCollect())
        {
            GetIndex();
            UpdateDailyRewardList();
        }
    }

    void InitializeList()
    {
        for (int i = 0; i < 6; i++)
        {
            var go = Instantiate(dailyRewardItemPrefab, this.transform);
            dailyReward.Add(go.GetComponent<DailyRewardItem>());
            dailyReward[i].ListController = this;
        }
        dailyReward.Add(specialReward.GetComponent<DailyRewardItem>());
        dailyReward[6].ListController = this;
    }

    public void UpdateDailyRewardList()
    {
        //dailyReward.Add(this.GetComponentInChildren<DailyRewardItem>());
        //dailyReward.Add(this.specialReward.GetComponent<DailyRewardItem>());
        for (int i = 0; i < dailyReward.Count; i++)
        {
            dailyReward[i].InitializeReward(data.dataList[i]);
        }
    }

    public void GetIndex()
    {
        //check datetime of this game opening
        collectIndex = PlayerPrefs.GetInt("Collect daily reward at index: ", 0);
        for (int i = 0; i < collectIndex; i++)
        {
            dailyReward[i].IsCollected = true;
        }

        for (int i = collectIndex; i < dailyReward.Count; i++)
        {
            dailyReward[i].Button.interactable = false;
        }
        if (DailyRewardSystem.CheckCanCollect())
        {
            dailyReward[collectIndex].Button.interactable = true;
        }
        count = 1;
    }

    public void ResetList()
    {
        foreach (DailyRewardItem item in dailyReward)
        {
            item.IsCollected = false;
        }
    }
}