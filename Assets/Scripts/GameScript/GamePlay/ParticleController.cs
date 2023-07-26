using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParticleController : MonoBehaviour
{
    [Header("Effect Camera")]
    [SerializeField] Camera _camera;

    [Space]
    [Header("Elements of System")]
    [SerializeField] GameObject clickGroup;
    [SerializeField] GameObject winGameGroup;
    public List<ParticleSystem> _clickEffect;
    public List<ParticleSystem> _winGameEffect;

    public static ParticleController instance;

    public ParticleSystem clickPrefab;
    public ParticleSystem winGamePrefab;

    int i = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        InitializeSystem();
    }

    public void InitializeSystem()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(clickPrefab.gameObject, clickGroup.transform);
            _clickEffect.Add(go.GetComponent<ParticleSystem>());
        }
        var win1 = Instantiate(winGamePrefab.gameObject, winGameGroup.transform);
        win1.transform.position = new Vector3(11, -10, 0);
        win1.transform.rotation = Quaternion.Euler(180, 0, 200);
        _winGameEffect.Add(win1.GetComponent<ParticleSystem>());
        var win2 = Instantiate(winGamePrefab.gameObject, winGameGroup.transform);
        win2.transform.position = new Vector3(-11, -10, 0);
        win2.transform.rotation = Quaternion.Euler(0, 0, 20);
        _winGameEffect.Add(win2.GetComponent<ParticleSystem>());
    }

    public void OnClick()
    {
        i = (i + 1) % _clickEffect.Count;
        Vector3 pos = this._camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = -20;
        _clickEffect[i].transform.position = pos;
        _clickEffect[i].Play();
    }

    public void OnWinGame()
    {
        foreach (var winGameEffect in _winGameEffect)
        {
            winGameEffect.Play();
        }
    }
}
