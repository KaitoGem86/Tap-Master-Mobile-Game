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
    public Vector3 size;
    public List<state> blockStates = new List<state>();

    public LevelData(int levelIndex, int numOfBlocks, Vector3 size, List<state> blockStates)
    {
        this.levelIndex = levelIndex;
        this.numOfBlocks = numOfBlocks;
        this.size = size;
        this.blockStates = blockStates;
    }
}