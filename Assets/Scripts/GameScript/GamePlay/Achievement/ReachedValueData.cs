using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Achievement Data")]
public class ReachedValueData : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<string> keys = new List<string>();

    [SerializeField]
    private List<int> values = new List<int>();
    public Dictionary<string, int> data = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<string, int> pair in data)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        data.Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            data.Add(keys[i], values[i]);
        }
    }
}
