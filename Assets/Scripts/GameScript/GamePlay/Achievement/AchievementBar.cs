using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBar : MonoBehaviour
{
    [SerializeField] private Image completedBar;

    public void UpdateCompletedBar(int value, int goal)
    {
        if (value > goal)
            completedBar.fillAmount = 1;
        completedBar.fillAmount = (float)value / goal;
    }
}
