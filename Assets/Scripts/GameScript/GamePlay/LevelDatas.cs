using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LevelData")]
public class LevelDatas : ScriptableObject
{
    public int numberOfLevels;
    public List<LevelData> datasControllers;
}

[System.Serializable]
public class LevelData{
    public int levelIndex;
    public int numOfBlocks;
    public int maxDis;
    public List<state> blockStates = new List<state>();
}