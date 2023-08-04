using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private GameObject[] coins;
    //[SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;

    public Tween t;
    //void Start()
    //{
    //    if (coinsAmount == 0)
    //        coinsAmount = 10; // you need to change this value based on the number of coins in the inspector

    //    initialPos = new Vector2[coinsAmount];
    //    initialRotation = new Quaternion[coinsAmount];

    //    for (int i = 0; i < coins.Length; i++)
    //    {
    //        initialPos[i] = coins[i].transform.GetComponent<RectTransform>().anchoredPosition;
    //        initialRotation[i] = coins[i].transform.GetComponent<RectTransform>().rotation;
    //    }
    //}


    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        CountCoins(new Vector3(400, 840, 0));
    //    }
    //}

    public void CountCoins(Vector3 target)
    {
        PreMove();
        var delay = 0f;

        for (int i = 0; i < coins.Length; i++)
        {
            t = coins[i].transform.DOScale(1f, 0.1f).SetDelay(delay).SetEase(Ease.OutBack);

            t = coins[i].GetComponent<RectTransform>().DOAnchorPos(target, 0.8f)
                .SetDelay(delay + 0.3f).SetEase(Ease.InBack);


            t = coins[i].transform.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.3f)
                .SetEase(Ease.Flash);

            t = coins[i].transform.DOScale(0f, 0.1f).SetDelay(delay + 1.3f).SetEase(Ease.OutBack);

            delay += 0.1f;
            //counter.transform.parent.GetChild(0).transform.DOScale(1.1f, 0.1f).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine).SetDelay(1.2f);
        }
    }

    IEnumerator CountDollars()
    {
        yield return new WaitForEndOfFrame();
    }

    void PreMove()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].transform.localScale = Vector3.zero;
        }
    }
}
