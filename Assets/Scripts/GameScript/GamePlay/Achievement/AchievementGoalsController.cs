using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AchievementGoalsController
{
    public static Dictionary<string, int> TaskValueList = new Dictionary<string, int>();

    public static void ReadData(ReachedValueData data)
    {
        if (data.data.Count > 0)
        {
            foreach (KeyValuePair<string, int> kvp in data.data)
            {
                if (!data.data.ContainsKey(kvp.Key))
                {
                    Debug.Log("Add Task");
                    TaskValueList.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    Debug.Log("Update Task");
                    TaskValueList[kvp.Key] = kvp.Value;
                }
            }
        }
    }

    public static void WriteData(ReachedValueData targetData, AchievementController writedData)
    {
        Debug.Log("Write Data");
        if (targetData.data.ContainsKey(writedData.TaskTag))
        {
            targetData.data[writedData.TaskTag] = writedData.Value;
        }
        else
        {
            targetData.data.Add(writedData.TaskTag, writedData.Value);
        }
    }

    public static void UpdateTaskList(string tag)
    {
        if (!TaskValueList.ContainsKey(tag))
        {
            Debug.Log("Add Task " + tag);
            TaskValueList.Add(tag, 0);
        }
    }

    public static void UpdateList(int blocks, int levels)
    {
        int n = TaskValueList.Count;
        Debug.Log(n);
        for (int i = 0; i < n; i++)
        {
            var pair = TaskValueList.ElementAt(i);
            if (pair.Key.Contains("blocks"))
            {
                int value = pair.Value + blocks;
                TaskValueList[pair.Key] = value;
            }
            if (pair.Key.Contains("levels"))
            {
                int value = levels;
                TaskValueList[pair.Key] = value > pair.Value ? value : pair.Value;
            }
            Debug.Log(pair.Key + "-" + TaskValueList[pair.Key]);
        }
    }

    public static int GetValue(string tag)
    {
        if (TaskValueList.ContainsKey(tag))
            return TaskValueList[tag];
        else
        {
            TaskValueList.Add(tag, 0);
            return 0;
        }
    }
}
