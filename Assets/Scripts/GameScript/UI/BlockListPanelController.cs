using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockListPanelController : MonoBehaviour
{
    //[SerializeField] TMP_Text CoinText;
    [Header("Item lists")]
    [SerializeField] ViewBlockListController blockList;
    [SerializeField] EffectItemList tapEffectList;
    [SerializeField] EffectItemList winEffectList;
    [SerializeField] EffectItemList trailEffectList;
    [SerializeField] Button buyButton;

    [Space]
    [Header("Elements of panel")]
    [SerializeField] GameObject blockSkinsList;
    [SerializeField] GameObject tapEffectsList;
    [SerializeField] GameObject trailsList;
    [SerializeField] GameObject winGameEffectsList;
    [SerializeField] Toggle blockSkinToggle;
    [SerializeField] Toggle tapEffectToggle;
    [SerializeField] Toggle trailToggle;
    [SerializeField] Toggle winGameEffectToggle;


    float width;
    GameObject currentList;

    public void OnEnable()
    {
        currentList = blockSkinsList;
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }

    private void Start()
    {
        UIManager.instance.SetCoinText();
        width = UIManager.instance.canvas.pixelRect.width;
        var r = this.transform.position;
        this.transform.position = r + Vector3.left * width;
    }

    private void Update()
    {
        CheckInteractableBuyButton();
    }

    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.left * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
    }

    public void Exit()
    {
        GameManager.Instance.blockPool.canRotate = true;
        GameManager.Instance.isOnMenu = false || UIManager.instance.isAwake;
        currentList.SetActive(false);
        blockSkinsList.SetActive(true);
        blockSkinToggle.isOn = true;
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

    public void BuyItem()
    {
        if (currentList == blockSkinsList)
        {
            BuyBlock();
        }
        else if (currentList == tapEffectsList)
        {
            BuyTapEffect();
        }
        else if (currentList == trailsList)
        {
            BuyTrail();
        }
        else if (currentList == winGameEffectsList)
        {
            BuyWinGameEffect();
        }
        else
        {
            Debug.Log("NULL Current List");
        }
    }

    void BuyBlock()
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

    void BuyTapEffect()
    {
        Debug.Log("Buy tap effect");
        if (tapEffectList.NotBuyedItems.Count <= 0)
            return;
        int random = Random.Range(0, tapEffectList.NotBuyedItems.Count);
        tapEffectList.Items[tapEffectList.NotBuyedItems[random]].SetInteractable();
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", currenCoin - GameManager.Instance.effectPrice);
        UIManager.instance.SetCoinText();
        tapEffectList.NotBuyedItems.RemoveAt(random);
    }

    void BuyTrail()
    {
        Debug.Log("Buy trail");
        if (trailEffectList.NotBuyedItems.Count <= 0)
            return;
        int random = Random.Range(0, trailEffectList.NotBuyedItems.Count);
        trailEffectList.Items[trailEffectList.NotBuyedItems[random]].SetInteractable();
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", currenCoin - GameManager.Instance.effectPrice);
        UIManager.instance.SetCoinText();
        trailEffectList.NotBuyedItems.RemoveAt(random);
    }

    void BuyWinGameEffect()
    {
        Debug.Log("Buy win game effect");
        if (winEffectList.NotBuyedItems.Count <= 0)
            return;
        int random = Random.Range(0, winEffectList.NotBuyedItems.Count);
        winEffectList.Items[winEffectList.NotBuyedItems[random]].SetInteractable();
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", currenCoin - GameManager.Instance.effectPrice);
        UIManager.instance.SetCoinText();
        winEffectList.NotBuyedItems.RemoveAt(random);
    }

    public void ChooseBlockSkinsList()
    {
        if (blockSkinToggle.isOn)
        {
            blockSkinsList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            blockSkinsList.SetActive(true);
            currentList.SetActive(false);
            currentList = blockSkinsList;
            blockSkinToggle.interactable = false;
            tapEffectToggle.interactable = true;
            trailToggle.interactable = true;
            winGameEffectToggle.interactable = true;
        }
    }

    public void ChooseTapEffectsList()
    {
        if (tapEffectToggle.isOn)
        {
            tapEffectsList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            tapEffectsList.SetActive(true);
            currentList.SetActive(false);
            currentList = tapEffectsList;
            blockSkinToggle.interactable = true;
            tapEffectToggle.interactable = false;
            trailToggle.interactable = true;
            winGameEffectToggle.interactable = true;
        }
    }

    public void ChooseTrailsList()
    {
        if (trailToggle.isOn)
        {
            trailsList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            trailsList.SetActive(true);
            currentList.SetActive(false);
            currentList = trailsList;
            blockSkinToggle.interactable = true;
            tapEffectToggle.interactable = true;
            trailToggle.interactable = false;
            winGameEffectToggle.interactable = true;
        }
    }

    public void ChooseWinGameEffectsList()
    {
        if (winGameEffectToggle.isOn)
        {
            winGameEffectsList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            winGameEffectsList.SetActive(true);
            currentList.SetActive(false);
            currentList = winGameEffectsList;
            blockSkinToggle.interactable = true;
            tapEffectToggle.interactable = true;
            trailToggle.interactable = true;
            winGameEffectToggle.interactable = false;
        }
    }
}
