using DG.Tweening;
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
            completedBar.DOFillAmount(1, 0.5f);
        completedBar.DOFillAmount((float)value / goal, 0.5f);
    }
}
