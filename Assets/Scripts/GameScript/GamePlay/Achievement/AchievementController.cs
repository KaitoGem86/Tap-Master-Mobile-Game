using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : BaseAchievement
{
    private int id;
    private string taskTag;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Image icon;
    [SerializeField] public AchievementBar bar;
    [SerializeField] private AchievementListController list;
    [SerializeField] private int goalValue;
    [SerializeField] private int reward;
    [SerializeField] private int value;
    private AchievementData data;

    public string TaskTag { get { return taskTag; } }
    public AchievementData Data { get { return data; } set { data = value; } }

    public AchievementListController List { get { return list; } set { list = value; } }
    public TMP_Text Description { get { return description; } }

    public int Value { get { return value; } }
    public int GoalValue { get { return goalValue; } }

    public override void InitializeAchievement()
    {
        this.id = data.id;
        this.taskTag = data.tag;
        this.description.text = data.description;
        this.icon.sprite = data.icon;
        this.goalValue = data.goalValue;
        this.reward = data.reward;
        rewardText.text = $"+{this.reward}";
        value = AchievementGoalsController.GetValue(taskTag);
        bar.UpdateCompletedBar(value, goalValue, list.existingList.FindIndex(x => x == this));
    }

    private void OnEnable()
    {
        if (value > goalValue)
        {
            value = goalValue;
            ReachedAchievement();
        }
    }

    public void UpdateValue()
    {
        if (!this.gameObject.IsDestroyed())
        {
            value = AchievementGoalsController.GetValue(taskTag);
            bar.UpdateCompletedBar(value, goalValue, list.existingList.FindIndex(x => x == this));
            AchievementGoalsController.WriteData(this.list.reachedData, this);
        }
    }


    public void CheckReachedAchievement()
    {
        if (value >= goalValue)
        {
            value = goalValue;
            ReachedAchievement();
        }
    }

    void ReachedAchievement()
    {
        int coin = PlayerPrefs.GetInt("Coin", 0);
        Debug.Log("Coin: " + coin);
        Debug.Log("Reward: " + this.reward);
        PlayerPrefs.SetInt("Coint", coin + this.reward);
        Debug.Log("Total Coin: " + PlayerPrefs.GetInt("Coint", 0));
        Debug.Log("Achievement Reached: " + this.description);
        this.list.UpdateAchievementList(this);
    }
}
