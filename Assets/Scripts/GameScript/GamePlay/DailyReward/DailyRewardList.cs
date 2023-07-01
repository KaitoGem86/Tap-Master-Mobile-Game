using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Daily List")]
public class DailyRewardList : ScriptableObject
{
    [SerializeField] public List<DailyRewardData> dataList = new List<DailyRewardData>();
}
