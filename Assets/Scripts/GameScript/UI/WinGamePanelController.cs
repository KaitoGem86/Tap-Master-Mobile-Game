using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGamePanelController : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] public AchievementListController achievementPanel;
    [SerializeField] RectTransform rect;

    float width;
    bool isContinue;

    // Start is called before the first frame update
    private void Start()
    {
        width = UIManager.instance.canvas.pixelRect.width;
        isContinue = true;
        SetSummaryCoin();
        rect = GetComponent<RectTransform>();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        TapToContinue();
    }

    void TapToContinue()
    {

        Debug.Log("OK");
        Debug.Log(Input.GetMouseButtonDown(0));

        if (InputController.instance.CheckSelect() && isContinue)
        {
            isContinue = false;
            if (rect != null)
            {
                var t = rect.position + Vector3.left * width;
                this.transform.DOMove(t, duration: 0.3f).SetEase(Ease.InOutSine).OnComplete(Continue);
            }
            else
                rect = this.GetComponent<RectTransform>();
        }
    }

    void Continue()
    {
        this.achievementPanel.HireList();
        GameManager.Instance.NextLevel();
    }

    void SetSummaryCoin()
    {
        if (GameManager.Instance != null)
            coinText.SetText($"+ {GameManager.Instance.coin}");
    }

    public void CheckTask()
    {
        achievementPanel.CheckTask();
    }

    public void CheckAchieve()
    {
        achievementPanel.CheckAchieve();
    }
}
