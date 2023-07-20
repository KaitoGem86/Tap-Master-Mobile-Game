using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardPanelController : MonoBehaviour
{
    bool isAwake = false;

    private void Awake()
    {
        DailyRewardSystem.isDailyCollected = PlayerPrefs.GetInt("Collected Daily Reward", 0) == 1;
    }

    private void Start()
    {
        if (DailyRewardSystem.CheckCanCollect())
        {
            GameManager.Instance.isOnMenu = true;
            this.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

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
        GameManager.Instance.blockPool.canRotate = true;
        if (!isAwake)
        {
            isAwake = true;
            PauseGameMenuController.isInteractable = true;
        }
        else
        {
            GameManager.Instance.isOnMenu = false;
        }
        this.gameObject.SetActive(false);
    }
}
