using System.Collections;
using System.Collections.Generic;
using Core.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _inertiaSlider;
    [SerializeField] private Slider _dampingSlider;
    [SerializeField] private TMP_Text _sensitivityText;
    [SerializeField] private TMP_Text _inertiaText;
    [SerializeField] private TMP_Text _dampingText;

    public void OnSensitivity(){
        _GameManager.Instance.CameraController.Sensitivity = _sensitivitySlider.value * 20;
        _sensitivityText.text =  "Sensitivity "+ (_sensitivitySlider.value * 20).ToString("0.00");
    }

    public void OnInertia(){
        _GameManager.Instance.CameraController.Inertia = _inertiaSlider.value;
        _inertiaText.text = "Inertia " + _inertiaSlider.value.ToString("0.00");
    }

    public void OnDamping(){
        _GameManager.Instance.CameraController.Damping = _dampingSlider.value;
        _dampingText.text = "Damping " + _dampingSlider.value.ToString("0.00");
    }
}
