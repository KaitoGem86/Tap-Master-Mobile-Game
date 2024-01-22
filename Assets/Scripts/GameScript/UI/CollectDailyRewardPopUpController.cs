using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectDailyRewardPopUpController : MonoBehaviour
{
    [Header("Pop Up Elements")]
    [SerializeField] TMP_Text notification;
    [SerializeField] Image iconNoti;
    [SerializeField] GameObject coinIcon;
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text tapToContinueNoti;
    [SerializeField] RectTransform notiPanel;

    bool isAwake = false;

    private void Awake()
    {
        isAwake = true;
    }

    private void OnEnable()
    {
        this.transform.DOScale(Vector3.one, duration: 0.5f).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        TapToContinue();
    }

    void TapToContinue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            if (pos.x < notiPanel.position.x + notiPanel.rect.width / 2
                && pos.x > notiPanel.position.x - notiPanel.rect.width / 2
                && pos.y < notiPanel.position.y + notiPanel.rect.height / 2
                && pos.y > notiPanel.position.y - notiPanel.rect.height / 2
                )
            {
                return;
            }
            else
            {
                ExitWithAnim();
            }
        }
    }

    public void ExitWithAnim()
    {
        this.transform.DOScale(Vector3.zero, duration: 0.5f).SetEase(Ease.InSine).OnComplete(Exit);
    }
    void Exit()
    {
        GameManager.Instance.camMoving.CanRotate = true;
        if (!isAwake)
        {
            isAwake = true;
        }
        else
        {
            GameManager.Instance.isOnMenu = false;
            PauseGameMenuController.isInteractable = true;
        }
        this.gameObject.SetActive(false);
    }

    public void PreActive(int amount)
    {
        this.coinText.SetText($"+ {amount}");
    }
}
