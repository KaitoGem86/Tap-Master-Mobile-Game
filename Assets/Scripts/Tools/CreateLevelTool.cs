using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[Serializable]
public class state
{
    public Vector3 pos;
    public Vector3 rotation;
    public Vector3 color;

    public state(Vector3 pos, Vector3 rotation)
    {
        this.pos = pos;
        this.rotation = rotation;
    }

    public state(Vector3 pos, Vector3 rotation, Vector3 color)
    {
        this.pos = pos;
        this.rotation = rotation;
        this.color = color;
    }

    public state()
    {
        pos = Vector3.zero;
        rotation = Vector3.zero;
    }
}
public class CreateLevelTool : MonoBehaviour
{

    Vector3[] direction = new Vector3[6] { Vector3.up, Vector3.forward, Vector3.down, Vector3.back, Vector3.right, Vector3.left };
    Dictionary<Vector3, Vector3> rotDir = new Dictionary<Vector3, Vector3>() {
        {Vector3.up, new Vector3(0, 0, 0)},
        {Vector3.down, new Vector3(180, 0, 0)},
        {Vector3.back, new Vector3(90, 0, 0)},
        {Vector3.forward, new Vector3(-90, 0, 0)},
        {Vector3.left, new Vector3(0, 0, 90)},
        {Vector3.right, new Vector3(0, 0, -90)}
    };

    [SerializeField] string levelName;
    [SerializeField] List<state> list = new List<state>();

    float maxDis;
    int count = 0;
    bool Find2CanCircle(List<state> list, int i, Vector3 dir)
    {
        for (int j = 0; j < i; j++)
        {

            if (list[j].rotation + dir == Vector3.zero && (list[j].pos - list[i].pos).normalized == dir.normalized)
            {
                Debug.Log("**********************************" + list[j].rotation + "***************************" + dir);
                Debug.Log("false");
                return false;
            }
        }
        return true;
    }

    private void Start()
    {
        maxDis = GetMaxDistance();
        int numOfText = list.Count * 10;
        PlayerPrefs.SetInt("Number of Texts", numOfText);
        InstantiateLevel(2);
    }

    void InKetQua(int j)
    {
        String folderpath = "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + levelName;
        if (!Directory.Exists(folderpath))
        {
            Directory.CreateDirectory(folderpath);
        }
        String filepath = "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + levelName + "\\" + j.ToString() + ".txt"; ;
        FileStream fs = new FileStream(filepath, FileMode.CreateNew);
        StreamWriter writer = new StreamWriter(fs);
        writer.WriteLine(list.Count);
        writer.WriteLine((int)maxDis);
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].rotation + " " + i);
            writer.Write(list[i].pos);
            writer.Write(list[i].rotation);
            writer.WriteLine();
        }
        writer.Flush();
        writer.Close();
    }

    void InstantiateLevel(int i)
    {
        foreach (Vector3 dir in direction)
        {
            if (Find2CanCircle(list, i, dir))
            {
                list[i].rotation = rotDir[dir];
                if (i == 0 || i == 1)
                    continue;
                if (list[i].rotation == list[i - 1].rotation || list[i].rotation == list[i - 2].rotation)
                    continue;
                if (i + 1 == list.Count)
                {
                    count++;
                    InKetQua(count);
                    Debug.Log("================================================");
                }
                else
                    InstantiateLevel(i + 1);
            }
            if (count == list.Count * 10)
                return;
        }
    }

    float GetMaxDistance()
    {
        float max = 0;
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i; j < list.Count; j++)
            {
                float dis = Vector3.Distance(list[i].pos, list[j].pos);
                if (dis > max) max = dis;
            }
        }
        return max;
    }
}
