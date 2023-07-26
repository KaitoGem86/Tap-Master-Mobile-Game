using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectItemList : MonoBehaviour
{
    [Header("Data To Generate Effect List")]
    [SerializeField] EffectDataList effectDataList;
    [SerializeField] EffectItem itemPrefab;

    [Space]
    [Header("Controller of Particle System")]
    [SerializeField] List<ParticleSystem> particleSystems;

    private EffectDataList.EffectType effectType;
    private List<EffectItem> items = new List<EffectItem>();
    private List<int> notBuyedItems = new List<int>();
    public List<EffectItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    public List<int> NotBuyedItems
    {
        get { return notBuyedItems; }
        set { notBuyedItems = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
    }


    void InitializeData()
    {
        this.effectType = effectDataList.type;
        switch (effectType)
        {
            case EffectDataList.EffectType.tapEffect:
                particleSystems = ParticleController.instance._clickEffect;
                break;
            case EffectDataList.EffectType.winGameEffect:
                Debug.Log(PlayerPrefs.GetInt("Current Win Game Effect", 0));
                particleSystems = ParticleController.instance._winGameEffect;
                break;
            case EffectDataList.EffectType.trails:
                Debug.Log("No need particle system");
                break;
        }
        for (int i = 0; i < effectDataList.effectData.Length; i++)
        {
            var go = Instantiate(itemPrefab.gameObject, this.transform);
            EffectItem item = go.GetComponent<EffectItem>();
            item.InitEffectData(effectDataList.effectData[i]);
            item.EffectType = this.effectType;
            item.EffecSystem = this.particleSystems;
            item.Toggle.group = this.GetComponent<ToggleGroup>();
            if (PlayerPrefs.GetInt(item.DataName, 0) == 0)
            {
                notBuyedItems.Add(item.Index);
            }
            items.Add(item);
        }
        switch (effectType)
        {
            case EffectDataList.EffectType.tapEffect:
                Debug.Log(PlayerPrefs.GetInt("Current Tap Effect", 0));
                particleSystems = ParticleController.instance._clickEffect;
                items[PlayerPrefs.GetInt("Current Tap Effect", 0)].Toggle.isOn = true;
                items[PlayerPrefs.GetInt("Current Tap Effect", 0)].ChangeEffect();
                break;
            case EffectDataList.EffectType.winGameEffect:
                Debug.Log(PlayerPrefs.GetInt("Current Win Game Effect", 0));
                particleSystems = ParticleController.instance._winGameEffect;
                items[PlayerPrefs.GetInt("Current Win Game Effect", 0)].Toggle.isOn = true;
                items[PlayerPrefs.GetInt("Current Win Game Effect", 0)].ChangeEffect();
                break;
            case EffectDataList.EffectType.trails:
                Debug.Log(PlayerPrefs.GetInt("Current Block's Trail", 0));
                items[PlayerPrefs.GetInt("Current Block's Trail", 0)].Toggle.isOn = true;
                items[PlayerPrefs.GetInt("Current Block's Trail", 0)].ChangeEffect();
                break;
        }
    }
}
