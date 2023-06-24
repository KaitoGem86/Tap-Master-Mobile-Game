using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement Data List")]
public class AchievementDataList : ScriptableObject
{
    [SerializeField] public List<AchievementData> list;

    public void AddAchievement(AchievementData achievement)
    {
        list.Add(achievement);
    }

    public void RemoveAchievement(AchievementData achievement)
    {
        list.Remove(achievement);
    }
}
