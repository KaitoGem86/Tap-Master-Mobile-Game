using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAsset : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystem;
    [SerializeField] private Renderer[] _renderer;
    [SerializeField] private Camera _camera;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            i = (i + 1) % _particleSystem.Length;
            Vector3 pos = this._camera.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -20;
            _particleSystem[i].transform.position = pos;

            _particleSystem[i].Play();
            //_renderer[i].material.DOFade(0, 1.2f).OnComplete(() =>
            //{
            //    Color color = _renderer[i].material.color;
            //    color.a = 1;
            //    _renderer[i].material.color = color;
            //});
        }
    }
}
