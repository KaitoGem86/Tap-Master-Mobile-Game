using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewBlockListController : MonoBehaviour
{
    [SerializeField] GridLayoutGroup blockGrid;

    [SerializeField] Data data;
    [SerializeField] GameObject blockPrefab;

    public List<BlockItem> blockItems = new List<BlockItem>();
    public List<int> interactableIndexs = new List<int>();
    // Start is called before the first frame update

    void Start()
    {
        foreach (var b in data.data)
        {
            if (b.isDefault)
            {
                PlayerPrefs.SetInt("BlockItem " + b.blockName + "(Clone)", Convert.ToInt16(true));
            }
        }
        InitBlockList();
        CheckInteractableBlock();
    }


    void CheckInteractableBlock()
    {
        for (int i = 0; i < blockItems.Count; i++)
        {
            if (blockItems[i].Islocked == false)
            {
                //Debug.Log(i);
                interactableIndexs.Add(i);
            }
        }
    }

    void InitBlockList()
    {
        foreach (var block in data.data)
        {
            var blockItem = blockPrefab.GetComponent<BlockItem>();
            blockItem.SetBlockItem(block.material, block.image, block.blockName, block.isDefault);
            blockItem.SetGOInfo();
            blockItem.SetInteractable(block.isDefault);
            //blockItem.InitializeBlockItem(block.material, block.image, block.blockName, block.isDefault);
            var go = Instantiate(blockItem.gameObject, blockGrid.transform);
            blockItems.Add(go.GetComponent<BlockItem>());
            if (block.isDefault)
            {
                PlayerPrefs.SetInt("BlockItem " + block.blockName, 1);
                go.GetComponent<Button>().interactable = true;
            }
        }
    }
}
