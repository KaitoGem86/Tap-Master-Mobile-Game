using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BombButtonController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] BombController bombObject;
    [SerializeField] Button bombButton;
    [SerializeField] Image countDownFilter;
    [SerializeField] public BombImageController bombImage;

    [Space]
    [Header("Status's Sprite")]
    [SerializeField] Image bombIcon;
    [SerializeField] Sprite bombSprite;
    [SerializeField] Sprite cancelSprite;


    float countDownTime;
    int coin;

    private void Start()
    {
        StartCoroutine(CountDown(PlayerPrefs.GetFloat("Reload bomb in", 0)));
    }

    public void UseBomb()
    {
        if (!bombImage.gameObject.activeSelf)
        {
            bombImage.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Coin", coin - 100);
            UIManager.instance.cancelArea.gameObject.SetActive(true);
            bombIcon.sprite = cancelSprite;
            //StartCoroutine(CountDown());
        }
        else
        {
            bombImage.gameObject.SetActive(false);
            countDownFilter.gameObject.SetActive(false);
            UIManager.instance.cancelArea.gameObject.SetActive(false);
            bombIcon.sprite = bombSprite;
        }
    }

    private void Update()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
        bombButton.interactable = coin >= 100 || bombImage.gameObject.activeSelf;
    }

    public void RechangeSprite()
    {
        bombIcon.sprite = bombSprite;
    }

    public IEnumerator CountDown(float value)
    {
        countDownTime = value;
        bombButton.interactable = false;
        countDownFilter.gameObject.SetActive(true);
        while (countDownTime > 0)
        {
            countDownTime -= Time.deltaTime;
            //Debug.Log(countDownTime);
            PlayerPrefs.SetFloat("Reload bomb in", countDownTime);
            countDownFilter.fillAmount = countDownTime / 15;
            bombButton.interactable = false;
            yield return new WaitForEndOfFrame();
        }
        PlayerPrefs.SetFloat("Reload bomb in", 0);
        bombButton.interactable = coin >= 100;
        countDownFilter.gameObject.SetActive(false);
        countDownTime = 15;
        yield return null;
    }
}
