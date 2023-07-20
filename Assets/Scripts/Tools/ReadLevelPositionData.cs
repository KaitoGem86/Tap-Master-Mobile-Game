using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadLevelPositionData : MonoBehaviour
{
    [SerializeField] ListOfPositions data;
    [SerializeField] GameObject obj;
    [SerializeField] LevelDatasController levelDatasController;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> s = data.positions;
        List<state> sts = levelDatasController.states;
        foreach (state st in sts)
        {
            Instantiate(obj, st.pos * 4, Quaternion.Euler(st.rotation));
        }
    }
}
