using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider dashSlider;
    public float dashSliderValue;

    public GameObject character;
    void Start()
    {
        dashSliderValue = 1;
        dashSlider.value = dashSliderValue;
    }

    void Update()
    {
        dashSlider.value = dashSliderValue;
    }
}
