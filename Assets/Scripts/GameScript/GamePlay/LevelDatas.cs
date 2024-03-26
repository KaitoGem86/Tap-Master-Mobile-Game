using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LevelData")]
public class LevelDatas : ScriptableObject
{
    public int numberOfLevels;
    public List<LevelData> datasControllers;
}

public class TempLevelClass{
    public int lvl;
    public String size;
    public String data;
    public int blockCount;
    public String dataColor;
    public bool isColored;

    public (List<Vector3>, List<Vector3>) GetBlocksData(){
        var posTmp = data.Split(',');
        var colorTmp = dataColor.Split(',');
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> color = new List<Vector3>();
        foreach (var p in posTmp){
            var tmp = p.Split(' ');
            pos.Add(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2])));
        }
        foreach (var c in colorTmp){
            var tmp = c.Split(' ');
            color.Add(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2])));
        }
        return (pos, color);
    }

    public Vector3 GetSize(){
        var tmp = size.Split(' ');
        return new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]));
    }
}

[System.Serializable]
public class LevelData{
    public int levelIndex;
    public int numOfBlocks;
    public Vector3 size;
    public List<state> blockStates = new List<state>();
    public bool isSetColor;

    public LevelData(int levelIndex, int numOfBlocks, Vector3 size, List<state> blockStates)
    {
        this.levelIndex = levelIndex;
        this.numOfBlocks = numOfBlocks;
        this.size = size;
        this.blockStates = blockStates;
    }
}