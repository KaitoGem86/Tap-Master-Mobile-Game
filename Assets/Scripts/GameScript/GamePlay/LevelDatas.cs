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
    public String lvl;
    public String size;
    public String data;
    public String blockCount;
    public String dataColor;
    public String isColored;

    public int GetLevel(){
        return int.Parse(lvl);
    }

    public int GetBlockCount(){
        return int.Parse(blockCount);
    }

    public bool IsColored(){
        return isColored == "false" ? false : true;
    }

    public Vector3Int[] GetPosData(){
        var tmp = data.Split(',');
        List<Vector3Int> pos = new List<Vector3Int>();
        foreach (var p in tmp){
            var tmp2 = p.Split(' ');
            pos.Add(new Vector3Int(int.Parse(tmp2[0]), int.Parse(tmp2[1]), int.Parse(tmp2[2])));
        }
        return pos.ToArray();
    }

    public Vector3Int[] GetColorData(){
        var tmp = dataColor.Split(',');
        List<Vector3Int> color = new List<Vector3Int>();
        if(tmp.Length <= 0) return color.ToArray();
        foreach (var p in tmp){
            var tmp2 = p.Split(' ');
            if(tmp2.Length < 2)
                break;
            color.Add(new Vector3Int(int.Parse(tmp2[0]), int.Parse(tmp2[1]), int.Parse(tmp2[2])));
        }
        return color.ToArray();
    }

    public (Vector3Int[], Vector3Int[]) GetBlocksData(){
        var posTmp = data.Split(',');
        var colorTmp = dataColor.Split(',');
        List<Vector3Int> pos = new List<Vector3Int>();
        List<Vector3Int> color = new List<Vector3Int>();
        foreach (var p in posTmp){
            var tmp = p.Split(' ');
            pos.Add(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2])));
        }
        foreach (var c in colorTmp){
            var tmp = c.Split(' ');
            color.Add(new Vector3Int(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2])));
        }
        return (pos.ToArray(), color.ToArray());
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

    public LevelData(int levelIndex, int numOfBlocks, Vector3 size, List<state> blockStates, bool isSetColor = false)
    {
        this.levelIndex = levelIndex;
        this.numOfBlocks = numOfBlocks;
        this.size = size;
        this.blockStates = blockStates;
        this.isSetColor = isSetColor;
    }
}