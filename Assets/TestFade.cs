using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DOTween.SetTweensCapacity(2000, 100);
        this.gameObject.GetComponent<Renderer>().material.DOFade(0f, 0f);
        this.gameObject.GetComponent<Renderer>().material.DOFade(1f, 2f).SetDelay(1f);
        this.gameObject.GetComponent<Renderer>().material.DOFade(0f, 2f).SetDelay(3f);
    }
}
