using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{
    RectTransform rect;
    float width;


    private void Start()
    {
        rect = GetComponent<RectTransform>();
        width = UIManager.instance.canvas.pixelRect.width;
    }

    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
    }

    void Exit()
    {
        GameManager.Instance.isOnMenu = false;
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        //var r = this.transform.position;
        //Debug.Log(r);
        //if (r == UIManager.instance.canvas.transform.position)
        //    this.transform.position = r + Vector3.right * width;
        //else
        //    r += Vector3.left * width;
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }
}
