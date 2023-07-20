using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;


public class CreateBlockFromModel : MonoBehaviour
{
    [SerializeField] string levelName;
    [SerializeField] bool isEvenAtX;
    [SerializeField] bool isEvenAtY;
    [SerializeField] bool isEvenAtZ;
    // Start is called before the first frame update
    List<Vector3> leftSideView = new List<Vector3>();
    List<Vector3> rightSideView = new List<Vector3>();
    List<Vector3> topView = new List<Vector3>();
    List<Vector3> bottomView = new List<Vector3>();
    List<Vector3> frontVew = new List<Vector3>();
    List<Vector3> behindVew = new List<Vector3>();
    ListOfPositions data;
    List<Vector3> states = new List<Vector3>();
    int cX, cY, cZ;
    void Awake()
    {
        data = ScriptableObject.CreateInstance<ListOfPositions>();

        if (isEvenAtX == true)
            cX = 1;
        else cX = 0;

        if (isEvenAtY == true)
            cY = 1;
        else cY = 0;

        if (isEvenAtZ == true)
            cZ = 1;
        else cZ = 0;

        for (int i = -20; i < 20; i++)
        {
            for (int j = -20; j < 20; j++)
            {
                leftSideView.Add(new Vector3(-20, i, j));
                rightSideView.Add(new Vector3(20, i, j));
                topView.Add(new Vector3(i, 20, j));
                bottomView.Add(new Vector3(i, -20, j));
                frontVew.Add(new Vector3(i, j, 20));
                behindVew.Add(new Vector3(i, j, -20));
            }
        }
    }

    private void Start()
    {
        foreach (Vector3 v in leftSideView)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.right, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.right * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(-0.5f, 0, 0));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("left - " + /*(hit.point - (new Vector3(-0.5f, 0, 0)))*/ t);

                if (!states.Contains(/*hit.point - (new Vector3(-0.5f, 0, 0))*/t))
                {
                    states.Add(t/*hit.point - (new Vector3(-0.5f, 0, 0))*/);
                }
            }
        }
        foreach (Vector3 v in rightSideView)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.left, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.left * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(0.5f, 0, 0));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("right - " + /*(hit.point - (new Vector3(0.5f, 0, 0)))*/ t);

                if (!states.Contains(t/*hit.point - (new Vector3(0.5f, 0, 0))*/))
                {
                    Debug.Log(t);
                    states.Add(/*hit.point - (new Vector3(0.5f, 0, 0))*/t);
                }
            }

            //else
            //    Debug.DrawRay(v, Vector3.left * 20, Color.red, 30);
        }
        foreach (Vector3 v in topView)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.down, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.down * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(0, 0.5f, 0));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("top - " + t/*(hit.point - (new Vector3(0, 0.5f, 0)))*/);
                if (!states.Contains(/*hit.point - (new Vector3(0.5f, 0, 0))*/t))
                {
                    states.Add(t);
                }

            }
        }
        foreach (Vector3 v in bottomView)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.up, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.up * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(0, -0.5f, 0));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("bottom - " + /*(hit.point - (new Vector3(0, -0.5f, 0)))*/t);
                if (!states.Contains(/*hit.point - (new Vector3(0, -0.5f, 0))*/t))
                {
                    states.Add(t);
                }
            }
            //else
            //    Debug.DrawRay(v, Vector3.up * 20, Color.red, 30);
        }
        foreach (Vector3 v in frontVew)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.back, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.back * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(0, 0, 0.5f));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("front - " + (t/*hit.point - (new Vector3(0, 0, 0.5f))*/));

                if (!states.Contains(/*hit.point - (new Vector3(0, 0, 0.5f))*/t))
                {
                    states.Add(t);
                }
            }
            //else
            //    Debug.DrawRay(v, Vector3.back * 20, Color.red, 30);
        }
        foreach (Vector3 v in behindVew)
        {
            if (Physics.Raycast(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.forward, out RaycastHit hit))
            {
                Debug.DrawRay(v - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ, Vector3.forward * hit.distance, Color.red, 300);
                var t = hit.point - (new Vector3(0, 0, -0.5f));
                t.x = (float)Math.Round(t.x * 2) / 2;
                t.y = (float)Math.Round(t.y * 2) / 2;
                t.z = (float)Math.Round(t.z * 2) / 2;
                Debug.Log("behind - " + (t/*hit.point - (new Vector3(0, 0, -0.5f))*/));

                if (!states.Contains(/*hit.point - (new Vector3(0, 0, -0.5f))*/t))
                {
                    states.Add(t);
                }
            }
            //else
            //    Debug.DrawRay(v, Vector3.forward * 20, Color.red, 30);
        }

        for (int i = -10; i < 11; i++)
        {
            for (int j = -10; j < 11; j++)
            {
                for (int k = -10; k < 11; k++)
                {
                    if (!states.Contains(new Vector3(i, j, k) - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ) &&
                        CheckXAxis(new Vector3(i, j, k) - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ) &&
                        CheckYAxis(new Vector3(i, j, k) - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ) &&
                        CheckZAxis(new Vector3(i, j, k) - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ))
                    {
                        states.Add(new Vector3(i, j, k) - (new Vector3(0.5f, 0, 0)) * cX - (new Vector3(0, 0.5f, 0)) * cY - (new Vector3(0, 0, 0.5f)) * cZ);
                    }
                }
            }
        }
        SortListOfPosition(ref states, 0, states.Count - 1);
        Debug.Log("Sorted");
        data.positions = states;
        //AssetDatabase.CreateAsset(data, "Assets/Data/Level Data/Position File/" + levelName + ".asset");
        //AssetDatabase.SaveAssets();
    }

    // Update is called once per frame 
    void Update()
    {
    }

    bool CheckXAxis(Vector3 checkPos)
    {
        int check = 0;
        for (int i = 1; i < 10; i++)
        {
            if (states.Contains(checkPos + new Vector3(i, 0, 0)))
            {
                check += 1;
                break;
            }
        }

        for (int i = -10; i < 0; i++)
        {
            if (states.Contains(checkPos + new Vector3(i, 0, 0)))
            {
                check += 1;
                break;
            }
        }
        return check == 2;
    }

    bool CheckYAxis(Vector3 checkPos)
    {
        int check = 0;
        for (int i = 1; i < 10; i++)
        {
            if (states.Contains(checkPos + new Vector3(0, i, 0)))
            {
                check += 1;
                break;
            }
        }

        for (int i = -10; i < 0; i++)
        {
            if (states.Contains(checkPos + new Vector3(0, i, 0)))
            {
                check += 1;
                break;
            }
        }
        return check == 2;
    }

    bool CheckZAxis(Vector3 checkPos)
    {
        int check = 0;
        for (int i = 1; i < 10; i++)
        {
            if (states.Contains(checkPos + new Vector3(0, 0, i)))
            {
                check += 1;
                break;
            }
        }

        for (int i = -10; i < 0; i++)
        {
            if (states.Contains(checkPos + new Vector3(0, 0, i)))
            {
                check += 1;
                break;
            }
        }
        return check == 2;
    }




    //use quick sort to sort the block's position list order by the magnitude with ASC


    void SortListOfPosition(ref List<Vector3> states, int low, int high)
    {
        if (low < 0 || high < 0)
        {
            return;
        }
        if (low < high)
        {
            int pivot = Partition(ref states, low, high);

            SortListOfPosition(ref states, low, pivot - 1);
            SortListOfPosition(ref states, pivot + 1, high);
        }
    }

    void Swap(ref List<Vector3> states, int i, int j)
    {
        Vector3 tmp = states[i];
        states[i] = states[j];
        states[j] = tmp;
    }

    int Partition(ref List<Vector3> states, int low, int high)
    {
        Vector3 pivot = states[high];
        float dis = Vector3.Distance(Vector3.zero, pivot);
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (Vector3.Distance(Vector3.zero, states[j]) < dis)
            {
                i++;
                Swap(ref states, i, j);
            }
        }
        Swap(ref states, i, high);
        return (i + 1);
    }
}
