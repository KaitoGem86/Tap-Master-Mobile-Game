using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelListController : MonoBehaviour
{
    [SerializeField] private LevelItem item;
    [SerializeField] private LevelData data;

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
        for (int i = 1; i <= n; i++)
        {
            var go = Instantiate(item.gameObject, this.transform);
            LevelItem li = go.GetComponent<LevelItem>();
            li.Level = i;
            li.SetLevelText(i);
            levelList.Add(li);
        }
    }
}
