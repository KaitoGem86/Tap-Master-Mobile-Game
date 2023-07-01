using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardPanelController : MonoBehaviour
{
    [SerializeField] GameObject dailyList;
    [SerializeField] GameObject specialDailyList;

    private void OnEnable()
    {
        this.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine);
    }
    public void ExitPanel()
    {
        this.transform.DOScale(Vector3.zero, duration: 0.5f).SetEase(Ease.InSine).OnComplete(Exit);
    }
    void Exit()
    {
        GameManager.Instance.selectBlock.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
