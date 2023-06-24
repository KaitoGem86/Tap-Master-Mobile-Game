using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGamePanelController : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] public AchievementListController achievementPanel;
    // Start is called before the first frame update
    private void Start()
    {
        SetSummaryCoin();
    }

    // Update is called once per frame
    void Update()
    {
        TapToContinue();
    }

    void TapToContinue()
    {
        if (InputController.instance.CheckSelect())
        {
            this.achievementPanel.HireList();
            GameManager.Instance.NextLevel();
        }
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
}
