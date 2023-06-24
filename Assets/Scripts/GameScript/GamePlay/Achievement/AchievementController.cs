using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : BaseAchievement
{
    private int id;
    private string taskTag;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image icon;
    [SerializeField] AchievementBar bar;
    private AchievementListController list;
    [SerializeField] private int goalValue;
    [SerializeField] private int value;
    private float timer = 1;
    private AchievementData data;

    public string TaskTag { get { return taskTag; } }
    public AchievementData Data { get { return data; } set { data = value; } }

    public AchievementListController List { get { return list; } set { list = value; } }

    public int Value { get { return value; } }

    public override void InitializeAchievement()
    {
        this.id = data.id;
        this.taskTag = data.tag;
        this.description.text = data.description;
        this.icon.sprite = data.icon;
        this.goalValue = data.goalValue;
        //this.list = this.gameObject.GetComponentInParent<AchievementListController>();
    }

    private void OnEnable()
    {
        if (value > goalValue)
        {
            value = goalValue;
            StartCoroutine(ReachedAchievement_2());
        }
    }

    public void UpdateValue()
    {
        value = AchievementGoalsController.GetValue(taskTag);
        Debug.Log(value);
        bar.UpdateCompletedBar(value, goalValue);
        //if (this.list != null)
        //    Debug.Log(this.list.gameObject.name);
        //else
        //    Debug.Log(null);
        //AchievementGoalsController.WriteData(this.list.reachedData, this);
        if (value >= goalValue)
        {
            value = goalValue;
            StartCoroutine(ReachedAchievement());
        }
    }

    IEnumerator ReachedAchievement()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Achievement Reached: " + this.description);
        this.list.UpdateAchievementList(data);
        Destroy(gameObject);
    }

    IEnumerator ReachedAchievement_2()
    {
        this.list.UpdateAchievementList(data);
        //this.list.existingList.Remove(this);
        Destroy(gameObject);
        yield return null;
    }
}
