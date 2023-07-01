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
        GetIndex();
        UpdateDailyRewardList();
    }

    private void OnEnable()
    {
        if (count == 0)
            return;
        GetIndex();
        UpdateDailyRewardList();
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
        Debug.Log("Init");
    }

    public void UpdateDailyRewardList()
    {
        //dailyReward.Add(this.GetComponentInChildren<DailyRewardItem>());
        //dailyReward.Add(this.specialReward.GetComponent<DailyRewardItem>());
        for (int i = 0; i < dailyReward.Count; i++)
        {
            Debug.Log(i);
            //Debug.Log(i);
            Debug.Log("Update");
            dailyReward[i].InitializeReward(data.dataList[i]);
        }
    }

    public void GetIndex()
    {
        //check datetime of this game opening

        collectIndex = PlayerPrefs.GetInt("Collect daily reward at index: ", 0);
        Debug.Log(collectIndex);
        for (int i = 0; i < collectIndex; i++)
        {
            dailyReward[i].IsCollected = true;
        }

        for (int i = collectIndex; i < dailyReward.Count; i++)
        {
            dailyReward[i].Button.interactable = false;
            Debug.Log(dailyReward[i].Button.interactable + " - " + i);
        }
        if (CheckCanCollectTime() || !isDailyCollected)
            dailyReward[collectIndex].Button.interactable = true;
        count = 1;
    }

    public void ResetList()
    {
        foreach (DailyRewardItem item in dailyReward)
        {
            item.IsCollected = false;
        }
    }

    bool CheckCanCollectTime()
    {
        now = DateTime.Now;
        DateTime oldDate;
        if (DateTime.TryParse(PlayerPrefs.GetString("The last time logged in"), out oldDate))
        {
            if (oldDate != now && now.Minute != oldDate.Minute)
            {
                isDailyCollected = false;
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
}