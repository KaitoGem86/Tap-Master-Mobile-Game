using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;


public class ParticleController : MonoBehaviour
{
    [Header("Effect Camera")]
    [SerializeField] Camera _camera;

    [Space]
    [Header("Elements of System")]
    [SerializeField] ParticleSystem[] _clickEffect;
    [SerializeField] ParticleSystem[] _winGameEffect;

    public static ParticleController instance;

    int i = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void OnClick()
    {
        i = (i + 1) % _clickEffect.Length;
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
