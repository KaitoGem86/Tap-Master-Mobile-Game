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
    List<RectTransform> _clickEffectRect = new List<RectTransform>();
    public List<ParticleSystem> _winGameEffect;

    public static ParticleController instance;

    public ParticleSystem clickPrefab;
    public ParticleSystem winGamePrefab;

    int i = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void InitializeSystem()
    {
        ParticleSystem.MainModule cMain = clickPrefab.main;
        //cMain.startSize = _camera.orthographicSize / 20;
        ParticleSystem.MainModule wMain = winGamePrefab.main;
        //wMain.startSize = _camera.orthographicSize / 20;

        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(clickPrefab.gameObject, clickGroup.transform);
            _clickEffect.Add(go.GetComponent<ParticleSystem>());
            _clickEffectRect.Add(go.GetComponent<RectTransform>());
        }
        var win1 = Instantiate(winGamePrefab.gameObject, winGameGroup.transform);
        win1.transform.SetPositionAndRotation(new Vector3(11, -10, 0) * _camera.orthographicSize / 20, Quaternion.Euler(180, 0, 200));
        //win1.transform.position = new Vector3(11, -10, 0) * _camera.orthographicSize / 20;
        //win1.transform.rotation = Quaternion.Euler(180, 0, 200);
        _winGameEffect.Add(win1.GetComponent<ParticleSystem>());
        var win2 = Instantiate(winGamePrefab.gameObject, winGameGroup.transform);
        win2.transform.SetPositionAndRotation(new Vector3(-11, -10, 0) * _camera.orthographicSize / 20, Quaternion.Euler(0, 0, 20));
        //win2.transform.position = new Vector3(-11, -10, 0) * _camera.orthographicSize / 20;
        //win2.transform.rotation = Quaternion.Euler(0, 0, 20);
        _winGameEffect.Add(win2.GetComponent<ParticleSystem>());
    }

    public void OnClick()
    {
        i = (i + 1) % _clickEffect.Count;
        Vector3 pos = this._camera.ScreenToWorldPoint(Input.mousePosition);
        //pos.z = -20;
        _clickEffectRect[i].position = pos;
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
