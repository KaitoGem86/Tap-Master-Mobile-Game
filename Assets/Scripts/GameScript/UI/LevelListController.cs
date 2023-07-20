using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelListController : MonoBehaviour
{
    enum Type
    {
        easy,
        medium,
        hard
    }

    [SerializeField] private LevelItem item;
    [SerializeField] private LevelData data;
    [SerializeField] Type levelType;


    int start;
    int end;
    List<LevelItem> levelList = new List<LevelItem>();
    // Start is called before the first frame update
    void Start()
    {
        InitilizeLevelList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitilizeLevelList()
    {
        int n = data.numberOfLevels;
        start = 0;
        end = n;

        switch (levelType)
        {
            case Type.easy:
                start = 1;
                end = 10;
                break;
            case Type.medium:
                start = 11;
                end = 30;
                break;
            case Type.hard:
                start = 31;
                end = n;
                break;
            default:
                break;
        }
        for (int i = start; i <= end; i++)
        {
            var go = Instantiate(item.gameObject, this.transform);
            LevelItem li = go.GetComponent<LevelItem>();
            li.Level = i;
            li.SetLevelText(i);
            levelList.Add(li);
        }
    }
}
