using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle Status")]
public class PuzzleStatus : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public Sprite _sprite;
    [SerializeField] public bool isCollected;
    [SerializeField] List<int> keys = new List<int>();
    [SerializeField] List<bool> values = new List<bool>();


    public Dictionary<int, bool> status = new Dictionary<int, bool>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<int, bool> pair in status)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        status.Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            status.Add(key: keys[i], value: values[i]);
        }
    }
}
