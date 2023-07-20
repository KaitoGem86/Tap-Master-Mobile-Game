using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class BombButtonController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] BombController bombObject;
    [SerializeField] Button bombButton;
    [SerializeField] Image countDownFilter;

    float countDownTime = 15;
    int coin;

    public void UseBomb()
    {
        Debug.Log("Click");
        if (!bombObject.gameObject.activeSelf)
        {
            bombObject.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Coin", coin - 100);
            bombButton.interactable = false;
            countDownFilter.gameObject.SetActive(true);
            StartCoroutine(CountDown());
        }
    }

    private void Update()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
        bombButton.interactable = coin >= 100;
    }

    IEnumerator CountDown()
    {
        while (countDownTime > 0)
        {
            countDownTime -= Time.deltaTime;
            countDownFilter.fillAmount = countDownTime / 15;
            bombButton.interactable = false;
            yield return new WaitForEndOfFrame();
        }
        bombButton.interactable = coin >= 100;
        countDownFilter.gameObject.SetActive(false);
        countDownTime = 15;
        yield return null;
    }
}
