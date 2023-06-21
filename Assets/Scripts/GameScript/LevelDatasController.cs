using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Level")]
public class LevelDatasController : ScriptableObject
{
    [SerializeField] public int levelIndex;
    [SerializeField] public string levelName;
    [SerializeField] public int numOfBlocks;
    [SerializeField] public int maxDis;
    [SerializeField] public List<state> states = new List<state>();
}
