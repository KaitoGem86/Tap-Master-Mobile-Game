using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBlock : MonoBehaviour
{
    [SerializeField] RewardType type;
    [SerializeField] int amount;
    [SerializeField] Material material;
    [SerializeField] MeshRenderer meshRenderer;

    private void Start()
    {
        this.enabled = false;
    }
    private void Update()
    {

    }

    public Material Material
    {
        get { return material; }
    }

    public void OnCollectReward()
    {
        this.meshRenderer.material.DOFade(endValue: 0, duration: 0.5f).OnComplete(() =>
        {
            switch (type)
            {
                case RewardType.Coin:
                    CollectCoin();
                    break;
            }
        });
    }

    void CollectCoin()
    {
        Debug.Log("Collect hehehe");
        StartCoroutine(UpdateData());
        int n = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", n + amount);
        this.gameObject.SetActive(false);
    }

    IEnumerator UpdateData()
    {
        GameManager.Instance.countBlocks -= 1;
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        GameManager.Instance.coin += 1;
        PlayerPrefs.SetInt("Coin", currenCoin + 1);
        UIManager.instance.SetCoinText();
        UIManager.instance.UpdateBlocksNum();
        if (GameManager.Instance.countBlocks == 0)
        {
            GameManager.Instance.WinGame();
        }
        yield return null;
    }
}
