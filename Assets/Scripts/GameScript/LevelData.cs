using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public int numberOfLevels;
    [SerializeField] public List<LevelDatasController> datasControllers = new List<LevelDatasController>();
}
