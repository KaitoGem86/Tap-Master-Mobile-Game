using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMove(new Vector3(5, 5, 5), 2).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
