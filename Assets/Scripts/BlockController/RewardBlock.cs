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
    [SerializeField] MeshRenderer pieceRenderer;


    private void Start()
    {
        if (type == RewardType.Coin)
            this.enabled = false;
    }

    public RewardType Type
    {
        get { return type; }
        set { type = value; }
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
                case RewardType.Puzzle:
                    CollectPuzzle();
                    break;
                case RewardType.ImediatedCoin:
                    CollectImediatedCoin();
                    break;
            }
        });
        this.pieceRenderer.material.DOFade(endValue: 0, duration: 0.5f);
    }

    void CollectPuzzle()
    {
        var p_s = UIManager.instance.PuzzleRewardPanel.GetComponent<PuzzleRewardPanel>().NotCollectReward;

        int i = Random.Range(0, p_s.Count);

        UIManager.instance.CollectPuzzleNotice(i);
        GameManager.Instance.isOnMenu = true;
        GameManager.Instance.camMoving.CanRotate = false;
        StartCoroutine(UpdateData());
        this.gameObject.SetActive(false);
    }

    void CollectCoin()
    {
        this.amount = Random.Range(20, 45);
        Debug.Log("Collect hehehe");
        StartCoroutine(UpdateData());
        UIManager.instance.CollectCoinPopUp.Amount = this.amount;
        UIManager.instance.CollectCoinPopUp.transform.localScale = Vector3.zero;
        UIManager.instance.CollectCoinPopUp.gameObject.SetActive(true);
        GameManager.Instance.isOnMenu = true;
        GameManager.Instance.camMoving.CanRotate = false;
        this.gameObject.SetActive(false);
    }

    void CollectImediatedCoin()
    {
        var gainCoinEffect = Instantiate(GameManager.Instance.gainCoinsAnim.gameObject,
            Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, UIManager.instance.existingPanel.transform).GetComponent<CoinController>();
        gainCoinEffect.CountCoins(new Vector3(-400, 840, 0));
        int n = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", n + 100);
        StartCoroutine(UpdateData());
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

    public void PreMove(Vector3 startPos, Vector3 startAngle, Vector3 endPos, Vector3 endAngle, int i)
    {
        this.transform.DOScale(Vector3.one, duration: 1.5f);
        this.transform.DOLocalMove(endPos, duration: 1.5f).SetEase(Ease.InSine);
        this.transform.DOLocalRotate(endAngle, duration: 1.5f).SetEase(Ease.InSine);
    }

    public void SetLocalScale()
    {
        this.transform.localScale = Vector3.zero;
    }
}
