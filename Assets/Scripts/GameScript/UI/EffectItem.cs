using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EffectItem : MonoBehaviour
{

    [SerializeField] Image icon;
    [SerializeField] Image icon1;
    [SerializeField] GameObject checkIcon;
    [SerializeField] Toggle toggle;


    private List<ParticleSystem> effectSystem;
    private EffectDataList.EffectType effectType;
    private int index;
    private string dataName;
    [SerializeField] private Material material;

    public List<ParticleSystem> EffecSystem
    {
        get { return effectSystem; }
        set { effectSystem = value; }
    }
    public EffectDataList.EffectType EffectType
    {
        get { return effectType; }
        set { effectType = value; }
    }

    public string DataName
    {
        get => dataName;
    }

    public int Index
    {
        get => index;
    }

    public Toggle Toggle
    {
        get { return toggle; }
        set { toggle = value; }
    }
    // Start is called before the first frame update

    public void InitEffectData(EffectItemData data)
    {
        this.icon.sprite = data.effectIcon;
        this.icon1.sprite = data.effectIcon;
        this.material = data.effectMaterial;
        this.index = data.index;
        this.dataName = data.effectName;
        this.toggle.interactable = data.isDefault || (PlayerPrefs.GetInt(data.effectName, 0) == 1);
        if (data.isDefault)
        {
            PlayerPrefs.SetInt(data.effectName, 1);
        }
        checkIcon.SetActive(this.toggle.interactable);
    }

    public void ChangeEffect()
    {
        if (this.Toggle.isOn == true)
        {
            switch (this.effectType)
            {
                case EffectDataList.EffectType.tapEffect:
                    foreach (ParticleSystem p in effectSystem)
                    {
                        p.GetComponent<Renderer>().material = this.material;
                    }
                    ParticleController.instance.clickPrefab.GetComponent<Renderer>().material = this.material;
                    PlayerPrefs.SetInt("Current Tap Effect", index);
                    Debug.Log(PlayerPrefs.GetInt("Current Tap Effect", index) + "- Choose");
                    break;
                case EffectDataList.EffectType.winGameEffect:
                    foreach (ParticleSystem p in effectSystem)
                    {
                        p.GetComponent<Renderer>().material = this.material;
                    }
                    ParticleController.instance.winGamePrefab.GetComponent<Renderer>().material = this.material;
                    PlayerPrefs.SetInt("Current Win Game Effect", index);
                    break;
                case EffectDataList.EffectType.trails:
                    foreach (GameObject b in GameManager.Instance.blockPool.pool)
                    {
                        TestMoveBlock tb = b.GetComponent<TestMoveBlock>();
                        tb.Trail.material = this.material;
                        GameManager.Instance.blockPool.BlockPrefab.GetComponent<TestMoveBlock>().Trail.material = this.material;
                    }
                    PlayerPrefs.SetInt("Current Block's Trail", index);
                    break;
            }
        }
    }

    public void SetInteractable()
    {
        this.Toggle.interactable = true;
        PlayerPrefs.SetInt(this.dataName, 1);
        this.checkIcon.SetActive(this.toggle.interactable);
    }
}
