using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinGamePanel : MonoBehaviour
{
    [SerializeField] GameObject winGameNotification;
    [SerializeField] GameObject coinSummaryText;
    [SerializeField] GameObject taskPanel;
    [SerializeField] GameObject tapNotification;


    List<GameObject> children = new List<GameObject>();
    private void OnEnable()
    {
        for (int i = 0; i < taskPanel.transform.childCount; i++)
        {
            var child = taskPanel.transform.GetChild(i);
            child.gameObject.transform.localScale = Vector3.zero;
            children.Add(child.gameObject);
        }
        var seq = DOTween.Sequence();
        seq.Append(winGameNotification.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine));
        seq.Append(coinSummaryText.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine));
        foreach (GameObject child in children)
        {
            seq.Append(child.transform.DOScale(Vector3.one, duration: 0.3f));
        }
        GameManager.Instance.GameWinMenu.GetComponent<WinGamePanelController>().CheckTask();

        seq.Append(tapNotification.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine));
    }



    private void Start()
    {
        winGameNotification.transform.localScale = Vector3.zero;
        coinSummaryText.transform.localScale = Vector3.zero;
        tapNotification.transform.localScale = Vector3.zero;
    }
}
