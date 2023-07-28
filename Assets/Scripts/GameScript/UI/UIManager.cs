using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    [SerializeField] private GameObject levelListPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] public Canvas canvas;
    [SerializeField] private GameObject dailyRewardPanel;
    [SerializeField] private GameObject dailyRewardNotification;

    DateTime now;
    public static UIManager instance;
    public bool isAwake;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Is Awake", 0);
        settingPanel.GetComponent<SettingPanel>().SaveSoundState();
    }

    private void Awake()
    {
        settingPanel.GetComponent<SettingPanel>().InitializeSound();
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
        settingPanel.GetComponent<SettingPanel>().InitializeVibrating();
        if (!pauseGameMenu.activeSelf)
        {
            float t = pauseGameMenu.transform.position.x + canvas.pixelRect.width;
            pauseGameMenu.GetComponent<RectTransform>().position = new Vector3(t, pauseGameMenu.transform.position.y, pauseGameMenu.transform.position.z);

        }
        blockListPanel.transform.position = this.transform.position + Vector3.left * canvas.pixelRect.width;
        blockListPanel.SetActive(false);
        levelListPanel.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        levelListPanel.SetActive(false);
        settingPanel.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        settingPanel.SetActive(false);
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
        remainingBlocks.SetText($"{GameManager.Instance.countBlocks} Blocks");
    }

    public void UpdateTouchsNum()
    {
        remainingTouchs.SetText($"{GameManager.Instance.countTouchs} Moves");
    }

    public void ReplayGame()
    {
        UIManager.instance.pauseGameMenu.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        GameManager.Instance.blockPool.canRotate = false;
        GameManager.Instance.isOnMenu = true;
        pauseGameMenu.SetActive(true);
    }

    public void ChooseBlock()
    {
        GameManager.Instance.blockPool.canRotate = false;
        GameManager.Instance.isOnMenu = true;
        blockListPanel.SetActive(true);
        UIManager.instance.SetCoinText();
    }

    public void ChooseLevel()
    {
        GameManager.Instance.blockPool.canRotate = false;
        GameManager.Instance.isOnMenu = true;
        levelListPanel.SetActive(true);
    }

    public void ChooseDailyRewardPanel()
    {
        GameManager.Instance.blockPool.canRotate = false;
        GameManager.Instance.isOnMenu = true;
        dailyRewardPanel.SetActive(true);
    }

}
