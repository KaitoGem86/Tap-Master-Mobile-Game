using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * 540;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
    }

    void Exit()
    {
        //GameManager.Instance.blockPool.gameObject.SetActive(true);
        GameManager.Instance.selectBlock.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        var r = this.transform.position;
        Debug.Log(r);
        if (r == new Vector3(540, 960, 0))
            this.transform.position = r + Vector3.right * 540;
        else
            r += Vector3.left * 540;
        this.transform.DOMove(r, duration: 0.5f).SetEase(Ease.InOutSine);
    }
}
