using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockListPanelController : MonoBehaviour
{
    //[SerializeField] TMP_Text CoinText;
    [SerializeField] ViewBlockListController blockList;
    [SerializeField] Button buyButton;


    private void Start()
    {
        UIManager.instance.SetCoinText();
    }

    private void Update()
    {
        CheckInteractableBuyButton();
    }

    public void ExitPanel()
    {
        Time.timeScale = 1;
        GameManager.Instance.blockPool.gameObject.SetActive(true);
        GameManager.Instance.selectBlock.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void IncreaseCoint()
    {
        int currentCoin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", currentCoin + 1000);
        UIManager.instance.SetCoinText();
    }

    public void CheckInteractableBuyButton()
    {
        buyButton.interactable = PlayerPrefs.GetInt("Coin", 0) >= GameManager.Instance.blockPrice;
    }

    public void BuyBlock()
    {
        if (blockList.interactableIndexs.Count <= 0)
            return;
        int random = Random.Range(0, blockList.interactableIndexs.Count);
        Debug.Log(random + "-" + blockList.interactableIndexs[random]);
        blockList.blockItems[blockList.interactableIndexs[random]].Islocked = true;
        blockList.blockItems[blockList.interactableIndexs[random]].SetInteractable(true);
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", currenCoin - GameManager.Instance.blockPrice);
        UIManager.instance.SetCoinText();
        blockList.interactableIndexs.Remove(blockList.interactableIndexs[random]);
    }
}
