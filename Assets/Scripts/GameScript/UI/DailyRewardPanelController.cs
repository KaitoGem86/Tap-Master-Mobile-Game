using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardPanelController : MonoBehaviour
{
    [SerializeField] RectTransform rewardPanel;
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
            PauseGameMenuController.isInteractable = true;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            if (pos.x < rewardPanel.position.x + rewardPanel.rect.width / 2
                && pos.x > rewardPanel.position.x - rewardPanel.rect.width / 2
                && pos.y < rewardPanel.position.y + rewardPanel.rect.height / 2
                && pos.y > rewardPanel.position.y - rewardPanel.rect.height / 2
                )
            {
                return;
            }
            else
            {
                ExitPanel();
            }
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
        GameManager.Instance.camMoving.CanRotate = true;
        if (!isAwake)
        {
            isAwake = true;
            if (!DailyRewardSystem.isDailyCollected)
                PauseGameMenuController.isInteractable = true;
        }
        else
        {
            GameManager.Instance.isOnMenu = false;
        }
        this.gameObject.SetActive(false);
    }
}
