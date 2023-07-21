using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider backgroundMusicCtrl;
    [SerializeField] private Slider effectSoundMusicCtrl;
    [SerializeField] private Toggle vibrationCtrl;

    public void InitializeSound()
    {
        backgroundMusicCtrl.value = PlayerPrefs.GetFloat("Background Music Volume", 1);
        ChangeBackgroundMusicVolume();
        effectSoundMusicCtrl.value = PlayerPrefs.GetFloat("Effect Sounds Volume", 1);
        ChangeEffectVolume();
    }

    public void InitializeVibrating()
    {
        vibrationCtrl.isOn = PlayerPrefs.GetInt("Vibration", 1) == 1;
        AllowVibrating();
    }

    public void SaveSoundState()
    {
        PlayerPrefs.SetFloat("Background Music Volume", backgroundMusicCtrl.value);
        PlayerPrefs.SetFloat("Effect Sounds Volume", effectSoundMusicCtrl.value);
        PlayerPrefs.SetInt("Vibration", vibrationCtrl.isOn ? 1 : 0);
    }

    private void OnEnable()
    {
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }

    public void ChangeBackgroundMusicVolume()
    {
        SoundManager.instance.ChangeBackgroundMusicVolume(backgroundMusicCtrl.value);
        PlayerPrefs.SetFloat("Background Music Volume", backgroundMusicCtrl.value);
    }

    public void ChangeEffectVolume()
    {
        SoundManager.instance.ChangeEffectsSoundVolume(effectSoundMusicCtrl.value);
    }

    public void AllowVibrating()
    {
        GameManager.Instance.allowedVibrating = vibrationCtrl.isOn;
        Debug.Log(vibrationCtrl.isOn);

        PlayerPrefs.SetInt("Vibration", vibrationCtrl.isOn ? 1 : 0);
    }

    public void ExitPanel()
    {
        float width = UIManager.instance.canvas.pixelRect.width;
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
    }

    public void Exit()
    {
        GameManager.Instance.blockPool.canRotate = true;
        GameManager.Instance.isOnMenu = false;
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
