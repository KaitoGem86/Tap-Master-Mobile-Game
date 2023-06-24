using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AchievementListController : MonoBehaviour
{
    public AchievementDataList waitedList;
    public AchievementDataList passedList;
    public ReachedValueData reachedData;

    [SerializeField] private GameObject achievementPrefab;
    public List<AchievementController> existingList = new List<AchievementController>(3);

    private void OnApplicationQuit()
    {
        foreach (var controller in existingList)
        {
            AchievementGoalsController.WriteData(reachedData, controller);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeList()
    {
        int n = waitedList.list.Count;
        if (n > 0)
            for (int i = 0; i < (n > 3 ? 3 : n); i++)
            {
                GameObject go = Instantiate(achievementPrefab, this.transform.position, Quaternion.identity, this.transform);
                var v = go.GetComponent<AchievementController>();
                v.Data = waitedList.list[i];
                v.List = this;
                if (v.List == null)
                    Debug.Log(null);
                v.InitializeAchievement();
                existingList.Add(v);
                AchievementGoalsController.UpdateTaskList(v.TaskTag);
            }
    }

    public void CheckTask()
    {
        foreach (var task in existingList)
        {
            task.UpdateValue();
        }
    }


    public void UpdateAchievementList(AchievementData data)
    {
        Debug.Log("AddList");
        waitedList.RemoveAchievement(data);
        passedList.AddAchievement(data);
    }

    public void HireList()
    {
        foreach (var task in existingList)
        {
            Destroy(task.gameObject);
        }
    }
}
