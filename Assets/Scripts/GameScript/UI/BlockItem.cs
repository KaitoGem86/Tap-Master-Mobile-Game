using System;
using UnityEngine;
using UnityEngine.UI;

public class BlockItem : MonoBehaviour
{
    BlockPool pool;
    private int isLocked = 0;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] public GameObject blockItem;
    [SerializeField] Material blockMaterial;
    [SerializeField] Button blockButton;
    [SerializeField] Image blockImage;


    private Sprite sprite;
    private String blockName;
    public void SetBlockItem(Material material, Sprite sprite, String name, bool isDefault)
    {
        this.blockMaterial = material;
        this.sprite = sprite;
        this.blockName = name;
        this.isLocked = Convert.ToInt16(isDefault);
    }

    public void SetGOInfo()
    {
        blockImage.sprite = sprite;
        this.gameObject.name = this.blockName;
    }

    public bool Islocked { get { return PlayerPrefs.GetInt("BlockItem " + this.gameObject.name, 0) == 1; } set { isLocked = Convert.ToInt16(value); } }

    private void Start()
    {
        isLocked = PlayerPrefs.GetInt("BlockItem " + this.gameObject.name, 0);
        pool = GameManager.Instance.blockPool;
        blockButton.interactable = isLocked == 1;
    }

    public void SetMaterialBlock()
    {
        foreach (GameObject go in pool.pool)
        {
            go.GetComponent<TestMoveBlock>().SetMaterial(blockMaterial);
        }
        blockPrefab.GetComponent<TestMoveBlock>().SetMaterial(blockMaterial);
    }

    public void SetInteractable(bool islo)
    {
        PlayerPrefs.SetInt("BlockItem " + this.gameObject.name, Convert.ToInt16(islo));
        Islocked = islo;
        blockButton.interactable = islo;
    }

    public void InitializeItem(Material material, Sprite sprite, String name, bool isDefault)
    {
        this.blockMaterial = material;
        blockImage.sprite = sprite;
        this.blockName = name;
        this.isLocked = Convert.ToInt16(isDefault);
    }
}