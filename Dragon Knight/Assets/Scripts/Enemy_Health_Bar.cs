using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health_Bar : MonoBehaviour
{
    public Slider slider;

    public void Health_Bar(int value1)
    {
        slider.value -= value1;
    }
}
