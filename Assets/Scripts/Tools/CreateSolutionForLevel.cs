using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class CreateSolutionForLevel : MonoBehaviour
{
    [SerializeField] ListOfPositions list;
    [SerializeField] string levelName;
    [SerializeField] int levelIndex;


    Dictionary<Vector3, Vector3> listDirection = new Dictionary<Vector3, Vector3>();
    List<Vector3> isTestingPos = new List<Vector3>();

    LevelDatasController data;
    List<state> states;
    Vector3 dirPos = Vector3.negativeInfinity;


    Vector3[] direction = new Vector3[6] { Vector3.up, Vector3.forward, Vector3.down, Vector3.back, Vector3.right, Vector3.left };
    Dictionary<Vector3, Vector3> rotDir = new Dictionary<Vector3, Vector3>() {
        {Vector3.up, new Vector3(0, 0, 0)},
        {Vector3.down, new Vector3(180, 0, 0)},
        {Vector3.back, new Vector3(90, 0, 0)},
        {Vector3.forward, new Vector3(-90, 0, 0)},
        {Vector3.left, new Vector3(0, 0, 90)},
        {Vector3.right, new Vector3(0, 0, -90)}
    };

    Dictionary<Vector3, bool> isTestedList = new Dictionary<Vector3, bool>();

    int count = 0;

    private void Awake()
    {
        states = new List<state>();

        foreach (Vector3 v in list.positions)
        {
            isTestedList.Add(v, false);
        }
        count = list.positions.Count;
    }

    private void Start()
    {
        int i = 0;
        while (count != 0)
        {
            Debug.Log(count);
            Vector3 pos = list.positions[i];
            if (!isTestedList[pos])
            {
                SetDir(pos);
            }
            i++;
        }
        foreach (KeyValuePair<Vector3, Vector3> kv in listDirection)
        {
            //Debug.Log(kv.Value);

            state s = new state();
            s.pos = kv.Key;
            if (states.Contains(s))
            {
                int j = states.FindIndex(x => x.pos == s.pos);
                states[j].rotation = kv.Value;
            }
            else
            {
                s.rotation = kv.Value;
                states.Add(s);
            }
        }
        SaveSolution();
    }

    void SetDir(Vector3 pos)
    {
        if (!isTestingPos.Contains(pos))
            isTestingPos.Add(pos);
        int k = Random.Range(0, 6);
        for (int i = 0; i < rotDir.Count; i++)
        {
            //Vector3 dir = rotDir[direction[k]];
            Vector3 dir = direction[k];
            int check = CheckPosDir(pos, dir, ref dirPos);
            if (check == 2)
            {
                Debug.Log("Ok");
                //Lưu và cập nhật các giá trị
                listDirection[pos] = rotDir[dir];
                isTestedList[pos] = true;
                count -= isTestingPos.Count;
                isTestingPos.Remove(pos);
                break;
            }
            if (check == 1)
            {
                Debug.Log("Bi can rui");
                SetDir(dirPos);
                listDirection[pos] = rotDir[dir];
                dirPos = Vector3.negativeInfinity;
                isTestedList[pos] = true;
                isTestingPos.Remove(pos);
                break;
            }
            if (check == 0)
            {
                Debug.Log("Sai roi nhe");
                k = (k + 1) % rotDir.Count;
            }
        }
    }

    int CheckPosDir(Vector3 pos, Vector3 dir, ref Vector3 dirPos)
    {
        //Debug.Log(pos);
        //Debug.Log(dir);
        for (int i = 1; i < 15; i++)
        {
            dirPos = pos + i * dir;
            if (list.positions.Contains(dirPos))
            {
                if (isTestedList[dirPos])
                {
                    Vector3 v = new Vector3();
                    v.x = dirPos.x;
                    v.y = dirPos.y;
                    v.z = dirPos.z;
                    for (int j = 0; j < 15; j++)
                    {
                        v += j * dir;
                        if (isTestedList.ContainsKey(v))
                            if (isTestedList[v])
                            {
                                if (listDirection[v] == rotDir[-dir])
                                    return 0;
                            }
                    }
                    Debug.Log(dirPos);
                    return 2;
                }
                else if (isTestingPos.Contains(dirPos))
                {
                    return 0;
                }
                else
                    return 1;
            }
        }
        return 2;
    }

    //void CreateSolution(int i)
    //{
    //    int k = Random.Range(0, 6);
    //    Debug.Log(k);
    //    for (int j = 0; j < direction.Length; j++)
    //    {
    //        //k = Random.Range(0, 6);
    //        Vector3 dir = rotDir[direction[k]];
    //        states[i].rotation = dir;
    //        if (i + 1 == states.Count)
    //        {
    //            count += 1;
    //            SaveSolution();
    //        }
    //        else
    //        {
    //            CreateSolution(i + 1);
    //        }
    //        if (count > 20)
    //            return;
    //        k = (k + 1) % 6;
    //    }
    //}

    void SaveSolution()
    {

        data = ScriptableObject.CreateInstance<LevelDatasController>();
        data.levelName = levelName;
        data.levelIndex = levelIndex;
        data.states = states;
        data.numOfBlocks = states.Count;
        data.maxDis = (int)GetMaxDistance();
        //AssetDatabase.CreateAsset(data, "Assets/Data/Level Data/Level File/" + levelName + ".asset");
        //AssetDatabase.SaveAssets();
    }

    float GetMaxDistance()
    {
        float max = 0;
        for (int i = 0; i < list.positions.Count - 1; i++)
        {
            for (int j = i; j < list.positions.Count; j++)
            {
                float dis = Vector3.Distance(list.positions[i], list.positions[j]);
                if (dis > max) max = dis;
            }
        }
        return max;
    }
}
