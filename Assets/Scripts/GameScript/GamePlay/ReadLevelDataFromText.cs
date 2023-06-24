using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ReadLevelDataFromText : MonoBehaviour
{
    [SerializeField] string testedLevel;
    [SerializeField] TestBlock block;
    [SerializeField] int test;

    String[] dataFiles;
    List<TestBlock> blocks = new List<TestBlock>();
    List<Vector3> testedBlockList = new List<Vector3>();
    RaycastHit hitInfo;
    string path;

    int size = 0;

    void RefreshData()
    {
        dataFiles = Directory.GetFiles("D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\", "*.txt");

        String[] newPath = new string[dataFiles.Length];
        String[] tmpPath = new string[dataFiles.Length];
        Debug.Log(dataFiles.Length);
        for (int i = 0; i < dataFiles.Length; i++)
        {
            //File.Move(dataFiles[i], "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\" + i / 2 + 1 + ".txt");
            newPath[i] = "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\" + (i + 1) + ".txt";
            tmpPath[i] = "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\" + (i + 1000) + ".txt";
            File.Move(dataFiles[i], tmpPath[i]);
        }
        for (int i = 0; i < dataFiles.Length; i++)
        {
            File.Move(tmpPath[i], newPath[i]);
        }
    }

    void ReadData(int i)
    {
        path = "D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\" + i + ".txt";
        StreamReader reader = new StreamReader(path);
        size = Int16.Parse(reader.ReadLine());
        reader.ReadLine();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            int j = line.IndexOf('(') + 1;
            float x = float.Parse(line.Substring(j, 4));
            int k = line.IndexOf(' ', (int)j) + 1;
            float y = float.Parse(line.Substring(k, 4));
            j = line.IndexOf(' ', (int)k) + 1;
            float z = float.Parse(line.Substring(j, 4));
            Vector3 pos = new Vector3(x, y, z);

            k = line.IndexOf('(', (int)j) + 1;
            x = float.Parse(line.Substring(k, 4));
            j = line.IndexOf(' ', (int)k) + 1;
            y = float.Parse(line.Substring(j, 4));
            k = line.IndexOf(' ', (int)j) + 1;
            z = float.Parse(line.Substring(k, 4));
            Vector3 angle = new Vector3(x, y, z);

            var go = Instantiate(block.gameObject, pos * 4.05f, Quaternion.Euler(angle));
            blocks.Add(go.GetComponent<TestBlock>());
        }
        reader.Close();
    }

    private void Start()
    {
        dataFiles = Directory.GetFiles("D:\\Unity Game\\My project\\Assets\\Data\\LevelData\\" + testedLevel + "\\", "*.txt");
        int l = dataFiles.Length > 20 ? 20 : dataFiles.Length;
        Debug.Log(l);
        //for (int j = l; j >= 1; j--)
        //{
        //    ReadData(j);
        //    for (int i = 0; i < blocks.Count; i++)
        //        CheckData(blocks[i], path);
        //    Debug.Log("==========================================================================================" + j);
        //    foreach (var block in blocks)
        //    {
        //        Destroy(block.gameObject);
        //    }
        //    blocks.Clear();
        //}
        ReadData(test);
        for (int i = 0; i < blocks.Count; i++)
            CheckData(blocks[i], path);
        RefreshData();
    }

    bool CheckLTestedList()
    {
        foreach (TestBlock testBlock in blocks)
        {
            if (testBlock.isTested == false)
                return false;
        }
        return true;
    }

    private void CheckData(TestBlock testBlock, string path)
    {
        Debug.Log("Test");
        if (testBlock.isTested == false)
        {
            testedBlockList.Add(testBlock.transform.position);
            testBlock.isTested = true;
            if (Physics.Raycast(testBlock.transform.position, testBlock.transform.up * 100, out hitInfo))
            {
                if (testedBlockList.Contains(hitInfo.transform.position))
                {
                    Debug.Log("tạch rồi bé");

                    System.IO.File.Delete(path);
                    return;
                }
                else
                {
                    CheckData(hitInfo.transform.gameObject.GetComponent<TestBlock>(), path);
                }
            }
            else
            {
                testedBlockList.Clear();
                if (CheckLTestedList())
                {
                    Debug.Log("Cái này oke nè");
                    return;
                }
            }
        }
    }
}
