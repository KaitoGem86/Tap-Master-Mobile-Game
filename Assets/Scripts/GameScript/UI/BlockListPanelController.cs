using DG.Tweening;
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
    float width;

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
        //GameManager.Instance.blockPool.gameObject.SetActive(true);
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

    public void OnEnable()
    {
        //Vector3 r = this.transform.position;
        //Debug.Log(r + " - " + UIManager.instance.canvas.transform.position);
        //if (r == UIManager.instance.canvas.transform.position)
        //{
        //    Vector3 p = this.GetComponent<RectTransform>().position;
        //    this.GetComponent<RectTransform>().position = p + Vector3.left * width;
        //    Debug.Log(this.transform.position);
        //}
        //else
        //    r += Vector3.right * width;
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }
}
