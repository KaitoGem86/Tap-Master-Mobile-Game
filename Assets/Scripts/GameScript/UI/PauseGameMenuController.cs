using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;

    public void ExitPanel()
    {
        Time.timeScale = 1;
        GameManager.Instance.blockPool.gameObject.SetActive(true);
        GameManager.Instance.selectBlock.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OpenBlockListPanel()
    {
        blockListPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OpenLevelList()
    {
        gamePlayModeMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
