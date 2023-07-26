using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectItemData
{
    public bool isDefault;
    public string effectName;
    public int index;
    public Material effectMaterial;
    public Sprite effectIcon;
}


[CreateAssetMenu(menuName = "Effect Data")]
public class EffectDataList : ScriptableObject
{
    public enum EffectType
    {
        tapEffect,
        trails,
        winGameEffect
    }
    [Header("Effect Type")]
    public EffectType type;

    [Space]
    [Header("Effect Datas")]
    public EffectItemData[] effectData;
}
