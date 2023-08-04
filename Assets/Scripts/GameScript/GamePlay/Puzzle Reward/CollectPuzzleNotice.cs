using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectPuzzleNotice : MonoBehaviour
{
    [SerializeField] Image imageNotice;
    [SerializeField] RectTransform imageTransform;


    public Tween t;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = imageTransform.position;
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        NoticeOnCollect();
    //    }
    //}

    // Update is called once per frame
    public void NoticeOnCollect()
    {
        t = this.imageNotice.DOFade(1, 0.35f).SetLoops(2, LoopType.Yoyo);
        t = this.imageTransform.DOMove(imageTransform.position + new Vector3(0, 100, 0), duration: 0.7f).OnComplete(() => imageTransform.position = startPos);
    }
}
