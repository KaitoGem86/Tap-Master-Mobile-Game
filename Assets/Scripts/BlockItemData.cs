using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BlockItemData
{
    public string blockName;
    public Material material;
    public Sprite image;
    public bool isDefault;
}
