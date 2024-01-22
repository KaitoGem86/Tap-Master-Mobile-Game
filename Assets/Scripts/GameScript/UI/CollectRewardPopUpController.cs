using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectRewardPopUpController : MonoBehaviour
{
    [SerializeField] GameObject[] list;
    [SerializeField] TMP_Text amountText;
    [SerializeField] TMP_Text multipleAmountText;
    [SerializeField] TMP_Text multipleCoeffText;
    [SerializeField] Button getButton;

    private int[] multipleCoeffs = { 2, 3, 4, 5, 4, 3, 2 };
    int i = 0;
    int amount;
    int coeff;
    CoinController gainCoinEffect;
    Coroutine checkNumberOfMultiple;
    Coroutine checkAppearGetButton;

    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    void Start()
    {
        var seq = DOTween.Sequence();
        seq.Append(this.transform.DOScale(Vector3.one, duration: 0.3f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            coeff = 1;
            amountText.SetText("+" + amount);
            checkNumberOfMultiple = StartCoroutine(CheckNumberOfMultiple());
            checkAppearGetButton = StartCoroutine(CheckAppearGetButton());
        });
    }

    IEnumerator CheckNumberOfMultiple()
    {
        int n = list.Length;
        GameObject currentGo = list[n - 1];
        while (true)
        {
            currentGo.SetActive(true);
            list[i].SetActive(false);
            currentGo = list[i];
            multipleAmountText.SetText($"+ {amount * multipleCoeffs[i]}");
            multipleCoeffText.SetText($"x{multipleCoeffs[i]} Get");
            i = (i + 1) % n;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator CheckAppearGetButton()
    {
        int time = 3;
        while (time > 0)
        {
            time -= 1;
            yield return new WaitForSeconds(1);
        }
        getButton.gameObject.SetActive(true);
        yield return null;
    }

    public void GetReward()
    {
        gainCoinEffect = Instantiate(GameManager.Instance.gainCoinsAnim.gameObject, getButton.transform.position, Quaternion.identity, this.transform).GetComponent<CoinController>();
        gainCoinEffect.CountCoins(new Vector3(400, 840, 0));
        StopCoroutine(checkNumberOfMultiple);
        StopCoroutine(checkAppearGetButton);
        int n = PlayerPrefs.GetInt("Coin", 0);
        Debug.Log(n);
        PlayerPrefs.SetInt("Coin", n + amount * coeff);
        Debug.Log(n + amount * coeff);
        ExitPopUp();
    }

    public void GetMultipleReward()
    {
        gainCoinEffect = Instantiate(GameManager.Instance.gainCoinsAnim.gameObject, multipleAmountText.transform.position, Quaternion.identity, this.transform).GetComponent<CoinController>();
        gainCoinEffect.CountCoins(new Vector3(400, 840, 0));
        coeff = multipleCoeffs[i];
        StopCoroutine(checkNumberOfMultiple);
        StopCoroutine(checkAppearGetButton);
        int n = PlayerPrefs.GetInt("Coin", 0);
        Debug.Log(n);
        PlayerPrefs.SetInt("Coin", n + amount * coeff);
        Debug.Log(n + amount * coeff);
        ExitPopUp();
    }

    void ExitWithAnim()
    {
        this.transform.DOScale(0, duration: 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            GameManager.Instance.isOnMenu = false;
            GameManager.Instance.camMoving.CanRotate = true;
            this.gameObject.SetActive(false);
            this.transform.localScale = Vector3.one;
        });
    }


    void ExitPopUp()
    {
        gainCoinEffect.t.OnComplete(() =>
        {
            ExitWithAnim();
        });
    }
}
