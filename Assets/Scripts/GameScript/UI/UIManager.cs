using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingBlocks;
    [SerializeField] private TMP_Text remainingTouchs;
    [SerializeField] private TMP_Text CoinText;
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] public Canvas canvas;
    [SerializeField] private GameObject dailyRewardPanel;
    [SerializeField] private GameObject dailyRewardNotification;

    DateTime now;
    public static UIManager instance;
    bool isAwake;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Is Awake", 0);
    }

    private void Awake()
    {
        isAwake = PlayerPrefs.GetInt("Is Awake", 0) == 0;
        instance = this;
        if (isAwake)
        {
            PlayerPrefs.SetInt("Is Awake", 1);
            pauseGameMenu.gameObject.SetActive(true);
            dailyRewardPanel.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //pauseGameMenu.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        //pauseGameMenu.SetActive(false);
        blockListPanel.transform.position = this.transform.position + Vector3.left * canvas.pixelRect.width;
        blockListPanel.SetActive(false);
        gamePlayModeMenu.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        gamePlayModeMenu.SetActive(false);
        SetLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        dailyRewardNotification.SetActive(DailyRewardSystem.CheckCanCollect());
    }

    private void FixedUpdate()
    {
        DateTime now = DateTime.Now;
    }

    public void SetCoinText()
    {
        CoinText.SetText(PlayerPrefs.GetInt("Coin", 0).ToString());
    }



    public void SetLevelText()
    {
        levelText.SetText($"Level " + GameManager.Instance.currentLevel);
    }

    public void UpdateBlocksNum()
    {
        remainingBlocks.SetText($"Remaining Blocks: {GameManager.Instance.countBlocks}");
    }

    public void UpdateTouchsNum()
    {
        remainingTouchs.SetText($"Remaining Touchs: {GameManager.Instance.countTouchs}");
    }

    public void ReplayGame()
    {
        UIManager.instance.pauseGameMenu.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        GameManager.Instance.isOnMenu = true;
        pauseGameMenu.SetActive(true);
    }

    public void ChooseBlock()
    {
        GameManager.Instance.isOnMenu = true;
        blockListPanel.SetActive(true);
        UIManager.instance.SetCoinText();
    }

    public void ChoosePlayMode()
    {
        GameManager.Instance.isOnMenu = true;
        gamePlayModeMenu.SetActive(true);
    }

    public void ChooseDailyRewardPanel()
    {
        GameManager.Instance.isOnMenu = true;
        dailyRewardPanel.SetActive(true);
    }

}
